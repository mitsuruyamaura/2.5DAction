using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    public EnemyMoveEventDataSO enemyMoveEventDataSO;

    public ExpTableSO expTableSO;

    [SerializeField]
    private Transform spriteMaskTran;

    public Tilemap tilemapCollider;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���̃��x���A�b�v�ɕK�v�Ȍo���l���v�Z���Ď擾
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int CalcNextLevelExp(int level) {
        return expTableSO.expTablesList[level].maxExp;
    }

    /// <summary>
    /// SpriteMask �Q�[���I�u�W�F�N�g�� Transfrom ���擾
    /// </summary>
    /// <returns></returns>
    public Transform GetSpriteMaskTransform() {
        return spriteMaskTran;
    }
}
