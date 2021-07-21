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


    public override void OnEnterSymbol() {
        base.OnEnterSymbol();

        // フィールドでのエフェクト演出

    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        // 獲得時のエフェクト演出


        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InExpo);

        PlayerConditionBase playerCondition;

        // Player にコンディションを付与
        playerCondition = conditionType switch {

            ConditionType.View => mapMoveController.gameObject.AddComponent<PlayerCondition_View>(),

            _ => null
        };

        // 初期設定を実行
        playerCondition.AddCondition(duration, itemValue, mapMoveController);

        // コンディション用の List に追加
        mapMoveController.AddConditionsList(playerCondition);

        base.TriggerAppearEffect(mapMoveController);
    }
}
