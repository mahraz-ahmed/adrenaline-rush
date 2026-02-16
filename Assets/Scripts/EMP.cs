using UnityEngine;
using System.Collections;

public class EMPBlast : MonoBehaviour
{
    private float radius = 6f;

    void Start()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in hitColliders)
        {
            GameObject parentObject = hit.transform.root.gameObject; // Get top-level parent

            if (parentObject.CompareTag("AI"))
            {
                Debug.Log("EMP Hit!");

                // Spin car in response to EMP hit
                float randomRotate = Random.Range(45f, 90f); // Random spin amount
                parentObject.transform.Rotate(Vector3.up * randomRotate);

                // Get AIController component of hit car
                AIController controller = parentObject.GetComponent<AIController>();

                // Start coroutine to set missileHit to true for 3 secs (prevents teleport)
                StartCoroutine(HandleMissileHit(controller));
            }
            else
            {
                Debug.Log("EMP Missed!");
            }
        }
    }

    private IEnumerator HandleMissileHit(AIController controller)
    {
        controller.missileHit = true;
        yield return new WaitForSeconds(3f);
        controller.missileHit = false;
    }
}