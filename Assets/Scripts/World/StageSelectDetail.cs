using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectDetail : MonoBehaviour
{
    [SerializeField]
    private Text txtStageSelect;

    [SerializeField]
    private Button btnStageSelectDetail;

    private StageData stageData;

    private World world;

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="stage"></param>
    public void SetUpStageSelectDetail(StageData stageData, World world) {
        this.stageData = stageData;
        this.world = world;

        txtStageSelect.text = this.stageData.stageName;
        btnStageSelectDetail.onClick.AddListener(OnClickStageSelectDetail);
    }

    /// <summary>
    /// ボタンを押した際の処理
    /// </summary>
    private void OnClickStageSelectDetail() {

        // キャラのアイコンをボタン上に配置
        world.SetPlayerTran(stageData.playerIconTran);

        // 選択しているステージの情報を更新
        GameData.instance.chooseStageNo = stageData.stageNo;
    }

    /// <summary>
    /// ボタン表示のオンオフ切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateButton(bool isSwitch) {
        btnStageSelectDetail.gameObject.SetActive(isSwitch);
    }
}
