using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpBase : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup canvasGroup;

    //public virtual void OnEnterPopUp<T>(T t) where T : class {
    //    canvasGroup.alpha = 0;
    //}

    public virtual void OnExitPopUp() {

    }
}
