using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class PosManager : MonoBehaviour
{
    // References to be assigned in inspector
    public GameObject playerCar;
    public List<GameObject> aiCars; 
    public Transform waypointsParent;
    public TMP_Text positionText;

    public float acceptableDistance = 35f; // Distance to reach waypoint
    public int totalLaps = 3;

    // Array to hold all waypoint transforms
    private Transform[] waypoints;

    // Dictionary to track progress of each car (player and AI)
    private Dictionary<GameObject, CarProgress> carProgressMap = new Dictionary<GameObject, CarProgress>();

    void Start()
    {
        // Load all waypoints from the waypointsParent into the waypoints array
        waypoints = new Transform[waypointsParent.childCount];
        for (int i = 0; i < waypointsParent.childCount; i++)
        {
            waypoints[i] = waypointsParent.GetChild(i); // Populate the array with child transforms
        }

        // Add the player car to the tracking system
        AddCarToTracking(playerCar);

        // Add each AI car to the tracking system
        foreach (GameObject aiCar in aiCars)
        {
            AddCarToTracking(aiCar);
        }
    }

    void Update()
    {
        // Update progress for every car in the dictionary
        foreach (GameObject car in carProgressMap.Keys.ToList())
        {
            UpdateCarProgress(car);
        }

        // Sort cars based on progress, update player position display
        SortCarPositions();
    }

    // Adds cars into tracking dictionary
    void AddCarToTracking(GameObject car)
    {
        if (car != null && !carProgressMap.ContainsKey(car)) // Check for null and duplicates
        {
            carProgressMap[car] = new CarProgress(); // Initialise progress object for each car
        }
    }

    void UpdateCarProgress(GameObject car)
    {
        CarProgress progress = carProgressMap[car]; // Retrieve car progress data

        // Calculate distance from car to next waypoint
        float distanceToWaypoint = Vector3.Distance(car.transform.position, waypoints[progress.currentWaypointIndex].position);

        if (distanceToWaypoint < acceptableDistance)
        {
            progress.currentWaypointIndex++; // Move to the next waypoint

            // If all waypoints passed, increment lap count and reset waypoint index
            if (progress.currentWaypointIndex >= waypoints.Length)
            {
                progress.currentWaypointIndex = 0; // Reset to the first waypoint
                progress.lapCount++; // Increment lap count
            }
        }

        // Calculate total number of waypoints in race (3 laps x total waypoints)
        int totalWaypointsInRace = waypoints.Length * totalLaps;

        // Calculate percentage of race completed based on current lap and waypoint index
        float raceProgressPercentage = ((progress.lapCount * waypoints.Length + progress.currentWaypointIndex) / (float)totalWaypointsInRace);

        // Calculate total progress considering lap count, percentage of race, and distance to next waypoint
        progress.totalProgress = raceProgressPercentage * 100f - (distanceToWaypoint / 1000f);
    }


    // Sorts cars based on progress and updates player position text
    void SortCarPositions()
    {
        // Order cars by total progress in descending order
        var sortedCars = carProgressMap.OrderByDescending(kvp => kvp.Value.totalProgress).ToList();

        // Find player car in sorted list and update position display
        for (int i = 0; i < sortedCars.Count; i++)
        {
            if (sortedCars[i].Key == playerCar) // Check if current car is player car
            {
                positionText.text = "Pos: " + (i + 1); // Update position text (convert to 1-based index)
                break; // Exit loop once player position is found
            }
        }
    }

    // Class to store progress data for each car
    class CarProgress
    {
        public int lapCount = 0;
        public int currentWaypointIndex = 0;
        public float totalProgress = 0;
    }
}