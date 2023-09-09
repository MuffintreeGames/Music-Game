using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] public Transform[] waypoints;
    [SerializeField] public float[] waypointSpeeds; //time spent moving to a single waypoint
    int currentIndex = -1;
    Vector2 goalPosition = Vector2.zero;
    Vector2 startPosition = Vector2.zero;
    Vector2 direction = Vector2.zero;
    float targetSpeed = 0f;
    //Vector2 oldTargetSpeed = Vector2.zero;
    //Vector2 speed = Vector2.zero;
    float timeSinceChangedGoal = 0f;
    Vector2 velocity = Vector3.zero;
    Vector2[] positions;

    static float accelTime = 0.1f;
    static float goalAllowance = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = -1;
        goalPosition = Vector2.zero;
        positions = new Vector2[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            positions[i] = waypoints[i].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentIndex == -1 || timeSinceChangedGoal >= targetSpeed/*(((transform.position.x - goalPosition.x) * direction.x >= 0 ) && ((transform.position.y - goalPosition.y) * direction.y >= 0))*/)
        {
            Debug.Log("currently at " + transform.position + "; goal is at " + goalPosition);
            currentIndex++;
            
            if (currentIndex < waypoints.Length && currentIndex < waypointSpeeds.Length)
            {
                goalPosition = positions[currentIndex];
                
                direction = (goalPosition - (Vector2)transform.position).normalized;
                startPosition = transform.position;
                targetSpeed = waypointSpeeds[currentIndex];
                timeSinceChangedGoal = 0f;
            }
        }
        if (currentIndex >= waypoints.Length) {
            return;
        }

        timeSinceChangedGoal += Time.deltaTime;
        /*if (timeSinceChangedGoal < accelTime)
        {
            speed = oldTargetSpeed + (targetSpeed - oldTargetSpeed) * timeSinceChangedGoal / accelTime;
        } else
        {
            speed = targetSpeed;
        }*/
        //transform.position = Vector3.Lerp(startPosition, goalPosition, timeSinceChangedGoal / targetSpeed);
        transform.position = Vector2.SmoothDamp(transform.position, goalPosition, ref velocity, targetSpeed);
        //transform.position = new Vector2(transform.position.x + speed.x * Time.deltaTime, transform.position.y + speed.y * Time.deltaTime);
    }
}
