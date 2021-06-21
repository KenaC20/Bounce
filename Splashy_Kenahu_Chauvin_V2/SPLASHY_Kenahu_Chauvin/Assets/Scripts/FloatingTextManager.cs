using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    private static FloatingTextManager _instance;
    public static FloatingTextManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FloatingTextManager();
            }
            return _instance;
        }
    }

    public GameObject FloatingTextPrefab;
    private Stack<GameObject> _FTextPool = new Stack<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        _instance = this;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatFloatingTexts(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            _FTextPool.Push(Instantiate(FloatingTextPrefab));
            _FTextPool.Peek().SetActive(false);
        }
    }

    public void CreatFloatingText(Vector3 position, string textToShow)
    {
        if (_FTextPool.Count == 0)
        {
            CreatFloatingTexts(2);
        }

        GameObject initText = _FTextPool.Pop();
        initText.transform.position = position;
        initText.transform.rotation = Quaternion.identity;

        initText.GetComponent<TextMesh>().text = textToShow;

        initText.SetActive(true);
    }

    public void ReturnToPoolText(GameObject gObjectText)
    {
        gObjectText.SetActive(false);
        _FTextPool.Push(gObjectText);
    }
}
