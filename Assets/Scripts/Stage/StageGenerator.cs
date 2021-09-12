using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public enum StageType {
    Field,
    Dungeon,
}

[System.Serializable]
public struct SymbolGenerateData {
    // ��������V���{���̃v���t�@�u��o�^
    public SymbolBase symbolBasePrefab;
    public int symbolWeight;
}

public class StageGenerator : MonoBehaviour
{
    // StageType.Field �p�̃^�C���Q
    [SerializeField] private Tile[] fieldBaseTiles;
    [SerializeField] private Tile[] fieldWalkTiles;
    [SerializeField] private Tile[] fieldCollisionTiles;

    // �^�C����z�u����^�C���}�b�v
    [SerializeField] private Tilemap tileMapBase;
    [SerializeField] private Tilemap tileMapWalk;
    [SerializeField] private Tilemap tileMapCollision;

    // ���ׂ鐔
    [SerializeField] private int row;      // �s/ ����(��)����
    [SerializeField] private int column;   // ��/ ����(�c)����

    // �V���{�������p�̃f�[�^���X�g
    [SerializeField] private List<SymbolGenerateData> symbolGenerateDatasList = new List<SymbolGenerateData>();
    [SerializeField] private List<SymbolGenerateData> specialSymbolGenerateDatasList = new List<SymbolGenerateData>();

    [SerializeField, Header("�V���{���̐�����"), Range(0, 100)] private int generateSymbolRate;



    //void Start() {
    //    // Debug �p
    //    GenerateStageFromRandomTiles();
    //    GenerateSymbols(-1);
    //}

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
                int maxRandomRange = UnityEngine.Random.Range(30, 80);

                // �����l�����Z
                generateValue += UnityEngine.Random.Range(0, maxRandomRange);

                // �����l�������ڕW�l(��)�𒴂��Ă��Ȃ��ꍇ
                if (generateValue <= 100) {
                    // �����s�킸�Ɏ��̏�����
                    continue;
                }

                // Walk �� Collision �����߂�(���ɁA20 % �̊m���� Collision) 
                if (UnityEngine.Random.Range(0, 100) <= 20) {
                    // Collision �p�̃^�C���̒��Ń����_���Ƀ^�C�������߂�
                    tileMapCollision.SetTile(new Vector3Int(i, j, 0), fieldCollisionTiles[UnityEngine.Random.Range(0, fieldCollisionTiles.Length)]);
                } else {
                    // Walk �p�̃^�C���̒��Ń����_���Ƀ^�C�������߂�
                    tileMapWalk.SetTile(new Vector3Int(i, j, 0), fieldWalkTiles[UnityEngine.Random.Range(0, fieldWalkTiles.Length)]);
                }
             
                // �^�C���𐶐������̂Ő����l�����Z�b�g
                generateValue = 0;
            }
        }
    }

    /// <summary>
    /// �ʏ�̃V���{���������_���ɍ쐬
    /// </summary>
    /// <param name="generateSymbolCount"></param>
    /// <returns></returns>
    public List<SymbolBase> GenerateSymbols(int generateSymbolCount) {
        // List �ɓo�^����
        List<SymbolBase> symbolsList = new List<SymbolBase>();

        // �d�ݕt���̍��v�l���Z�o
        int totalWeight = symbolGenerateDatasList.Select(x => x.symbolWeight).Sum();
        //Debug.Log(totalWeight);

        for (int i = -row +1; i < row -1; i++) {
            for (int j = -column +1; j < column -1; j++) {

                // �v���C���[�̃X�^�[�g�n�_�̏ꍇ
                if (i == 0 && j == 0) {
                    // �����s�킸�Ɏ��̏�����
                    continue;
                }
               
                // �^�C���}�b�v�̍��W�ɕϊ�
                Vector3Int tilePos = tileMapCollision.WorldToCell(new Vector3(i, j, 0));

                // �^�C���� ColliderType �� Grid �ł͂Ȃ����m�F
                if (tileMapCollision.GetColliderType(tilePos) == Tile.ColliderType.Grid) {
                    // Grid �̏ꍇ�ɂ͔z�u���Ȃ��̂ŁA�����s�킸�Ɏ��̏�����
                    continue;
                }

                // 80 % �̓V���{���Ȃ� => 264 �}�X�̏ꍇ�A���35�`55�V���{�����o����
                if (UnityEngine.Random.Range(0, 100) > generateSymbolRate) {
                    continue;
                }

                int index = 0;
                int value = UnityEngine.Random.Range(0, totalWeight);

                // �d�݂Â����琶������V���{�����m�F
                for (int x = 0; x < symbolGenerateDatasList.Count; x++) {
                    if (value <= symbolGenerateDatasList[x].symbolWeight) {
                        index = x;
                        //Debug.Log(index + " value : " + value);
                        break;
                    }
                    value -= symbolGenerateDatasList[x].symbolWeight;
                }

                // ���I���ꂽ�V���{���𐶐�
                symbolsList.Add(Instantiate(symbolGenerateDatasList[index].symbolBasePrefab, new Vector3(i, j, 0), Quaternion.identity));
                
                generateSymbolCount--;

                // generateSymbolCount = -1 �ŃX�^�[�g�̏ꍇ�͒��I�񐔂Ȃ�
                if (generateSymbolCount == 0) {
                    break;
                }
            }
            if (generateSymbolCount == 0) {
                break;
            }
        }

        // �����������X�g��߂�
        return symbolsList;
    }

    /// <summary>
    /// �S�̓���V���{���������_���ȏ��Ԃɍ쐬
    /// </summary>
    /// <returns></returns>
    public List<SymbolBase> GenerateSpecialSymbols() {

        List<SymbolBase> symbolsList = new List<SymbolBase>();

        // �����_���ɕ��ёւ���
        List<SymbolGenerateData> randomSymbolsList = new List<SymbolGenerateData>(specialSymbolGenerateDatasList);
        randomSymbolsList =  randomSymbolsList.OrderBy(x => Guid.NewGuid()).ToList();

        int index = 0;

        // �ŏ��̓L�����̂��΂ɐ���
        for (int x = -1; x < 2; x += 2) {
            for (int y = -1; y < 2; y += 2) {
                symbolsList.Add(Instantiate(randomSymbolsList[index].symbolBasePrefab, new Vector3(x, y, 0), Quaternion.identity));
                index++;
            }
        }
        // List �ɓo�^����
        return symbolsList;
    }    
}
