using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public struct InventryAbilityItemData {
    public AbilityType abilityType;
    public int abilityNo;
} 

public class GameData : MonoBehaviour
{
    public static GameData instance;

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


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        InitialzeGameData();

        void InitialzeGameData() {
            maxHp = currentCharaData.maxHp;
            hp = maxHp;

            playerLevel = 1;

            totalExp = 0;

            abilityPoint += playerLevel;
        }
    }

    /// <summary>
    /// アビリティポイントの加算
    /// </summary>
    public void AddAbilityPoint() {
        abilityPoint += playerLevel;
    }
}
