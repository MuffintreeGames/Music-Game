using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwapParticle : MonoBehaviour
{
    public float xDirection = 0f;
    public float goalX = 0f;
    static float travelTime = 0.05f;
    float speed = 0f;
    float timeAlive = 0f;   //used as a backup plan
    
    // Start is called before the first frame update
    void Start()
    {
        speed = goalX - transform.position.x / travelTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed/* * xDirection*/ * Time.deltaTime, 0, 0);
        timeAlive += Time.deltaTime;
        if (transform.position.x * xDirection > goalX * xDirection || timeAlive > travelTime) //multiplying by xDirection ensures we don't need to do separate cases for positive/negative
        {
            PositionTracker.concludeSwap.Invoke();
            Destroy(gameObject);
            return;
        }
    }

}
