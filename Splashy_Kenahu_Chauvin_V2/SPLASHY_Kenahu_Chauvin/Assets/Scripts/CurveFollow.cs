using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveFollow : MonoBehaviour
{
    public AnimationCurve Curve;
    public float RatioMultiply = 1f;

    public bool MoveX = false;
    public bool MoveY = false;
    public bool MoveZ = false;

    public bool _started = false;

    public float TimePassed = 0;
    Vector3 _originPosition;

    // Start is called before the first frame update
    void Start()
    {
        _originPosition = transform.position;

        GameManager.Instance.GameStartEvent.AddListener(GameStart);
        GameManager.Instance.GameStopEvent.AddListener(GameStop);
    }

    // Update is called once per frame
    void Update()
    {
        if (_started)
        {
            TimePassed += Time.deltaTime * GameManager.Instance.GetCheckSpeed();

            Vector3 newPosition = transform.position;

            if (MoveX)
                newPosition.x = _originPosition.x + (Curve.Evaluate(TimePassed) * RatioMultiply);
            if (MoveY)
                newPosition.y = _originPosition.y + (Curve.Evaluate(TimePassed) * RatioMultiply);
            if (MoveZ)
                newPosition.y = _originPosition.z + (Curve.Evaluate(TimePassed) * RatioMultiply);

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
