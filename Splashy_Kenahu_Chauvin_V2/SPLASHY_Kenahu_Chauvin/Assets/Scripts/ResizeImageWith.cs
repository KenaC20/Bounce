using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeImageWith : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 size = GetComponent<RectTransform>().sizeDelta;
        size.x = Screen.width;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
