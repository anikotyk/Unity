using System.Collections;
using Unity.Burst;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private RawImage _shownImage;
    [SerializeField] private GameObject _saveBtn;
    [SerializeField] private GameObject _undoBtn;
    [SerializeField] private Texture2D _standardImage;
    [SerializeField] private Toggle _isToUseMultithreading;
    [SerializeField] private Text _timeText;

    private Texture2D _textureToChange;
    private string _pathToShownImage;

    private void Start()
    {
        _textureToChange = new Texture2D(_standardImage.width, _standardImage.height);
        _textureToChange.SetPixels(_standardImage.GetPixels());
        _textureToChange.Apply();
        _shownImage.texture = _textureToChange;

        _saveBtn.SetActive(false);
        _undoBtn.SetActive(false);
    }

    public void LoadImageBtn()
    {
        LoadImage(EditorUtility.OpenFilePanel("Open image", "", "png,jpg"));
    }

    private void LoadImage(string path)
    {
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            if (fileContent.Length > 0)
            {
                _pathToShownImage = path;
                (_shownImage.texture as Texture2D).LoadImage(fileContent);
                float coeff = _shownImage.texture.width*1f / _shownImage.texture.height;
                _shownImage.GetComponent<RectTransform>().sizeDelta = new Vector2(_shownImage.GetComponent<RectTransform>().rect.height * coeff, _shownImage.GetComponent<RectTransform>().rect.height);
                _saveBtn.SetActive(false);
                _undoBtn.SetActive(false);
            }
        }
    }

    public void UndoChangesInImage()
    {
        LoadImage(_pathToShownImage);
    }

    public void SaveImageBtn()
    {
        string path = EditorUtility.SaveFilePanel("Save image", "", "image.png", "png");
        if (path == "")
        {
            return;
        }
        File.WriteAllBytes(path, _textureToChange.EncodeToPNG());
    }

    public void ChangeColorImageBtn()
    {
        float startTime = Time.realtimeSinceStartup;

        _textureToChange = new Texture2D(_shownImage.texture.width, _shownImage.texture.height);
        _textureToChange.SetPixels((_shownImage.texture as Texture2D).GetPixels());
        _textureToChange.Apply();

        if (_isToUseMultithreading.isOn)
        {
            var textureData = _textureToChange.GetRawTextureData<Color32>();
            ImageProcessor jobProcessImage = new ImageProcessor()
            {
                Texture = textureData
            };
            JobHandle jobProcessImageHandle = jobProcessImage.Schedule(textureData.Length, 4);
            jobProcessImageHandle.Complete();
        }
        else
        {
            for (int y = 0; y < _textureToChange.height; y++)
            {
                for (int x = 0; x < _textureToChange.width; x++)
                {
                    ChangePixelColor(x, y);
                }
            }
        }
        
        _textureToChange.Apply();
        _shownImage.texture = _textureToChange;
        _saveBtn.SetActive(true);
        _undoBtn.SetActive(true);
        _timeText.text = "Time: " + (Time.realtimeSinceStartup - startTime);
    }

    private void ChangePixelColor(int indexX, int indexY)
    {
        float newColor = (_textureToChange.GetPixel(indexX, indexY).r + _textureToChange.GetPixel(indexX, indexY).b + _textureToChange.GetPixel(indexX, indexY).g) / 3;
        Color color = new Color(newColor, newColor, newColor);
        _textureToChange.SetPixel(indexX, indexY, color);
    }
}

[BurstCompile(CompileSynchronously = false)]
public struct ImageProcessor : IJobParallelFor
{
    public NativeArray<Color32> Texture;
    
    public void Execute(int index)
    {
        ChangePixelColor(index);
    }

    private void ChangePixelColor(int x)
    {
        byte newColor = (byte)(0.30f * Texture[x].r + 0.59f * Texture[x].b + 0.11f * Texture[x].g);
        Color32 color = new Color32(newColor, newColor, newColor, Texture[x].a);
        Texture[x] = color;
    }
}