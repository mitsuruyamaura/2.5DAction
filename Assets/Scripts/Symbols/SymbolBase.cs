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
    /// 侵入判定時のエフェクト生成用
    /// </summary>
    public virtual void TriggerAppearEffect(MapMoveController mapMoveController) {

        if (isSymbolTriggerd) {
            return;
        }

        isSymbolTriggerd = true;
    }

    /// <summary>
    /// シンボル生成時の処理
    /// </summary>
    public virtual void OnEnterSymbol(SymbolManager symbolManager) {
        this.symbolManager = symbolManager;
    }

    protected virtual void OnExitSymbol() {

        if (tween != null) {
            tween.Kill();
        }

        // List からシンボルを削除
        symbolManager.RemoveSymbolsList(this);

        Destroy(gameObject);
    }

    /// <summary>
    /// シンボル画像の表示/非表示切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchDisplaySymbol(bool isSwitch) {
        spriteSymbol.enabled = isSwitch;
    }

    /// <summary>
    /// シンボルのゲームオブジェクトの表示/非表示
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateSymbol(bool isSwitch) {
        gameObject.SetActive(isSwitch);
    }
}
