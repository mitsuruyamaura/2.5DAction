using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text txtMoveCount;

    void Start()
    {
        // 購読開始
        GameData.instance.moveCount.Subscribe(_ => UpdateDisplayMoveCount());
    }

    private void UpdateDisplayMoveCount() {
        txtMoveCount.text = GameData.instance.moveCount.ToString();

        if (GameData.instance.moveCount.Value <= 0) {
            Debug.Log("ボス戦");

            // 購読停止
            GameData.instance.moveCount.Dispose();


            // TODO ボスとのバトルシーンへ遷移
        }
    }
}
