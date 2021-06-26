using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    // Player ‘¤
    public GameObject chargeUpEffectPrefab;
    public GameObject dashWindPrefab;

    // Enemy ‘¤
    public GameObject enemyHitEffectPrefab;
    public GameObject destroyEffectPrefab;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }   
    }
}
