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
        // �w�ǊJ�n
        GameData.instance.moveCount.Subscribe(_ => UpdateDisplayMoveCount());
    }

    private void UpdateDisplayMoveCount() {
        txtMoveCount.text = GameData.instance.moveCount.ToString();

        if (GameData.instance.moveCount.Value <= 0) {
            Debug.Log("�{�X��");

            // �w�ǒ�~
            GameData.instance.moveCount.Dispose();


            // TODO �{�X�Ƃ̃o�g���V�[���֑J��
        }
    }
}
