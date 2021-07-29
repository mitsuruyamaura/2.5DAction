using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

/// <summary>
/// �V���{���̐����E�Ǘ��E������s���N���X
/// </summary>
public class SymbolManager : MonoBehaviour
{
    [SerializeField]
    private List<SymbolBase> symbolsList = new List<SymbolBase>();

    public List<SymbolBase> SymbolsList
    {
        set => symbolsList = value;
        get => symbolsList;
    }


    //void Start() {
    //    // Debug
    //    SetUpAllSymbos();
    //}

    /// <summary>
    /// ���ׂẴV���{���̏����ݒ�
    /// </summary>
    public void SetUpAllSymbos() {
        for (int i = 0; i < symbolsList.Count; i++) {
            symbolsList[i].transform.SetParent(this.transform);
            symbolsList[i].OnEnterSymbol(this);
        }
    }

    /// <summary>
    /// Symbol �� List ���擾
    /// </summary>
    /// <returns></returns>
    public List<SymbolBase> GetSymbolsList() {
        return symbolsList;
    }

    /// <summary>
    /// ���ׂẴV���{���̉摜��\��/��\��
    /// </summary>
    public void SwitchDisplayAllSymbols(bool isSwitch) {

        for (int i = 0; i < symbolsList.Count; i++) {
            symbolsList[i].SwitchDisplaySymbol(isSwitch);
        }
    }

    /// <summary>
    /// �w�肳�ꂽ�ȊO�̃V���{���̃Q�[���I�u�W�F�N�g�̕\��/��\��
    /// </summary>
    public void SwitchActivateExceptSymbols(bool isSwitch, int exceptSymbolTypeNo) {


        //for (int i = 0; i < symbolsList.Count; i++) {
        //    if (symbolsList[i].symbolType != exceptSymbolType) {
        //        symbolsList[i].SwitchActivateSymbol(isSwitch);
        //    }
        //}

        foreach(SymbolBase symbol in symbolsList.Where(x => x.symbolType != (SymbolType)exceptSymbolTypeNo)) {
            symbol.SwitchActivateSymbol(isSwitch);
        }
    }

    /// <summary>
    /// List ����V���{�����폜
    /// </summary>
    /// <param name="symbol"></param>
    public void RemoveSymbolsList(SymbolBase symbol) {
        symbolsList.Remove(symbol);
    }

    /// <summary>
    /// List ���炷�ׂẴV���{�����폜
    /// </summary>
    public void AllClearSymbolsList() {
        symbolsList.Clear();
    }
}
