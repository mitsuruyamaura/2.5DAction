using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Sleep : PlayerConditionBase
{
    /// <summary>
    /// 睡眠による移動制限の効果(足踏みしかできない)
    /// </summary>
    public override void ApplyEffect() {

        // Stage 側で制御
        mapMoveController.GetStage().GetInputManager().SwitchActivateMoveButtons(false);

        Debug.Log("Sleep");
    }
}
