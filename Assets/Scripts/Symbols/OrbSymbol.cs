using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrbSymbol : SymbolBase {

    [SerializeField]
    private int bonusStaminaPoint;

    public override void OnEnterSymbol(SymbolManager symbolManager) {
        base.OnEnterSymbol(symbolManager);

        tween = transform.DORotate(new Vector3(0, 360, 0), 3.0f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        GameData.instance.orbs[no] = true;

        // TODO ボーナス
        GameData.instance.staminaPoint.Value += bonusStaminaPoint;

        GameObject effect = Instantiate(EffectManager.instance.orbGetEffectPrefab, effectTran);
        effect.transform.SetParent(EffectManager.instance.effectConteinerTran);

        Destroy(effect, 1.5f);

        transform.DOScale(0, 0.5f).SetEase(Ease.InQuart);

        base.TriggerAppearEffect(mapMoveController);
    }
}
