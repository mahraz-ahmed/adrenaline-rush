using UnityEngine;

public class PowerUpTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject parentObject = other.transform.root.gameObject; // Get top-level parent object

        if (parentObject.CompareTag("Car")) // Check if object has "Car" tag (indicates player car)
        {
            PowerUpManager powerUpManager = parentObject.GetComponent<PowerUpManager>();
            powerUpManager.GiveRandomPowerUp(); // Give random power-up to player

            Debug.Log("Player car detected!");
            gameObject.SetActive(false); // Temp disable power-up orb
            Invoke(nameof(Respawn), 3f); // Call Respawn after 3 secs
        }
        else
        {
            Debug.Log("Not player car.");
        }
    }

    private void Respawn()
    {
        gameObject.SetActive(true); // Reactivate power-up orb
    }
}
