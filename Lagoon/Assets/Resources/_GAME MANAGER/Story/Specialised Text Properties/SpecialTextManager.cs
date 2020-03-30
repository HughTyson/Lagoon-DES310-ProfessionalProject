using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public class SpecialTexManager
    {
        SpecialTextData specialTextData;
        int iterator;
        TMPro.TextMeshProUGUI tmp;

        List<TextProperties.Base> UnactiveProperties = new List<TextProperties.Base>();
        List<TextProperties.Base> TransitioningProperties = new List<TextProperties.Base>();
        List<TextProperties.Base> EndlessUpdateProperties = new List<TextProperties.Base>();

        bool isReverted = false;

        public void Revert()
        {
            TMPro.TMP_TextInfo info = tmp.textInfo;

            isReverted = true;
            int charIndex = 0;
            for (int i = 0; i < info.characterCount; ++i)
            {
                charIndex = i;
                while (!info.characterInfo[charIndex].isVisible)
                {
                    charIndex++;
                    if (charIndex > info.characterCount - 1)
                        break;
                }
                if (charIndex > info.characterCount - 1)
                    continue;

                TMPro.TMP_CharacterInfo char_info = tmp.textInfo.characterInfo[charIndex];
                int meshIndex = tmp.textInfo.characterInfo[charIndex].materialReferenceIndex;
                int vertexIndex = tmp.textInfo.characterInfo[charIndex].vertexIndex;

                specialTextData.specialTextCharacters[i].ResetToInit();
                specialTextData.specialTextCharacters[i].ApplyToTMPChar(ref tmp.textInfo.meshInfo[meshIndex], vertexIndex);
            }
            tmp.UpdateVertexData(TMPro.TMP_VertexDataUpdateFlags.All);
        }
        public void Begin(SpecialTextData specialTextData_, TMPro.TextMeshProUGUI tmp_)
        {
            isReverted = false;
            for (int i = 0; i < specialTextData_.propertyDataList.Count; i++)
            {
                specialTextData_.propertyDataList[i].Init();
            }

            specialTextData = specialTextData_;
            tmp = tmp_;
            iterator = 0;
            UnactiveProperties.Clear();
            TransitioningProperties.Clear();
            EndlessUpdateProperties.Clear();
            UnactiveProperties.AddRange(specialTextData.propertyDataList);

            tmp.alpha = 1.0f;
            tmp.text = specialTextData_.fullTextString;
            tmp.ForceMeshUpdate();

            TMPro.TMP_TextInfo info = tmp.textInfo;
            int charIndex;
            for (int i = 0; i < info.characterCount; ++i)
            {
                charIndex = i;
                while (!info.characterInfo[charIndex].isVisible)
                {
                    charIndex++;
                    if (charIndex > info.characterCount - 1)
                        break;
                }
                if (charIndex > info.characterCount - 1)
                    continue;


                TMPro.TMP_CharacterInfo char_info = tmp.textInfo.characterInfo[charIndex];
                int meshIndex = tmp.textInfo.characterInfo[charIndex].materialReferenceIndex;
                int vertexIndex = tmp.textInfo.characterInfo[charIndex].vertexIndex;


                Vector3[] vertices = tmp.textInfo.meshInfo[meshIndex + 0].vertices;
                specialTextData.specialTextCharacters[i].SetupDefVals(vertices[vertexIndex + 0], vertices[vertexIndex + 1], vertices[vertexIndex + 2], vertices[vertexIndex + 3]);




                specialTextData.specialTextCharacters[i].SetupInitialVals(
                        vertices[vertexIndex + 0], vertices[vertexIndex + 1], vertices[vertexIndex + 2], vertices[vertexIndex + 3],
                        char_info.color
                    );

            }
        }

        public bool AreAllCompleted()
        {
            return (UnactiveProperties.Count == 0 && TransitioningProperties.Count == 0);
        }


        public void ForceAll()
        {
            UnactiveProperties.RemoveAll(y =>
            {
                    y.TransitionStart();
                    EndlessUpdateProperties.Add(y);
                    return true;
            });

            TransitioningProperties.RemoveAll(y =>
            {
                EndlessUpdateProperties.Add(y);
                return true;
            }
            );
        }

        public void Update()
        {
            if (isReverted)
                return;

            // reset all character values
            for (int i = 0; i < specialTextData.specialTextCharacters.Count; i++)
            {
                specialTextData.specialTextCharacters[i].Reset();
            }

            // if the iterator is greater or equal to the lowest character index. Begin property transition.
            UnactiveProperties.RemoveAll(y =>
            { 
                if (iterator >= y.LowestCharacterIndex)
                {
                    y.TransitionStart();
                    TransitioningProperties.Add(y);
                    return true;
                }
                return false;
            });

            int lowestHoldBackIndex = int.MaxValue;
            for (int i = 0; i < TransitioningProperties.Count; i++)
                lowestHoldBackIndex = Mathf.Min(lowestHoldBackIndex, TransitioningProperties[i].HoldingBackIndex);
            for (int i = 0; i < UnactiveProperties.Count; i++)
                lowestHoldBackIndex = Mathf.Min(lowestHoldBackIndex, UnactiveProperties[i].HoldingBackIndex);


            for (int i = 0; i < EndlessUpdateProperties.Count; i++)
                EndlessUpdateProperties[i].EndlessUpdate();

            TransitioningProperties.RemoveAll( y =>
            {
                if (y.TransitionUpdate(lowestHoldBackIndex))
                {
                    EndlessUpdateProperties.Add(y);
                    lowestHoldBackIndex = Mathf.Min(lowestHoldBackIndex, y.HoldingBackIndex);
                    return true;
                }
                return false;
            }
            );
            iterator = lowestHoldBackIndex;



            TMPro.TMP_TextInfo info = tmp.textInfo;

            int charIndex = 0;
            for (int i = 0; i < info.characterCount; ++i)
            {
                charIndex = i;
                while (!info.characterInfo[charIndex].isVisible)
                {
                    charIndex++;
                    if (charIndex > info.characterCount - 1)
                        break;
                }
                if (charIndex > info.characterCount - 1)
                    continue;

                TMPro.TMP_CharacterInfo char_info = tmp.textInfo.characterInfo[charIndex];
                int meshIndex = tmp.textInfo.characterInfo[charIndex].materialReferenceIndex;
                int vertexIndex = tmp.textInfo.characterInfo[charIndex].vertexIndex;

                specialTextData.specialTextCharacters[i].ApplyToTMPChar(ref tmp.textInfo.meshInfo[meshIndex], vertexIndex);

                
            }

            tmp.UpdateVertexData(TMPro.TMP_VertexDataUpdateFlags.All);

        }
    }
}
