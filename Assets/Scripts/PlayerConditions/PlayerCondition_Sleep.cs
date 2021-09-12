using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCondition_Sleep : PlayerConditionBase
{
    Tween tween;

    /// <summary>
    /// �����ɂ��ړ������̌���(�����݂����ł��Ȃ�)
    /// </summary>
    public override void ApplyEffect() {

        // Stage ���Ő���
        mapMoveController.GetStage().GetInputManager().SwitchActivateMoveButtons(false);

        Debug.Log("Sleep");
    }

    protected override void OnEnterCondition() {

        base.OnEnterCondition();

        tween = conditionEffect.transform.DOLocalMoveY(0.25f, 0.5f).SetEase(Ease.InQuart).SetLoops(-1, LoopType.Yoyo);
    }


    protected override void OnExitCondition() {

        tween.Kill();

        base.OnExitCondition();
    }
}