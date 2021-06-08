using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_control_try : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameObject = GameObject.Find("lPinky3");
        Debug.Log(gameObject.transform.position.x);
        Debug.Log(gameObject.transform.position.y);
        Debug.Log(gameObject.transform.position.z);

    }
}
