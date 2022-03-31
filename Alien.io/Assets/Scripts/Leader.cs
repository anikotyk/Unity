using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
   // [SerializeField] List<Unit> allUnits = new List<Unit>();
    
    // Fixed update for physics calculations, with fixed timestep increased to 0.04 reducing
    // fixed update calls per second and increasing performance
    void FixedUpdate()
    {
        foreach(Unit unit in GetComponentsInChildren<Unit>())
        {
            unit.Flock(transform.position);
        }
        /*
        for (int i = 0; i < allUnits.Count; i++)
            allUnits[i].Flock(transform.position);*/
    }
}

