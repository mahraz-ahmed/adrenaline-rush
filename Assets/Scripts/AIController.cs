using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
    // Define variables for wheel colliders
    public WheelCollider FLCollider;
    public WheelCollider FRCollider;
    public WheelCollider RLCollider;
    public WheelCollider RRCollider;

    // Define variables for wheel models (to rotate them)
    public Transform FRTransform;
    public Transform RLTransform;
    public Transform RRTransform;
    public Transform FLTransform;

    // Define variables for car movement settings
    private float motorForceMin;
    private float motorForceMax; 
    private float brakeForce = 1000f;
    private float steeringAngle = 45f; // Max steering angle for front wheels
    private float minSteeringAngle = 10f; // Min steering angle at high speeds
    private float maxSpeed = 100f; // Speed at which steering is minimized
    private Rigidbody carRigidbody; // Rigidbody to calculate car speed
    private int driveLock = 0; // Allows cars to drive upon race start

    // AI-specific variables for path-following
    public Transform path; // Waypoint parent object
    private float waypointThreshold = 7f; // Threshold for reaching a waypoint
    private float maxAllowedAngle = 100f; // Maximum allowed angle to next waypoint before teleport
    private Transform[] waypoints; 
    private int currentWaypointIndex = 0;
    public float waypointOffset = 0; // Unique waypoint path offset for each car on the z-axis

    // Cooldown variables
    private bool isTeleportOnCooldown = false;
    public float teleportCooldownTime = 2f; // Cooldown time for teleporting
    public bool missileHit = false;

    // Randomized values
    private float randomMotorForce;
    private float randomDeviation;

    // Collision avoidance variables
    private float forwardDetectionRange = 5f;
    private float sideDetectionRange = 1f;
    public bool isAvoidingObstacle = false; 
    public int brakeOverride;
    public int steerOverride;

    // Variables for teleport when still
    private float timeStill = 0f; 
    private float stillThreshold = 3f; // Number of secs till teleport

    
    
    private void Start()
    {
        // Initialise Rigidbody reference
        carRigidbody = GetComponent<Rigidbody>();

        // Initialise waypoints array from path's child objects
        waypoints = new Transform[path.childCount];
        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i);
        }
        currentWaypointIndex = 0;

    }

    public void FixedUpdate()
    {

        // Random deviations, varies with difficulty
        if (DifficultyMenu.selectedDifficulty == "easy")
        {
            randomDeviation = Random.Range(-10f, 10f); // Steering deviation
            randomMotorForce = Random.Range(300, 500);
        }
        else if (DifficultyMenu.selectedDifficulty == "medium")
        {
            randomDeviation = Random.Range(-5f, 5f);
            randomMotorForce = Random.Range(400, 600);
        }
        else if (DifficultyMenu.selectedDifficulty == "hard")
        {
            // No deviations in hard mode
            randomDeviation = 0;
            randomMotorForce = 600;
        }

       // Debug.Log($"Car {gameObject.name}: Difficulty = {DifficultyMenu.selectedDifficulty}, " +
          //$"Deviation = {randomDeviation}, MinForce = {randomMotorForce}");


        // Calculate current speed of car
        float speed = carRigidbody.linearVelocity.magnitude * 2.23694f; // Convert to mph

        // Unlock controls upon race start
        if (RaceManager.raceStarted == true)
        {
            driveLock = 1; // Enable driving
        }
        else
        {
            driveLock = 0; // Disable driving
            brakeOverride = -10; // Halt car
        }

        // Check for obstacles and avoid if detected
        CheckForObstacles();

        // Get waypoint target position 
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        targetPosition.z += waypointOffset; // Unique offset to separate cars 

        // Calculate direction to target position
        Vector3 directionToTarget = targetPosition - transform.position;

        // Calculate steering angle to turn towards target
        float angle = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);
        
        float adjustedSteeringAngle = Mathf.Clamp(angle, -steeringAngle, steeringAngle);

        // Add random deviation to steering when NOT avoiding obstacles
        if (!isAvoidingObstacle)
        {
            adjustedSteeringAngle += randomDeviation;
        }

        // Adjust steering angle dynamically based on speed
        float dynamicSteeringAngle = Mathf.Lerp(adjustedSteeringAngle, minSteeringAngle, speed / maxSpeed);

        if (isAvoidingObstacle)
        {
            FLCollider.steerAngle = steerOverride;
            FRCollider.steerAngle = steerOverride;
        }
        else
        {
            // Apply steering angle to front wheels
            FLCollider.steerAngle = dynamicSteeringAngle;
            FRCollider.steerAngle = dynamicSteeringAngle;
        }

        // Calculate distance to target
        float distanceToTarget = directionToTarget.magnitude;

        // Update target waypoint if car is close enough to current target
        if (distanceToTarget < waypointThreshold)
        {
            // Move to next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop through waypoints
        }

        // Check if car is too far off course (angle exceeds threshold)
        if (Mathf.Abs(angle) > maxAllowedAngle && !isTeleportOnCooldown && !missileHit)
        {
            Debug.Log("Car off course! Teleporting to waypoint: " + currentWaypointIndex);
            TeleportToWaypoint();
            return; 
        }

        // Check if car has been still for too long
        if (speed < 0.1f && RaceManager.raceStarted == true) 
        {
            timeStill += Time.deltaTime; // Increase the time still counter
        }
        else
        {
            timeStill = 0f; // Reset timer if car is moving
        }

        if (timeStill > stillThreshold && !isTeleportOnCooldown) // If still for more than 3 secs
        {
            Debug.Log("Car is still for too long! Teleporting to waypoint: " + currentWaypointIndex);
            TeleportToWaypoint();
            timeStill = 0f; // Reset timer after teleport
            return; 
        }

        // Apply motor force or braking based on conditions
        if (Mathf.Abs(angle) <= 20)
        {
            // Apply random motor force to rear wheels
            // Brake override applied from collision avoidance system
            // Drive lock disabled when race begins
            RLCollider.motorTorque = randomMotorForce * driveLock * brakeOverride;
            RRCollider.motorTorque = randomMotorForce * driveLock * brakeOverride;

            // Clear brake torque when not braking
            ClearBrakes();
        }
        else if (Mathf.Abs(angle) > 20 && Mathf.Abs(angle) <= 50)
        {
            ApplyBraking(2);
        }
        else
        {
            ClearBrakes();
        }

        // Update each wheel's position and rotation
        UpdateWheelPos(FLCollider, FLTransform);
        UpdateWheelPos(FRCollider, FRTransform);
        UpdateWheelPos(RLCollider, RLTransform);
        UpdateWheelPos(RRCollider, RRTransform);
    }
    
    private void CheckForObstacles()
    {
        // Cast ray in front of car
        RaycastHit forwardHit;
        Vector3 forward = transform.forward;

        if (Physics.Raycast(transform.position, forward, out forwardHit, forwardDetectionRange))
        {
            if (IsCar(forwardHit.collider.transform))
            {
               //Debug.Log("Car detected in front: " + forwardHit.collider.transform.root.name);

                // Get position of obstacle relative to car
                Vector3 localPosition = transform.InverseTransformPoint(forwardHit.point);

                // Decide whether to steer left or right based on obstacle's position
                if (localPosition.x > 0) // Obstacle is on right
                {
                  // Debug.Log("Obstacle in front on right, steering left.");
                    steerOverride = -30; // Steer left
                }
                else // Obstacle is on left
                {
                  //  Debug.Log("Obstacle in front on left, steering right.");
                    steerOverride = 30; // Steer right
                }
                Debug.Log("BRAKING DUE TO OBSTACLE!");
                brakeOverride = -2; // Apply brakes
            }
        }
        else
        {
            // Reset overrides if no obstacle detected 
            steerOverride = 0;
            brakeOverride = 1;
        }

        // Check for side obstacles
        RaycastHit leftHit, rightHit;
        Vector3 left = -transform.right;
        Vector3 right = transform.right;

        bool leftObstacle = Physics.Raycast(transform.position, left, out leftHit, sideDetectionRange);
        bool rightObstacle = Physics.Raycast(transform.position, right, out rightHit, sideDetectionRange);

        if (leftObstacle && IsCar(leftHit.collider.transform))
        {
            //Debug.Log("Car detected on left: " + leftHit.collider.transform.root.name);
            steerOverride = 5;

        }
        else if (rightObstacle && IsCar(rightHit.collider.transform))
        {
           // Debug.Log("Car detected on right: " + rightHit.collider.transform.root.name);
            steerOverride = -5;
        }
        else
        {
            // No side obstacles detected
            steerOverride = 0;
        }
    }

    // Check if collision object is a car
    private bool IsCar(Transform transform)
    {
        while (transform != null)
        {
            if (transform.CompareTag("BRAKE"))
            {
                return true;
            }
            transform = transform.parent; // Traverse up the hierarchy
        }
        return false;
    }


    private void TeleportToWaypoint()
    {
      // Check if waypoint is clear
      //  if (!IsWaypointClear(waypoints[currentWaypointIndex].position))
      //  {
      //      Debug.Log("Waypoint " + currentWaypointIndex + " is not clear. Teleport delayed.");
      //      return; // Exit if waypoint is blocked 
      //  }

        // Teleport car to current target waypoint
        transform.position = waypoints[currentWaypointIndex].position;

        // Determine next waypoint's position
        int nextWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        Vector3 directionToNextWaypoint = waypoints[nextWaypointIndex].position - waypoints[currentWaypointIndex].position;

        // Set rotation of car to face next waypoint
        transform.rotation = Quaternion.LookRotation(directionToNextWaypoint, Vector3.up);

        // Reset velocity and angular velocity to avoid drift after teleport
        ResetVelocityAndRotation();

        Debug.Log("Teleported to waypoint: " + currentWaypointIndex);

        // Start cooldown
        StartCoroutine(TeleportCooldownCoroutine());
    }

    private bool IsWaypointClear(Vector3 waypointPosition)
    {
        float detectionRadius = 5f; // Define radius for clearance check

        // Check for nearby objects
        Collider[] overlappingObjects = Physics.OverlapSphere(waypointPosition, detectionRadius);

        // Check each object in the overlapping objects array
        foreach (var obj in overlappingObjects)
        {
            // Traverse up hierarchy to checkroot object for the "BRAKE" tag
            if (IsCar(obj.transform))
            {
                Debug.Log("Waypoint blocked by: " + obj.name);
                return false; // "BRAKE" tag indicates car, waypoint not clear
            }
        }

        return true; // If no object with "BRAKE" tag found, waypoint clear
    }

    private void ResetVelocityAndRotation()
    {
        carRigidbody.linearVelocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;
    }
    

    public IEnumerator TeleportCooldownCoroutine()
    {
        isTeleportOnCooldown = true;
        yield return new WaitForSeconds(teleportCooldownTime);
        isTeleportOnCooldown = false;
    }

    private void ClearBrakes()
    {
        RLCollider.brakeTorque = 0;
        RRCollider.brakeTorque = 0;
        FLCollider.brakeTorque = 0;
        FRCollider.brakeTorque = 0;
    }


    private void UpdateWheelPos(WheelCollider wheelCollider, Transform wheelTransform)
    {
        // Get position and rotation from WheelCollider
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        // Set wheel model's position and rotation to match collider
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void ApplyBraking(float duration)
    {
        // Apply braking for set time
        StartCoroutine(ApplyBrakingCoroutine(duration, brakeForce));
    }

    private IEnumerator ApplyBrakingCoroutine(float duration, float brakeForce)
    {
        // Apply brake force to all wheels
        RLCollider.brakeTorque = brakeForce;
        RRCollider.brakeTorque = brakeForce;
        FLCollider.brakeTorque = brakeForce;
        FRCollider.brakeTorque = brakeForce;
        
        yield return new WaitForSeconds(duration);

        // Remove braking torque after time is up
        ClearBrakes();
    }
}
