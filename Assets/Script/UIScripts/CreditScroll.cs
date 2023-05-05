using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditScroll : MonoBehaviour
{
    private VisualElement root;

    [SerializeField, Header("Scroll"), Tooltip("How fast the text should scroll."), Range(0f, 1000f)]
    private float scrollSpeed = 100f;
    [SerializeField, Tooltip("How far the text should scroll."), Range(0f, 10000f)]
    private float maxScroll = 2150f;

    private ListView listView;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        listView = root.Q<ListView>("Credits");
    }

    private void Update()
    {

        if (Mathf.Abs(listView.transform.position.y) >= maxScroll) return;

        listView.transform.position = new Vector3(
            0f,
            listView.transform.position.y - scrollSpeed * Time.deltaTime,
            0f
        );

    }

}
