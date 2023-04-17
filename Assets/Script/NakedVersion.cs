using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NakedVersion : MonoBehaviour
{
    [SerializeField] GameObject[] enviromentAssets;
    [SerializeField] GameObject[] basicAssets;
    bool isShown = false;
    public void HideShowAssets()
    {
        if (isShown)
        {
            isShown = false;
            foreach (GameObject asset in basicAssets)
            {
                asset.GetComponent<MeshRenderer>().enabled = true;
            }
            foreach (GameObject asset in enviromentAssets)
            {
                asset.SetActive(false);
            }
            
        }

        else if (!isShown)
        {
            isShown = true;
            foreach (GameObject asset in basicAssets)
            {
                asset.GetComponent<MeshRenderer>().enabled = false;
            }
            foreach (GameObject asset in enviromentAssets)
            {
                asset.SetActive(true);
            }
            
        }
    }
}
