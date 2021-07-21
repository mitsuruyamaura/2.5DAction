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


    public override void OnEnterSymbol() {
        base.OnEnterSymbol();

        // �t�B�[���h�ł̃G�t�F�N�g���o

    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        // �l�����̃G�t�F�N�g���o


        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InExpo);

        PlayerConditionBase playerCondition;

        // Player �ɃR���f�B�V������t�^
        playerCondition = conditionType switch {

            ConditionType.View => mapMoveController.gameObject.AddComponent<PlayerCondition_View>(),

            _ => null
        };

        // �����ݒ�����s
        playerCondition.AddCondition(duration, itemValue, mapMoveController);

        // �R���f�B�V�����p�� List �ɒǉ�
        mapMoveController.AddConditionsList(playerCondition);

        base.TriggerAppearEffect(mapMoveController);
    }
}
