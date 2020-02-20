using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairGameBase : MonoBehaviour
{
    public enum GameType
    {
        SwitchGame
    }

    public virtual void GameInit(Transform transform) { }
    public virtual void GameUpdate() { }

    public virtual void GameCleanUp() { }

    [SerializeField] public GameType type;

}

