using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Experiment;

public class Move_loop : MonoBehaviour
{
    public float speed;
    public Vector3 offset;
    
    [HideInInspector]
    public Vector3 initial_pos;
    public int scale;

    public GameObject plane;

    // Start is called before the first frame update
    void Start()
    {
        initial_pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offset.x -= speed*SceneManager.Instance.globalSpeed/10000;
        offset.x %= 1;
        transform.position = initial_pos+(offset*scale);
        if(offset.x < -0.9f){
            plane.SetActive(false);
            SceneManager.Instance.sprite = Resources.Load<Sprite>("Sprites/img" + SceneManager.Instance.index);
        }
    }
}
