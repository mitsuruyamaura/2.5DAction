using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    // Player ��
    public GameObject chargeUpEffectPrefab;
    public GameObject dashWindPrefab;
    public GameObject recoveryStaminaEffectPrefab;
    public GameObject recoveryLifeEffectPrefab;
    public GameObject orbGetEffectPrefab;
    public GameObject abilityPowerUpPrefab_1;
    public GameObject abilityPowerUpPrefab_2;
    public GameObject levelUpPrefab;

    // Enemy ��
    public GameObject enemyHitEffectPrefab;
    public GameObject destroyEffectPrefab;

    // ����
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
