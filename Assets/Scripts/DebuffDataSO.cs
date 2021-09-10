using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebuffDataSO", menuName = "Create DebuffDataSO")]
public class DebuffDataSO : ScriptableObject
{
    public List<DebuffData> debuffDatasList = new List<DebuffData>();   
}
