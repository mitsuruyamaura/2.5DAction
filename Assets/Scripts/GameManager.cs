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
        GameData.instance.staminaPoint.Subscribe(_ => UpdateDisplayMoveCount());
    }

    private void UpdateDisplayMoveCount() {
        txtMoveCount.text = GameData.instance.staminaPoint.ToString();

        if (GameData.instance.staminaPoint.Value <= 0) {
            Debug.Log("�{�X��");

            // �w�ǒ�~
            GameData.instance.staminaPoint.Dispose();


            // �ړ��֎~


            // TODO �{�X�Ƃ̃o�g���V�[���֑J��
        }
    }
}
