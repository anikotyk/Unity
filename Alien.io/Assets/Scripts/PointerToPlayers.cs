using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerToPlayers : MonoBehaviour
{
    [SerializeField] private GameObject pointersParent;

    [SerializeField] private GameObject pointerPrefab;

    [SerializeField] private int countShownPointers;

    [SerializeField] private Transform squadsParent;

    [SerializeField] private float xMax;
    [SerializeField] private float xMin;
    [SerializeField] private float yMax;
    [SerializeField] private float yMin;

    [SerializeField]  private Transform playerContainter;

    private Dictionary<Pointer, AddRuners> squadsNear = new Dictionary<Pointer, AddRuners>();
    private List<KeyValuePair<AddRuners, float>> squadsNearTop = new List<KeyValuePair<AddRuners, float>>();

    private GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        for (int i = 0; i < countShownPointers; i++)
        {
            GameObject pointer = Instantiate(pointerPrefab, pointersParent.transform);
            pointer.GetComponent<Pointer>().Initialize(playerContainter, xMax, xMin, yMax, yMin);
        }
    }

    private void Update()
    {
        GetNearest();
        ShowPointers();
    }

    private void GetNearest()
    {
        squadsNearTop.Clear();
        for (int i = 0; i < squadsParent.childCount; i++)
        {
            float distance = Vector3.Distance(playerContainter.position, squadsParent.GetChild(i).position);
            squadsNearTop.Add(new KeyValuePair<AddRuners, float>(squadsParent.GetChild(i).GetComponent<AddRuners>(), distance));
        }
        squadsNearTop.Sort((x, y) => (x.Value.CompareTo(y.Value)));
        for(int i = 0; i< squadsNearTop.Count; i++)
        {
           // Debug.Log(squadsNearTop[i].Key.nameInTop + " " + squadsNearTop[i].Value);
        }
        /*for (int i = 0; i < squadsNearTop.Count - 1; i++)
        {
            for (int j = 0; j < squadsNearTop.Count - i - 1; j++)
            {
                if (squadsNearTop[j].Value < squadsNearTop[j + 1].Value)
                {
                    KeyValuePair<AddRuners, float> temp = squadsNearTop[j];
                    squadsNearTop[j] = squadsNearTop[j + 1];
                    squadsNearTop[j + 1] = temp;
                }
            }
        }*/
    }

    private void ShowPointers()
    {
        for(int i = 0; i < pointersParent.transform.childCount; i++)
        {
            Pointer pointer = pointersParent.transform.GetChild(i).GetComponent<Pointer>();
            if(squadsNear.ContainsKey(pointer))
            {
                int index = FindIndexByKey(squadsNearTop, squadsNear[pointer]);
                if (index == -1 || index >= countShownPointers)
                {
                    squadsNear[pointer].isPointedNow = false;
                    pointer.SetTarget(null, 0);
                    squadsNear.Remove(pointer);
                }
            }
        }

        int cnt = Mathf.Min(squadsNearTop.Count, countShownPointers);
        for (int i = 0; i < cnt; i++)
        {
            if (!squadsNearTop[i].Key.isPointedNow)
            {
                Pointer pointer = GetPointerWithoutTarget();
                pointer.SetTarget(squadsNearTop[i].Key, squadsNearTop[i].Key.transform.childCount);
                squadsNearTop[i].Key.isPointedNow = true;
                squadsNear.Add(pointer, squadsNearTop[i].Key);
            }
        }
    }

    private int FindIndexByKey(List<KeyValuePair<AddRuners, float>> array, AddRuners elem)
    {
        for(int i=0; i<array.Count; i++)
        {
            if (array[i].Key == elem)
            {
                return i;
            }
        }
        return -1;
    }

    private Pointer GetPointerWithoutTarget()
    {
        for(int i = 0; i < pointersParent.transform.childCount; i++)
        {
            if (pointersParent.transform.GetChild(i).GetComponent<Pointer>().Target == null)
            {
                return pointersParent.transform.GetChild(i).GetComponent<Pointer>();
            }
        }
        return null;
    }
    
}
