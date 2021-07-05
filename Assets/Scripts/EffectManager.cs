using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    // Player ‘¤
    public GameObject chargeUpEffectPrefab;
    public GameObject dashWindPrefab;
    public GameObject recoverStaminaEffectPrefab;

    // Enemy ‘¤
    public GameObject enemyHitEffectPrefab;
    public GameObject destroyEffectPrefab;

    // ‹¤’Ê
    public FloatingMessageControler floatingMessagePrefab;

    public Transform effectConteinerTran;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }   
    }
}
