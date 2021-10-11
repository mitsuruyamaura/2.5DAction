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

        // ���I�p�̏d�݂̍��v�l���v�Z
        int totalWeight = DataBaseManager.instance.dropItemDatasList.Select(x => x.weight).Sum();

        //Debug.Log(totalWeight);

        // �����_���Ȓl���擾
        int randaomValue = Random.Range(0, totalWeight);

        //Debug.Log(randaomValue);

        // ���I���ʂ̃A�C�e���̕ێ��悤
        AbilityItemDataSO.AbilityItemData getItemData = null;

        // ���I(����̓��A���e�B�������� Weight �݂̂Œ��I)
        foreach (AbilityItemDataSO.AbilityItemData itemData in DataBaseManager.instance.dropItemDatasList) {
            if (randaomValue <= itemData.weight) {
                getItemData = itemData;
                break;
            }
            randaomValue -= itemData.weight;
        }

        // TODO ���o(���݂̓I�[�u�Ɠ�������)
        GameObject effect = Instantiate(EffectManager.instance.orbGetEffectPrefab, effectTran);
        effect.transform.SetParent(EffectManager.instance.effectConteinerTran);

        Destroy(effect, 1.5f);

        // GameData �ɒǉ�
        GameData.instance.AddaAbilityItemDatasList(getItemData.abilityType, getItemData.abitilyNo);

        // TODO �ǂ̃A�C�e�����擾���������|�b�v�A�b�v�\��
        TreasurePopUp treasurePopUp = Instantiate(treasurePopUpPrefab, mapMoveController.GetStage().GetOverlayCanvasTran());

        treasurePopUp.DisplayPopUp(getItemData);

        // ���������Ȃ���j��
        tween = transform.DOScale(0, 0.5f).SetEase(Ease.OutBack).OnComplete(() => { base.OnExitSymbol(); });
    }


    // TODO �G�l�~�[��ɔz�u


}
