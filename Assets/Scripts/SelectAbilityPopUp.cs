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

    // �����ʒu���S�ӏ� for �����d�ŉ񂷂̂Ŕz��ɂ���

    // �I�����Ă���A�r���e�B�̕ێ�


    /// <summary>
    /// 
    /// </summary>
    /// <param name="stage"></param>
    public void SetUpSelectAbilityPopUp(Stage stage) {

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        this.stage = stage;

        // �{�^���̐���

        // �����邩�{�^�����Ń`�F�b�N


        // �{�^���Ƀ��\�b�h��o�^
        btnExit.onClick.AddListener(ClosePopUp);


    }

    /// <summary>
    /// �|�b�v�A�b�v��\������
    /// </summary>
    public void ShowPopUp() {
        canvasGroup.blocksRaycasts = true;

        // �ŐV�̒l��\��
        UpdateDisplayAbilityPoint();

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
        txtAbilityPoint.text = GameData.instance.abilityPoint + " / " + GameData.instance.maxAbilityPoint;
    }
}
