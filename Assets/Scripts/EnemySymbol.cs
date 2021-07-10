using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySymbol : SymbolBase
{

    public override void TriggerAppearEffect() {

        base.TriggerAppearEffect();

        Debug.Log("移動先で敵に接触");

        transform.DOShakeScale(0.75f, 1.0f)
            .SetEase(Ease.OutQuart)
            .OnComplete(() => { PreparateBattle(); } );
    }

    /// <summary>
    /// バトルの準備
    /// </summary>
    private void PreparateBattle() {



        // TODO バトル前に座標情報を GameData に保持


        // TODO エフェクトや SE


        // TODO 敵の情報を取得


        // TODO シーン遷移

        SceneStateManager.instance.LoadBattleScene();
    }
}
