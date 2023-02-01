using UnityEngine;

public class CollisionParticles : MonoBehaviour
{
    public ParticleSystem particleSystem;
    private float particleDuration = 2f;

    void OnTriggerEnter(Collider other)
    {
        particleSystem.Play();
        Invoke("StopParticles", particleDuration);
    }

    void StopParticles()
    {
        particleSystem.Stop();
    }
}
