using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Sleep : PlayerConditionBase
{
    /// <summary>
    /// �����ɂ��ړ������̌���(�����݂����ł��Ȃ�)
    /// </summary>
    public override void ApplyEffect() {

        // Stage ���Ő���
        mapMoveController.GetStage().GetInputManager().SwitchActivateMoveButtons(false);

        Debug.Log("Sleep");
    }
}
