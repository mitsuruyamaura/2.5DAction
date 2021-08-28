using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

        // �擾���������g���Đݒ�
        imgAbility.sprite = abilityData.abilityTable.abilitySprite;
        txtAbilityCost.text = abilityData.abilityTable.abilityCost.ToString();

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

        if (GameData.instance.abilityPoint >= abilityData.abilityTable.abilityCost) {
            btnAbility.interactable = true;
        }
    }
}
