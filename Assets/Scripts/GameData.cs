using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �C���x���g���p�̃A�C�e���f�[�^
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

    // �A�r���e�B�A�C�e���̃��X�g
    public List<InventryAbilityItemData> abilityItemDatasList = new List<InventryAbilityItemData>();

    // �o�g���ŕt�^���ꂽ�f�o�t�̃��X�g
    public List<ConditionType> debuffConditionsList = new List<ConditionType>();

    // �I�����Ă���X�e�[�W�̔ԍ�
    public int chooseStageNo;

    // �N���A�ς̃X�e�[�W�̔ԍ�
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

        // �Q�[���̏�����
        InitialzeGameData();


        // �Q�[���̏�����
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
    /// �A�r���e�B�|�C���g�̉��Z
    /// </summary>
    public void AddAbilityPoint() {
        abilityPoint += playerLevel;
    }

    /// <summary>
    /// �l�������g���W���[�̏���ǉ�
    /// </summary>
    public void AddaAbilityItemDatasList(AbilityType abilityType, int abilityNo) {
        abilityItemDatasList.Add(new InventryAbilityItemData { abilityType = abilityType, abilityNo = abilityNo});
    }
}
