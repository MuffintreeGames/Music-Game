using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Target : MonoBehaviour
{
    public FollowPath ownPath;
    static int points = 100;
    static float fallSpeed = 5f;
    public string targetColor = "None";

    MusicBar collidingBar = null;
    Vector2 collisionPoint = Vector2.negativeInfinity;
    bool barUnderneath = false;
    bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        HealthTracker.playerDeath.AddListener(OnPlayerDeath);
        HealthTracker.songReset.AddListener(OnPlayerRewind);
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, ownPath.GetDestination(), 10f, 1 << LayerMask.NameToLayer("MusicBar") | 1 << LayerMask.NameToLayer("FailBox"));
        if (hit.rigidbody != null)
        {
            collisionPoint = hit.point;
            if (hit.rigidbody.gameObject.layer == LayerMask.NameToLayer("FailBox"))
            {
                barUnderneath = false;
            }
            else
            {
                collidingBar = hit.rigidbody.GetComponentInParent<MusicBar>();
                //Debug.Log("hit something with raycast; position = " + collisionPoint + ", object = " + hit.rigidbody.gameObject + ", current position = " + transform.position);
                barUnderneath = true;
            }
        }/*
        else
        {
            barUnderneath = false;
        }*/

        if (transform.position.y <= collisionPoint.y)
        {
            Debug.Log("should catch now!");
            if (barUnderneath)
            {
                HandleCaughtNote();
            }
            else
            {
                Debug.Log("failed to hit target!");
                HealthTracker.TakeDamage();
                Destroy(gameObject);
            }
        }
    }

    void HandleCaughtNote()
    {
            if (targetColor == "Blue")
            {
                if (!collidingBar.IsCurrentlyBlue())
                {
                    Debug.Log("hit the wrong color!");
                    HealthTracker.TakeDamage();
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("hit the correct color!");
                    Destroy(gameObject);
                }
                return;
            }
            else if (targetColor == "Yellow")
            {
                if (!collidingBar.IsCurrentlyYellow())
                {
                    Debug.Log("hit the wrong color!");
                    HealthTracker.TakeDamage();
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("hit the correct color!");
                    Destroy(gameObject);
                }
                return;
            }
            Debug.Log("hit a target!");
            PointTracker.pointEvent.Invoke(points);
            Destroy(gameObject);
    }

    void OnPlayerDeath()
    {
        paused = true;
    }

    void OnPlayerRewind()
    {
        Destroy(gameObject);
    }
}
