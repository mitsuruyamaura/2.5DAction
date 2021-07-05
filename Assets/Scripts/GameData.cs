using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public ReactiveProperty<int> staminaPoint = new ReactiveProperty<int>();


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }        
    }
}
