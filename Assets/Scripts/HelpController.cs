using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpController : MonoBehaviour
{
    public GameObject helpScreen1;
    public GameObject helpScreen2;

    bool helping = false;
    int page = 0;
    // Start is called before the first frame update
    void Start()
    {
        page = 0;
        helping = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if ()
    }

    public void EnableHelp()
    {
        helping = true;
        page = 0;
        helpScreen1.SetActive(true);
    }

    public void DisableHelp()
    {
        helping = false;
        helpScreen1.SetActive(false);
    }

    public void TogglePage()
    {
        if (page == 0)
        {
            page = 1;
            helpScreen2.SetActive(true);
            helpScreen1.SetActive(false);
        } else
        {
            page = 0;
            helpScreen1.SetActive(true);
            helpScreen2.SetActive(false);
        }
    }
}
