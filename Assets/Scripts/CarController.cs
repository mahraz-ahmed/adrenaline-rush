using UnityEngine;

public class CarController : MonoBehaviour
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
    public Transform steeringWheel; 


    // Define variables for car movement settings
    public float motorForce = 500f; 
    public float brakeForce = 1000f; 
    public float steeringAngle = 30f; // Maximum steering angle for the front wheels
    public static float speed;
    private int driveLock = 0;

    // Additional variables for dynamic steering adjustment
    public float minSteeringAngle = 10f; // Minimum steering angle at high speeds
    public float maxSpeed = 100f; // Speed at which steering is minimized
    private Rigidbody carRigidbody; // Rigidbody to calculate car speed

    private void Start()
    {
        // Initialise Rigidbody reference
        carRigidbody = GetComponent<Rigidbody>();
        driveLock = 0; 
    }

    private void FixedUpdate()
    {
        // Get player input
        float motorInput = Input.GetAxis("Vertical"); // W/S keys for forward/backward
        float steeringInput = Input.GetAxis("Horizontal"); // A/D keys for steering

        // Calculate the current speed of the car
        speed = carRigidbody.linearVelocity.magnitude * 2.23694f; // Convert to mph

        // Adjust steering angle based on speed
        float adjustedSteeringAngle = Mathf.Lerp(steeringAngle, minSteeringAngle, speed / maxSpeed);

        bool isBraking = Input.GetKey(KeyCode.LeftShift); // Detect if brake key is pressed

        // Unlock controls upon countdown end
        if (RaceManager.raceStarted == true)
        {
            driveLock = 1;

        } 
        else
        {
            driveLock = 0;
            isBraking = true; // Brake car at race end
        }

        // Apply motor force to rear wheels
        if (isBraking == true)
        {
            ApplyBraking(); // Apply brake force if braking
        }
        else
        {
            RLCollider.motorTorque = motorInput * motorForce * driveLock;
            RRCollider.motorTorque = motorInput * motorForce * driveLock;

            // Clear brake torque when not braking
            RLCollider.brakeTorque = 0;
            RRCollider.brakeTorque = 0;
            FLCollider.brakeTorque = 0;
            FRCollider.brakeTorque = 0;
        }

        // Apply steering angle to front wheels
        FLCollider.steerAngle = steeringInput * adjustedSteeringAngle;
        FRCollider.steerAngle = steeringInput * adjustedSteeringAngle;

        // Update each wheel's position and rotation
        UpdateWheelPos(FLCollider, FLTransform);
        UpdateWheelPos(FRCollider, FRTransform);
        UpdateWheelPos(RLCollider, RLTransform);
        UpdateWheelPos(RRCollider, RRTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform wheelTransform)
    {
        // Get position and rotation from WheelCollider
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        // Set the wheel model's position and rotation to match collider
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
    private void ApplyBraking()
    {
        // Apply braking torque to all wheels
        RLCollider.brakeTorque = brakeForce;
        RRCollider.brakeTorque = brakeForce;
        FLCollider.brakeTorque = brakeForce;
        FRCollider.brakeTorque = brakeForce;

    }
}
