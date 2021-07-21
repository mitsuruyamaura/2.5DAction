using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_View : PlayerConditionBase
{
    [SerializeField]
    private Transform spriteMaskTran;

    private float originScale;

    protected override IEnumerator OnEnterCondition() {

        // 獲得時の演出

        spriteMaskTran = Camera.main.transform.GetChild(0).transform;

        originScale = spriteMaskTran.localScale.x;

        spriteMaskTran.localScale = Vector3.one * conditionValue;

        return base.OnEnterCondition();
    }

    /// <summary>
    /// コンディションの残り時間の更新
    /// </summary>
    public override void CalcDuration() {

        conditionDuration--;

        base.CalcDuration();
    }

    protected override IEnumerator OnExitCondition() {

        // 終了時の演出


        spriteMaskTran.localScale = Vector3.one * originScale;

        return base.OnExitCondition();
    }
}
