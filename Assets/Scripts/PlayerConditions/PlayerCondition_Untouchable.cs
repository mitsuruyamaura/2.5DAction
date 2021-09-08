using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Untouchable : PlayerConditionBase
{
    protected override void OnEnterCondition() {

        // �w�肳�ꂽ�ȊO�̃V���{�����\���ɂ���
        symbolManager.SwitchActivateExceptSymbols(false, (int)conditionValue);

        base.OnEnterCondition();
    }

    protected override void OnExitCondition() {

        // �I�����̉��o

        // �V���{����\������
        symbolManager.SwitchActivateExceptSymbols(true, (int)conditionValue);

        base.OnExitCondition();
    }
}
