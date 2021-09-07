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
    protected override IEnumerator OnEnterCondition() {
        conditionValue = 0.5f;

        // ���ɖ߂����߂ɕێ�
        originValue = GameData.instance.currentCharaData.attackPower;

        // �o�g�����̍U���͂𔼌�
        GameData.instance.currentCharaData.attackPower = Mathf.FloorToInt(GameData.instance.currentCharaData.attackPower * conditionValue);

        return base.OnEnterCondition();
    }

    /// <summary>
    /// �U���͂�߂�
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator OnExitCondition() {

        // �U���͂����̒l�ɖ߂�
        GameData.instance.currentCharaData.attackPower = originValue;

        return base.OnExitCondition();
    }
}
