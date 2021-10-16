using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject winUI;
    public GameObject loseUI;
    public GameObject titleUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetWin()
    {
        winUI.SetActive(true);
        loseUI.SetActive(false);
    }

    public void SetLose()
    {
        winUI.SetActive(false);
        loseUI.SetActive(true);
    }


}
