using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour {

    public static SceneStateManager instance;

    [SerializeField]
    private Stage stage;

    public Battle battle;

    [SerializeField]
    private float sliderWaitTime;

    [SerializeField]
    private Fade fade;

    [SerializeField, Header("フェードの時間")]
    private float fadeDuration = 1.0f;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Stage シーンへの遷移準備
    /// </summary>
    public void PreparateStageScene() {

        fade.FadeIn(fadeDuration, () => { ActiveStageScene(); });
    }

    /// <summary>
    /// Stage シーンへ遷移
    /// </summary>
    /// <returns></returns>
    private void ActiveStageScene() {

        string oldSceneName = SceneManager.GetActiveScene().name;

        //Scene scene = SceneManager.GetSceneByName(nextLoadSceneName.ToString());

        //while (!scene.isLoaded) {
        //    yield return null;
        //}

        //SceneManager.SetActiveScene(scene);

        stage.gameObject.SetActive(true);

        fade.FadeOut(fadeDuration);

        SceneManager.UnloadSceneAsync(oldSceneName);
    }

    /// <summary>
    /// Battle シーンへの遷移準備
    /// </summary>
    public void PreparateBattleScene() {
        Debug.Log("Load Battle Scene");

        fade.FadeIn(fadeDuration, () => { StartCoroutine(LoadBattleScene()); });
    }

    /// <summary>
    /// Battle シーンへの遷移
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadBattleScene() {

        SceneManager.LoadScene(SceneName.Battle.ToString(), LoadSceneMode.Additive);

        Scene scene = SceneManager.GetSceneByName(SceneName.Battle.ToString());

        yield return new WaitUntil(() => scene.isLoaded);

        stage.gameObject.SetActive(false);

        fade.FadeOut(fadeDuration);

        SceneManager.SetActiveScene(scene);
    }


    /*** 未使用 ***/

    void Start() {
        //SceneManager.sceneLoaded += SceneLoaded;
    }

    /// <summary>
    /// 未使用
    /// </summary>
    /// <param name="nextScene"></param>
    /// <param name="mode"></param>
    private void SceneLoaded(Scene nextScene, LoadSceneMode mode) {
        //StartCoroutine(stage.UpdateDisplayHp(sliderWaitTime));
    }

    /// <summary>
    /// 指定したシーンへ遷移準備。現在は未使用
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    public void PrepareteNextScene(SceneName nextLoadSceneName) {

        if (!fade) {
            // フェードインなし
            StartCoroutine(LoadNextScene(nextLoadSceneName));
        } else {
            // フェードインあり
            fade.FadeIn(fadeDuration, () => { StartCoroutine(LoadNextScene(nextLoadSceneName)); });
        }
    }

    /// <summary>
    /// 指定したシーンへ遷移。現在は未使用
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadNextScene(SceneName nextLoadSceneName) {

        SceneManager.LoadScene(nextLoadSceneName.ToString());
    
        // フェードインしている場合には
        if (fade) {
            // シーンの読み込み終了を待つ
            Scene scene = SceneManager.GetSceneByName(nextLoadSceneName.ToString());
            yield return new WaitUntil(() => scene.isLoaded);

            // フェードアウト
            fade.FadeOut(fadeDuration);
        }
    }

    public Scene GetScene(SceneName nextLoadSceneName) {
        return SceneManager.GetSceneByName(nextLoadSceneName.ToString());
    }
}
