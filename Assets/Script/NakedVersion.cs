using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NakedVersion : MonoBehaviour
{
    [SerializeField] GameObject[] nakedVersion;
    [SerializeField] GameObject[] clothedVersion;
    bool isShown = false;
    public void HideShowAssets()
    {
        if (isShown)
        {
            isShown = false;
            //foreach (GameObject asset in clothedVersion)
            //{
            //    asset.GetComponent<MeshRenderer>().enabled = true;
            //}
            foreach (GameObject asset in nakedVersion)
            {
                asset.SetActive(false);
            }
            foreach (GameObject asset in clothedVersion)
            {
                asset.SetActive(true);
            }

        }

        else if (!isShown)
        {
            isShown = true;
            //foreach (GameObject asset in clothedVersion)
            //{
            //    asset.GetComponent<MeshRenderer>().enabled = false;
            //}
            foreach (GameObject asset in nakedVersion)
            {
                asset.SetActive(true);
            }
            foreach (GameObject asset in clothedVersion)
            {
                asset.SetActive(false);
            }

        }
    }
}
