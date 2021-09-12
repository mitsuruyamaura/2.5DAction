using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectDetail : MonoBehaviour
{
    [SerializeField]
    private Text txtStageSelect;

    [SerializeField]
    private Button btnStageSelectDetail;

    private StageData stageData;

    private World world;

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="stage"></param>
    public void SetUpStageSelectDetail(StageData stageData, World world) {
        this.stageData = stageData;
        this.world = world;

        txtStageSelect.text = this.stageData.stageName;
        btnStageSelectDetail.onClick.AddListener(OnClickStageSelectDetail);
    }

    /// <summary>
    /// �{�^�����������ۂ̏���
    /// </summary>
    private void OnClickStageSelectDetail() {

        // �L�����̃A�C�R�����{�^����ɔz�u
        world.SetPlayerTran(stageData.playerIconTran);

        // �I�����Ă���X�e�[�W�̏����X�V
        GameData.instance.chooseStageNo = stageData.stageNo;
    }

    /// <summary>
    /// �{�^���\���̃I���I�t�؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateButton(bool isSwitch) {
        btnStageSelectDetail.gameObject.SetActive(isSwitch);
    }
}
