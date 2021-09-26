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

    // �f�o�t�p�̃R���f�B�V�����̃f�[�^
    public EnemyDebuffData[] debuffDatas;
}

/// <summary>
/// �f�o�t�p�̃R���f�B�V�����̓o�^�p
/// </summary>
[System.Serializable]
public class EnemyDebuffData {

    // �f�o�t�p�̃R���f�B�V�����̐ݒ�
    public ConditionType debuffConditionType;

    // �f�o�t�p�̃R���f�B�V�����̕t�^�m��
    [Range(0, 100)]
    public int rate;
}
