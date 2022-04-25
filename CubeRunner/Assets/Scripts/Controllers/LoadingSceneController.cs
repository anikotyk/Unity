using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] private GameObject LoadingCanvas;
    
    public void LoadScene(string scene)
    {
        StartCoroutine(LoadingSceneCoroutine(scene));
    }

    private IEnumerator LoadingSceneCoroutine(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        if (LoadingCanvas != null)
        {
            LoadingCanvas.SetActive(true);
        }
        
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
