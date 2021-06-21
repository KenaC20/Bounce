using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float SpeedChecks;

    public GameObject checker;

    bool _isStart = false;

    public CurveFollow bouncePlayer;

    public ParticleSystem particuleOnBounce;

    public bool IsMultiply = false;
    private float _currentmultiplyTime = 0.0f;
    public float multiplyTime = 6f;

    public GameObject goMultiply;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameStartEvent.AddListener(GameStart);
        GameManager.Instance.GameStopEvent.AddListener(GameStop);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isStart)
        {
            Vector3 newPos = transform.position;

            if (GameControl.Instance.GetRatioX() != 0)
            {
                newPos.x = GameControl.Instance.GetPosX();
            }

            newPos.z += Time.deltaTime * PlatformManager.Instance.jumpSpaceSpawn * GameManager.Instance.GetCheckSpeed();

            transform.position = newPos;
        }

        if (IsMultiply)
        {
            _currentmultiplyTime += Time.deltaTime;

            if (_currentmultiplyTime >= multiplyTime)
            {
                MultiplyStop();
            }
        }
    }

    void GameStart()
    {
        _isStart = true;
    }
    void GameStop()
    {
        _isStart = false;
    }

    public void AlinePlayer(Vector3 alineWith)
    {
        if (bouncePlayer)
            bouncePlayer.TimePassed = 0;

        Vector3 newPos = transform.position;
        newPos.z = alineWith.z;
        transform.position = newPos;

        if (particuleOnBounce)
        {
            particuleOnBounce.gameObject.transform.position = transform.position;
            particuleOnBounce.Play();
        }
    }

    public void MultiplyStart()
    {
        if (goMultiply)
            goMultiply.SetActive(true);

        IsMultiply = true;
    }
    public void MultiplyStop()
    {
        IsMultiply = false;

        if (goMultiply)
            goMultiply.SetActive(false);

        _currentmultiplyTime = 0;
    }
}
