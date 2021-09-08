using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCondition_Sleep : PlayerConditionBase
{
    Tween tween;

    /// <summary>
    /// ‡–°‚É‚æ‚éˆÚ“®§ŒÀ‚ÌŒø‰Ê(‘«“¥‚İ‚µ‚©‚Å‚«‚È‚¢)
    /// </summary>
    public override void ApplyEffect() {

        // Stage ‘¤‚Å§Œä
        mapMoveController.GetStage().GetInputManager().SwitchActivateMoveButtons(false);

        Debug.Log("Sleep");
    }

    protected override IEnumerator OnEnterCondition() {

        yield return base.OnEnterCondition();

        tween = conditionEffect.transform.DOLocalMoveY(0.25f, 0.5f).SetEase(Ease.InQuart).SetLoops(-1, LoopType.Yoyo);
    }


    protected override IEnumerator OnExitCondition() {

        tween.Kill();

        return base.OnExitCondition();
    }
}
