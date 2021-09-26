using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MoveTimeScale {
    Normal = 10,
    One_Half = 15,
    Double = 20,
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
    public int currentNo;

    public void SetUpMoveButtonController() {
        currentMoveTimeScale = MoveTimeScale.Normal;
        imgSpeedIcon.sprite = doubleSpeedIcon;
        btnStaminaFrame.onClick.AddListener(SwitchMoveTimeScale);
    }

    private void SwitchMoveTimeScale() {

        currentMoveTimeScale = currentMoveTimeScale switch {
            MoveTimeScale.Normal => MoveTimeScale.One_Half,
            MoveTimeScale.One_Half => MoveTimeScale.Double,
            MoveTimeScale.Double => MoveTimeScale.Normal,
            _ => MoveTimeScale.Normal
        };

        currentNo++;
        currentNo = currentNo % (int)MoveTimeScale.Count == 0 ? 0 : currentNo;

        imgSpeedIcon.sprite = currentMoveTimeScale switch {
            MoveTimeScale.Normal => oneHalfSpeedIcon,
            MoveTimeScale.One_Half => doubleSpeedIcon,
            MoveTimeScale.Double => normalSpeedIcon,
            _ => normalSpeedIcon
        };

        GameData.instance.moveTimeScale = (float)currentMoveTimeScale / 10;
    }
}
