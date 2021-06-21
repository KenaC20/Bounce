using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }

    public Canvas preStartInfo;
    public Canvas postStartInfo;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        GameManager.Instance.GameStartEvent.AddListener(GameStart);
        GameManager.Instance.GameStopEvent.AddListener(GameStop);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameStart()
    {
        preStartInfo.gameObject.SetActive(false);
        postStartInfo.gameObject.SetActive(true);

    }
    void GameStop()
    {
        preStartInfo.gameObject.SetActive(true);
        postStartInfo.gameObject.SetActive(false);
    }
}
