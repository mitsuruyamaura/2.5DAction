using UnityEngine;

[System.Serializable]
public class StageData
{
    public string stageName;
    public int stageNo;
    public Sprite stageView;           // �X�e�[�W�̔w�i�摜
    public Transform playerIconTran;   // �v���C���[�̃A�C�R���̔z�u�ꏊ
    public int initStamina;            // �X�e�[�W�J�n���̏����X�^�~�i
    public int[] appearEnemyNos;       // �o������G�l�~�[�̎��
    public int bossNo;                 // �o������{�X�̎��
    public int clearBonusPoint;        // �N���A�����Ƃ��̃{�[�i�X
    public StageType stageType;        // �X�e�[�W�̃^�C���}�b�v�̎��

    public OrbType[] orbTypes;         // �o������I�[�u�̎��

    // TODO ���ɂ�����Βǉ�

}
