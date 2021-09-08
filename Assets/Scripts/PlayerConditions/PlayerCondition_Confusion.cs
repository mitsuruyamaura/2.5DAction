using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCondition_Confusion : PlayerConditionBase 
{
    Tween tween;

    /// <summary>
    /// 混乱によるランダム移動と足踏み不可の効果のうち、足踏み不可の効果
    /// </summary>
    public override void ApplyEffect() {

        // Stage 側で制御
        mapMoveController.GetStage().GetInputManager().SwitchActivateSteppingButton(false);

        Debug.Log("Confusion");
    }
}
