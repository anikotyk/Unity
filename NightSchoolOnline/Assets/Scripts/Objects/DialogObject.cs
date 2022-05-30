using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogObject : MonoBehaviour
{
    public List<ReplicaSet> replicas = new List<ReplicaSet>();
    public int nowReplicasSet = 0;

    //public List<UnityEvent> funcsAfterDialogs = new List<UnityEvent>();
    //public UnityEvent funcsAfterEverySet = new UnityEvent();
    
    public void NextReplicasSet()
    {
        nowReplicasSet++;
    }

    public string GetReplica(int replicaIndex)
    {
        return replicas[nowReplicasSet].replicasSet[replicaIndex];
    }
    
}

[System.Serializable]
public class ReplicaSet
{
    public List<string> replicasSet;
}