using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCondition_HideSymbol : PlayerConditionBase
{
    protected override IEnumerator OnEnterCondition() {

        // �V���{���̉摜���\���ɂ���
        symbolManager.SwitchDisplayAllSymbols(false);
 
        return base.OnEnterCondition();
    }

    protected override IEnumerator OnExitCondition() {

        // �I�����̉��o

        // �V���{���̉摜��\������
        symbolManager.SwitchDisplayAllSymbols(true);

        return base.OnExitCondition();
    }
}
