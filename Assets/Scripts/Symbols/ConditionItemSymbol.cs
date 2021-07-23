using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConditionItemSymbol : SymbolBase {

    [SerializeField]
    private ConditionType conditionType;

    [SerializeField, Header("��������")]
    private float duration;

    [SerializeField, Header("���ʒl")]
    private float itemValue;


    public override void OnEnterSymbol(SymbolManager symbolManager) {
        base.OnEnterSymbol(symbolManager);

        // �t�B�[���h�ł̃G�t�F�N�g���o

    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        // �l�����̃G�t�F�N�g���o


        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InExpo);

        // ���łɓ����R���f�B�V�������t�^����Ă��邩�m�F
        if (mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == conditionType)) {
            // ���łɕt�^����Ă���ꍇ�́A�������Ԃ��X�V���A���ʂ͏㏑�����ď������I������
            mapMoveController.GetConditionsList().Find(x => x.GetConditionType() == conditionType).ExtentionCondition(duration, itemValue);
            return;
        }

        // �t�^����Ă��Ȃ��R���f�B�V�����̏ꍇ�́A�t�^���鏀������
        PlayerConditionBase playerCondition;

        // Player �ɃR���f�B�V������t�^
        playerCondition = conditionType switch {

            ConditionType.View => mapMoveController.gameObject.AddComponent<PlayerCondition_View>(),
            ConditionType.Hide_Symbols => mapMoveController.gameObject.AddComponent<PlayerCondition_HideSymbol>(),
            ConditionType.Untouchable => mapMoveController.gameObject.AddComponent<PlayerCondition_Untouchable>(),
            ConditionType.Walk_through => mapMoveController.gameObject.AddComponent<PlayerCondition_WalkThrough>(),
            _ => null
        };

        // �����ݒ�����s
        playerCondition.AddCondition(conditionType, duration, itemValue, mapMoveController, symbolManager);

        // �R���f�B�V�����p�� List �ɒǉ�
        mapMoveController.AddConditionsList(playerCondition);

        base.TriggerAppearEffect(mapMoveController);
    }
}
