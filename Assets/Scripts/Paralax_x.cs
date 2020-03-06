using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Experiment;

public class Paralax_x : MonoBehaviour
{
    public float speed;
    public Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offset.x += speed*SceneManager.Instance.globalSpeed/10000;
        offset.x %= 1;
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
