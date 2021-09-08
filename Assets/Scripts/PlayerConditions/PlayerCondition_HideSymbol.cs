using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCondition_HideSymbol : PlayerConditionBase
{
    protected override void OnEnterCondition() {

        // �V���{���̉摜���\���ɂ���
        symbolManager.SwitchDisplayAllSymbols(false);
 
        base.OnEnterCondition();
    }

    protected override void OnExitCondition() {

        // �I�����̉��o

        // �V���{���̉摜��\������
        symbolManager.SwitchDisplayAllSymbols(true);

        base.OnExitCondition();
    }
}
