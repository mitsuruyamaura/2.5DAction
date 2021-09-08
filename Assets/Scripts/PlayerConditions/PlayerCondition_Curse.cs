using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCondition_Curse : PlayerConditionBase
{
    Tween tween; 

    protected override IEnumerator OnEnterCondition() {

        base.OnEnterCondition();

        tween = conditionEffect.transform.DOLocalMoveY(0.5f, 0.25f).SetEase(Ease.InQuart).SetLoops(-1, LoopType.Yoyo);

        yield break;
    }


    protected override IEnumerator OnExitCondition() {

        tween.Kill();

        // �G�t�F�N�g�j��
        Destroy(conditionEffect);

        return base.OnExitCondition();
    }
}
