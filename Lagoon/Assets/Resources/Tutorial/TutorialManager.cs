using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    //string casting_tutorial = "Pull the left analogue stick back to get ready to cast, flick it forward to cast.";
    //string attract_tutoiral = "Feel for the fish getting attracted to the bob. If they are attracted don't move away or they will get scared. When they bite '!' hook them on by holding RT. But be careful, it may just be testing the bob '?' and get frightened if you try to hook to early.";
    string noFish_tutorial = "Hmmmmm, fish arn't interested in the bob. Reel in to try and attract them or cast out again";
    string reel_tutoiral = "You have hooked a fish! Reel him in with RT. Don't let the line snap or you will lose him";

    [SerializeField] string reel_tutorial;

    SpecialText.SpecialTextData new_tutoiral_text = new SpecialText.SpecialTextData();
    


    bool casting_complete;
    bool attract_complete;
    public bool no_fish_complete;
    bool reel_complete;

    public enum TutorialType
    {
        CASTING,
        ATTRACT,
        NOFISH,
        REEL,
        OTHER
    }

    TutorialType currently_playing;

    public bool WhatTutorial(TutorialType type)
    {
        switch (type)
        {
            case TutorialType.CASTING:
                {
                    if(!casting_complete)
                    {
                        currently_playing = TutorialType.CASTING;


                        new_tutoiral_text = new SpecialText.SpecialTextData();
                        string casting_tutorial = "Pull the ";
                        int character_data = 0;

                        new_tutoiral_text.CreateCharacterData(casting_tutorial);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,255),
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
                                    new SpecialText.TextProperties.Colour(80,238,67),
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
                                    new SpecialText.TextProperties.Colour(255,255,255),
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
                        currently_playing = TutorialType.ATTRACT;

                        GAME_UI.Instance.state_Tutorial.box.string_data = new SpecialText.SpecialTextData();
                        new_tutoiral_text = new SpecialText.SpecialTextData();
                        string attract_tutoiral = "Feel for the fish ";// nibbling at the bob. If they are attracted don't move away or they will get scared. When they bite '!' hook them on by holding RT. But be careful, it may just be testing the bob '?' and get frightened if you try to hook to early.";
                        int character_data = 0;

                        new_tutoiral_text.CreateCharacterData(attract_tutoiral);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,255),
                                    new SpecialText.TextProperties.CharSpeed(35)
                            },
                                    character_data,
                                    attract_tutoiral.Length
                            );

                        character_data += attract_tutoiral.Length;


                        attract_tutoiral = "nibbling ";

                        new_tutoiral_text.AddCharacterData(attract_tutoiral);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(80,238,67),
                                    new SpecialText.TextProperties.CharSpeed(35),
                                    new SpecialText.TextProperties.Shiver(0.05f, 10f),
                            },
                                    character_data,
                                    attract_tutoiral.Length
                            );

                        character_data += attract_tutoiral.Length;
                        Debug.Log(character_data);

                        attract_tutoiral = " If they are attracted don't move away or they will get scared. When they bite '!' hook them on by holding RT. But be careful, it may just be testing the bob '?' and get frightened if you try to hook to early.";

                        new_tutoiral_text.AddCharacterData(attract_tutoiral);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,255),
                                    new SpecialText.TextProperties.CharSpeed(35)
                            },
                                    character_data,
                                    attract_tutoiral.Length
                            );

                        //character_data += attract_tutoiral.Length;
                        //Debug.Log(character_data);

                        //attract_tutoiral = "'!' ";

                        //Debug.Log(attract_tutoiral);

                        //new_tutoiral_text.AddCharacterData(attract_tutoiral);
                        //new_tutoiral_text.AddPropertyToText(
                        //    new List<SpecialText.TextProperties.Base>()
                        //    {
                        //            new SpecialText.TextProperties.Colour(255,255,255),
                        //            new SpecialText.TextProperties.CharSpeed(35)
                        //            //new SpecialText.TextProperties.Colour(80,238,67),
                        //            //new SpecialText.TextProperties.CharSpeed(35),
                        //            //new SpecialText.TextProperties.WaveScaled(1,3,1)
                        //    },
                        //            character_data,
                        //            attract_tutoiral.Length
                        //    );

                        //character_data += attract_tutoiral.Length;
                        //Debug.Log(character_data);


                        //attract_tutoiral = "hook them on by holding ";

                        //new_tutoiral_text.AddCharacterData(attract_tutoiral);
                        //new_tutoiral_text.AddPropertyToText(
                        //    new List<SpecialText.TextProperties.Base>()
                        //    {
                        //            new SpecialText.TextProperties.Colour(255,255,255),
                        //            new SpecialText.TextProperties.CharSpeed(35)
                        //    },
                        //            character_data,
                        //            attract_tutoiral.Length
                        //    );

                        //character_data += attract_tutoiral.Length;


                        //attract_tutoiral = "RT";

                        //new_tutoiral_text.AddCharacterData(attract_tutoiral);
                        //new_tutoiral_text.AddPropertyToText(
                        //    new List<SpecialText.TextProperties.Base>()
                        //    {
                        //            new SpecialText.TextProperties.Colour(80,238,67),
                        //            new SpecialText.TextProperties.CharSpeed(35),
                        //            new SpecialText.TextProperties.WaveScaled(1,3,1)
                        //    },
                        //            character_data,
                        //            attract_tutoiral.Length
                        //    );

                        //character_data += attract_tutoiral.Length;

                        //attract_tutoiral = ". But be careful, it may just be testing the bob ";

                        //new_tutoiral_text.AddCharacterData(attract_tutoiral);
                        //new_tutoiral_text.AddPropertyToText(
                        //    new List<SpecialText.TextProperties.Base>()
                        //    {
                        //            new SpecialText.TextProperties.Colour(255,255,255),
                        //            new SpecialText.TextProperties.CharSpeed(35)
                        //    },
                        //            character_data,
                        //            attract_tutoiral.Length
                        //    );

                        //character_data += attract_tutoiral.Length;

                        //attract_tutoiral = "'?' ";

                        //new_tutoiral_text.AddCharacterData(attract_tutoiral);
                        //new_tutoiral_text.AddPropertyToText(
                        //    new List<SpecialText.TextProperties.Base>()
                        //    {
                        //            new SpecialText.TextProperties.Colour(247,158,32),
                        //            new SpecialText.TextProperties.CharSpeed(35),
                        //            new SpecialText.TextProperties.WaveScaled(1,3,1)
                        //    },
                        //            character_data,
                        //            attract_tutoiral.Length
                        //    );

                        //character_data += attract_tutoiral.Length;

                        //attract_tutoiral = "and get frightened if you try to hook to early.";

                        //new_tutoiral_text.AddCharacterData(attract_tutoiral);
                        //new_tutoiral_text.AddPropertyToText(
                        //    new List<SpecialText.TextProperties.Base>()
                        //    {
                        //            new SpecialText.TextProperties.Colour(255,255,255),
                        //            new SpecialText.TextProperties.CharSpeed(35)
                        //    },
                        //            character_data,
                        //            attract_tutoiral.Length
                        //    );

                        //character_data += attract_tutoiral.Length;




                        //new_tutoiral_text.CreateCharacterData(attract_tutoiral);
                        //new_tutoiral_text.AddPropertyToText(
                        //    new List<SpecialText.TextProperties.Base>()
                        //    {
                        //            new SpecialText.TextProperties.Colour(255,255,32),
                        //            new SpecialText.TextProperties.CharSpeed(35)
                        //            },
                        //            0,
                        //            attract_tutoiral.Length
                        //    );
                        GAME_UI.Instance.state_Tutorial.box.string_data = new_tutoiral_text;
                        GAME_UI.Instance.state_Tutorial.box.Appear();

                        return true;
                    }
                }
                break;
            case TutorialType.NOFISH:
                {
                    if(!no_fish_complete)
                    {
                        currently_playing = TutorialType.NOFISH;
                        new_tutoiral_text = new SpecialText.SpecialTextData();
                        new_tutoiral_text.CreateCharacterData(noFish_tutorial);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,255),
                                    new SpecialText.TextProperties.CharSpeed(35)
                                    },
                                    0,
                                    noFish_tutorial.Length
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
                        currently_playing = TutorialType.REEL;
                        new_tutoiral_text = new SpecialText.SpecialTextData();

                        //new_tutoiral_text.CreateCharacterData(reel_tutoiral);
                        //new_tutoiral_text.AddPropertyToText(
                        //    new List<SpecialText.TextProperties.Base>()
                        //    {
                        //            new SpecialText.TextProperties.Colour(255,255,32),
                        //            new SpecialText.TextProperties.CharSpeed(35)
                        //            },
                        //            0,
                        //            reel_tutoiral.Length
                        //    );

                        //GAME_UI.Instance.state_Tutorial.box.string_data = new_tutoiral_text;
                        //GAME_UI.Instance.state_Tutorial.box.Appear();



                        new_tutoiral_text = new SpecialText.SpecialTextData();
                        string ree_tutorial = "You have hooked a fish! Reel him in with ";
                        int character_data = 0;

                        new_tutoiral_text.CreateCharacterData(ree_tutorial);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,255),
                                    new SpecialText.TextProperties.CharSpeed(35)
                            },
                                    character_data,
                                    ree_tutorial.Length
                            );

                        character_data += ree_tutorial.Length;

                        ree_tutorial = "RT ";

                        new_tutoiral_text.AddCharacterData(ree_tutorial);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(80,238,67),
                                    new SpecialText.TextProperties.CharSpeed(35),
                                    //new SpecialText.TextProperties.WaveScaled(1,3,1)
                            },
                                    character_data,
                                    ree_tutorial.Length
                            );

                        character_data += ree_tutorial.Length;

                        ree_tutorial = "Don't let the line snap or you will lose him";

                        new_tutoiral_text.AddCharacterData(ree_tutorial);
                        new_tutoiral_text.AddPropertyToText(
                            new List<SpecialText.TextProperties.Base>()
                            {
                                    new SpecialText.TextProperties.Colour(255,255,255),
                                    new SpecialText.TextProperties.CharSpeed(35)
                            },
                                    character_data,
                                    ree_tutorial.Length
                            );

                        character_data += ree_tutorial.Length;

                        //GAME_UI.Instance.state_Tutorial.box.tmp.text = casting_tutorial;
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
        if(GAME_UI.Instance.state_Tutorial.box.specialText.AreAllCompleted())
        {
            switch (currently_playing)
            {
                case TutorialType.CASTING:
                    casting_complete = true;
                    break;
                case TutorialType.ATTRACT:
                    attract_complete = true;
                    break;
                case TutorialType.NOFISH:
                    no_fish_complete = true;
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
        else
        {
            GAME_UI.Instance.state_Tutorial.box.SkipTransition();
        }

        return true;

    }
}
