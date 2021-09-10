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
    /// �G�̐����� List �ւ̓o�^
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateEnemies(Battle battle) {

        this.battle = battle;

        //yield return null;   // ���ꂪ�Ȃ��� Stage �ɓG�����������

        for (int i = 0; i < battle.maxEnemyCount; i++) {

            int index = Random.Range(0, enemyPrefabs.Length);

            EnemyData enemyData = DataBaseManager.instance.enemyDataSO.enemyDatasList.Find(x => x.enemyNo == index);

            // �G�̐���
            EnemyController enemyController = Instantiate(enemyPrefabs[index], GetRandomEnemyPos(index), Quaternion.identity);   // enemyData.enemyPrefab

            // �G�̏����ݒ�
            enemyController.SetUpEnemy(battle, enemyData);

            // List �֓o�^
            this.battle.AddEnemyFromEnemiesList(enemyController);

            // Debug 
            //enemiesList[0].SetUpEnemy(this);

            yield return new WaitForSeconds(0.25f);
        }
    }

    /// <summary>
    /// �����_���ȓG�̐����ʒu�̎擾
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomEnemyPos(int index) {
        return new Vector3(Random.Range(enemyGenerateTrans[0].position.x, enemyGenerateTrans[1].position.x), enemyPrefabs[index].transform.position.y, Random.Range(enemyGenerateTrans[0].position.z, enemyGenerateTrans[1].position.z));
    }
}
