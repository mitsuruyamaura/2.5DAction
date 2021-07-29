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



    }

    // シンボルを作る

    // List に登録する

}
