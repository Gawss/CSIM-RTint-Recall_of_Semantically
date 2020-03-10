using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Experiment;

public class UpdateStimuli : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.Instance.sprite = Resources.Load<Sprite>("Sprites/img" + SceneManager.Instance.index);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", SceneManager.Instance.sprite.texture);
    }
}
