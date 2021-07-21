using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_View : PlayerConditionBase
{
    [SerializeField]
    private Transform spriteMaskTran;

    private float originScale;

    protected override IEnumerator OnEnterCondition() {

        // �l�����̉��o

        spriteMaskTran = Camera.main.transform.GetChild(0).transform;

        originScale = spriteMaskTran.localScale.x;

        spriteMaskTran.localScale = Vector3.one * conditionValue;

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


        spriteMaskTran.localScale = Vector3.one * originScale;

        return base.OnExitCondition();
    }
}
