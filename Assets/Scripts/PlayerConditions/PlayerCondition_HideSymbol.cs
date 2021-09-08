using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCondition_HideSymbol : PlayerConditionBase
{
    protected override void OnEnterCondition() {

        // シンボルの画像を非表示にする
        symbolManager.SwitchDisplayAllSymbols(false);
 
        base.OnEnterCondition();
    }

    protected override void OnExitCondition() {

        // 終了時の演出

        // シンボルの画像を表示する
        symbolManager.SwitchDisplayAllSymbols(true);

        base.OnExitCondition();
    }
}
