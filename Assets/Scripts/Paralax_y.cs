using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Experiment;

public class Paralax_y : MonoBehaviour
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
        offset.y += speed*SceneManager.Instance.globalSpeed/10000;
        offset.y %= 1;
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
