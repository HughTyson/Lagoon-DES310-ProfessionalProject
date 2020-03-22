using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MenuScreenBase : MonoBehaviour
{
    [SerializeField]
    MenuScreenBase goBackMenu;


    public abstract void TransitionIn();
    public abstract void TransitionOut();
}
