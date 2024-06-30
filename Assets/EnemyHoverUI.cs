using UnityEngine;

public class EnemyHoverUI : MonoBehaviour {

    public void OnMouseEnter() {
        EnemyInfoDisplayManager.instance.ShowEnemyInfo();
        //Debug.Log("Enter");
    }

    public void OnMouseExit() {
        EnemyInfoDisplayManager.instance.HideEnemyInfo();
        //Debug.Log("Exit");
    }
}