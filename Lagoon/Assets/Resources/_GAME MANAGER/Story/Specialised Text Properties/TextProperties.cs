using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SpecialText
{

    public static partial class TextProperties
    {
        public class PropertyInfo
        {
            public PropertyInfo(string name_, Type type_, Type[] soft_incompatibles_ = null, string functionName_ = "", string description_ = "", string parameters_ = "", string example_ = "")
            {
                name = name_;
                type = type_;
                description = description_;
                parameters = parameters_;
                example = example_;
                functionName = functionName_;

                if (soft_incompatibles_ == null)
                    soft_incompatibles = new Type[0];
                else
                    soft_incompatibles = soft_incompatibles_;
            }
            public readonly string name = "";
            public readonly Type type;
            public readonly string functionName = "";
            public readonly string description = "";
            public readonly string parameters = "";
            public readonly string example = "";
            public readonly Type[] soft_incompatibles; // remove all except the closet property causing incompabability
        }

        public static readonly PropertyInfo[] propertyInfos = new PropertyInfo[]
        {
            new PropertyInfo(
                "Colour",
                typeof(Colour),
                functionName_: "Colour(R,G,B)",
                description_: "The RGB colour of the text. Channels are from 0 to 255.",
                parameters_: "R: Red channel. G: Green channel. B: Blue channel.",
                example_: "[Colour(0.5,1,0)] text [/Colour]"
                ),
            new PropertyInfo(
                "Shiver",
                 typeof(Shiver),
                 functionName_: "Shiver(I,S)",
                description_: "Shiver each character of the text.",
                parameters_: "I: Intensity of shiver. S: Speed of shiver.",
                example_: "[Shiver(1, 0.1)] text [/Shiver]"
                ),
            new PropertyInfo(
                "CharSpeed",
                typeof(CharSpeed),
                soft_incompatibles_: new Type[] {typeof(AppearAtOnce)},
                functionName_: "CharSpeed(S)",
                description_: "The speed of showing the characters of the text.",
                parameters_: "S: Speed of showing characters.",
                example_: "[CharSpeed(10)] text [/CharSpeed]"
                ),
            new PropertyInfo(
                "WaveUnscaled",
                typeof(WaveUnscaled),
                functionName_: "Wave(F,A,S)",
                description_: "Moves each character of the text up and down forming a wave",
                parameters_: "F: Frequincy of wave. A: Amplitude of wave. S: Speed of wave",
                example_: "[Wave(3,2,2)] text [/Wave]"
                ),
           new PropertyInfo(
                "WaveScaled",
                typeof(WaveScaled),
                functionName_: "Wave(F,A,S)",
                description_: "Moves each character of the text up and down forming a wave. Scales the frequincy so when there are more or less characters, the wave will be the same",
                parameters_: "F: Frequincy of wave. A: Amplitude of wave. S: Speed of wave",
                example_: "[Wave(3,2,2)] text [/Wave]"
                ),
            new PropertyInfo(
                "Scale",
                typeof(Scale),
                functionName_: "Scale(S)",
                description_: "Scale difference of text in relation to normal text size",
                parameters_: "S: Scale difference of text",
                example_: "[Scale(0.3)] text [/Size]"
                ),
            new PropertyInfo(
                "AppearAtOnce",
                typeof(AppearAtOnce),
                soft_incompatibles_: new Type[] {typeof(CharSpeed) },
                functionName_: "AppearAtOnce",
                description_: "Shows the text at the same.",
                parameters_: "None",
                example_: "[AppearAtOnce] text [/AppearAtOnce]"
                ),
            new PropertyInfo(
                "StaticAppear",
                typeof(StaticAppear),
                soft_incompatibles_: new Type[] { typeof(CharSpeed), typeof(AppearAtOnce) },
                functionName_: "StaticAppear",
                description_: "Shows the text at the same time, without any transition",
                parameters_: "None",
                example_: "[StaticAppear] text [/StaticAppear]"
                ),
            new PropertyInfo(
               "Rotate",
               typeof(Rotate),
                functionName_: "Rotate(A)",
                description_: "Set a rotation for the characters in degress",
                parameters_: "A: signed angle in degrees of rotation",
                example_: "[Rotate(-45)] text [/Rotate]"
                ),
            new PropertyInfo(
                "RotateFromTo",
                typeof(RotateFromTo),
                functionName_: "RotateFromTo(F,T,D,TP)",
                description_: "Rotate from an angle to an angle, with a duration and a tween type",
                parameters_: "F : from angle in degress, T : to angle in degress, D: duration in sections, TP: Type of tween. this can be either; L, EI, EO, EIO or NEIO. These stand for; Linear, Ease-In, Ease-Out, Ease-In-Out and Non-Ease-In-Out"
                )
        };
        public static readonly PropertyInfo[] noExitPropertyInfo = new PropertyInfo[]
       {
            new PropertyInfo(
                "Delay",
                typeof(Delay),
                functionName_: "Delay(D)",
                description_: "Stop the flow of execution for a period of time",
                parameters_: "D: Delay of execution.",
                example_: "[#Delay(1)]"
                )
       };

        public static readonly Dictionary<int, PropertyInfo> propertyInfoDictionary = propertyInfos.ToDictionary(key => key.name.GetHashCode(), propertyInfo => propertyInfo);
        public static readonly Dictionary<int, PropertyInfo> noExitPropertyInfoDictionary = noExitPropertyInfo.ToDictionary(key => key.name.GetHashCode(), propertyInfo => propertyInfo);

        public static readonly Dictionary<Type, PropertyInfo> propertyInfoDictionaryType = propertyInfos.ToDictionary(key => key.type, propertyInfo => propertyInfo);
        public static Base CreatePropertyDataFromName(string name)
        {
            PropertyInfo info;
            if (propertyInfoDictionary.TryGetValue(name.GetHashCode(), out info))
            {
                return (Base)Activator.CreateInstance(info.type);
            }
            return null;
        }
        public static NoExitBase CreateNoExitPropertyDataFromName(string name)
        {
            PropertyInfo info;
            if (noExitPropertyInfoDictionary.TryGetValue(name.GetHashCode(), out info))
            {
                return (NoExitBase)Activator.CreateInstance(info.type);
            }
            return null;
        }

        public static string FindMostSimilarPropertyName(string name)
        {
            string most_similar_name = "";
            int most_similar_value = int.MaxValue;
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                int new_similarity = StringDistance.GetLevenshteinDistance(name, propertyInfos[i].name);
                if (new_similarity < most_similar_value)
                {
                    most_similar_value = new_similarity;
                    most_similar_name = propertyInfos[i].name;
                }
            }
            return most_similar_name;
        }
        public static string FindMostSimilarNoExitPropertyName(string name)
        {
            string most_similar_name = "";
            int most_similar_value = int.MaxValue;
            for (int i = 0; i < noExitPropertyInfo.Length; i++)
            {
                int new_similarity = StringDistance.GetLevenshteinDistance(name, noExitPropertyInfo[i].name);
                if (new_similarity < most_similar_value)
                {
                    most_similar_value = new_similarity;
                    most_similar_name = noExitPropertyInfo[i].name;
                }
            }
            return most_similar_name;
        }

        public static void ApplyDefaults(SpecialTextData specialTextData, List<Base> included_properties, List<SpecialTextCharacterData> characters, GlobalPropertiesNode globalProperties)
        {
            if (!included_properties.Exists(y => { return y.GetType() == typeof(AppearAtOnce); }))
            {
                if (!included_properties.Exists(y => { return (y.GetType() == typeof(CharSpeed)); }))
                {
                    Base default_property = new CharSpeed(globalProperties.DefaultSpeedPerTextCharacter);
                    default_property.AddCharacterReference(characters);
                    specialTextData.propertyDataList.Add(default_property);
                }
            }
            if (!included_properties.Exists(y => { return y.GetType() == typeof(Colour); }))
            {
                Base default_property = new Colour(globalProperties.DefaultColour.r, globalProperties.DefaultColour.g, globalProperties.DefaultColour.b);
                default_property.AddCharacterReference(characters);
                specialTextData.propertyDataList.Add(default_property);
            }
        }
    }        
}
