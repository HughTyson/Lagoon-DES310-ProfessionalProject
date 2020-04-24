using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModularPieceManager
{
    public CharacterModularPieceManager(GameObject characterRootGameObject, string jointNameIdentifier_)
    {
        recursiveFillCharacterJoints(characterRootGameObject.transform, jointNameIdentifier_);
    }
    void recursiveFillCharacterJoints(Transform transform, string jointNameIdentifier_)
    {
        if (transform.gameObject.name.Contains(jointNameIdentifier_))
        {
            characterJoints.Add(transform.gameObject.name, transform);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            recursiveFillCharacterJoints(transform.GetChild(i), jointNameIdentifier_);
        }
    }


    Dictionary<string, Transform> characterJoints = new Dictionary<string, Transform>();

    class ModularPeice
    {
        List<LinkedModularPeiceJoint> linkedJoints = new List<LinkedModularPeiceJoint>();

        GameObject modularPeiceRootGameObject;

        public ModularPeice(GameObject modularPeiceRootGameObject_, string jointNameIdentifier_, IReadOnlyDictionary<string, Transform> characterJoints)
        {
            modularPeiceRootGameObject = modularPeiceRootGameObject_;
            recursiveFillModularJoints(modularPeiceRootGameObject_.transform, jointNameIdentifier_, characterJoints);
        }

        void recursiveFillModularJoints(Transform transform, string jointNameIdentifier_, IReadOnlyDictionary<string, Transform> characterJoints)
        {
            if (transform.gameObject.name.Contains(jointNameIdentifier_))
            {
                linkedJoints.Add(new LinkedModularPeiceJoint(transform, characterJoints[transform.name]));
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                recursiveFillModularJoints(transform.GetChild(i), jointNameIdentifier_, characterJoints);
            }
        }

        struct LinkedModularPeiceJoint
        {
            public LinkedModularPeiceJoint(Transform modularPeiceJoint_, Transform characterPeiceJoint_)
            {
                modularPeiceJoint = modularPeiceJoint_;
                characterJoint = characterPeiceJoint_;
            }
            public readonly Transform modularPeiceJoint;
            public readonly Transform characterJoint;
        
        }


        private bool enabled = false;
        public bool Enabled
        { 
        get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                modularPeiceRootGameObject.SetActive(enabled);
            } 
        }


        public void Update()
        {
            if (enabled)
            {
                for (int i = 0; i < linkedJoints.Count; i++)
                {
                    Transform characterJoint = linkedJoints[i].characterJoint;
                    Transform modularPeiceJoint = linkedJoints[i].modularPeiceJoint;

                    modularPeiceJoint.position = characterJoint.position;
                    modularPeiceJoint.rotation = characterJoint.rotation;
                }
            }
        }
    
    }


    Dictionary<object, ModularPeice> allModularPeices = new Dictionary<object, ModularPeice>();

    public void EnablePiece(object ID)
    {
        allModularPeices[ID].Enabled = true;
    }
    public void DisablePiece(object ID)
    {
        allModularPeices[ID].Enabled = false;
    }

    public void AddNewModularPeice(object ID, GameObject modularPeiceRootGameObject, string jointNameIdentifier_, bool enabled)
    {
        allModularPeices.Add(ID, new ModularPeice(modularPeiceRootGameObject, jointNameIdentifier_, characterJoints));
        allModularPeices[ID].Enabled = enabled;
    }
    public void DeleteModularPeice(object ID)
    {
        allModularPeices.Remove(ID);
    }

    public void LateUpdate()
    {
        foreach (KeyValuePair<object, ModularPeice> pair in allModularPeices)
        {
            pair.Value.Update();
        }
    }
}
