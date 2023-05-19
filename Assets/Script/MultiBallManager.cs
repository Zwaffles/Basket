using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MultiBallManager : MonoBehaviour
{
    [SerializeField] GameObject[] balls;
    void Start()
    {
        StartCoroutine("RespawnBalls");
    }

    IEnumerator RespawnBalls()
    {
        yield return new WaitForSeconds(.4f);
        balls[0].SetActive(true);
        yield return new WaitForSeconds(.5f);
        balls[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        balls[2].SetActive(true);
        yield return new WaitForSeconds(.0f);
        balls[3].SetActive(true);
        yield return new WaitForSeconds(1.2f);
        balls[4].SetActive(true);
        yield return new WaitForSeconds(.0f);
        balls[5].SetActive(true);
        yield return new WaitForSeconds(.7f);
        balls[6].SetActive(true);
        yield return new WaitForSeconds(.0f);
        balls[7].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        balls[8].SetActive(true);
        yield return new WaitForSeconds(.0f);
        balls[9].SetActive(true);
        yield return new WaitForSeconds(1.2f);
        balls[10].SetActive(true);
        yield return new WaitForSeconds(.8f);
        balls[11].SetActive(true);
        yield return new WaitForSeconds(0f);
        balls[12].SetActive(true);
        yield return new WaitForSeconds(.9f);
        balls[13].SetActive(true);
        yield return new WaitForSeconds(0f);
        balls[14].SetActive(true);
    }
}
