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

    [SerializeField, Header("�t�F�[�h�̎���")]
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
    /// Stage �V�[���ւ̑J�ڏ���
    /// </summary>
    public void PreparateStageScene() {

        fade.FadeIn(fadeDuration, () => { ActiveStageScene(); });
    }

    /// <summary>
    /// Stage �V�[���֑J��
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
    /// Battle �V�[���ւ̑J�ڏ���
    /// </summary>
    public void PreparateBattleScene() {
        Debug.Log("Load Battle Scene");

        fade.FadeIn(fadeDuration, () => { StartCoroutine(LoadBattleScene()); });
    }

    /// <summary>
    /// Battle �V�[���ւ̑J��
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


    /*** ���g�p ***/

    void Start() {
        //SceneManager.sceneLoaded += SceneLoaded;
    }

    /// <summary>
    /// ���g�p
    /// </summary>
    /// <param name="nextScene"></param>
    /// <param name="mode"></param>
    private void SceneLoaded(Scene nextScene, LoadSceneMode mode) {
        //StartCoroutine(stage.UpdateDisplayHp(sliderWaitTime));
    }

    /// <summary>
    /// �w�肵���V�[���֑J�ڏ����B���݂͖��g�p
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    public void PrepareteNextScene(SceneName nextLoadSceneName) {

        if (!fade) {
            // �t�F�[�h�C���Ȃ�
            StartCoroutine(LoadNextScene(nextLoadSceneName));
        } else {
            // �t�F�[�h�C������
            fade.FadeIn(fadeDuration, () => { StartCoroutine(LoadNextScene(nextLoadSceneName)); });
        }
    }

    /// <summary>
    /// �w�肵���V�[���֑J�ځB���݂͖��g�p
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadNextScene(SceneName nextLoadSceneName) {

        SceneManager.LoadScene(nextLoadSceneName.ToString());
    
        // �t�F�[�h�C�����Ă���ꍇ�ɂ�
        if (fade) {
            // �V�[���̓ǂݍ��ݏI����҂�
            Scene scene = SceneManager.GetSceneByName(nextLoadSceneName.ToString());
            yield return new WaitUntil(() => scene.isLoaded);

            // �t�F�[�h�A�E�g
            fade.FadeOut(fadeDuration);
        }
    }

    public Scene GetScene(SceneName nextLoadSceneName) {
        return SceneManager.GetSceneByName(nextLoadSceneName.ToString());
    }
}
