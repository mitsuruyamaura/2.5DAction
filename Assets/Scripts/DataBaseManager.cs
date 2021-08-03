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
    /// 次のレベルアップに必要な経験値を計算して取得
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int CalcNextLevelExp(int level) {
        return expTableSO.expTablesList[level].maxExp;
    }

    /// <summary>
    /// SpriteMask ゲームオブジェクトの Transfrom を取得
    /// </summary>
    /// <returns></returns>
    public Transform GetSpriteMaskTransform() {
        return spriteMaskTran;
    }
}
