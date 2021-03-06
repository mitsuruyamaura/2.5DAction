using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

/// <summary>
/// シンボルの生成・管理・制御を行うクラス
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

    [SerializeField]　　// Debug 用
    private List<EnemySymbol> enemiesList = new List<EnemySymbol>();

    public List<OrbSymbol> specialSymbols = new List<OrbSymbol>();


    [SerializeField]
    private Transform spriteMaskTran;

    public Tilemap tilemapCollider;   // Grid_Collison の Tilemap をアサイン


    //void Start() {
    //    // Debug
    //    SetUpAllSymbos();
    //}

    /// <summary>
    /// すべてのシンボルの初期設定
    /// </summary>
    public void SetUpAllSymbols() {

        int orbNo = 0;

        // 各シンボルの設定
        for (int i = 0; i < symbolsList.Count; i++) {
            symbolsList[i].transform.SetParent(this.transform);
            symbolsList[i].OnEnterSymbol(this);

            // 特殊シンボルの場合
            if (symbolsList[i].symbolType == SymbolType.Orb) {
                // 追加設定(画像を変えたり、オーブの種類を設定)
                symbolsList[i].GetComponent<OrbSymbol>().SetOrbData(GameData.instance.currentStageData.orbTypes[orbNo], orbNo);
                specialSymbols.Add(symbolsList[i].GetComponent<OrbSymbol>());
                orbNo++;
            }
        }

        // Enemy の種類だけを抽出して List に代入
        enemiesList = GetListSimbolTypeFromSymbolsList(SymbolType.Enemy);

        // オーブの配置用にエネミーの数分の番号を登録
        List<int> numbers = new List<int>();

        for (int i = 0; i < enemiesList.Count; i++) {
            numbers.Add(i);
        }

        //各オーブをランダムなエネミーの上に配置
        for (int i = 0; i < specialSymbols.Count; i++) {
            int randomIndex = Random.Range(0, numbers.Count);
            specialSymbols[i].GetComponent<OrbSymbol>().SetPositionOrbSymbol(enemiesList[randomIndex].transform.position);
            //Debug.Log(randomIndex);
            numbers.RemoveAt(randomIndex);
        }

        ////各オーブをエネミーの上に配置(動くけどイマイチ)
        //int randomIndex = Mathf.FloorToInt(enemiesList.Count / specialSymbols.Count);
        //for (int i = 0; i < specialSymbols.Count; i++) {
        //    if (i == 3 && enemiesList.Count % specialSymbols.Count == 0) {
        //        randomIndex = 0;
        //    }
        //    specialSymbols[i].GetComponent<OrbSymbol>().SetPositionOrbSymbol(enemiesList[randomIndex * (i + 1)].transform.position);
        //}
    }

    /// <summary>
    /// Symbol の List を取得
    /// </summary>
    /// <returns></returns>
    public List<SymbolBase> GetSymbolsList() {
        return symbolsList;
    }

    /// <summary>
    /// すべてのシンボルの画像を表示/非表示
    /// </summary>
    public void SwitchDisplayAllSymbols(bool isSwitch) {

        for (int i = 0; i < symbolsList.Count; i++) {
            symbolsList[i].SwitchDisplaySymbol(isSwitch);
        }
    }

    /// <summary>
    /// 指定された以外のシンボルのゲームオブジェクトの表示/非表示
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
    /// List からシンボルを削除
    /// </summary>
    /// <param name="symbol"></param>
    public void RemoveSymbolsList(SymbolBase symbol) {
        symbolsList.Remove(symbol);
    }

    /// <summary>
    /// List からすべてのシンボルを削除
    /// </summary>
    public void AllClearSymbolsList() {
        symbolsList.Clear();
    }

    /// <summary>
    /// SymbolList から引数で指定した種類のみを抽出する
    /// </summary>
    private List<EnemySymbol> GetListSimbolTypeFromSymbolsList(SymbolType getSymbolType) {
        return symbolsList.Where(x => x.symbolType == getSymbolType).Select(x => x.GetComponent<EnemySymbol>()).ToList();
    }

    /// <summary>
    /// 全エネミーのシンボルの移動処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemisMove() {

        for (int i = 0; i < enemiesList.Count; i++) {

            // プレイヤーと接触しているエネミーは移動させない
            if (enemiesList[i].isSymbolTriggerd) {
                continue;
            }

            enemiesList[i].EnemyMove();
            yield return new WaitForSeconds(0.05f);
            //Debug.Log("敵の移動 :" + i + " 体目");
        }
    }

    /// <summary>
    /// エネミーの List から情報の削除
    /// </summary>
    /// <param name="enemySymbol"></param>
    public void RemoveEnemySymbol(EnemySymbol enemySymbol) {
        enemiesList.Remove(enemySymbol);
    }

    /// <summary>
    /// すべてのエネミーのコライダーを制御
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchEnemyCollider(bool isSwitch) {
        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].SwtichCollider(isSwitch);
        }
    }

    /// <summary>
    /// SpriteMask ゲームオブジェクトの Transfrom を取得
    /// </summary>
    /// <returns></returns>
    public Transform GetSpriteMaskTransform() {
        return spriteMaskTran;
    }
}
