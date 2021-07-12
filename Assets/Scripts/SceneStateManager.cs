using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;

    [SerializeField]
    private Stage stage;

    [SerializeField]
    private float sliderWaitTime;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void SceneLoaded(Scene nextScene, LoadSceneMode mode) {
        //StartCoroutine(stage.UpdateDisplayHp(sliderWaitTime));
    }


    public void PreparateNextScene(SceneName nextLoadSceneName) {
        StartCoroutine(NextLoadScene(nextLoadSceneName));
    }

    private IEnumerator NextLoadScene(SceneName nextLoadSceneName) {

        string oldSceneName = SceneManager.GetActiveScene().name;

        Scene scene = SceneManager.GetSceneByName(nextLoadSceneName.ToString());

        while (!scene.isLoaded) {
            yield return null;
        }

        SceneManager.SetActiveScene(scene);

        stage.gameObject.SetActive(true);

        SceneManager.UnloadSceneAsync(oldSceneName);
    }

    public void LoadBattleScene() {
        Debug.Log("Load Battle Scene");

        StartCoroutine(BattleScene());       
    }

    /// <summary>
    /// バトルシーンへの遷移
    /// </summary>
    /// <returns></returns>
    private IEnumerator BattleScene() {
        SceneManager.LoadScene(SceneName.Battle.ToString(), LoadSceneMode.Additive);

        Scene scene = SceneManager.GetSceneByName(SceneName.Battle.ToString());

        yield return new WaitUntil(() => scene.isLoaded);

        stage.gameObject.SetActive(false);

        SceneManager.SetActiveScene(scene);
    }
}
