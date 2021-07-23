using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConditionItemSymbol : SymbolBase {

    [SerializeField]
    private ConditionType conditionType;

    [SerializeField, Header("持続時間")]
    private float duration;

    [SerializeField, Header("効果値")]
    private float itemValue;


    public override void OnEnterSymbol(SymbolManager symbolManager) {
        base.OnEnterSymbol(symbolManager);

        // フィールドでのエフェクト演出

    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        // 獲得時のエフェクト演出


        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InExpo);

        // すでに同じコンディションが付与されているか確認
        if (mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == conditionType)) {
            // すでに付与されている場合は、持続時間を更新し、効果は上書きして処理を終了する
            mapMoveController.GetConditionsList().Find(x => x.GetConditionType() == conditionType).ExtentionCondition(duration, itemValue);
            return;
        }

        // 付与されていないコンディションの場合は、付与する準備する
        PlayerConditionBase playerCondition;

        // Player にコンディションを付与
        playerCondition = conditionType switch {

            ConditionType.View => mapMoveController.gameObject.AddComponent<PlayerCondition_View>(),
            ConditionType.Hide_Symbols => mapMoveController.gameObject.AddComponent<PlayerCondition_HideSymbol>(),
            ConditionType.Untouchable => mapMoveController.gameObject.AddComponent<PlayerCondition_Untouchable>(),
            ConditionType.Walk_through => mapMoveController.gameObject.AddComponent<PlayerCondition_WalkThrough>(),
            _ => null
        };

        // 初期設定を実行
        playerCondition.AddCondition(conditionType, duration, itemValue, mapMoveController, symbolManager);

        // コンディション用の List に追加
        mapMoveController.AddConditionsList(playerCondition);

        base.TriggerAppearEffect(mapMoveController);
    }
}
