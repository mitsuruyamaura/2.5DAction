using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class World : MonoBehaviour
{
    [SerializeField]
    private Transform playerTran;

    [SerializeField]
    private Button btnSubmit;

    [SerializeField]
    private StageSelectDetail stageSelectDetailPrefab;

    [SerializeField]
    private Transform stageSelectDetailTran;

    [SerializeField]
    private List<StageSelectDetail> stageSelectDetailsList = new List<StageSelectDetail>();


    void Start() {
        // �X�^�[�g�p�{�^���̐ݒ�
        btnSubmit.onClick.AddListener(OnClickSubmit);
        btnSubmit.interactable = false;

        // �X�e�[�W�I��p�{�^���̐����Ɛݒ�
        for (int i = 0; i < DataBaseManager.instance.stageDataSO.stageDatasList.Count; i++) {
            StageSelectDetail stageSelectDetail = Instantiate(stageSelectDetailPrefab, stageSelectDetailTran, false);
            stageSelectDetail.SetUpStageSelectDetail(DataBaseManager.instance.stageDataSO.stageDatasList[i], this);
            stageSelectDetailsList.Add(stageSelectDetail);

            // TODO �ŏ��̃X�e�[�W�ȊO�́A�N���A���Ă���X�e�[�W�̂ݕ\������
            if (i > 0 && !GameData.instance.clearedStageNos.Contains(i)) {
                stageSelectDetail.SwitchActivateButton(false);
            }
        }
    }

    /// <summary>
    /// �v���C���[�̃A�C�R����z�u
    /// </summary>
    /// <param name="newTran"></param>
    public void SetPlayerTran(Transform newTran) {
        playerTran.localPosition = newTran.position;

        btnSubmit.interactable = true;
    }

    /// <summary>
    /// �Q�[���J�n�p�̃{�^���̏���
    /// </summary>
    public void OnClickSubmit() {
        
        // �I�����Ă���X�e�[�W�̔ԍ����� StageData ���擾
        GameData.instance.currentStageData = DataBaseManager.instance.stageDataSO.stageDatasList.Find(x => x.stageNo == GameData.instance.chooseStageNo);

        // �{�^���A�j�����o
        btnSubmit.transform.DOShakeScale(0.35f, 0.5f, 5)
            .SetEase(Ease.OutQuart)
            .OnComplete(() => 
            {
                // �V�[���J�ڂ̏���
                SceneStateManager.instance.PrepareteNextScene(SceneName.Stage);
            } );
    }
}
