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
    [SerializeField] private GameObject _loadingCanvas;
    
    public void LoadScene(string scene)
    {
        StartCoroutine(LoadingSceneCoroutine(scene));
    }

    private IEnumerator LoadingSceneCoroutine(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        
        if (_loadingCanvas != null)
        {
            _loadingCanvas.SetActive(true);
        }

        yield return operation;
    }
}
