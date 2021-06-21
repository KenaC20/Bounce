using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    private static GameControl _instance;
    public static GameControl Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameControl();
            }
            return _instance;
        }
    }

    public PlayerMovement playerMov;

    Vector2 _screenSize;

    Vector3 _originPlayerCheckerPos;
    Vector2 _inputOriginPosition;
    public Vector2 GetInputOriginPosition()
    {
        return _inputOriginPosition;
    }

    Vector2 _inputPosition;
    public Vector2 GetInputPosition()
    {
        return _inputPosition;
    }
    public float GetRatioX()
    {
        return ((_inputPosition.x / _screenSize.x) * 2) - 1;
    }
    public float GetPosX()
    {
        return _originPlayerCheckerPos.x + (((_inputPosition.x - _inputOriginPosition.x) / _screenSize.x) * 2 * (PlatformManager.Instance.widthSpawn + 2));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        _screenSize = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                if (GameManager.Instance.isDead)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                CheckStartGame();

                _inputOriginPosition = Input.touches[0].position;
                _originPlayerCheckerPos = playerMov.checker.transform.position;
            }
            _inputPosition = Input.touches[0].position;
        }

        else if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(GameManager.Instance.isDead)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                CheckStartGame();

                _inputOriginPosition = Input.mousePosition;
                _originPlayerCheckerPos = playerMov.checker.transform.position;
            }
            _inputPosition = Input.mousePosition;
        }
    }

    void CheckStartGame()
    {
        if (GameManager.Instance.GameStart == false)
        {
            GameManager.Instance.GameStartEvent.Invoke();
        }
    }
}
