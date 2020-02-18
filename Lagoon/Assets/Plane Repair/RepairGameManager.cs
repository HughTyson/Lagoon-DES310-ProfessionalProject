using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepairGameBase : MonoBehaviour
{
    public enum GameType
    {
        SwitchGame
    }

    public virtual void GameInit() { }
    public virtual void GameUpdate() { }

    public virtual void GameCleanUp() { }

    [SerializeField] public GameType type;

}

