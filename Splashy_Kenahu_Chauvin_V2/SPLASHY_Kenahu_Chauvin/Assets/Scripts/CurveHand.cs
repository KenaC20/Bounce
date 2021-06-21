using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveHand : MonoBehaviour
{
    public AnimationCurve Curve;
    float RatioMultiply = 1f;
    public float Speed = 1f;
    
    public bool _started = false;

    float _timePassed = 0;
    Vector3 _originPosition;

    // Start is called before the first frame update
    void Start()
    {
        RatioMultiply = Screen.width - 80f;

        _originPosition = transform.position + new Vector3(40f, 0, 0);

        _started = true;

        GameManager.Instance.GameStartEvent.AddListener(GameStop);
        GameManager.Instance.GameStopEvent.AddListener(GameStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (_started)
        {
            _timePassed += Time.deltaTime * Speed;

            Vector3 newPosition = transform.position;

            newPosition.x = _originPosition.x + (Curve.Evaluate(_timePassed) * RatioMultiply);

            transform.position = newPosition;

        }
    }

    void GameStart()
    {
        _started = true;
    }
    void GameStop()
    {
        _started = false;
    }
}
