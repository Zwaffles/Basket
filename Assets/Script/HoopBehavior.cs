using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoopBehavior : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine("ActivateCollider", other);
    }

    IEnumerator ActivateCollider(Collider other)
    {
        yield return new WaitForSeconds(2f);
        Physics.IgnoreCollision(other, _collider, false);
    }

}