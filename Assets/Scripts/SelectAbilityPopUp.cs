using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectAbilityPopUp : MonoBehaviour
{
    [SerializeField]
    private Button btnExit;

    [SerializeField]
    private Button btnSubmit;

    [SerializeField]
    private Text txtAbilityPoint;

    [SerializeField]
    private Text txtDescription;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private Stage stage;

    // アビリティ用のボタンのプレファブ

    // 生成位置を４箇所 for 文を二重で回すので配列にする

    // 選択しているアビリティの保持


    /// <summary>
    /// 
    /// </summary>
    /// <param name="stage"></param>
    public void SetUpSelectAbilityPopUp(Stage stage) {

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        this.stage = stage;

        // ボタンの生成

        // 押せるかボタン側でチェック


        // ボタンにメソッドを登録
        btnExit.onClick.AddListener(ClosePopUp);


    }

    /// <summary>
    /// ポップアップを表示する
    /// </summary>
    public void ShowPopUp() {
        canvasGroup.blocksRaycasts = true;

        // 最新の値を表示
        UpdateDisplayAbilityPoint();

        canvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// ポップアップを非表示にする
    /// </summary>
    public void ClosePopUp() {
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(0.0f, 0.5f).SetEase(Ease.Linear);
        stage.SwitchMaskField(true);
    }

    /// <summary>
    /// アビリティポイントの表示更新
    /// </summary>
    public void UpdateDisplayAbilityPoint() {
        txtAbilityPoint.text = GameData.instance.abilityPoint + " / " + GameData.instance.maxAbilityPoint;
    }
}
