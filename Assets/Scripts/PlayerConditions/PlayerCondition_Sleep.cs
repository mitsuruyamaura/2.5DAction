using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Sleep : PlayerConditionBase
{
    /// <summary>
    /// ‡–°‚É‚æ‚éˆÚ“®§ŒÀ‚ÌŒø‰Ê(‘«“¥‚İ‚µ‚©‚Å‚«‚È‚¢)
    /// </summary>
    public override void ApplyEffect() {

        // Stage ‘¤‚Å§Œä
        mapMoveController.GetStage().GetInputManager().SwitchActivateMoveButtons(false);

        Debug.Log("Sleep");
    }
}
