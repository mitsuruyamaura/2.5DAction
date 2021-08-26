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

    [SerializeField]�@�@// Debug �p
    private List<EnemySymbol> enemiesList = new List<EnemySymbol>();

    //void Start() {
    //    // Debug
    //    SetUpAllSymbos();
    //}

    /// <summary>
    /// ���ׂẴV���{���̏����ݒ�
    /// </summary>
    public void SetUpAllSymbos() {

        List<SymbolBase> specialSymbols = new List<SymbolBase>();

        for (int i = 0; i < symbolsList.Count; i++) {
            symbolsList[i].transform.SetParent(this.transform);
            symbolsList[i].OnEnterSymbol(this);

            if (symbolsList[i].symbolType == SymbolType.Orb) {
                specialSymbols.Add(symbolsList[i]);
            }
        }

        // Enemy �̎�ނ����𒊏o���� List �ɑ��
        enemiesList = GetListSimbolTypeFromSymbolsList(SymbolType.Enemy);

        // �e�I�[�u���G�l�~�[�̏�ɔz�u
        int randomIndex = Mathf.FloorToInt(enemiesList.Count / specialSymbols.Count);
        for (int i = 0; i < specialSymbols.Count; i++) {
            if (i == 3 && enemiesList.Count % specialSymbols.Count == 0) {
                randomIndex = 0;
            }
            specialSymbols[i].GetComponent<OrbSymbol>().SetPositionOrbSymbol(enemiesList[randomIndex * (i + 1)].transform.position);
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

    /// <summary>
    /// SymbolList ��������Ŏw�肵����ނ݂̂𒊏o����
    /// </summary>
    private List<EnemySymbol> GetListSimbolTypeFromSymbolsList(SymbolType getSymbolType) {
        return symbolsList.Where(x => x.symbolType == getSymbolType).Select(x => x.GetComponent<EnemySymbol>()).ToList();
    }

    /// <summary>
    /// �S�G�l�~�[�̃V���{���̈ړ�����
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemisMove() {

        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].EnemyMove();
            yield return new WaitForSeconds(0.05f);
            //Debug.Log("�G�̈ړ� :" + i + " �̖�");
        }
    }

    /// <summary>
    /// �G�l�~�[�� List ������̍폜
    /// </summary>
    /// <param name="enemySymbol"></param>
    public void RemoveEnemySymbol(EnemySymbol enemySymbol) {
        enemiesList.Remove(enemySymbol);
    }

    /// <summary>
    /// ���ׂẴG�l�~�[�̃R���C�_�[�𐧌�
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchEnemyCollider(bool isSwitch) {
        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].SwtichCollider(isSwitch);
        }
    }
}
