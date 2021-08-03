using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class InputButtonManager : MonoBehaviour
{
    [SerializeField]
    private Button btnUp;

    [SerializeField]
    private Button btnDown;

    [SerializeField]
    private Button btnLeft;

    [SerializeField]
    private Button btnRight;

    //[SerializeField]
    private MapMoveController mapMoveController;

    public void SetUpInputButtonManager(MapMoveController mapMoveController) {
        this.mapMoveController = mapMoveController;

        // 各ボタンにメソッド登録。移動中は重複タップ防止
        btnUp?
            .OnClickAsObservable()
            .Where(_ => !mapMoveController.IsMoving)
            // .TakeUntilDestroy(this) を追加すると引数に指定したゲームオブジェクトかクラスの破棄と一緒に購読が終了
            .ThrottleFirst(TimeSpan.FromSeconds(mapMoveController.MoveDuration))
            .Subscribe(_ => {
                InputMoveButton(new Vector2(0, 1));
            });

        btnDown?
            .OnClickAsObservable()
            .Where(_ => !mapMoveController.IsMoving)
            .ThrottleFirst(TimeSpan.FromSeconds(mapMoveController.MoveDuration))
            .Subscribe(_ => {
                InputMoveButton(new Vector2(0, -1));
            });

        btnLeft?
            .OnClickAsObservable()
            .Where(_ => !mapMoveController.IsMoving)
            .ThrottleFirst(TimeSpan.FromSeconds(mapMoveController.MoveDuration))
            .Subscribe(_ => {
                 InputMoveButton(new Vector2(-1, 0));
            });

        btnRight?
            .OnClickAsObservable()
            .Where(_ => !mapMoveController.IsMoving)
            .ThrottleFirst(TimeSpan.FromSeconds(mapMoveController.MoveDuration))
            .Subscribe(_ => {
                InputMoveButton(new Vector2(1, 0));
            });

        //btnUp.onClick.AddListener(() => InputMoveButton(new Vector2(0, 1)));
        //btnDown.onClick.AddListener(() => InputMoveButton(new Vector2(0, -1)));
        //btnLeft.onClick.AddListener(() => InputMoveButton(new Vector2(-1, 0)));
        //btnRight.onClick.AddListener(() => InputMoveButton(new Vector2(1, 0)));
    }

    /// <summary>
    /// 入力されたキーの方向を取得
    /// </summary>
    /// <param name="pos"></param>
    private void InputMoveButton(Vector2 pos) {
        mapMoveController.CheckMoveTile(pos);
    }
}
