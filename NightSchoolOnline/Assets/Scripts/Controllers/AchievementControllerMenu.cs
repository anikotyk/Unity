using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AchievementControllerMenu : MonoBehaviour
{
    [SerializeField] private GameObject _achievementPrefab;
    [SerializeField] private Transform _achievementParent;
    [SerializeField] private Sprite _achievementStar;
    
    [SerializeField] private List<string> _achievementsList;
    private Dictionary<string, bool> _achievementsDictionary;
    
    private void Awake()
    {
        _achievementsDictionary = new Dictionary<string, bool>();
        GetAllAchievements();
        SpawnAchievementsList();
    }
    
    private void GetAllAchievements()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
        string filePath = Path.Combine(folderPath, "achievements.json");
        Debug.Log(folderPath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            string data = reader.ReadToEnd();
            reader.Close();
            if (data.Length > 0)
            {
                _achievementsDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(data);
            }
        }

        foreach (string achievementName in _achievementsList)
        {
            if (!_achievementsDictionary.ContainsKey(achievementName))
            {
                _achievementsDictionary.Add(achievementName, false);
            }
        }

        SaveAchievements();
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

    public void SpawnAchievementsList()
    {
        foreach (string achievement in _achievementsDictionary.Keys)
        {
            GameObject newAchievement = Instantiate(_achievementPrefab);
            newAchievement.transform.SetParent(_achievementParent);
            if (_achievementsDictionary[achievement])
            {
                newAchievement.GetComponent<AchievementObject>().StarImage.sprite = _achievementStar;
                newAchievement.GetComponent<AchievementObject>().AchievementName.text = achievement;
            }
        }
    }
}
