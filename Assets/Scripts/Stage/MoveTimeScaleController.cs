using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// シンボル移動時の速度の種類と設定値
/// </summary>
public enum MoveTimeScale {
    Normal = 100,
    One_Half = 75,
    Double = 50,
    Count = 3
}

public class MoveTimeScaleController : MonoBehaviour
{
    [SerializeField]
    private Sprite normalSpeedIcon;

    [SerializeField]
    private Sprite oneHalfSpeedIcon;

    [SerializeField]
    private Sprite doubleSpeedIcon;

    [SerializeField]
    private Image imgSpeedIcon;

    [SerializeField]
    private Button btnStaminaFrame;

    public MoveTimeScale currentMoveTimeScale;
    public int currentTimeScaleNo;

    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetUpMoveButtonController() {
        currentMoveTimeScale = MoveTimeScale.Normal;
        imgSpeedIcon.sprite = normalSpeedIcon;
        btnStaminaFrame.onClick.AddListener(SwitchMoveTimeScale);
    }

    /// <summary>
    /// 残りのスタミナ数表示ボタンを押した際の処理
    /// </summary>
    private void SwitchMoveTimeScale() {

        // enum 管理
        currentMoveTimeScale = currentMoveTimeScale switch {
            MoveTimeScale.Normal => MoveTimeScale.One_Half,
            MoveTimeScale.One_Half => MoveTimeScale.Double,
            MoveTimeScale.Double => MoveTimeScale.Normal,
            _ => MoveTimeScale.Normal
        };

        // int 管理
        currentTimeScaleNo++;
        currentTimeScaleNo = currentTimeScaleNo % (int)MoveTimeScale.Count == 0 ? 0 : currentTimeScaleNo;

        // アイコン画像の設定
        imgSpeedIcon.sprite = currentMoveTimeScale switch {
            MoveTimeScale.Normal => normalSpeedIcon,
            MoveTimeScale.One_Half => oneHalfSpeedIcon,
            MoveTimeScale.Double => doubleSpeedIcon,
            _ => normalSpeedIcon
        };

        // プレイヤーとエネミーシンボルの移動速度の設定値を更新
        GameData.instance.moveTimeScale = (float)currentMoveTimeScale / 100;
    }
}
