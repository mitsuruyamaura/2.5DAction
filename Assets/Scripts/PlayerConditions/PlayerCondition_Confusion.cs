using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCondition_Confusion : PlayerConditionBase 
{
    Tween tween;

    /// <summary>
    /// �����ɂ�郉���_���ړ��Ƒ����ݕs�̌��ʂ̂����A�����ݕs�̌���
    /// </summary>
    public override void ApplyEffect() {

        // Stage ���Ő���
        mapMoveController.GetStage().GetInputManager().SwitchActivateSteppingButton(false);

        Debug.Log("Confusion");
    }
}
