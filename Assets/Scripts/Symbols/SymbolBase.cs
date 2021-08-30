using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SymbolBase : MonoBehaviour
{
    public SymbolType symbolType;
    public int no;

    [SerializeField]
    protected Transform effectTran;

    [SerializeField]
    protected SpriteRenderer spriteSymbol;

    protected Tween tween;

    protected SymbolManager symbolManager;

    public bool isSymbolTriggerd;


    //protected virtual void Start() {
    //    OnEnterSymbol();
    //}

    /// <summary>
    /// �N�����莞�̃G�t�F�N�g�����p
    /// </summary>
    public virtual void TriggerAppearEffect(MapMoveController mapMoveController) {

        if (isSymbolTriggerd) {
            return;
        }

        isSymbolTriggerd = true;
    }

    /// <summary>
    /// �V���{���������̏���
    /// </summary>
    public virtual void OnEnterSymbol(SymbolManager symbolManager) {
        this.symbolManager = symbolManager;
    }

    protected virtual void OnExitSymbol() {

        if (tween != null) {
            tween.Kill();
        }

        // List ����V���{�����폜
        symbolManager.RemoveSymbolsList(this);

        Destroy(gameObject);
    }

    /// <summary>
    /// �V���{���摜�̕\��/��\���؂�ւ�
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchDisplaySymbol(bool isSwitch) {
        spriteSymbol.enabled = isSwitch;
    }

    /// <summary>
    /// �V���{���̃Q�[���I�u�W�F�N�g�̕\��/��\��
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateSymbol(bool isSwitch) {
        gameObject.SetActive(isSwitch);
    }
}
