using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableInPlayMode : MonoBehaviour
{
    public bool disableWhenPlaying = true;

    ChartManager chartManager;
    // Start is called before the first frame update
    void Start()
    {
        chartManager = GameObject.Find("ChartManager").GetComponent<ChartManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chartManager.longPause != disableWhenPlaying)
        {
            GetComponent<Button>().interactable = false;
        } else
        {
            GetComponent<Button>().interactable = true;
        }
    }
}
