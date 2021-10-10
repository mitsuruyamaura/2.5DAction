using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    public EnemyDataSO enemyDataSO;
    public ConditionDataSO conditionDataSO;
    public ExpTableSO expTableSO;
    public StageDataSO stageDataSO;


    // mi
    public EnemyMoveEventDataSO enemyMoveEventDataSO;
    public OrbDataSO orbDataSO;
    public TreasureTableSO treasureTableSO;


    public List<AbilityItemDataSO> abilityItemDataSOList;

    // ドロップするトレジャーをすべて入れる
    public List<AbilityItemDataSO.AbilityItemData> dropItemDatasList = new List<AbilityItemDataSO.AbilityItemData>();


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 次のレベルアップに必要な経験値を計算して取得
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int CalcNextLevelExp(int level) {
        return expTableSO.expTablesList[level].maxExp;
    }

    // mi

    /// <summary>
    /// レベルと AbilityType による AbilityPointTable の取得
    /// </summary>
    /// <returns></returns>
    public AbilityItemDataSO.AbilityItemData GetAbilityPointTable(int level, AbilityType abilityType) {
        return abilityItemDataSOList[(int)abilityType].abilityItemDatasList.Find(x => x.abilityLevel == level);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dropTreasureLevel"></param>
    public void CreateDropItemDatasList(int dropTreasureLevel) {

        // TreasureTable 内の DropItemDatas を取得
        DropItemData[] dropItemDatas = treasureTableSO.treasureTablesList.Find(x => x.treasureLevel == dropTreasureLevel).dropItemDatas;

        //Debug.Log(dropItemDatas.Length);

        // すべてのアイテムデータのリストを検索
        for (int i = 0; i < abilityItemDataSOList.Count; i++) {

            // テーブルに含まれるレアリティを取得
            int[] rarityArray = dropItemDatas[i].rarities.Split(',').ToArray().Select(x => int.Parse(x)).ToArray();
            //Debug.Log(rarityArray.Length);

            foreach (AbilityItemDataSO.AbilityItemData itemData in abilityItemDataSOList[i].abilityItemDatasList) {
                for (int x = 0; x < rarityArray.Length; x++) {

                    if (itemData.rarity == rarityArray[x]) {
                        dropItemDatasList.Add(itemData);
                        continue;
                    }
                }
            }
        }
    }
}
