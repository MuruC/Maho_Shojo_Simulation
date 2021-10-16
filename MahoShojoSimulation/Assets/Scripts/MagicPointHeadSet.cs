using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPointHeadSet : MonoBehaviour
{
    IEnumerator WaitForAWhile()
    {
        yield return new WaitForSeconds(2.0f);
        this.transform.SetParent(null);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForAWhile());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
