using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Fatigue : PlayerConditionBase
{
    private int originValue;

    /// <summary>
    /// �U���͔���
    /// </summary>
    /// <returns></returns>
    protected override void OnEnterCondition() {
        conditionValue = 0.5f;

        // ���ɖ߂����߂ɕێ�
        originValue = GameData.instance.currentCharaData.attackPower;

        // �o�g�����̍U���͂𔼌�
        GameData.instance.currentCharaData.attackPower = Mathf.FloorToInt(GameData.instance.currentCharaData.attackPower * conditionValue);

        base.OnEnterCondition();
    }

    /// <summary>
    /// �U���͂�߂�
    /// </summary>
    /// <returns></returns>
    protected override void OnExitCondition() {

        // �U���͂����̒l�ɖ߂�
        GameData.instance.currentCharaData.attackPower = originValue;

        base.OnExitCondition();
    }
}
