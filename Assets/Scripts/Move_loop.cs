﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_loop : MonoBehaviour
{
    public float speed;
    public Vector3 offset;
    private Vector3 initial_pos;
    public int scale;

    // Start is called before the first frame update
    void Start()
    {
        initial_pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offset.x -= speed / 10000;
        offset.x %= 1;
        transform.position = initial_pos+(offset*scale);
    }
}
