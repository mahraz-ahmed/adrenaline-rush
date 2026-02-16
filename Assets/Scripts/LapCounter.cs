using UnityEngine;

public class LapCounter : MonoBehaviour
{
    public static float lapCount = 0;

    // Check if car has collided with lap checker
    private void OnTriggerEnter(Collider other)
    {
        GameObject parentObject = other.transform.root.gameObject; // Get top-level parent

        if (parentObject.CompareTag("Car")) // Check for object with tag 'Car'
        {
            lapCount++; // Increment lap counter
            Debug.Log("Lap incremented! Total laps: " + lapCount);
        }
    }

}
