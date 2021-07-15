using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public ReactiveProperty<int> staminaPoint = new ReactiveProperty<int>();

    public ReactiveDictionary<int, bool> orbs = new ReactiveDictionary<int, bool>();

    public int hp;

    public int maxHp;

    public bool isDebugOn;

    public int playerLevel;

    public int totalExp;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
