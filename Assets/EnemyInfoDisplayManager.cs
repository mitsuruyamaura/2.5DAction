using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoDisplayManager : AbstractSingleton<EnemyInfoDisplayManager> {

    [SerializeField] private Canvas enemyInfoCanvas;


    public void ShowEnemyInfo() {
        // 引数で、敵の情報(Hp、バフ、デバフ、装備品)をもらって設定する


        if (enemyInfoCanvas != null) {
            enemyInfoCanvas.enabled = true;
        }
    }


    public void HideEnemyInfo() {
        if (enemyInfoCanvas != null) {
            enemyInfoCanvas.enabled = false;
        }
    }
}