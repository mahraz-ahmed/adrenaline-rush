using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    public float speed = 20f; // Speed at which missile moves forward

    private void Update()
    {
        // Move missile forward continuously based on its speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get root parent object of collider that was hit
        GameObject parentObject = other.transform.root.gameObject;

        // Check if object hit has tag "AI"
        if (parentObject.CompareTag("AI"))
        {
            Debug.Log("Missile Hit!");

            // Flip car in response to missile hit
            parentObject.transform.Rotate(Vector3.right * 180);

            // Get AIController component of hit car
            AIController controller = parentObject.GetComponent<AIController>();

            // Start coroutine to set missileHit to true for 3 secs (prevents teleport)
            StartCoroutine(HandleMissileHit(controller));

            // Disable missile upon impact
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Missile Missed!");
        }
    }

    private IEnumerator HandleMissileHit(AIController controller)
    {
        controller.missileHit = true; 
        yield return new WaitForSeconds(3f);
        controller.missileHit = false;
    }
}
