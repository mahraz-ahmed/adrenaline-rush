using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public enum PowerUpType {Missile, EMP, Boost} // Define power-ups
    private PowerUpType currentPowerUp; // Store current power-up
    public GameObject missilePrefab; 
    public GameObject empPrefab; 
    public Transform firePoint; // Position where missiles spawn
    public TextMeshProUGUI powerUpText;
    private bool hasPowerUp = false; // Check if player has a power-up


    void Update()
    {
        // Activate power-up upon spacebar press
        if (hasPowerUp && Input.GetKeyDown(KeyCode.Space))
        {
            ActivatePowerUp(); 
        }
    }

    public void GiveRandomPowerUp()
    {
        // Generate random number between 0 and 3 (upper bound 3 is exclusive, enum is 0-indexed)
        int randomIndex = Random.Range(0, 3);
        currentPowerUp = (PowerUpType)randomIndex; // Convert number into corresponding enum value
        hasPowerUp = true; // Indicate player has power-up
        powerUpText.text = ("Power Up: " + currentPowerUp.ToString()); // Relay to user on UI
        Debug.Log("Got power-up: " + currentPowerUp);
    }

    void ActivatePowerUp()
    {
        // Activate corresponding power-up based on currentPowerUp value
        switch (currentPowerUp)
        {
            case PowerUpType.Missile:
                FireMissile();
                break;
            case PowerUpType.EMP:
                ActivateEMP();
                break;
            case PowerUpType.Boost:
                StartCoroutine(BoostSpeed());
                break;
        }
        hasPowerUp = false; // Reset power-up flag after use
        powerUpText.text = ("Power Up: none"); // Relay to user on UI
    }

    IEnumerator BoostSpeed()
    {
        Debug.Log("BOOST APPLIED!");
        Rigidbody rb = GetComponent<Rigidbody>(); // Get player car's Rigidbody component

        Vector3 originalVelocity = rb.linearVelocity; // Store original velocity of car

        float boostForce = 10f; // Apply forward boost force
        float duration = 1f; // Quick burst effect that lasts 1 sec
        float elapsed = 0f;

        while (elapsed < duration) // While boost is active
        {
            // Apply forward force to car's Rigidbody
            rb.AddForce(transform.forward * boostForce, ForceMode.Acceleration);

            // Increment boost elapsed time
            elapsed += Time.deltaTime;

            // Wait for next frame
            yield return null;
        }
        rb.linearVelocity = originalVelocity; // Return to original velocity, prevent loss of control
    }

    void FireMissile()
    {
        // Spawn a missile 
        Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
    }

    void ActivateEMP()
    {
        // Instantiate EMP effect at car's position
        GameObject empInstance = Instantiate(empPrefab, transform.position, Quaternion.identity);

        // Attach EMP to car so it moves with it
        empInstance.transform.SetParent(transform);

        // Destroy EMP effect after 2 seconds
        Destroy(empInstance, 2f);
    }
}
