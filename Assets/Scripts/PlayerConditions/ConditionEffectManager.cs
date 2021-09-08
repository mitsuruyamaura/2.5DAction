using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionEffectManager : MonoBehaviour
{
    public static ConditionEffectManager instance;

    public List<ConditionEffect> conditionEffectsList = new List<ConditionEffect>();

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �����Ŏw�肵���R���f�B�V�����̃G�t�F�N�g�p�v���t�@�u���擾
    /// </summary>
    /// <param name="conditionType"></param>
    /// <returns></returns>
    public ConditionEffect GetConditionEffect(ConditionType conditionType) {
        return conditionEffectsList.Find(x => x.conditionType == conditionType);
    }
}
