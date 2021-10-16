using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleNameSpace;
public class CheckTrial : MonoBehaviour
{
    public Transform magicPoint;
    public Battle battleScript;
    public GameObject checkPointsParent;
    public GameObject magicCircle;
    public GameObject magicTriangle;
    public GameObject magicRectangle;
    public GameObject checkPointsCircleParent;
    public GameObject checkPointsTriangleParent;
    public GameObject checkPointsRectangleParent;
    public List<GameObject> checkPoints;
    public TrackPatternEnum curMagicType = TrackPatternEnum.none;
    int numOfCheckPoints = 0;
    public int numOfChecked = 0;

    public float checkTrueTime = 3.0f;
    //float timeCounter = 0.0f;
    public bool gameContinue = true;
    public bool bShifa = false;
    private TrackPatternEnum voiceType = TrackPatternEnum.none;
    public GameObject shifa;
    public GameObject zhuizong;
    // Start is called before the first frame update
    void Start()
    {
        //SetCheckPoints(checkPointsParent);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameContinue)
        {
            //timeCounter += Time.deltaTime;
            if (numOfChecked >= numOfCheckPoints * 0.95f)
            {
                GenerateMagic();
                //GoToNextMagic();
            }
            //else
            //{
            //    if (timeCounter >= checkTrueTime)
            //    {
            //        gameContinue = false;
            //    }
            //}
        }
        //else
        //{
        //    Debug.Log("You lose.");//lose
        //}
    }

    void SetCheckPoints(GameObject cpp)
    {
        checkPointsParent = cpp;
        SetNumOfCheckPoint();
        for (int i = 0; i < numOfCheckPoints; i++)
        {
            checkPoints.Add(checkPointsParent.transform.GetChild(i).gameObject);
        }
        foreach (var cp in checkPoints)
        {
            cp.GetComponent<CheckPointReact>().checkTrial = this.GetComponent<CheckTrial>();
        }
    }

    void SetNumOfCheckPoint()
    {
        numOfCheckPoints = checkPointsParent.transform.childCount;
        numOfChecked = 0;
        //timeCounter = 0.0f;
    }

    void GenerateMagic()
    {
        GameObject tempMagic;
        if (curMagicType == TrackPatternEnum.circle)
        {
            tempMagic = Instantiate(magicCircle, magicPoint.transform);
        }
        else if (curMagicType == TrackPatternEnum.triangle)
        {
            tempMagic = Instantiate(magicTriangle, magicPoint.transform);
        }
        else if (curMagicType == TrackPatternEnum.rectangle)
        {
            tempMagic = Instantiate(magicRectangle, magicPoint.transform);
        }
        else
        {
            tempMagic = null;
        }
        tempMagic.transform.SetParent(null);
        Destroy(tempMagic, 7.001f);
        GoToNextMagic();
        battleScript.RegisterTrack(curMagicType);
    }

    void GoToNextMagic()
    {
        //timeCounter = 0.0f;
        foreach (var go in checkPoints)
        {
            go.GetComponent<CheckPointReact>().ResetIsTriggerd();
        }
        gameContinue = false;
        //set next checkPointsParent
    }

    public void SetCheckPointsParent()
    {
        if (curMagicType == TrackPatternEnum.circle)
        {
            checkPointsCircleParent.SetActive(true);
            checkPointsTriangleParent.SetActive(false);
            checkPointsRectangleParent.SetActive(false);
            checkPointsParent = checkPointsCircleParent;
        }
        else if (curMagicType == TrackPatternEnum.triangle)
        {
            checkPointsCircleParent.SetActive(false);
            checkPointsTriangleParent.SetActive(true);
            checkPointsRectangleParent.SetActive(false);
            checkPointsParent = checkPointsTriangleParent;
        }
        else if (curMagicType == TrackPatternEnum.rectangle)
        {
            checkPointsCircleParent.SetActive(false);
            checkPointsTriangleParent.SetActive(false);
            checkPointsRectangleParent.SetActive(true);
            checkPointsParent = checkPointsRectangleParent;
        }
    }

    public void ResetCheckPointParent()
    {
        checkPointsCircleParent.SetActive(false);
        checkPointsTriangleParent.SetActive(false);
        checkPointsRectangleParent.SetActive(false);
    }

    public void SetCurMagicType(TrackPatternEnum trackPatternEnum)
    {
        curMagicType = trackPatternEnum;
        gameContinue = true;
        voiceType = TrackPatternEnum.none;
        SetCheckPointsParent();
        SetCheckPoints(checkPointsParent);
        SetNumOfCheckPoint();
    }

    public void SetCircleShifa()
    {
        voiceType = TrackPatternEnum.circle;
        Debug.Log("circle voice: " + voiceType);
    }
    public void SetTriangleShifa()
    {
        voiceType = TrackPatternEnum.triangle;
        Debug.Log("triangle voiceType" + voiceType);
    }
    public void SetRectangleShifa()
    {
        voiceType = TrackPatternEnum.rectangle;
        Debug.Log("rectangle voiceType" + voiceType);
    }
}
