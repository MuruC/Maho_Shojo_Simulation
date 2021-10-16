using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointReact : MonoBehaviour
{
    public CheckTrial checkTrial;
    public bool isTriggered = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isTriggered)//other is left or right hand
        {
            checkTrial.numOfChecked++;
            isTriggered = true;
        }
    }

    public void ResetIsTriggerd()
    {
        this.isTriggered = false;
    }
}
