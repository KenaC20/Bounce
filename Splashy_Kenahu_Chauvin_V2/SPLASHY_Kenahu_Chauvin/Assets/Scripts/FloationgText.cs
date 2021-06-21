using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloationgText : MonoBehaviour
{
    private float _currentLifeTime = 0.0f;
    public float LifeTime = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentLifeTime += Time.deltaTime;
        transform.position += new Vector3(0, Time.deltaTime, 0);
        if (_currentLifeTime >= LifeTime)
        {
            FloatingTextManager.Instance.ReturnToPoolText(this.gameObject);
        }
    }

    private void OnEnable()
    {
        _currentLifeTime = 0.0f;
    }
}
