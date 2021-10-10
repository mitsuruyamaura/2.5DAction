using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrbSymbol : SymbolBase {

    [SerializeField]
    private int bonusStaminaPoint;

    [SerializeField]
    private OrbType orbType;


    public override void OnEnterSymbol(SymbolManager symbolManager) {
        base.OnEnterSymbol(symbolManager);

        tween = transform.DORotate(new Vector3(0, 360, 0), 3.0f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        tween.Kill();

        base.TriggerAppearEffect(mapMoveController);

        GameData.instance.orbs[no] = true;

        // TODO ボーナス
        GameData.instance.staminaPoint.Value += bonusStaminaPoint;

        GameObject effect = Instantiate(EffectManager.instance.orbGetEffectPrefab, effectTran);
        effect.transform.SetParent(EffectManager.instance.effectConteinerTran);

        Destroy(effect, 1.5f);

        tween = transform.DOScale(0, 0.5f).SetEase(Ease.InQuart).OnComplete(() => { base.OnExitSymbol(); });        
    }

    /// <summary>
    /// エネミーの上に配置のための移動
    /// </summary>
    /// <param name="newPos"></param>
    public void SetPositionOrbSymbol(Vector3 newPos) {
        // 配置移動中はキャラが取れないようにする
        BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
        boxCol.enabled = false;
        transform.DOMove(newPos, 1.0f).SetEase(Ease.InQuart).OnComplete(() => { boxCol.enabled = true; });
    }
}
