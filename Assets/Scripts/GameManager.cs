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
        // w“ÇŠJŽn
        GameData.instance.moveCount.Subscribe(_ => UpdateDisplayMoveCount());
    }

    private void UpdateDisplayMoveCount() {
        txtMoveCount.text = GameData.instance.moveCount.ToString();

        if (GameData.instance.moveCount.Value <= 0) {
            Debug.Log("ƒ{ƒXí");

            // w“Ç’âŽ~
            GameData.instance.moveCount.Dispose();


            // ˆÚ“®‹ÖŽ~


            // TODO ƒ{ƒX‚Æ‚Ìƒoƒgƒ‹ƒV[ƒ“‚Ö‘JˆÚ
        }
    }
}
