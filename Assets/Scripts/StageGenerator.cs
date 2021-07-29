using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public enum StageType {
    Field,
    Dungeon,
}

[System.Serializable]
public struct SymbolGenerateData {
    // 生成するシンボルのプレファブを登録
    public SymbolBase symbolBasePrefab;
    public int symbolWeight;
}

public class StageGenerator : MonoBehaviour
{
    // StageType.Field 用のタイル群
    [SerializeField] private Tile[] fieldBaseTiles;
    [SerializeField] private Tile[] fieldWalkTiles;
    [SerializeField] private Tile[] fieldCollisionTiles;

    // タイルを配置するタイルマップ
    [SerializeField] private Tilemap tileMapBase;
    [SerializeField] private Tilemap tileMapWalk;
    [SerializeField] private Tilemap tileMapCollision;

    // 並べる数
    [SerializeField] private int row;      // 行/ 水平(横)方向
    [SerializeField] private int column;   // 列/ 垂直(縦)方向

    // シンボル生成用のデータリスト
    public List<SymbolGenerateData> symbolGenerateDatasList = new List<SymbolGenerateData>();


    void Start()
    {
        GenerateStageFromRandomTiles();
        GenerateSymbols(20);
    }

    /// <summary>
    /// ランダムなタイルをタイルマップに配置してステージを作る
    /// </summary>
    /// <param name="stageType"></param>
    public void GenerateStageFromRandomTiles(StageType stageType = StageType.Field) {

        // Grid_Base と外壁用の Grid_Collider を配置
        for (int i = -row; i < row; i++) {
            for (int j = -column; j < column; j++) {

                switch (stageType) {

                    case StageType.Field:
                        // 一番外側の場合
                        if (i == -row || i == row - 1 || j == -column || j == column -1) {
                            // 壁用のコライダータイルを配置
                            tileMapCollision.SetTile(new Vector3Int(i, j, 0), fieldCollisionTiles[0]);
                        } else {
                            // フィールド用のタイルを配置
                            tileMapBase.SetTile(new Vector3Int(i, j, 0), fieldBaseTiles[0]);
                        }
                        break;

                    case StageType.Dungeon:
                    default:
                        break;
                }
            }
        }

        // Grid_Walk と Grid_Collider を配置
        int generateValue = 0;

        for (int i = -row; i < row; i++) {
            for (int j = -column; j < column; j++) {
                // 一番外側の場合とプレイヤーのスタート地点の場合
                if (i == -row || i == row - 1 || j == -column || j == column - 1 || (i == 0 && j == 0)) {
                    // 何も行わずに次の処理へ
                    continue;
                }

                // 生成値用のランダム値を取得
                int maxRandomRange = Random.Range(30, 80);

                // 生成値を加算
                generateValue += Random.Range(0, maxRandomRange);

                // 生成値が生成目標値(仮)を超えていない場合
                if (generateValue <= 100) {
                    // 何も行わずに次の処理へ
                    continue;
                }

                // Walk か Collision か決める(仮に、20 % の確率で Collision) 
                if (Random.Range(0, 100) <= 20) {
                    // Collision 用のタイルの中でランダムにタイルを決める
                    tileMapCollision.SetTile(new Vector3Int(i, j, 0), fieldCollisionTiles[Random.Range(0, fieldCollisionTiles.Length)]);
                } else {
                    // Walk 用のタイルの中でランダムにタイルを決める
                    tileMapWalk.SetTile(new Vector3Int(i, j, 0), fieldWalkTiles[Random.Range(0, fieldWalkTiles.Length)]);
                }
             
                // タイルを生成したので生成値をリセット
                generateValue = 0;
            }
        }
    }

    // 通常のシンボルを作る

    public List<SymbolBase> GenerateSymbols(int generateSymbolCount) {
        // List に登録する

        List<SymbolBase> symbolsList = new List<SymbolBase>();

        // 重み付けの合計値を算出
        int totalWeight = symbolGenerateDatasList.Select(x => x.symbolWeight).Sum();

        for (int i = -row +1; i < row -1; i++) {
            for (int j = -column +1; j < column -1; j++) {

                // プレイヤーのスタート地点の場合
                if (i == 0 && j == 0) {
                    // 何も行わずに次の処理へ
                    continue;
                }

                // 70 % はシンボルなし
                if (Random.Range(0, 100) > 30) {
                    continue;
                }
               
                // タイルマップの座標に変換
                Vector3Int tilePos = tileMapCollision.WorldToCell(new Vector3(i, j, 0));

                // タイルの ColliderType が Grid ではないか確認
                if (tileMapCollision.GetColliderType(tilePos) == Tile.ColliderType.Grid) {
                    // Grid の場合には配置しないので、何も行わずに次の処理へ
                    continue;
                }

                int index = 0;
                int value = Random.Range(0, totalWeight);
                // 重みづけから生成するシンボルを確認
                for (int x = 0; x < symbolGenerateDatasList.Count; x++) {
                    if (value <= symbolGenerateDatasList[x].symbolWeight) {
                        index = x;
                        break;
                    }
                    value -= symbolGenerateDatasList[x].symbolWeight;
                }

                symbolsList.Add(Instantiate(symbolGenerateDatasList[index].symbolBasePrefab, new Vector3(i, j, 0), Quaternion.identity));
                generateSymbolCount--;

                //if (generateSymbolCount <= 0) {
                //    break;
                //}

            }


        }

        return symbolsList;
    }


    // ４つの特殊シンボルを作る


    // ランダムに並び替える

    // List に登録する
}
