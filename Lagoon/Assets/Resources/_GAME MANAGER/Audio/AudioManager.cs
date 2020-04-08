﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    int maxAudioSources;

    Dictionary<string, AudioSFX> SFX = new Dictionary<string, AudioSFX>();

    List<GameObject> availableSFXInstanceObjs = new List<GameObject>();

    List<SFXInstance> sfxInstances = new List<SFXInstance>();


    AudioMixer masterMixer;
    AudioMixer nonMenuMixer;


    AudioMixerGroup masterFXMixerGroup;
    AudioMixerGroup masterMusicMixerGroup;
    AudioMixerGroup nonMenuFXMixerGroup;



    AudioSource musicSource_Current;
    void Start()
    {
        masterMixer = (AudioMixer)Resources.Load("Sound FX/MasterMixer");
        masterMusicMixerGroup = masterMixer.FindMatchingGroups("Music")[0];
        masterFXMixerGroup = masterMixer.FindMatchingGroups("Sound FX")[0];

        nonMenuMixer = (AudioMixer)Resources.Load("Sound FX/NonMenuFXMixer");
        nonMenuFXMixerGroup = nonMenuMixer.FindMatchingGroups("Master")[0];


        Object[] sFXes = Resources.LoadAll("", typeof(AudioSFX));

        for (int i = 0; i < sFXes.Length; i++)
        {
            SFX.Add(sFXes[i].name, (AudioSFX)sFXes[i]);
        }

        for (int i = 0; i < maxAudioSources; i++)
        {
            GameObject sfxInstance = new GameObject("SFX Instance", typeof(AudioSource));
            sfxInstance.transform.SetParent(transform,false);
            sfxInstance.SetActive(false);
            availableSFXInstanceObjs.Add(sfxInstance);
        }


        musicSource_Current = gameObject.AddComponent<AudioSource>();
        musicSource_Current.priority = 0;
    }

    private void Update()
    {
        bool isPaused = (GM_.Instance.pause.GetPausedState() == PauseManager.PAUSED_STATE.PAUSED); 
       sfxInstances.RemoveAll( y =>
       {
           if (!isPaused)
               y.Update();
           else if (y.source?.outputAudioMixerGroup == masterFXMixerGroup)
               y.Update();

           return completedSFXInstanceCheck(y);
       }           
       );      
    }

    bool completedSFXInstanceCheck(SFXInstance instance)
    {
        if (instance.isCompleted)
        {
            GameObject sfxInstanceObj;

            if (instance.source == null) // hooked on object was destroyed
                sfxInstanceObj = new GameObject("SFX Instance", typeof(AudioSource));
            else
                sfxInstanceObj = instance.source.gameObject;

            sfxInstanceObj.transform.SetParent(transform, false);
            sfxInstanceObj.SetActive(false);
            availableSFXInstanceObjs.Add(sfxInstanceObj);

            instance.source = null;
            return true;
        }
        return false;
    }

    public AudioSFX GetSFX(string name)
    {
        AudioSFX output;
        SFX.TryGetValue(name, out output);
        return output;
    }


    public void PlayMusic(AudioClip musicClip, TweenManager.TweenPath currentMusicFadeOutVolumeTween = null, TweenManager.TweenPath newMusicFadeInVolumeTween = null)
    {

    }
    public void StopMusic(TweenManager.TweenPath fadeOutVolumeTween = null)
    {

    }


    /// <summary>
    /// Play a sound effect
    /// Return an interface to the SFX Instance. You can further change settings via the interface however, note that a change to a setting via the interface will remove the setting class applied to that setting.
    /// With the interface it is possible to use the TweenAnimator to create complex modulations to the audio instance.
    /// </summary>
    /// <param name="audioSFX"> The audioSFX scriptable object. THis can be gained either by using the GetSFX, or by linking the reference</param>
    /// <param name="sourceTransform"> what object the audio source will become the audio sources parent. This is where 3D sounds will come from</param>
    /// <param name="IsMenuSound"> Is this sound part of a menu?</param>
    /// <param name="settingLoop"></param>
    /// <param name="settingMute"></param>
    /// <param name="settingPanning"></param>
    /// <param name="settingPitch"></param>
    /// <param name="settingPriority"></param>
    /// <param name="settingSpatialBlend"></param>
    /// <param name="settingVolume"></param>
    public SFXInstanceInterface PlaySFX(AudioSFX audioSFX, Transform sourceTransform, bool IsMenuSound = false, SFXSettings.Loop.Base settingLoop = null, SFXSettings.Mute.Base settingMute = null, SFXSettings.Panning.Base settingPanning = null, SFXSettings.Pitch.Base settingPitch = null, SFXSettings.Priority.Base settingPriority = null, SFXSettings.SpatialBlend.Base settingSpatialBlend = null, SFXSettings.Volume.Base settingVolume = null)
    {
        AudioMixerGroup mixerGroup = (IsMenuSound) ? masterFXMixerGroup : nonMenuFXMixerGroup;

        SFXInstanceInterface instanceInterface = null;
        if (availableSFXInstanceObjs.Count != 0)
        {
            GameObject sfxInstanceObj = availableSFXInstanceObjs[availableSFXInstanceObjs.Count - 1];
            availableSFXInstanceObjs.RemoveAt(availableSFXInstanceObjs.Count - 1);

            sfxInstanceObj.transform.SetParent(sourceTransform,false);
            sfxInstanceObj.SetActive(true);
            SFXInstance instance = new SFXInstance(audioSFX, sfxInstanceObj.GetComponent<AudioSource>(), mixerGroup, settingLoop, settingMute, settingPanning, settingPitch, settingPriority, settingSpatialBlend, settingVolume);
            sfxInstances.Add(instance);
            instanceInterface = new SFXInstanceInterface(instance);
        }
        else // all sfx objs in the pool are being used
        {
            int myPriority = (settingPriority == null) ? audioSFX.Priority : settingPriority.CurrentValue;

            for (int i = 0; i < sfxInstances.Count; i++)
            {
                if (myPriority <= sfxInstances[i].priority)
                {
                    sfxInstances[i].isCompleted = true;
                    completedSFXInstanceCheck(sfxInstances[i]);
                    sfxInstances.RemoveAt(i);


                    GameObject sfxInstanceObj = availableSFXInstanceObjs[availableSFXInstanceObjs.Count - 1];
                    availableSFXInstanceObjs.RemoveAt(availableSFXInstanceObjs.Count - 1);

                    sfxInstanceObj.transform.SetParent(sourceTransform, false);
                    sfxInstanceObj.SetActive(true);
                    SFXInstance instance = new SFXInstance(audioSFX, sfxInstanceObj.GetComponent<AudioSource>(), mixerGroup, settingLoop, settingMute, settingPanning, settingPitch, settingPriority, settingSpatialBlend, settingVolume);
                    sfxInstances.Add(instance);
                    instanceInterface = new SFXInstanceInterface(instance);
                    break;
                }
            }
        }

        return instanceInterface;
    }

    

    public class SFXInstanceInterface
    {

        SFXInstance instance;
        public SFXInstanceInterface(SFXInstance instance_)
        {
            instance = instance_;
        }


        public void Stop()
        {
            instance.source?.Stop();
        }

        public bool Mute
        {
            set
            {
                instance.isMuted = value;
                if (instance.source != null)
                    instance.source.mute = instance.isMuted;
                instance.settingMute = null;
            }
            get
            {
                return instance.isMuted;
            }
        }
        public bool Loop
        {
            set
            {
                instance.isLooping = value;
                if (instance.source != null)
                    instance.source.loop = instance.isLooping;
                instance.settingLoop = null;
            }
            get
            {
                return instance.isLooping;
            }
        }
        public float Volume
        {
            set
            {
                instance.volume = value;
                if (instance.source != null)
                    instance.source.volume = instance.volume;
                instance.settingLoop = null;
            }
            get
            {
                return instance.volume;
            }
        }
        public float Panning
        {
            set
            {
                instance.panning = value;
                if (instance.source != null)
                    instance.source.panStereo = instance.panning;
                instance.settingPanning = null;
            }
            get
            {
                return instance.panning;
            }
        }
        public float SpatialBlend
        {
            set
            {
                instance.spatialBlend2DTo3D = value;
                if (instance.source != null)
                    instance.source.spatialBlend = instance.spatialBlend2DTo3D;
                instance.settingSpatialBlend = null;
            }
            get
            {
                return instance.spatialBlend2DTo3D;
            }
        }
        public float Pitch
        {
            set
            {
                instance.pitch = value;
                if (instance.source != null)
                    instance.source.pitch = instance.pitch;
                instance.settingPitch = null;
            }
            get
            {
                return instance.pitch;
            }
        }
        public int Priority
        {
            set
            {
                instance.priority = value;
                if (instance.source != null)
                    instance.source.priority = instance.priority;
                instance.settingPriority = null;
            }
            get
            {
                return instance.priority;
            }
        }
    }

    public class SFXInstance
    {

        public bool isCompleted;

        public int priority;
        public bool isLooping;
        public bool isMuted;
        public float pitch;
        public float spatialBlend2DTo3D;
        public float volume;
        public float panning;

        public AudioSFX audioSFX;
        public AudioSource source;

        public SFXSettings.Loop.Base settingLoop = null;
        public SFXSettings.Mute.Base settingMute = null;
        public SFXSettings.Panning.Base settingPanning = null;
        public SFXSettings.Pitch.Base settingPitch = null;
        public SFXSettings.Priority.Base settingPriority = null;
        public SFXSettings.SpatialBlend.Base settingSpatialBlend = null;
        public SFXSettings.Volume.Base settingVolume = null;

        public SFXInstance(AudioSFX audioSFX_,AudioSource source_, AudioMixerGroup mixerGroup = null, SFXSettings.Loop.Base settingLoop_ = null, SFXSettings.Mute.Base settingMute_ = null, SFXSettings.Panning.Base settingPanning_ = null, SFXSettings.Pitch.Base settingPitch_ = null, SFXSettings.Priority.Base settingPriority_ = null, SFXSettings.SpatialBlend.Base settingSpatialBlend_ = null, SFXSettings.Volume.Base settingVolume_ = null)
        {
            isCompleted = false;

            audioSFX = audioSFX_;
            source = source_;
            settingLoop = settingLoop_;
            settingMute = settingMute_;
            settingPanning = settingPanning_;
            settingPitch = settingPitch_;
            settingPriority = settingPriority_;
            settingSpatialBlend = settingSpatialBlend_;
            settingVolume = settingVolume_;
         

            if (settingPriority != null)
            {
                priority = settingPriority.CurrentValue;
                if (settingPriority.GetType() == typeof(SFXSettings.Priority.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingPriority = null;
            }
            else
            {
                priority = audioSFX.Priority;
            }

            if (settingLoop != null)
            {
                isLooping = settingLoop.CurrentValue;
                if (settingLoop.GetType() == typeof(SFXSettings.Loop.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingLoop = null;
            }
            else
            {
                isLooping = audioSFX.IsLooping;
            }

            if (settingPitch != null)
            {
                pitch = settingPitch.CurrentValue;
                if (settingPitch.GetType() == typeof(SFXSettings.Pitch.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingPitch = null;
            }
            else
            {
                pitch = audioSFX.Pitch;
            }

            if (settingSpatialBlend != null)
            {
                spatialBlend2DTo3D = settingSpatialBlend.CurrentValue;
                if (settingSpatialBlend.GetType() == typeof(SFXSettings.SpatialBlend.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingSpatialBlend = null;
            }
            else
            {
                spatialBlend2DTo3D = audioSFX.SpatialBlend2DTo3D;
            }

            if (settingVolume != null)
            {
                volume = settingVolume.CurrentValue;
                if (settingVolume.GetType() == typeof(SFXSettings.Volume.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingVolume = null;
            }
            else
            {
                volume = audioSFX.Volume;
            }

            if (settingPanning != null)
            {
                panning = settingPanning.CurrentValue;
                if (settingPanning.GetType() == typeof(SFXSettings.Panning.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingPanning = null;
            }
            else
            {
                panning = audioSFX.Panning;
            }

            if (settingMute != null)
            {
                isMuted = settingMute.CurrentValue;
                if (settingMute.GetType() == typeof(SFXSettings.Mute.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingMute = null;
            }
            else
            {
                isMuted = false;
            }

            source.clip = audioSFX.Clip;
            source.loop = isLooping;
            source.mute = isMuted;
            source.volume = volume;
            source.panStereo = panning;
            source.spatialBlend = spatialBlend2DTo3D;
            source.pitch = pitch;
            source.priority = priority;
            source.outputAudioMixerGroup = mixerGroup;
            source.Play();
        }

        public void Update()
        {
            // gameObject which had source attached to it was destroyed
            if (source == null)
            {
                isCompleted = true;
                return;
            }


            if (settingPriority != null)
            {
                settingPriority.Update();
                priority = settingPriority.CurrentValue;
                source.priority = priority;

                if (settingPriority.IsCompleted)
                    settingPriority = null;
            }

            if (settingLoop != null)
            {
                settingLoop.Update();
                isLooping = settingLoop.CurrentValue;
                source.loop = isLooping;

                if (settingLoop.IsCompleted)
                    settingLoop = null;
            }

            if (settingPitch != null)
            {
                settingPitch.Update();
                pitch = settingPitch.CurrentValue;
                source.pitch = pitch;

                if (settingPitch.IsCompleted)
                    settingPitch = null;
            }

            if (settingSpatialBlend != null)
            {
                settingSpatialBlend.Update();
                spatialBlend2DTo3D = settingSpatialBlend.CurrentValue;
                source.spatialBlend = spatialBlend2DTo3D;

                if (settingSpatialBlend.IsCompleted)
                    settingSpatialBlend = null;
            }

            if (settingVolume != null)
            {
                settingVolume.Update();
                volume = settingVolume.CurrentValue;
                source.volume = volume;

                if (settingVolume.IsCompleted)
                    settingVolume = null;
            }

            if (settingPanning != null)
            {
                settingPanning.Update();
                panning = settingPanning.CurrentValue;
                source.panStereo = panning;
                if (settingPanning.IsCompleted)
                    settingPanning = null;
            }

            if (settingMute != null)
            {
                settingMute.Update();
                isMuted = settingMute.CurrentValue;
                source.mute = isMuted;

                if (settingMute.IsCompleted)
                    settingMute = null;
            }

            if (!source.isPlaying)
                isCompleted = true;
        }   
    }


}