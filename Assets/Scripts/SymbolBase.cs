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

    protected Tween tween;

    public bool isSymbolTriggerd;


    protected virtual void Start() {
        OnEnterSymbol();
    }

    /// <summary>
    /// �N�����莞�̃G�t�F�N�g�����p
    /// </summary>
    public virtual void TriggerAppearEffect() {

        if (isSymbolTriggerd) {
            return;
        }

        isSymbolTriggerd = true;

        OnExitSymbol();
    }

    /// <summary>
    /// �V���{���������̏���
    /// </summary>
    public virtual void OnEnterSymbol() {

    }

    protected virtual void OnExitSymbol() {

        if (tween != null) {
            tween.Kill();
        }

        Destroy(gameObject, 1.0f);
    }
}
