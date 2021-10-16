using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOGODisappear : MonoBehaviour
{
    public float waitTime = 5.0f;
    public bool startTrans = false;
    public float transTime = 5.0f;

    float timeCounter = 0.0f;
    public MeshRenderer[] allRenders;
    public List<Color> originCs;
    public float finalA = 0.3f;
    bool finishTrans = false;
    // Start is called before the first frame update
    void Start()
    {
        allRenders = this.GetComponentsInChildren<MeshRenderer>();
        foreach (var ren in allRenders)
        {
            originCs.Add(ren.material.color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!startTrans)
        {
            if (timeCounter <= waitTime)
            {
                timeCounter += Time.deltaTime;
            }
            else
            {
                startTrans = true;
                timeCounter = 0.0f;
            }
        }
        if (startTrans && !finishTrans)
        {
            timeCounter += Time.deltaTime;
            for (int i = 0; i < originCs.Count; i++)
            {
                Color tempColor = originCs[i];
                tempColor.a = Mathf.Lerp(originCs[i].a, finalA, timeCounter / transTime);
                allRenders[i].material.color = tempColor;
            }
            if (timeCounter >= transTime)
            {
                Destroy(this.gameObject);
                finishTrans = true;
            }
        }
    }
}
