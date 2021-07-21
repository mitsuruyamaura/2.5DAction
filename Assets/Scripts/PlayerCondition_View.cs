using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCondition_View : PlayerConditionBase
{
    [SerializeField]
    private Transform spriteMaskTran;

    private float originScale;
    private float viewAnimeDuration = 0.5f;

    protected override IEnumerator OnEnterCondition() {
                        
        // �}�X�N�̏����擾
        spriteMaskTran = DataBaseManager.instance.GetSpriteMaskTransform();

        // ���݂̃}�X�N�̃T�C�Y��ێ�
        originScale = spriteMaskTran.localScale.x;

        // �}�X�N�̃X�P�[���𑀍삵�āA���E�̃T�C�Y��ύX
        spriteMaskTran.DOScale(Vector3.one * conditionValue, viewAnimeDuration).SetEase(Ease.InBack);

        return base.OnEnterCondition();
    }

    /// <summary>
    /// �R���f�B�V�����̎c�莞�Ԃ̍X�V
    /// </summary>
    public override void CalcDuration() {

        conditionDuration--;

        base.CalcDuration();
    }

    protected override IEnumerator OnExitCondition() {

        // �I�����̉��o


        // �}�X�N�̃X�P�[���𑀍삵�āA���E�̃T�C�Y��ύX
        spriteMaskTran.DOScale(Vector3.one * originScale, viewAnimeDuration).SetEase(Ease.InBack);

        return base.OnExitCondition();
    }
}
