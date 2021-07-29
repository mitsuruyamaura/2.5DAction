using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum StageType {
    Field,
    Dungeon,
}

public class StageGenerator : MonoBehaviour
{
    // StageType.Field 用のタイル群
    public Tile[] fieldBaseTiles;
    public Tile[] fieldWalkTiles;
    public Tile[] fieldCollisionTiles;

    // タイルを配置するタイルマップ
    public Tilemap tileMapBase;
    public Tilemap tileMapWalk;
    public Tilemap tileMapCollision;

    // 並べる数
    public int row;      // 行/ 水平(横)方向
    public int column;   // 列/ 垂直(縦)方向


    void Start()
    {
        GenerateStageFromRandomTiles();    
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
                    // 決まった中でランダムにタイルを決める
                    tileMapCollision.SetTile(new Vector3Int(i, j, 0), fieldCollisionTiles[Random.Range(0, fieldCollisionTiles.Length)]);

                } else {
                    tileMapWalk.SetTile(new Vector3Int(i, j, 0), fieldWalkTiles[Random.Range(0, fieldWalkTiles.Length)]);
                }
             
                // タイルを生成したので生成値をリセット
                generateValue = 0;
            }
        }

    }

    // シンボルを作る

    // List に登録する

}
