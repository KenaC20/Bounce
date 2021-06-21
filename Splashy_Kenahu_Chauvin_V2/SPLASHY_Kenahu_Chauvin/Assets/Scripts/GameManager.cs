using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public enum spawnStateNumber
{
    SIMPLE,
    DOUBLE,
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    public spawnStateNumber spawnState = spawnStateNumber.SIMPLE;

    private float _nextTimeCheck = 0.0f;
    public float checkSpeed = 2f;
    public float GetCheckSpeed()
    {
        return checkSpeed;
    }

    public bool GameStart = false;
    public bool isDead = false;

    public double GameScore = 0;
    public Text TextScore;

    public Vector3 OffsetText = new Vector3(-1, 1, 0);

    public UnityEvent GameStartEvent;
    public UnityEvent GameStopEvent;

    public PlatformPatern[] paternChallenge;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        if (GameStartEvent == null)
            GameStartEvent = new UnityEvent();
        if (GameStopEvent == null)
            GameStopEvent = new UnityEvent();

        GameStartEvent.AddListener(OnStartGame);
        GameStopEvent.AddListener(OnStopGame);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart)
        {
            _nextTimeCheck += Time.deltaTime * checkSpeed;
            if (_nextTimeCheck >= 1f)
            {
                _nextTimeCheck = 0;

                int scoreADD = PlatformManager.Instance.CheckPlatform(GameControl.Instance.playerMov.transform.position);
                
                if (scoreADD > 0)
                {
                    GameScore += scoreADD;
                    if (TextScore)
                        TextScore.text = GameScore.ToString();

                    if (scoreADD == 1)
                        FloatingTextManager.Instance.CreatFloatingText(GameControl.Instance.playerMov.transform.position + OffsetText, "+" + scoreADD.ToString());
                    else
                        FloatingTextManager.Instance.CreatFloatingText(GameControl.Instance.playerMov.transform.position + OffsetText, "Great!\n +" + scoreADD.ToString());


                    if (GameScore >= 1)
                        spawnState = spawnStateNumber.DOUBLE;
                    if (GameScore % 15 == 0 && PlatformManager.Instance.currentPatern == null)
                        PlatformManager.Instance.currentPatern = paternChallenge[Random.Range(0, paternChallenge.Length)];

                }
                else
                {
                    FloatingTextManager.Instance.CreatFloatingText(GameControl.Instance.playerMov.transform.position + OffsetText, "X");
                    GameStopEvent.Invoke();
                    isDead = true;

                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }


                PlatformManager.Instance.SpawnPlatform();
            }
        }
    }

    void OnStartGame()
    {
        GameStart = true;
    }
    void OnStopGame()
    {
        GameStart = false;
    }
}