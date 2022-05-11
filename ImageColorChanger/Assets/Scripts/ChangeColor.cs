using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private RawImage shownImage;
    [SerializeField] private GameObject saveBtn;
    [SerializeField] private Texture2D standardImage;
    [SerializeField] private bool isToUseJobSystem;
    private Texture2D textureToChange;

    private void Start()
    {
        textureToChange = new Texture2D(standardImage.width, standardImage.height);
        textureToChange.SetPixels(standardImage.GetPixels());
        textureToChange.Apply();
        shownImage.texture = textureToChange;

        saveBtn.SetActive(false);
    }

    public void LoadImageBtn()
    {
        string path = EditorUtility.OpenFilePanel("Open image", "", "png,jpg");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            if (fileContent.Length > 0)
            {
                (shownImage.texture as Texture2D).LoadImage(fileContent);
                float coeff = shownImage.texture.width*1f / shownImage.texture.height;
                shownImage.GetComponent<RectTransform>().sizeDelta = new Vector2(shownImage.GetComponent<RectTransform>().rect.height * coeff, shownImage.GetComponent<RectTransform>().rect.height);
                saveBtn.SetActive(false);
            }
            
        }
    }

    public void SaveImageBtn()
    {
        string path = EditorUtility.SaveFilePanel("Save image", "", "image.png", "png");
        if (path == "")
        {
            return;
        }
        File.WriteAllBytes(path, textureToChange.EncodeToPNG());
    }

    public void ChangeColorImageBtn()
    {
        float startTime = Time.realtimeSinceStartup;
        textureToChange = new Texture2D(shownImage.texture.width, shownImage.texture.height);
        textureToChange.SetPixels((shownImage.texture as Texture2D).GetPixels());
        textureToChange.Apply();
        if (isToUseJobSystem)
        {
            var data = textureToChange.GetRawTextureData<Color32>();
            ImageChanger jobChangeImage = new ImageChanger()
            {
                texture = data
            };
            JobHandle jobChangeImageHandle = jobChangeImage.Schedule();
            jobChangeImageHandle.Complete();
            
        }
        else
        {
            for (int y = 0; y < textureToChange.height; y++)
            {
                for (int x = 0; x < textureToChange.width; x++)
                {
                    ChangePixelColor(x, y);
                }
            }
        }
        
        textureToChange.Apply();
        shownImage.texture = textureToChange;
        saveBtn.SetActive(true);
        Debug.Log("Time of changing image: "+(Time.realtimeSinceStartup - startTime));
    }

    private void ChangePixelColor(int indexX, int indexY)
    {
        float newColor = (textureToChange.GetPixel(indexX, indexY).r + textureToChange.GetPixel(indexX, indexY).b + textureToChange.GetPixel(indexX, indexY).g) / 3;
        Color color = new Color(newColor, newColor, newColor);
        textureToChange.SetPixel(indexX, indexY, color);
    }
}


public struct ImageChanger : IJob
{
    public NativeArray<Color32> texture;
    
    public void Execute()
    {
        for (int x = 0; x < texture.Length; x++)
        {
            ChangePixelColor(x);
        }
    }

    private void ChangePixelColor(int x)
    {
        byte newColor = (byte)((texture[x].r + texture[x].b + texture[x].g) / 3);
        Color32 color = new Color32(newColor, newColor, newColor, texture[x].a);
        texture[x] = color;
    }
}