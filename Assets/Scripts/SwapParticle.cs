using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapParticle : MonoBehaviour
{
    public float xDirection = 0f;
    public float goalX = 0f;
    static float speed = 8f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x * xDirection > goalX * xDirection) //multiplying by xDirection ensures we don't need to do separate cases for positive/negative
        {
            Destroy(gameObject);
            return;
        }
        transform.position += new Vector3(speed * xDirection * Time.deltaTime, 0, 0);
    }

}
