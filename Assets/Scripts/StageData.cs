using UnityEngine;

[System.Serializable]
public class StageData
{
    public string stageName;
    public int stageNo;
    public Transform stageIconTran;    // プレイヤーのアイコンの配置場所
    public int initStamina;            // ステージ開始時の初期スタミナ
    public int[] appearEnemyNos;       // 出現するエネミーの種類
    public int bossNo;                 // 出現するボスの種類
    public int clearBonusPoint;        // クリアしたときのボーナス

    // TODO 他にもあれば追加

}
