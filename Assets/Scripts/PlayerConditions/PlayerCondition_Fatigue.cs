using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Fatigue : PlayerConditionBase
{
    private int originValue;

    /// <summary>
    /// UŒ‚—Í”¼Œ¸
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator OnEnterCondition() {
        conditionValue = 0.5f;

        // Œ³‚É–ß‚·‚½‚ß‚É•Û
        originValue = GameData.instance.currentCharaData.attackPower;

        // ƒoƒgƒ‹‚ÌUŒ‚—Í‚ğ”¼Œ¸
        GameData.instance.currentCharaData.attackPower = Mathf.FloorToInt(GameData.instance.currentCharaData.attackPower * conditionValue);

        return base.OnEnterCondition();
    }

    /// <summary>
    /// UŒ‚—Í‚ğ–ß‚·
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator OnExitCondition() {

        // UŒ‚—Í‚ğŒ³‚Ì’l‚É–ß‚·
        GameData.instance.currentCharaData.attackPower = originValue;

        return base.OnExitCondition();
    }
}
