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

        // Grid_Walk �� Grid_Collider ��z�u
        int generateValue = 0;

        for (int i = -row; i < row; i++) {
            for (int j = -column; j < column; j++) {
                // ��ԊO���̏ꍇ�ƃv���C���[�̃X�^�[�g�n�_�̏ꍇ
                if (i == -row || i == row - 1 || j == -column || j == column - 1 || (i == 0 && j == 0)) {
                    // �����s�킸�Ɏ��̏�����
                    continue;
                }
                // �����l�p�̃����_���l���擾
                int maxRandomRange = Random.Range(30, 80);

                // �����l�����Z
                generateValue += Random.Range(0, maxRandomRange);

                // �����l�������ڕW�l(��)�𒴂��Ă��Ȃ��ꍇ
                if (generateValue <= 100) {
                    // �����s�킸�Ɏ��̏�����
                    continue;
                }

                // Walk �� Collision �����߂�(���ɁA20 % �̊m���� Collision) 
                if (Random.Range(0, 100) <= 20) {
                    // ���܂������Ń����_���Ƀ^�C�������߂�
                    tileMapCollision.SetTile(new Vector3Int(i, j, 0), fieldCollisionTiles[Random.Range(0, fieldCollisionTiles.Length)]);

                } else {
                    tileMapWalk.SetTile(new Vector3Int(i, j, 0), fieldWalkTiles[Random.Range(0, fieldWalkTiles.Length)]);
                }
             
                // �^�C���𐶐������̂Ő����l�����Z�b�g
                generateValue = 0;
            }
        }

    }

    // �V���{�������

    // List �ɓo�^����

}
