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
                        
        // マスクの情報を取得
        spriteMaskTran = DataBaseManager.instance.GetSpriteMaskTransform();

        // 現在のマスクのサイズを保持
        originScale = spriteMaskTran.localScale.x;

        // マスクのスケールを操作して、視界のサイズを変更
        spriteMaskTran.DOScale(Vector3.one * conditionValue, viewAnimeDuration).SetEase(Ease.InBack);

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


        // マスクのスケールを操作して、視界のサイズを変更
        spriteMaskTran.DOScale(Vector3.one * originScale, viewAnimeDuration).SetEase(Ease.InBack);

        return base.OnExitCondition();
    }
}
