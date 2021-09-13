using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public string name;
    public int enemyNo;
    public int hp;
    public int attackPower;
    public float moveSpeed;
    public float attackInterval;
    public int exp;

    public EnemyController enemyPrefab;
    public EnemyDebuffData[] debuffDatas;
}

[System.Serializable]
public class EnemyDebuffData {
    public ConditionType debuffConditionType;
    public int rate;
}
