using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �v���C���[�̎��E���L�������苷�������肷��R���f�B�V����
/// </summary>
public class PlayerCondition_View : PlayerConditionBase
{
    [SerializeField]
    private Transform spriteMaskTran;

    private float originScale = 4.5f;            // �}�X�N�̏����T�C�Y
    private float viewAnimeDuration = 0.5f;

    protected override void OnEnterCondition() {
                        
        // �}�X�N�̏����擾
        spriteMaskTran = symbolManager.GetSpriteMaskTransform();

        // �}�X�N�̃X�P�[���𑀍삵�āA���E�̃T�C�Y��ύX
        spriteMaskTran.DOScale(Vector3.one * conditionValue, viewAnimeDuration).SetEase(Ease.InBack);

        base.OnEnterCondition();
    }

    protected override void OnExitCondition() {

        // �I�����̉��o


        // �}�X�N�̃X�P�[���𑀍삵�āA���E�̃T�C�Y�����̃T�C�Y�ɕύX
        spriteMaskTran.DOScale(Vector3.one * originScale, viewAnimeDuration).SetEase(Ease.InBack);

        base.OnExitCondition();
    }
}
