using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Untouchable : PlayerConditionBase
{
    protected override IEnumerator OnEnterCondition() {

        // �w�肳�ꂽ�ȊO�̃V���{�����\���ɂ���
        symbolManager.SwitchActivateExceptSymbols(false, (int)conditionValue);

        return base.OnEnterCondition();
    }

    protected override IEnumerator OnExitCondition() {

        // �I�����̉��o

        // �V���{����\������
        symbolManager.SwitchActivateExceptSymbols(true, (int)conditionValue);

        return base.OnExitCondition();
    }
}
