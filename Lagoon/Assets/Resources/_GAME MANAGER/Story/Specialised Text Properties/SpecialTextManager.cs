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


        List<TextPropertyData.Base> UnactiveProperties = new List<TextPropertyData.Base>();
        List<TextPropertyData.Base> TransitioningProperties = new List<TextPropertyData.Base>();
        List<TextPropertyData.Base> EndlessUpdateProperties = new List<TextPropertyData.Base>();

        Color32[] newVertexColors;
        public void Begin(SpecialTextData specialTextData_, TMPro.TextMeshProUGUI tmp_)
        {
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
            newVertexColors = tmp.textInfo.meshInfo[0].mesh.colors32;
        }

        public void Update()
        {
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
                    y.Begin();
                    TransitioningProperties.Add(y);
                    return true;
                }
                return false;
            });


            int lowestHoldBackValue = int.MaxValue;
            TransitioningProperties.RemoveAll( y =>
            {
                if (y.TransitionUpdate())
                {
                    EndlessUpdateProperties.Add(y);
                    lowestHoldBackValue = Mathf.Min(lowestHoldBackValue, y.HoldingBackIndex);
                    return true;
                }
                lowestHoldBackValue = Mathf.Min(lowestHoldBackValue, y.HoldingBackIndex);
                return false;
            }
            );
            iterator = lowestHoldBackValue;

            for (int i = 0; i < EndlessUpdateProperties.Count; i++)
                EndlessUpdateProperties[i].EndlessUpdate();

            newVertexColors = tmp.textInfo.meshInfo[0].mesh.colors32;

            TMPro.TMP_TextInfo info = tmp.textInfo;

            int charIndex = 0;
            for (int i = 0; i < info.characterCount; ++i)
            {
                charIndex = i;
                while (!info.characterInfo[charIndex].isVisible)
                    charIndex++;
                if (charIndex > info.characterCount - 1)
                    Debug.LogError("Error, something went wrong :(");

                TMPro.TMP_CharacterInfo char_info = tmp.textInfo.characterInfo[charIndex];
                int meshIndex = tmp.textInfo.characterInfo[charIndex].materialReferenceIndex;
                int vertexIndex = tmp.textInfo.characterInfo[charIndex].vertexIndex;

                Color32[] vertexColors = tmp.textInfo.meshInfo[meshIndex].colors32;
                vertexColors[vertexIndex + 0] = specialTextData.specialTextCharacters[i].colour;
                vertexColors[vertexIndex + 1] = specialTextData.specialTextCharacters[i].colour;
                vertexColors[vertexIndex + 2] = specialTextData.specialTextCharacters[i].colour;
                vertexColors[vertexIndex + 3] = specialTextData.specialTextCharacters[i].colour;
            }

            tmp.UpdateVertexData(TMPro.TMP_VertexDataUpdateFlags.Colors32);

        }
    }
}
