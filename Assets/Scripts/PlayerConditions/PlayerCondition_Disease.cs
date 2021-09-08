using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Disease : PlayerConditionBase {

    private float originValue;

    /// <summary>
    /// �ړ����x����
    /// </summary>
    /// <returns></returns>
    protected override void OnEnterCondition() {
        conditionValue = 0.5f;

        // ���ɖ߂����߂ɕێ�
        originValue = GameData.instance.currentCharaData.moveSpeed;

        // �o�g�����̈ړ����x�𔼌�
        GameData.instance.currentCharaData.moveSpeed *= conditionValue;

        base.OnEnterCondition();
    }

    /// <summary>
    /// �ړ����x��߂�
    /// </summary>
    /// <returns></returns>
    protected override void OnExitCondition() {

        // �ړ����x�����̒l�ɖ߂�
        GameData.instance.currentCharaData.moveSpeed = originValue;

        base.OnExitCondition();
    }
}
