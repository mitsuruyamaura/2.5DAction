using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Confusion : PlayerConditionBase 
{
    [SerializeField]
    private GameObject confusionEffectPrefab;

    private GameObject confusionEffect;

    /// <summary>
    /// 混乱によるランダム移動と足踏み不可の効果のうち、足踏み不可の効果
    /// </summary>
    public override void ApplyEffect() {

        // Stage 側で制御
        mapMoveController.GetStage().GetInputManager().SwitchActivateSteppingButton(false);

        Debug.Log("Confusion");
    }

    protected override IEnumerator OnEnterCondition() {
        // エフェクト生成

        Debug.Log("混乱用のエフェクト　生成");

        return base.OnEnterCondition();
    }


    protected override IEnumerator OnExitCondition() {

        // エフェクト破棄
        Destroy(confusionEffect);

        return base.OnExitCondition();
    }
}
