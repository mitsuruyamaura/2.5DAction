using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoDisplayManager : AbstractSingleton<EnemyInfoDisplayManager> {

    [SerializeField] private Canvas enemyInfoCanvas;


    public void ShowEnemyInfo() {
        // �����ŁA�G�̏��(Hp�A�o�t�A�f�o�t�A�����i)��������Đݒ肷��


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