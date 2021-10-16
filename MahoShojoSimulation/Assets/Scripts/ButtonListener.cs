using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleNameSpace;
public class ButtonListener : MonoBehaviour
{
    public Battle battleScript;
    public TrackPatternEnum inputShape;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => battleScript.RegisterTrack(inputShape));
        //gameObject.GetComponent<Button>().onClick.AddListener(() => Test());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
