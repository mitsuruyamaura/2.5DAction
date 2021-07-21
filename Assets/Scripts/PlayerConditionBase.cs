using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �R���f�B�V�����̃x�[�X�N���X
/// </summary>
public class PlayerConditionBase : MonoBehaviour
{
    protected float conditionDuration;
    protected float conditionValue;

    /// <summary>
    /// �R���f�B�V�������Z�b�g����ۂɌĂяo��
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="value"></param>
    public void AddCondition(float duration, float value) {
        conditionDuration = duration;
        conditionValue = value;

        StartCoroutine(OnEnterCondition());
    }

    protected virtual IEnumerator OnEnterCondition() {
        yield return null;

        Debug.Log("�R���f�B�V�����t�^");
    }

    /// <summary>
    /// �R���f�B�V�������I������Ƃ��ɌĂяo��
    /// </summary>
    public void RemoveCondition() {
        StartCoroutine(OnExitCondition());
    }

    protected virtual IEnumerator OnExitCondition() {
        yield return null;

        Debug.Log("�R���f�B�V�����폜");
    }

    /// <summary>
    /// �R���f�B�V�����̎c�莞�Ԃ̍X�V
    /// </summary>
    public virtual void CalcDuration() {

        // �R���f�B�V�����̎c�莞�Ԃ��Ȃ��Ȃ�����
        if (conditionDuration <= 0) {

            // �R���f�B�V�������폜���ďI������
            RemoveCondition();
        }
    }
}
