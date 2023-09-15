using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class BlastNote : MonoBehaviour
{
    static float fallSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        HealthTracker.songReset.AddListener(OnPlayerRewind);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector2(transform.position.x, transform.position.y - (fallSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Blast"))
        {
            Debug.Log("hit a blast note!");
            //PointTracker.pointEvent.Invoke(points);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("FailBox"))
        {
            Debug.Log("failed to hit blast note!");
            HealthTracker.TakeDamage();
            Destroy(gameObject);
        }
    }

    void OnPlayerRewind()
    {
        Destroy(gameObject);
    }
}
