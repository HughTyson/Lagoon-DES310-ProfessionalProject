using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Pools objects instead of creating and destroying objects constantly
/// <para> Useful for objects that get constantly created and destroyed, which slows down garbage collection </para>
/// </summary>
public class ObjectPooler<T> where T : new()
{

    List<T> activeObjects = new List<T>();
    Stack<T> unactiveObjects = new Stack<T>();

    public ObjectPooler(int initSize = 0)
    {
        for (int i = 0; i < initSize; i++)
        {
            unactiveObjects.Push(new T());
        }
    }


    /// <summary>
    /// Selected an unactive object and return it to allow setup of the object to take place.
    /// <para> If there are no unactive objects in the pool, make a new one and return it. This will make the pool bigger. </para>
    /// </summary>
    public T ActivateObject()
    {
        if (unactiveObjects.Count > 0)
        {
            T activatedObj = unactiveObjects.Pop();
            activeObjects.Add(activatedObj);
            return activatedObj;
        }
        else
        {
            T activateObj = new T();
            activeObjects.Add(activateObj);
            return activateObj;
        }
    }

    /// <summary>
    /// Deactivate an object based on its index. 
    /// <para> Index is used as it is faster to compute </para>
    /// </summary>
    public void DeactivateObject(int index)
    {
        T deactivatedObj = activeObjects[index];
        activeObjects.RemoveAt(index);
        unactiveObjects.Push(deactivatedObj);
    }

    public void DeactivateAll()
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            unactiveObjects.Push(activeObjects[i]);
        }
        activeObjects.Clear();
    }

    /// <summary>
    /// List of active objects
    /// </summary>
    public IReadOnlyList<T> ActiveObjects
    {
        get { return activeObjects.AsReadOnly(); }    
    }



}
