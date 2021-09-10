using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
