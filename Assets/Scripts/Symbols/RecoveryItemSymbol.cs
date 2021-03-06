using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RecoveryItemSymbol : SymbolBase
{
    [SerializeField]
    private int recoveryPoint;

    [SerializeField]
    private Text txtRecoveryPoint;


    public override void OnEnterSymbol(SymbolManager symbolManager) {
        base.OnEnterSymbol(symbolManager);

        txtRecoveryPoint.text = recoveryPoint.ToString();

        if (symbolType == SymbolType.Stamina) {
            tween = transform.GetChild(0).transform.DORotate(new Vector3(0, 0, Random.Range(15, 30)), 1.5f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
        } else if (symbolType == SymbolType.Life) {
            tween = transform.GetChild(0).transform.DORotate(new Vector3(0, 0, Random.Range(-15, -30)), 1.5f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
        }     
    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        base.TriggerAppearEffect(mapMoveController);

        GameObject effectPrefab = null;

        if (symbolType == SymbolType.Stamina) {
            effectPrefab = EffectManager.instance.recoveryStaminaEffectPrefab;

            GameData.instance.staminaPoint.Value += recoveryPoint;
        } else if (symbolType == SymbolType.Life) {
            effectPrefab = EffectManager.instance.recoveryLifeEffectPrefab;

            GameData.instance.hp = Mathf.Clamp(GameData.instance.hp += recoveryPoint, 0, GameData.instance.maxHp);
        }

        GameObject effect = Instantiate(effectPrefab, effectTran);
        effect.transform.SetParent(EffectManager.instance.effectConteinerTran);

        Destroy(effect, 1.5f);

        transform.DOScale(0, 0.5f).SetEase(Ease.InQuart).OnComplete(() => { base.OnExitSymbol(); });
    }
}
