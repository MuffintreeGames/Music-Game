using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportLayerController : MonoBehaviour
{
    public GameObject layer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateLayer()
    {
        layer.SetActive(true);
    }

    public void DeactivateLayer()
    {
        layer.SetActive(false);
    }
}
