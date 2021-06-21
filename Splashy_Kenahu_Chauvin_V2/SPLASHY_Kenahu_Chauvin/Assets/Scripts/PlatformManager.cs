using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    private static PlatformManager _instance;
    public static PlatformManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlatformManager();
            }
            return _instance;
        }
    }

    public GameObject platformPrefab;
    public GameObject currentPlatform;

    private Stack<GameObject> _platformPool = new Stack<GameObject>();

    public float widthSpawn = 5f;
    public float jumpSpaceSpawn = 2f;
    public float spawnRange = 2f;
    public float spawnRangeDouble = 5f;
    public Vector3 OffsetFallDestination = Vector3.zero;
    public float FallSpeed = 1f;

    List<PlatformGroup> _platformGroupList = new List<PlatformGroup>();


    int _raowIndex = 10;
    public PlatformPatern currentPatern = null;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        for (int i = 0; i < 10; i++)
        {
            SpawnPlatform();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatPlatforms(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            _platformPool.Push(Instantiate(platformPrefab));
            _platformPool.Peek().SetActive(false);
        }
    }

    public GameObject PopPlatform(Vector3 toPosition)
    {
        if (_platformPool.Count < 1)
        {
            CreatPlatforms(1);
        }

        GameObject newPlat = _platformPool.Pop();
        newPlat.transform.position = toPosition;
        newPlat.transform.rotation = Quaternion.identity;
        newPlat.SetActive(true);
        return newPlat;
    }

    public void  PushPlatform(GameObject toPushGO)
    {
        toPushGO.SetActive(false);
        _platformPool.Push(toPushGO);
    }

    public void SpawnPlatform()
    {
        if(currentPatern)
        {
            if (_raowIndex > -1)
            {
                PlatformGroup newGroup = GetGroupPlatform_ROW(currentPatern.Patern.rows[_raowIndex].row);

                if (newGroup.PlatformList.Count > 0)
                {
                    _platformGroupList.Add(newGroup);
                    _raowIndex--;
                }
                else
                {
                    currentPatern = null;
                    _raowIndex = 10;
                }
            }
            else
            {
                currentPatern = null;
                _raowIndex = 10;
            }
        }

        else if (GameManager.Instance.spawnState == spawnStateNumber.SIMPLE)
        {
            Vector3 nextPlatPos = GetNextPositionPlatform(currentPlatform.transform.position);

            GameObject initPlat = PopPlatform(nextPlatPos);

            //RollBonusPlatform(initPlat);

            currentPlatform = initPlat;

            PlatformGroup newGroup = new PlatformGroup();

            newGroup.PlatformList.Add(currentPlatform);
            currentPlatform.GetComponent<Platform>().GroupOwner = newGroup;

            _platformGroupList.Add(newGroup);
        }

        else if (GameManager.Instance.spawnState == spawnStateNumber.DOUBLE)
        {
            Vector3 nextPlatPos = GetNextPositionPlatform_DOUBLE(currentPlatform);
            GameObject initPlat = PopPlatform(nextPlatPos);

            Vector3 nextPlatPos2 = GetNextPositionPlatform_DOUBLE(initPlat, true);
            GameObject initPlat2 = PopPlatform(nextPlatPos2);

            RollBonusPlatform(initPlat);
            RollBonusPlatform(initPlat2);

            currentPlatform = initPlat;

            PlatformGroup newGroup = new PlatformGroup();

            newGroup.PlatformList.Add(initPlat);
            newGroup.PlatformList.Add(initPlat2);

            initPlat.GetComponent<Platform>().GroupOwner = newGroup;
            initPlat2.GetComponent<Platform>().GroupOwner = newGroup;

            _platformGroupList.Add(newGroup);
        }

        if (_platformGroupList.Count < 10)
            SpawnPlatform();
    }

    Vector3 GetNextPositionPlatform(Vector3 currentPlatformPos)
    {
        Vector3 nextPlatPos = currentPlatformPos;

        int offset = (Random.Range(0, (int)spawnRange + 1) - (int)(spawnRange / 2));

        nextPlatPos.x = currentPlatformPos.x + offset;

        if (widthSpawn < nextPlatPos.x)
            nextPlatPos.x = currentPlatformPos.x - (offset * 2);
        else if (-widthSpawn > nextPlatPos.x)
            nextPlatPos.x = currentPlatformPos.x - (offset * 2);

        nextPlatPos.z += jumpSpaceSpawn;

        return nextPlatPos;
    }

    Vector3 GetNextPositionPlatform_DOUBLE(GameObject currentPlatformPos, bool isOtherPlat = false)
    {
        Vector3 nextPlatPos = currentPlatformPos.transform.position;
        float gapePlat = currentPlatformPos.GetComponent<Platform>().GapeSize;

        int offset = (Random.Range(0, (int)spawnRangeDouble + 1) - (int)(spawnRangeDouble / 2));

        if(offset >= 0)
            nextPlatPos.x = currentPlatformPos.transform.position.x + offset + gapePlat;
        else
            nextPlatPos.x = currentPlatformPos.transform.position.x + offset - gapePlat;

        if (widthSpawn < nextPlatPos.x)
            nextPlatPos.x = currentPlatformPos.transform.position.x - (offset * 2) - gapePlat;
        else if (-widthSpawn > nextPlatPos.x)
            nextPlatPos.x = currentPlatformPos.transform.position.x - (offset * 2) + gapePlat;

        if (!isOtherPlat)
            nextPlatPos.z += jumpSpaceSpawn;

        return nextPlatPos;
    }

    PlatformGroup GetGroupPlatform_ROW(bool[] boolRow)
    {
        PlatformGroup newGroup = new PlatformGroup();

        Vector3 basePos = currentPlatform.transform.position;
        basePos.z += jumpSpaceSpawn;
        basePos.x = -widthSpawn;

        foreach (bool bPlat in boolRow)
        {
            if(bPlat)
            {
                GameObject newPlat = PopPlatform(basePos);
                newPlat.GetComponent<Platform>().GroupOwner = newGroup;
                newGroup.PlatformList.Add(newPlat);
                currentPlatform = newPlat;

                RollBonusPlatform(newPlat);
            }

            basePos.x++;
        }

        return newGroup;
    }

    public void RollBonusPlatform(GameObject bonusPlat)
    {
        int bonusR = Random.Range(0, 3);
        if(bonusR == 0)
        {
            bonusPlat.GetComponent<Platform>().SetBonus();
            return;
        }
        bonusR = Random.Range(0, 8);
        if (bonusR == 0)
        {
            bonusPlat.GetComponent<Platform>().SetBonus(false);
            return;
        }
    }

    public int CheckPlatform(Vector3 positionP)
    {
        float gape;
        foreach (GameObject go_Platform in _platformGroupList[0].PlatformList)
        {
            gape = go_Platform.GetComponent<Platform>().GapeSize;

            landingResult result = go_Platform.GetComponent<Platform>().CheckLanding(positionP);

            if (result == landingResult.DOUBLEPOINT)
            {
                GameControl.Instance.playerMov.AlinePlayer(go_Platform.transform.position);
                StartCoroutine(DisaperPlatform(go_Platform));

                if (GameControl.Instance.playerMov.IsMultiply)
                    return _platformGroupList[0].PlatformList.Count * 2;

                return 2;
            }
            else if (result == landingResult.VALID)
            {
                GameControl.Instance.playerMov.AlinePlayer(go_Platform.transform.position);
                StartCoroutine(DisaperPlatform(go_Platform));

                if (GameControl.Instance.playerMov.IsMultiply)
                    return _platformGroupList[0].PlatformList.Count;

                return 1;
            }
            else if (result == landingResult.MULTIPLY)
            {
                GameControl.Instance.playerMov.AlinePlayer(go_Platform.transform.position);
                GameControl.Instance.playerMov.MultiplyStart();
                StartCoroutine(DisaperPlatform(go_Platform));
                return 1;
            }

        }

        //return 0;

        PlatformGroup DelGroup = _platformGroupList[0];

        if (_platformGroupList.Contains(DelGroup))
            _platformGroupList.Remove(DelGroup);


        foreach (GameObject go_Platform in DelGroup.PlatformList)
        {
            PushPlatform(go_Platform);
            go_Platform.GetComponent<Platform>().GroupOwner = null;
        }

        return 0;
    }

    IEnumerator DisaperPlatform(GameObject platformFall)
    {
        PlatformGroup DelGroup = platformFall.GetComponent<Platform>().GroupOwner;

        if (_platformGroupList.Contains(DelGroup))
            _platformGroupList.Remove(DelGroup);

        Vector3 fallDest = platformFall.transform.position + OffsetFallDestination;
        Vector3 cPos = platformFall.transform.position;

        while (Vector3.Distance(platformFall.transform.position, fallDest) > 0.05f)
        {
            platformFall.transform.position = Vector3.Lerp(platformFall.transform.position, fallDest, FallSpeed * Time.deltaTime);

            yield return null;
        }

        foreach (GameObject go_Platform in DelGroup.PlatformList)
        {
            PushPlatform(go_Platform);
            go_Platform.GetComponent<Platform>().GroupOwner = null;
            go_Platform.GetComponent<Platform>().ResetBonus();
        }
    }

}