using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConditionItemSymbol : SymbolBase {

    [SerializeField, Header("��������")]
    private float viewDuration;

    [SerializeField, Header("���E�̍L���̒����l")]
    private float viewScale;


    public override void OnEnterSymbol() {
        base.OnEnterSymbol();

        // �t�B�[���h�ł̉��o
        
    }

    public override void TriggerAppearEffect(MapMoveController mapMoveController) {

        // Player �ɃR���f�B�V������t�^
        mapMoveController.gameObject.AddComponent<PlayerConditionBase>().AddCondition(viewDuration, viewScale);

        base.TriggerAppearEffect(mapMoveController);
    }
}
