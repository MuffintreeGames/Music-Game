using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    static int points = 100;
    static float fallSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - (fallSpeed * Time.deltaTime)); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MusicBar"))
        {
            Debug.Log("hit a target!");
            PointTracker.pointEvent.Invoke(points);
            Destroy(gameObject);
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("FailBox"))
        {
            Debug.Log("failed to hit target!");
            Destroy(gameObject);
        }
    }
}
