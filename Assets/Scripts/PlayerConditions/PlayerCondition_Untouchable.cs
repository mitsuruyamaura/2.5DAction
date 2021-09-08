using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Untouchable : PlayerConditionBase
{
    protected override void OnEnterCondition() {

        // 指定された以外のシンボルを非表示にする
        symbolManager.SwitchActivateExceptSymbols(false, (int)conditionValue);

        base.OnEnterCondition();
    }

    protected override void OnExitCondition() {

        // 終了時の演出

        // シンボルを表示する
        symbolManager.SwitchActivateExceptSymbols(true, (int)conditionValue);

        base.OnExitCondition();
    }
}
