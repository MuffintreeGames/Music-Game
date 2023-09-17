using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfLoadedInto : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 0)
        {
            Destroy(this.gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
