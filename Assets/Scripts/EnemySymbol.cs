using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySymbol : SymbolBase
{

    public override void TriggerAppearEffect() {

        transform.DOShakeScale(0.75f, 3.0f).SetEase(Ease.OutQuart);

    }
}
