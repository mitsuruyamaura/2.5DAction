using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class TreasureBoxSymbol : SymbolBase
{
    [SerializeField]
    private TreasurePopUp treasurePopUpPrefab;

    public override void OnEnterSymbol(SymbolManager symbolManager) {
        base.OnEnterSymbol(symbolManager);

        tween = transform.DOPunchScale(Vector3.one * 0.35f, 3.0f, 1)
            .SetEase(Ease.OutBack)
            .SetLoops(-1, LoopType.Restart);
    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        base.TriggerAppearEffect(mapMoveController);

        // 抽選用の重みの合計値を計算
        int totalWeight = DataBaseManager.instance.dropItemDatasList.Select(x => x.weight).Sum();

        //Debug.Log(totalWeight);

        // ランダムな値を取得
        int randaomValue = Random.Range(0, totalWeight);

        //Debug.Log(randaomValue);

        // 抽選結果のアイテムの保持よう
        AbilityItemDataSO.AbilityItemData getItemData = null;

        // 抽選(今回はレアリティも混合で Weight のみで抽選)
        foreach (AbilityItemDataSO.AbilityItemData itemData in DataBaseManager.instance.dropItemDatasList) {
            if (randaomValue <= itemData.weight) {
                getItemData = itemData;
                break;
            }
            randaomValue -= itemData.weight;
        }

        // TODO 演出(現在はオーブと同じもの)
        GameObject effect = Instantiate(EffectManager.instance.orbGetEffectPrefab, effectTran);
        effect.transform.SetParent(EffectManager.instance.effectConteinerTran);

        Destroy(effect, 1.5f);

        // GameData に追加
        GameData.instance.AddaAbilityItemDatasList(getItemData.abilityType, getItemData.abitilyNo);

        // TODO どのアイテムを取得したかをポップアップ表示
        TreasurePopUp treasurePopUp = Instantiate(treasurePopUpPrefab, mapMoveController.GetStage().GetOverlayCanvasTran());

        treasurePopUp.DisplayPopUp(getItemData);

        // 小さくしながら破棄
        tween = transform.DOScale(0, 0.5f).SetEase(Ease.OutBack).OnComplete(() => { base.OnExitSymbol(); });
    }


    // TODO エネミー上に配置


}
