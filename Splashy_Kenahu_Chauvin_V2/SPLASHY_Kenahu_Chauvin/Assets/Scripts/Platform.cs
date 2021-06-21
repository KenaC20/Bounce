using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum landingResult
{
    NOT,
    VALID,
    DOUBLEPOINT,
    SPIKE,
    MULTIPLY,
}

public class Platform : MonoBehaviour
{
    public float GapeSize = 2f;

    public PlatformGroup GroupOwner = null;

    public GameObject DoublePoint;
    public GameObject Spicke;
    public GameObject Multiply;
    public float GapeSizeDoublePoint = 0.5f;
    public float GapeSizeSpicke = 0.5f;
    public float GapeSizeMultiply = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public landingResult CheckLanding(Vector3 posToCheck)
    {
        if (DoublePoint.activeSelf)
        {
            if (posToCheck.x >= DoublePoint.transform.position.x - (GapeSizeDoublePoint / 2) && posToCheck.x <= DoublePoint.transform.position.x + (GapeSizeDoublePoint / 2))
            {
                return landingResult.DOUBLEPOINT;
            }
        }
        if (Spicke.activeSelf)
        {
            if (posToCheck.x >= Spicke.transform.position.x - (GapeSizeSpicke / 2) && posToCheck.x <= Spicke.transform.position.x + (GapeSizeSpicke / 2))
            {
                return landingResult.SPIKE;
            }
        }
        if (Multiply.activeSelf)
        {
            if (posToCheck.x >= Multiply.transform.position.x - (GapeSizeMultiply / 2) && posToCheck.x <= Multiply.transform.position.x + (GapeSizeMultiply / 2))
            {
                return landingResult.MULTIPLY;
            }
        }
        if (posToCheck.x >= transform.position.x - (GapeSize / 2) && posToCheck.x <= transform.position.x + (GapeSize / 2))
        {
            return landingResult.VALID;
        }

        return landingResult.NOT;
    }

    public void SetBonus(bool positiveBonus = true)
    {
        if(positiveBonus)
        {
            int rand = Random.Range(0, 3);

            if (rand > 0 || GameControl.Instance.playerMov.IsMultiply)
            {
                DoublePoint.transform.position = new Vector3(transform.position.x + Random.Range(0f, GapeSize) - (GapeSize / 2), 0, transform.position.z);
                DoublePoint.SetActive(true);
            }
            else if (rand == 0)
            {
                Multiply.transform.position = new Vector3(transform.position.x + Random.Range(0f, GapeSize) - (GapeSize / 2), 0, transform.position.z);
                Multiply.SetActive(true);
            }
        }
        else
        {
            Spicke.transform.position = new Vector3(transform.position.x + Random.Range(0f, GapeSize) - (GapeSize / 2), 0, transform.position.z);
            Spicke.SetActive(true);
        }
    }

    public void ResetBonus()
    {
        DoublePoint.SetActive(false);
        Spicke.SetActive(false);
        Multiply.SetActive(false);
    }

}
