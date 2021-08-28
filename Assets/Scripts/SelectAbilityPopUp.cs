using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectAbilityPopUp : MonoBehaviour
{
    [SerializeField]
    private Button btnExit;

    [SerializeField]
    private Button btnSubmit;

    [SerializeField]
    private Text txtAbilityPoint;

    [SerializeField]
    private Text txtDescription;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private Stage stage;

    // �A�r���e�B�p�̃{�^���̃v���t�@�u
    [SerializeField]
    private AbilityDetail abilityDetailPrefab;

    // �����ʒu���S�ӏ� for �����d�ŉ񂷂̂Ŕz��ɂ���
    [SerializeField]
    private Transform[] abilityDetailTrans;

    // �I�����Ă���A�r���e�B�̕ێ�
    private AbilityDetail currentAbilityDetail;

    public List<AbilityDetail[]> abilityDetailsList;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stage"></param>
    public void SetUpSelectAbilityPopUp(Stage stage) {

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        this.stage = stage;

        // �{�^���̐����Ɛݒ�
        CreateAbilityDetails();

        // �����l�ݒ�
        Initialize();

        // �{�^���Ƀ��\�b�h��o�^
        btnExit.onClick.AddListener(ClosePopUp);

        btnSubmit.onClick.AddListener(PowerUpAbility);
        btnSubmit.interactable = false;
    }

    /// <summary>
    /// AbilityDetail �{�^���̐����Ɛݒ�
    /// </summary>
    private void CreateAbilityDetails() {
        abilityDetailsList = new List<AbilityDetail[]>(4);

        for (int i = 0; i < (int)AbilityType.Count - 2; i++) {
            abilityDetailsList.Add(new AbilityDetail[10]);

            for (int x = 0; x < DataBaseManager.instance.abilityPointTableSOList[i].abilityTablesList.Count; x++) {
                AbilityDetail abilityDetail = Instantiate(abilityDetailPrefab, abilityDetailTrans[i]);
                abilityDetail.SetUpAbilityDetail(x + 1, (AbilityType)i, this);
                abilityDetailsList[i][x] = abilityDetail;
            }
        }
    }

    /// <summary>
    /// �|�b�v�A�b�v��\������
    /// </summary>
    public void ShowPopUp() {
        canvasGroup.blocksRaycasts = true;

        // �����l�ɖ߂�
        Initialize();

        canvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// �|�b�v�A�b�v���\���ɂ���
    /// </summary>
    public void ClosePopUp() {
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(0.0f, 0.5f).SetEase(Ease.Linear);
        stage.SwitchMaskField(true);
    }

    /// <summary>
    /// �A�r���e�B�|�C���g�̕\���X�V
    /// </summary>
    public void UpdateDisplayAbilityPoint() {
        txtAbilityPoint.text = GameData.instance.abilityPoint.ToString();
    }

    /// <summary>
    /// �I������ AbilityDetail ��I�𒆂ɐݒ�
    /// </summary>
    public void SetAbilityDetail(AbilityDetail abilityDetail) {
        currentAbilityDetail = abilityDetail;
        btnSubmit.interactable = true;

        // �I�����Ă��� AbilityDetail �̕\���X�V
        UpdateDisplayAbilityDescription();
    }

    /// <summary>
    /// �I�����Ă��� AbilityDetail �𗘗p���āA�Ή�����\�͂�����
    /// </summary>
    private void PowerUpAbility() {
        btnSubmit.interactable = false;

        switch (currentAbilityDetail.abilityData.abilityType) {
            case AbilityType.Hp:
                GameData.instance.currentCharaData.maxHp += (int)currentAbilityDetail.abilityData.abilityTable.powerUpValue;
                GameData.instance.maxHp = GameData.instance.currentCharaData.maxHp;
                GameData.instance.hp += (int)currentAbilityDetail.abilityData.abilityTable.powerUpValue;
                StartCoroutine(stage.UpdateDisplayHp());
                break;
            case AbilityType.AttackPower:
                GameData.instance.currentCharaData.attackPower += (int)currentAbilityDetail.abilityData.abilityTable.powerUpValue;
                break;
            case AbilityType.MoveSpeed:
                GameData.instance.currentCharaData.moveSpeed += currentAbilityDetail.abilityData.abilityTable.powerUpValue;
                break;
            case AbilityType.ChargeSpeed:
                GameData.instance.currentCharaData.chargeSpeed += (int)currentAbilityDetail.abilityData.abilityTable.powerUpValue;
                break;
        }

        // �����ɕK�v�ȃR�X�g���x����
        GameData.instance.abilityPoint -= currentAbilityDetail.abilityData.abilityTable.abilityCost;

        // �����l�ɖ߂�
        Initialize();
    }

    /// <summary>
    /// ���ׂĂ� AbilityDetail �̃R�X�g�x�����L�����m�F���ă{�^���̊�����/�񊈐����̐؂�ւ�
    /// </summary>
    private void SwitchActivateAbilityDetails() {
        for (int i = 0; i < abilityDetailsList.Count; i++) {
            for (int x = 0; x < abilityDetailsList[i].Length; x++) {
                abilityDetailsList[i][x].JudgeAbilityCost();
            }
        }
    }

    /// <summary>
    /// �I������Ă��� AbilityDetail �̏ڍו\���̍X�V
    /// </summary>
    private void UpdateDisplayAbilityDescription() {

        // �I������Ă��� AbilityDetail �����݂��Ȃ��ꍇ
        if (currentAbilityDetail == null) {
            txtDescription.text = "�A�r���e�B��I�����Ă�������";
        } else {
            txtDescription.text = currentAbilityDetail.abilityData.abilityTable.abilityLevel.ToString();
            txtDescription.text += "/n" + currentAbilityDetail.abilityData.abilityTable.abilityName;
            txtDescription.text += "/n" + currentAbilityDetail.abilityData.abilityType.ToString();
            txtDescription.text += "/n" + "���� : + " + currentAbilityDetail.abilityData.abilityTable.powerUpValue.ToString();
        }
    }

    /// <summary>
    /// �����l�ɖ߂�
    /// </summary>
    private void Initialize() {

        // �R�X�g�̕\���X�V
        UpdateDisplayAbilityPoint();

        currentAbilityDetail = null;

        // �I�����Ă��� AbilityDetail �̕\���X�V
        UpdateDisplayAbilityDescription();

        // �ŐV�̃R�X�g�Ŏx�����\�� AbilityDetail �����邩���`�F�b�N���A�{�^���̊��������s��
        SwitchActivateAbilityDetails();
    }
}
