using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConditionItemSymbol : SymbolBase {

    [SerializeField, Header("持続時間")]
    private float viewDuration;

    [SerializeField, Header("視界の広さの調整値")]
    private float viewScale;


    public override void OnEnterSymbol() {
        base.OnEnterSymbol();

        // フィールドでの演出
        
    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        // Player にコンディションを付与
        mapMoveController.gameObject.AddComponent<PlayerConditionBase>().AddCondition(viewDuration, viewScale);

        base.TriggerAppearEffect(mapMoveController);
    }
}
