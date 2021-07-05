using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SymbolBase : MonoBehaviour
{
    public SymbolType symbolType;
    public int no;

    protected Tween tween;

    protected virtual void Start() {
        OnEnterSymbol();
    }

    /// <summary>
    /// 侵入判定時のエフェクト生成用
    /// </summary>
    public virtual void TriggerAppearEffect() {
        OnExitSymbol();
    }

    /// <summary>
    /// シンボル生成時の処理
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
