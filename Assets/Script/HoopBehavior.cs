using UnityEngine;

public class HoopBehavior : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, _collider, true);
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, _collider, false);
    }
}