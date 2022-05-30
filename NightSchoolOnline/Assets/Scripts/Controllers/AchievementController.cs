using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour
{
    [SerializeField] private Transform _achievementCanvas;
    [SerializeField] private GameObject _achievementContainer;
    [SerializeField] private Text _achievementText;
    [SerializeField] private Vector3 _positionShown;
    [SerializeField] private Vector3 _positionHidden;
    [SerializeField] private float _timeToShow;
    [SerializeField] private float _timeBetweenShowAndHide;

    private Dictionary<string, bool> _achievementsDictionary = new Dictionary<string, bool>();

    private void Awake()
    {
        _positionShown = new Vector3(_positionShown.x, _positionShown.y + _achievementCanvas.GetComponent<RectTransform>().rect.height/2, _positionShown.z);
        _positionHidden = new Vector3(_positionHidden.x, _positionHidden.y + _achievementCanvas.GetComponent<RectTransform>().rect.height / 2, _positionHidden.z);
        GetAllAchievements();
    }

    public void GetAchievement(string achivementName)
    {
        if (!_achievementsDictionary.ContainsKey(achivementName))
        {
            return;
        }
        _achievementText.text = "You got the achivement!\n\""+achivementName+"\"";
        _achievementsDictionary[achivementName] = true;
        SaveAchievements();
        StartCoroutine(ShowAndHideAchievement());
    }

    private IEnumerator ShowAndHideAchievement() {
        LeanTween.moveLocal(_achievementContainer, _positionShown, _timeToShow);
        yield return new WaitForSeconds(_timeBetweenShowAndHide);
        LeanTween.moveLocal(_achievementContainer, _positionHidden, _timeToShow);
    }

    private void GetAllAchievements()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Saves", "achievements.json");
        
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            string data = reader.ReadToEnd();
            reader.Close();
            _achievementsDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(data);
        }
    }

    private void SaveAchievements()
    {
        string path = Path.Combine(Application.persistentDataPath, "Saves", "achievements.json");
        if (!File.Exists(path))
        {
            FileStream file = File.Create(path);
            file.Close();
        }
        File.WriteAllText(path, JsonConvert.SerializeObject(_achievementsDictionary));
    }
}


