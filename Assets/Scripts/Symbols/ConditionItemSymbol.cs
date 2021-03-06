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

        ConditionData conditionData = DataBaseManager.instance.conditionDataSO.conditionDatasList.Find(x=> x.conditionType == conditionType);

        duration = conditionData.duration;
        itemValue = conditionData.conditionValue;

        // フィールドでのエフェクト演出

    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        base.TriggerAppearEffect(mapMoveController);

        // 獲得時のエフェクト演出


        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InExpo);

        // すでに同じコンディションが付与されているか確認
        if (mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == conditionType)) {
            // すでに付与されている場合は、持続時間を更新し、効果は上書きして処理を終了する
            mapMoveController.GetConditionsList().Find(x => x.GetConditionType() == conditionType).ExtentionCondition(duration, itemValue);
            return;
        }

        // 付与するコンディションが睡眠かつ、すでに混乱のコンディションが付与されているときには、睡眠のコンディションは無視する(操作不能になるため)
        if (conditionType == ConditionType.Sleep &&  mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == ConditionType.Confusion)) {
            return;
        }

        // 付与されていないコンディションの場合は、付与する準備する
        PlayerConditionBase playerCondition;

        // Player にコンディションを付与
        playerCondition = conditionType switch {

            ConditionType.View_Wide => mapMoveController.gameObject.AddComponent<PlayerCondition_View>(),
            ConditionType.View_Narrow => mapMoveController.gameObject.AddComponent<PlayerCondition_View>(),
            ConditionType.Hide_Symbols => mapMoveController.gameObject.AddComponent<PlayerCondition_HideSymbol>(),
            ConditionType.Untouchable => mapMoveController.gameObject.AddComponent<PlayerCondition_Untouchable>(),
            ConditionType.Walk_through => mapMoveController.gameObject.AddComponent<PlayerCondition_WalkThrough>(),
            ConditionType.Sleep => mapMoveController.gameObject.AddComponent<PlayerCondition_Sleep>(),
            ConditionType.Confusion => mapMoveController.gameObject.AddComponent<PlayerCondition_Confusion>(),
            ConditionType.Curse => mapMoveController.gameObject.AddComponent<PlayerCondition_Curse>(),
            _ => null
        };

        // 初期設定を実行
        playerCondition.AddCondition(conditionType, duration, itemValue, mapMoveController, symbolManager);

        // コンディション用の List に追加
        mapMoveController.AddConditionsList(playerCondition);

        base.OnExitSymbol();
    }
}
