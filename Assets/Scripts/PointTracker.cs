using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PointEvent : UnityEvent<int>
{
    public int pointsGained;
}

public class PointTracker : MonoBehaviour
{

    public static int points = 0;
    public static PointEvent pointEvent;
    public TextMeshProUGUI scoreBox;
    
    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        if (pointEvent == null)
        {
            pointEvent = new PointEvent();
        }
        pointEvent.AddListener(GainPoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GainPoints(int pointsGained)
    {
        points += pointsGained;
        Debug.Log("points were gained, score is " + points);
        scoreBox.text = points.ToString();
    }
}
