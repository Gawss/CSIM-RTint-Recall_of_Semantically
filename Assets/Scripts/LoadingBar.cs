using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Experiment;
public class LoadingBar : MonoBehaviour
{
    void loadCompleted()
    {
        Debug.Log("Load completed...");
        this.GetComponent<Animator>().SetBool("isLoading", false);
        SceneManager.Instance.PlayTrial();

    }
}