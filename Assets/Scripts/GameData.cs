using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// インベントリ用のアイテムデータ
/// </summary>
[System.Serializable]
public class InventryAbilityItemData {
    public AbilityType abilityType;
    public int abilityNo;
} 

public class GameData : AbstractSingleton<GameData>
{
    public ReactiveProperty<int> staminaPoint = new ReactiveProperty<int>();

    public ReactiveDictionary<int, bool> orbs = new ReactiveDictionary<int, bool>();

    public int hp;

    public int maxHp;

    public bool isDebugOn;

    public int playerLevel;

    public int totalExp;

    public CharacterData currentCharaData;

    public int abilityPoint;

    // アビリティアイテムのリスト
    public List<InventryAbilityItemData> abilityItemDatasList = new List<InventryAbilityItemData>();

    // バトルで付与されたデバフのリスト
    public List<ConditionType> debuffConditionsList = new List<ConditionType>();

    // 選択しているステージの番号
    public int chooseStageNo;

    // クリア済のステージの番号
    public List<int> clearedStageNos;

    public StageData currentStageData;

    public bool isBossBattled;

    public float moveTimeScale;

    public CharaStatus currentCharaStatus;
    public UserData userData;

    public CombatData playerCombatData;
    public CombatData enemyCombatData;


    protected override void Awake() {
        base.Awake();

        // ゲームの初期化
        InitialzeGameData();


        // ゲームの初期化
        void InitialzeGameData() {
            maxHp = currentCharaData.maxHp;
            hp = maxHp;

            playerLevel = 1;

            totalExp = 0;

            abilityPoint += playerLevel;

            moveTimeScale = 1.0f;
        }
    }

    /// <summary>
    /// アビリティポイントの加算
    /// </summary>
    public void AddAbilityPoint() {
        abilityPoint += playerLevel;
    }

    /// <summary>
    /// 獲得したトレジャーの情報を追加
    /// </summary>
    public void AddaAbilityItemDatasList(AbilityType abilityType, int abilityNo) {
        abilityItemDatasList.Add(new InventryAbilityItemData { abilityType = abilityType, abilityNo = abilityNo});
    }
}
