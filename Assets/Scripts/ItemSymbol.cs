using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ItemSymbol : SymbolBase
{
    [SerializeField]
    private int recoverStamina;

    [SerializeField]
    private Text txtRecoverStamina;

    [SerializeField]
    private Transform effectTran;


    public override void OnEnterSymbol() {
        base.OnEnterSymbol();

        txtRecoverStamina.text = recoverStamina.ToString();

        tween = transform.GetChild(0).transform.DORotate(new Vector3(0, 0, Random.Range(15, 30)), 1.5f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public override void TriggerAppearEffect() {

        GameObject effect = Instantiate(EffectManager.instance.recoverStaminaEffectPrefab, effectTran);
        effect.transform.SetParent(EffectManager.instance.effectConteinerTran);

        Destroy(effect, 1.5f);

        GameData.instance.staminaPoint.Value += recoverStamina;

        transform.DOScale(0, 0.5f).SetEase(Ease.InQuart);

        base.TriggerAppearEffect();
    }
}
