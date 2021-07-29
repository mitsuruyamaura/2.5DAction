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
    // StageType.Field �p�̃^�C���Q
    public Tile[] fieldBaseTiles;
    public Tile[] fieldWalkTiles;
    public Tile[] fieldCollisionTiles;

    // �^�C����z�u����^�C���}�b�v
    public Tilemap tileMapBase;
    public Tilemap tileMapWalk;
    public Tilemap tileMapCollision;

    // ���ׂ鐔
    public int row;      // �s/ ����(��)����
    public int column;   // ��/ ����(�c)����


    void Start()
    {
        GenerateStageFromRandomTiles();    
    }

    /// <summary>
    /// �����_���ȃ^�C�����^�C���}�b�v�ɔz�u���ăX�e�[�W�����
    /// </summary>
    /// <param name="stageType"></param>
    public void GenerateStageFromRandomTiles(StageType stageType = StageType.Field) {

        // Grid_Base �ƊO�Ǘp�� Grid_Collider ��z�u
        for (int i = -row; i < row; i++) {
            for (int j = -column; j < column; j++) {

                switch (stageType) {

                    case StageType.Field:
                        // ��ԊO���̏ꍇ
                        if (i == -row || i == row - 1 || j == -column || j == column -1) {
                            // �Ǘp�̃R���C�_�[�^�C����z�u
                            tileMapCollision.SetTile(new Vector3Int(i, j, 0), fieldCollisionTiles[0]);
                        } else {
                            // �t�B�[���h�p�̃^�C����z�u
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

    // �V���{�������

    // List �ɓo�^����

}
