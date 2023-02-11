using System.Collections;
using UnityEngine;

public class AutoPushCornerBall : MonoBehaviour
{
    [Header("Bouncer Settings")]
    [Tooltip("Tag of the object that will trigger the bouncer.")]
    public string triggerTag;
    [Tooltip("Speed of the bouncer animation.")]
    public float speed = 1f;
    [Tooltip("Distance that the bouncer will move up and down.")]
    public float distance = 1f;
    private Vector3 startPos;

    bool isPushing;

    private void Start()
    {
        startPos = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == triggerTag)
        {
            isPushing = true;
            StartCoroutine(BounceUpAndDown());
        }
    }

    IEnumerator BounceUpAndDown()
    {
        while (isPushing)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                transform.position = new Vector3(startPos.x, Mathf.SmoothStep(startPos.y, startPos.y + distance, t), startPos.z);
                yield return null;
            }
            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                transform.position = new Vector3(startPos.x, Mathf.SmoothStep(startPos.y + distance, startPos.y, t), startPos.z);
                yield return null;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == triggerTag)
        {
            isPushing = false;
            StopCoroutine(BounceUpAndDown());
            transform.position = startPos;
        }
    }
    }
