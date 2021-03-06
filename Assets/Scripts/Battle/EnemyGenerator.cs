using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private EnemyController[] enemyPrefabs;

    [SerializeField]
    private Transform[] enemyGenerateTrans;

    private Battle battle;

    /// <summary>
    /// 敵の生成と List への登録
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateEnemies(Battle battle) {

        this.battle = battle;

        // ステージのデータがある場合
        if (GameData.instance.currentStageData != null) {

            // 生成するエネミーの種類を新しく登録
            enemyPrefabs = new EnemyController[GameData.instance.currentStageData.appearEnemyNos.Length];

            for (int i = 0; i < GameData.instance.currentStageData.appearEnemyNos.Length; i++) {
                enemyPrefabs[i] = DataBaseManager.instance.enemyDataSO.enemyDatasList.Find(x => x.enemyNo == GameData.instance.currentStageData.appearEnemyNos[i]).enemyPrefab;
            }
        }

        //yield return null;   // これがないと Stage に敵が生成される

        for (int i = 0; i < battle.maxEnemyCount; i++) {

            int index = Random.Range(0, enemyPrefabs.Length);

            EnemyData enemyData = DataBaseManager.instance.enemyDataSO.enemyDatasList.Find(x => x.enemyNo == index);

            // 敵の生成
            EnemyController enemyController = Instantiate(enemyPrefabs[index], GetRandomEnemyPos(index), Quaternion.identity);   // enemyData.enemyPrefab

            // 敵の初期設定
            enemyController.SetUpEnemy(battle, enemyData);

            // List へ登録
            this.battle.AddEnemyFromEnemiesList(enemyController);

            // Debug 
            //enemiesList[0].SetUpEnemy(this);

            yield return new WaitForSeconds(0.25f);
        }
    }

    /// <summary>
    /// ランダムな敵の生成位置の取得
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomEnemyPos(int index) {
        return new Vector3(Random.Range(enemyGenerateTrans[0].position.x, enemyGenerateTrans[1].position.x), enemyPrefabs[index].transform.position.y, Random.Range(enemyGenerateTrans[0].position.z, enemyGenerateTrans[1].position.z));
    }

    /// <summary>
    /// ボスの生成
    /// </summary>
    public void GenerateBoss(Battle battle) {

        this.battle = battle;

        // ボスの情報を取得
        EnemyData enemyData = DataBaseManager.instance.enemyDataSO.enemyDatasList.Find(x => x.enemyNo == GameData.instance.currentStageData.bossNo);

        // ボスの生成
        EnemyController enemyController = Instantiate(enemyData.enemyPrefab, GetRandomEnemyPos(0), Quaternion.identity);

        // ボスの初期設定
        enemyController.SetUpEnemy(battle, enemyData);

        // List へ登録
        battle.AddEnemyFromEnemiesList(enemyController);
    }
}
