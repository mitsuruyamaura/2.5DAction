using UnityEngine;

[System.Serializable]
public class StageData
{
    public string stageName;
    public int stageNo;
    public Transform stageIconTran;    // �v���C���[�̃A�C�R���̔z�u�ꏊ
    public int initStamina;            // �X�e�[�W�J�n���̏����X�^�~�i
    public int[] appearEnemyNos;       // �o������G�l�~�[�̎��
    public int bossNo;                 // �o������{�X�̎��
    public int clearBonusPoint;        // �N���A�����Ƃ��̃{�[�i�X

    // TODO ���ɂ�����Βǉ�

}
