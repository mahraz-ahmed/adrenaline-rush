using UnityEngine;

public class CarAudio : MonoBehaviour
{
    public AudioSource engineIdleAudio;  // Idle engine hum
    public AudioSource engineRevAudio;   // Accel & decel sound
    public AudioSource crashAudio;       // Crash sound effect

    public Rigidbody carRigidbody;       // Reference to the car's Rigidbody
    private float maxPitch = 5.0f;        // Maximum pitch for accel
    private float minPitch = 0.1f;        // Minimum pitch when decel

    private float speed;                 // Current speed of car

    void Update()
    {
        AdjustEngineSound();
    }

    void AdjustEngineSound()
    {
        // Get car's speed from Rigidbody
        speed = carRigidbody.linearVelocity.magnitude;

        // Adjust pitch of the engine rev sound based on speed
        float newPitch = Mathf.Lerp(minPitch, maxPitch, speed / 50f); // Adjust based on max speed
        engineRevAudio.pitch = newPitch;

        // Ensure engine rev sound plays when moving
        if (speed > 0.1f && !engineRevAudio.isPlaying)
        {
            engineRevAudio.Play();
        }
        else if (speed <= 0.1f && engineRevAudio.isPlaying)
        {
            engineRevAudio.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Play crash sound when colliding with an object at high speed
        if (collision.relativeVelocity.magnitude > 5f)
        {
            crashAudio.Play();
        }
    }
}
