using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    //string casting_tutorial = "Pull the left analogue stick back to get ready to cast, flick it forward to cast.";
    string attract_tutoiral = "Keep the bob steady, fish are attracted!";
    string emote_tutoiral = "The fish is very interested in the bob. Once it's bitten '!' hook him by holding RT. But be careful, it may just be testing the bob '?'.";
    string reel_tutoiral = "You have hooked a fish! Reel him in with RT. Don't let the line snap or you will lose him";

    [SerializeField] string reel_tutorial;

    SpecialText.SpecialTextData new_tutoiral_text = new SpecialText.SpecialTextData();
    


    bool casting_complete;
    bool attract_complete;
    bool emote_complete;
    bool reel_complete;

    public enum TutorialType
    {
        CASTING,
        ATTRACT,
        EMOTE,
        REEL
    }

    public bool WhatTutorial(TutorialType type)
    {
        switch (type)
        {
            case TutorialType.CASTING:
                {
                    if(!casting_complete)
                    {
                        new_tutoiral_text = new SpecialText.SpecialTextData();
                        string casting_tutorial = "Pull the ";
                        int character_data = 0;

                        new_tutoiral_text.CreateCharacterData(casting_tutorial);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,32),
                                    new SpecialText.TextProperties.CharSpeed(35)
                            },
                                    character_data,
                                    casting_tutorial.Length
                            );

                        character_data += casting_tutorial.Length;

                        casting_tutorial = "left analogue ";

                        new_tutoiral_text.AddCharacterData(casting_tutorial);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(0,255,255),
                                    new SpecialText.TextProperties.CharSpeed(35),
                                    new SpecialText.TextProperties.WaveScaled(1,3,1)
                            },
                                    character_data,
                                    casting_tutorial.Length
                            );

                        character_data += casting_tutorial.Length;

                        casting_tutorial = "stick back to get ready to cast, flick it forward to cast.";

                        new_tutoiral_text.AddCharacterData(casting_tutorial);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,32),
                                    new SpecialText.TextProperties.CharSpeed(35)
                            },
                                    character_data,
                                    casting_tutorial.Length
                            );



                        //GAME_UI.Instance.state_Tutorial.box.tmp.text = casting_tutorial;
                        GAME_UI.Instance.state_Tutorial.box.string_data = new_tutoiral_text;
                        GAME_UI.Instance.state_Tutorial.box.Appear();
                        //GAME_UI.Instance.state_Tutorial.box.WriteText(casting_text);
                        
                        return true;
                    }

                }
                break;
            case TutorialType.ATTRACT:
                {
                    if (!attract_complete)
                    {
                        new_tutoiral_text = new SpecialText.SpecialTextData();
                        new_tutoiral_text.CreateCharacterData(attract_tutoiral);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,32),
                                    new SpecialText.TextProperties.CharSpeed(35)
                                    },
                                    0,
                                    attract_tutoiral.Length
                            );
                        GAME_UI.Instance.state_Tutorial.box.string_data = new_tutoiral_text;
                        GAME_UI.Instance.state_Tutorial.box.Appear();
                       
                        return true;
                    }
                }
                break;
            case TutorialType.EMOTE:
                {
                    if(!emote_complete)
                    {
                        new_tutoiral_text = new SpecialText.SpecialTextData();
                        new_tutoiral_text.CreateCharacterData(emote_tutoiral);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,32),
                                    new SpecialText.TextProperties.CharSpeed(35)
                                    },
                                    0,
                                    emote_tutoiral.Length
                            );

                        GAME_UI.Instance.state_Tutorial.box.string_data = new_tutoiral_text;
                        GAME_UI.Instance.state_Tutorial.box.Appear();
                        
                        return true;
                    }

                }
                break;
            case TutorialType.REEL:
                {
                    if(!reel_complete)
                    {
                        new_tutoiral_text = new SpecialText.SpecialTextData();
                        new_tutoiral_text.CreateCharacterData(reel_tutoiral);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,32),
                                    new SpecialText.TextProperties.CharSpeed(35)
                                    },
                                    0,
                                    reel_tutoiral.Length
                            );

                        GAME_UI.Instance.state_Tutorial.box.string_data = new_tutoiral_text;
                        GAME_UI.Instance.state_Tutorial.box.Appear();
                        
                        return true;
                    }

                }
                break;
            default:
                break;
        }

        return false;
    }

    public bool CloseTutorial(TutorialType type)
    {

        switch (type)
        {
            case TutorialType.CASTING:
                casting_complete = true;
                break;
            case TutorialType.ATTRACT:
                attract_complete = true;
                break;
            case TutorialType.EMOTE:
                emote_complete = true;
                break;
            case TutorialType.REEL:
                reel_complete = true;
                break;
            default:
                break;
        }

        GAME_UI.Instance.state_Tutorial.box.Disappear();

        

        return false;

    }
}



