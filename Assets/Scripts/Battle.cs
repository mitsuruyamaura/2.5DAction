using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);

        // Debug—p
        SceneStateManager.instance.PreparateNextScene(SceneName.Stage);
    }
}
