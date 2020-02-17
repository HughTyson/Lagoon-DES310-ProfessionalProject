using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepairGameBase : MonoBehaviour
{
    enum GameType
    {
        SwitchGame
    }

    public virtual void GameInit() { }
    public virtual void GameUpdate() { }

}
