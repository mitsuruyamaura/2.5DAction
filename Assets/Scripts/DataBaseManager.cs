using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    public EnemyMoveEventDataSO enemyMoveEventDataSO;

    public ExpTableSO expTableSO;

    public List<AbilityItemDataSO> abilityItemDataSOList;

    public EnemyDataSO enemyDataSO;
    public DebuffDataSO debuffDataSO;
    public StageDataSO stageDataSO;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���̃��x���A�b�v�ɕK�v�Ȍo���l���v�Z���Ď擾
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int CalcNextLevelExp(int level) {
        return expTableSO.expTablesList[level].maxExp;
    }



    /// <summary>
    /// ���x���� AbilityType �ɂ�� AbilityPointTable �̎擾
    /// </summary>
    /// <returns></returns>
    public AbilityItemDataSO.AbilityItemData GetAbilityPointTable(int level, AbilityType abilityType) {
        return abilityItemDataSOList[(int)abilityType].abilityItemDatasList.Find(x => x.abilityLevel == level);
    }
}
