using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateHorizontal : MonoBehaviour
{
    public int DuplicationCount = 3;

    // Use this for initialization
    void Start()
    {
        GameObject original = this.transform.GetChild(0).gameObject;

        for (int i = 0; i < DuplicationCount; i++)
        {
            GameObject newGo = Instantiate(original);
            newGo.transform.parent = this.transform;
            newGo.transform.localPosition = original.transform.localPosition + Vector3.right * i * 10;
        }
    }
}
