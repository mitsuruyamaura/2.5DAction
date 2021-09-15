using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �R���f�B�V�����̃x�[�X�N���X
/// </summary>
public class PlayerConditionBase : MonoBehaviour
{
    [SerializeField]  // Debug
    protected float conditionDuration;�@�@//�@��������

    [SerializeField]  // Debug
    protected float conditionValue;       //  ����  =>  �U���͂𑝌�����l�A�}�b�v�̌�����͈͂𑝌�����l

    protected ConditionEffect conditionEffect;

    protected MapMoveController mapMoveController;
    protected SymbolManager symbolManager;

    protected ConditionType conditionType;   //  ���̏�񂪁A�R���f�B�V�����̓K�p�l�ɂȂ�

    /// <summary>
    /// �R���f�B�V�������Z�b�g����ۂɌĂяo��
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="value"></param>
    public void AddCondition(ConditionType conditionType, float duration, float value, MapMoveController mapMoveController, SymbolManager symbolManager) {
        this.conditionType = conditionType;
        conditionDuration = duration;
        conditionValue = value;
        this.mapMoveController = mapMoveController;
        this.symbolManager = symbolManager;

        OnEnterCondition();
    }

    /// <summary>
    /// �R���f�B�V�����̌��ʂ�K�p
    /// </summary>
    /// <returns></returns>
    protected virtual void OnEnterCondition() {

        // ��������G�t�F�N�g�̃v���t�@�u���擾
        ConditionEffect conditionEffectPrefab = ConditionEffectManager.instance.GetConditionEffect(conditionType);
        Debug.Log(conditionEffectPrefab);

        // �v���t�@�u���擾�ł�����
        if (conditionEffectPrefab != null) {
            // �G�t�F�N�g����
            conditionEffect = Instantiate(conditionEffectPrefab, mapMoveController.GetConditionEffectTran());

            Debug.Log("�G�t�F�N�g���� : " + conditionType.ToString());
        }

        Debug.Log("�R���f�B�V�����t�^");
    }

    /// <summary>
    /// �R���f�B�V�������I������Ƃ��ɌĂяo��
    /// </summary>
    public void RemoveCondition() {
        OnExitCondition();
    }

    protected virtual void OnExitCondition() {

        if (conditionEffect != null) {
            // �G�t�F�N�g�j��
            Destroy(conditionEffect.gameObject);
        }

        Debug.Log("�R���f�B�V�����폜");

        // �R���f�B�V������ List ����폜
        mapMoveController.RemoveConditionsList(this);
    }

    /// <summary>
    /// �R���f�B�V�����̎c�莞�Ԃ̍X�V
    /// </summary>
    public virtual void CalcDuration() {

        // �������Ԃ�����
        conditionDuration--;

        // �R���f�B�V�����̎c�莞�Ԃ��Ȃ��Ȃ�����
        if (conditionDuration <= 0) {

            // �R���f�B�V�������폜���ďI������
            RemoveCondition();
        }
    }

    /// <summary>
    /// �ݒ肳��Ă���R���f�B�V�����̎�ނ��擾
    /// </summary>
    /// <returns></returns>
    public ConditionType GetConditionType() {
        return conditionType;
    }

    /// <summary>
    /// �������Ԃ̉����ƌ��ʂ̏㏑��
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="value"></param>
    public void ExtentionCondition(float duration, float value) {
        conditionDuration += duration;
        conditionValue = value;

        // �R���f�B�V�����̌��ʂ�K�p
        OnEnterCondition();
    }

    /// <summary>
    /// �R���f�B�V�����̌��ʒl���擾
    /// </summary>
    /// <returns></returns>
    public float GetConditionValue() {
        return conditionValue;
    }

    /// <summary>
    /// �R���f�B�V�����̌��ʂ�K�p
    /// </summary>
    public virtual void ApplyEffect() {

        // �ł̃_���[�W�A�U���͔����A�ړ����x�����Ȃǂ�K�p����

        // �l��ω���������ʂ̏ꍇ�́A�������Ԍo�ߌ�� OnExitCondition() ���㏑�����Č��̒l�ɖ߂�����

    }
}
