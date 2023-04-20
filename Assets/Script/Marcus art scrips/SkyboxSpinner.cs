using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Skybox))]
public class SkyboxSpinner : MonoBehaviour
{

    public float speed;

// Update is called once per frame
	void Update () 
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speed);

	}
}
