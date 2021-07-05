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
        GameData.instance.staminaPoint.Subscribe(_ => UpdateDisplayMoveCount());
    }

    private void UpdateDisplayMoveCount() {
        txtMoveCount.text = GameData.instance.staminaPoint.ToString();

        if (GameData.instance.staminaPoint.Value <= 0) {
            Debug.Log("ƒ{ƒXí");

            // w“Ç’âŽ~
            GameData.instance.staminaPoint.Dispose();


            // ˆÚ“®‹ÖŽ~


            // TODO ƒ{ƒX‚Æ‚Ìƒoƒgƒ‹ƒV[ƒ“‚Ö‘JˆÚ
        }
    }
}
