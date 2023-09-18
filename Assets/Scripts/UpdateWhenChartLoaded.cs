using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateWhenChartLoaded : MonoBehaviour
{
    public CustomChartLoader chartLoader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (chartLoader == null)
        {
            return;
        }

        if (chartLoader.GetError())
        {
            GetComponent<TextMeshProUGUI>().text = "Error!";
        } else if (MusicController.customSongChart != null) {
            GetComponent<TextMeshProUGUI>().text = "Loaded!";
        } else {
            GetComponent<TextMeshProUGUI>().text = "Not Loaded";
        }
    }
}
