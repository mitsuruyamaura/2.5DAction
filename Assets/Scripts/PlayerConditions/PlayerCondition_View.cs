using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// プレイヤーの視界を広くしたり狭くしたりするコンディション
/// </summary>
public class PlayerCondition_View : PlayerConditionBase
{
    [SerializeField]
    private Transform spriteMaskTran;

    private float originScale = 4.5f;            // マスクの初期サイズ
    private float viewAnimeDuration = 0.5f;

    protected override void OnEnterCondition() {
                        
        // マスクの情報を取得
        spriteMaskTran = symbolManager.GetSpriteMaskTransform();

        // マスクのスケールを操作して、視界のサイズを変更
        spriteMaskTran.DOScale(Vector3.one * conditionValue, viewAnimeDuration).SetEase(Ease.InBack);

        base.OnEnterCondition();
    }

    protected override void OnExitCondition() {

        // 終了時の演出


        // マスクのスケールを操作して、視界のサイズを元のサイズに変更
        spriteMaskTran.DOScale(Vector3.one * originScale, viewAnimeDuration).SetEase(Ease.InBack);

        base.OnExitCondition();
    }
}
