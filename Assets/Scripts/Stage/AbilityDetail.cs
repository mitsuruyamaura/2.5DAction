using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

[System.Serializable]
public class AbilityDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnAbility;

    [SerializeField]
    private Image imgAbility;

    [SerializeField]
    private Text txtAbilityCost;

    public AbilityData abilityData;

    private SelectAbilityPopUp selectAbilityPopUp;

    private bool haveAbility = false;       // �A�r���e�B�p�̃A�C�e�����������Ă��邩�ǂ����̔���

    private bool learnedAbility = false;    // �A�r���e�B���K�����Ă��邩�ǂ����̔���

    /// <summary>
    /// AbilityButtonDetail �̐ݒ�
    /// </summary>
    /// <param name="level"></param>
    /// <param name="abilityType"></param>
    /// <param name="selectAbilityPopUp"></param>
    public void SetUpAbilityDetail(int level, AbilityType abilityType, SelectAbilityPopUp selectAbilityPopUp) {
        this.selectAbilityPopUp = selectAbilityPopUp;
        abilityData.abilityType = abilityType;

        // AbilityLevel �� AbilityType ���� AbilityTable ���擾
        abilityData.abilityTable = DataBaseManager.instance.GetAbilityPointTable(level, abilityType);

        btnAbility.onClick.AddListener(OnClickAbilityDetail);
    }

    /// <summary>
    /// �{�^�����N���b�N�����ۂ̏���
    /// </summary>
    public void OnClickAbilityDetail() {
        transform.DOShakeScale(0.5f).SetEase(Ease.OutQuart);

        selectAbilityPopUp.SetAbilityDetail(this);
    }

    /// <summary>
    /// �R�X�g�̎x�����L�����m�F���ă{�^���̊�����/�񊈐����̐؂�ւ�
    /// </summary>
    public void JudgeAbilityCost() {
        btnAbility.interactable = false;

        // ���łɏK���ς�����
        if (learnedAbility) {
            return;
        }

        // �Ή�����A�r���e�B�A�C�e�����������Ă��邩����
        CheckHaveAbilityItem();

        // �������Ă��Ȃ��ꍇ
        if (!haveAbility) {
            return;
        }

        // �A�r���e�B�̃R�X�g���x�����邩�ǂ����𔻒�
        if (GameData.instance.abilityPoint >= abilityData.abilityTable.abilityCost) {
            btnAbility.interactable = true;
        }
    }

    /// <summary>
    /// �Ή�����A�r���e�B�A�C�e�����������Ă��邩����
    /// </summary>
    private void CheckHaveAbilityItem() {

        // �A�r���e�B�A�C�e�������łɏ������Ă���ꍇ
        if (haveAbility) {
            // �`�F�b�N�s�v
            return;
        }

        // ���̃A�r���e�B�̃^�C�v�Ɠ����A�r���e�B�^�C�v�݂̂𒊏o
        List<InventryAbilityItemData> checkList = GameData.instance.abilityItemDatasList.Where(x => x.abilityType == abilityData.abilityType).ToList();

        //Debug.Log(checkList.Count);

        // �`�F�b�N���X�g���̃A�r���e�B�A�C�e���Ƃ��̃A�r���e�B�̔ԍ������v������A�������Ă���Ɣ���
        if(checkList.Exists(x => x.abilityNo == abilityData.abilityTable.abitilyNo)) {
            // �擾���������g���ăA�����b�N�ݒ�
            imgAbility.sprite = abilityData.abilityTable.abilitySprite;
            txtAbilityCost.text = abilityData.abilityTable.abilityCost.ToString();

            haveAbility = true;
        } else {
            // ���b�N�ݒ�
            txtAbilityCost.text = "";
        }
    }

    /// <summary>
    /// �A�r���e�B���K���ςɂ���
    /// </summary>
    public void LearnAbility() {
        learnedAbility = true;
    }
}
