using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraColor : MonoBehaviour
{
    private Camera camera;
    private float elapsedTime = 0f;
    private float interval = .01f;
    [HideInInspector] public bool changeBGColor = false;
    Color32 originalColor;
    private Color[] brightColors = { Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta };
    void Awake()
    { 
        camera = GetComponent<Camera>();
        originalColor = camera.backgroundColor;
    }

    void Update()
    {
        if (changeBGColor)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= interval)
            {
                elapsedTime = 0;
                camera.backgroundColor = GetRandomBrightColor();
                StartCoroutine("SetFirstColor");
            }
        }
        
    }
    IEnumerator SetFirstColor()
    {
        yield return new WaitForSeconds(.02f);
        changeBGColor = false;
        camera.backgroundColor = originalColor;
        elapsedTime = 0;
    }
    Color GetRandomBrightColor()
    {
        //int randomIndex = Random.Range(0, brightColors.Length);
        return new Color(Random.Range(.5f, 0.6f), Random.Range(.5f, 0.6f), Random.Range(.5f, 0.6f));
        //return brightColors[randomIndex];
    }
}
