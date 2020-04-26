using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    int maxAudioSources;

    Dictionary<string, AudioSFX> SFX = new Dictionary<string, AudioSFX>();
    Dictionary<string, AudioMusic> MUSIC = new Dictionary<string, AudioMusic>();

    List<GameObject> availableSFXInstanceObjs = new List<GameObject>();

    List<SFXInstance> sfxInstances = new List<SFXInstance>();
    List<MusicInstance> musicInstances = new List<MusicInstance>();


    AudioMixer masterMixer;
    AudioMixer nonMenuMixer;


    AudioMixerGroup masterFXMixerGroup;
    AudioMixerGroup masterMusicMixerGroup;
    AudioMixerGroup nonMenuFXMixerGroup;

    AudioSource musicSource_Current;

    public float MasterVolume
    { 
    get
        {
            float output;
            masterMixer.GetFloat("MasterVolume",out output);
            return output;
        }
    set
        {
            masterMixer.SetFloat("MasterVolume", value);
        }
    }
    public float MusicVolume
    {
        get
        {
            float output;
            masterMixer.GetFloat("MusicVolume", out output);
            return output;
        }
        set
        {
            masterMixer.SetFloat("MusicVolume", value);
        }
    }
    public float SFXVolume
    {
        get
        {
            float output;
            masterMixer.GetFloat("SFXVolume", out output);
            return output;
        }
        set
        {
            masterMixer.SetFloat("SFXVolume", value);
        }
    }


    bool hasBeenInit = false;

    int uniqueIDCount = 0;

    void Init()
    {
        hasBeenInit = true;
        uniqueIDCount = 0;

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

        Object[] music = Resources.LoadAll("", typeof(AudioMusic));

        for (int i = 0; i < music.Length; i++)
        {
            MUSIC.Add(music[i].name, (AudioMusic)music[i]);
        }


        for (int i = 0; i < maxAudioSources; i++)
        {
            GameObject sfxInstance = new GameObject("SFX Instance", typeof(AudioSource));
            sfxInstance.transform.SetParent(transform, false);
            sfxInstance.SetActive(false);
            availableSFXInstanceObjs.Add(sfxInstance);
        }


        musicSource_Current = gameObject.AddComponent<AudioSource>();
        musicSource_Current.priority = 0;
    }

    void Awake()
    {
        if (!hasBeenInit)
            Init();
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


        musicInstances.RemoveAll( y =>
        {
            y.Update();
            return completedMusicInstanceCheck(y);
        }
        ); 
    }

    bool completedMusicInstanceCheck(MusicInstance instance)
    {
        if (instance.isCompleted)
        {
            Destroy(instance.source.gameObject);
            return true;
        }
        return false;
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
        if (!hasBeenInit)
            Init();

        AudioSFX output;
        SFX.TryGetValue(name, out output);
        return output;
    }
    public AudioMusic GetMUSIC(string name)
    {
        if (!hasBeenInit)
            Init();

        AudioMusic output;
        MUSIC.TryGetValue(name, out output);
        return output;
    }


    public SFXInstanceInterface GetSFXInstanceUsingUniqueID(int uniqueID)
    {
        for (int i = 0; i < sfxInstances.Count; i++)
        {
            if (sfxInstances[i].uniqueID == uniqueID)
            {
                return new SFXInstanceInterface(sfxInstances[i]);
            }
        }
        return null;
    }
    public SFXInstanceInterface GetFirstSFXInstanceUsingAppliedID(object appliedID)
    {
        for (int i = 0; i < sfxInstances.Count; i++)
        {
            if (sfxInstances[i].appliedID == appliedID)
            {
                return new SFXInstanceInterface(sfxInstances[i]);
            }
        }
        return null;
    }
    public List<SFXInstanceInterface> GetFirstSFXInstancesUsingAppliedID(object appliedID)
    {
        List<SFXInstanceInterface> interfaces = new List<SFXInstanceInterface>();
        for (int i = 0; i < sfxInstances.Count; i++)
        {
            if (sfxInstances[i].appliedID == appliedID)
            {
                interfaces.Add(new SFXInstanceInterface(sfxInstances[i]));
            }
        }
        return interfaces;
    }


    public MusicInstanceInterface PlayMusic(AudioMusic audioMusic, AudioSettings.MusicOnly.Volume.Fade fadeInOfNewMusic = null, AudioSettings.MusicOnly.Volume.Fade fadeOutOfOldMusic = null)
    {

        if (musicInstances.Count != 0)
        {
            musicInstances[musicInstances.Count - 1].Stop(fadeOutOfOldMusic);
        }

        GameObject musicInstanceObj = new GameObject("Music Instance", typeof(AudioSource));
        musicInstanceObj.transform.SetParent(transform, false);

        MusicInstance newInstance = new MusicInstance(audioMusic, musicInstanceObj.GetComponent<AudioSource>(), mixerGroup: masterMusicMixerGroup,  fadeIn_: fadeInOfNewMusic);

        musicInstances.Add(newInstance);


        return new MusicInstanceInterface(newInstance);
    }

    public enum MUSIC_FADE_PRESETS
    { 
    DEFAULT_FADEIN,
    DEFAULT_FADEOUT   
    }

    public AudioSettings.MusicOnly.Volume.Fade GetMusicFadePreset(MUSIC_FADE_PRESETS preset)
    {
        switch (preset)
        {
            case MUSIC_FADE_PRESETS.DEFAULT_FADEIN:
                {
                    return new AudioSettings.MusicOnly.Volume.Fade(
                        new TweenManager.TweenPath(
                            new TweenManager.TweenPart_Start(0,1,2,TweenManager.CURVE_PRESET.LINEAR)
                        )
                    );
                }

            case MUSIC_FADE_PRESETS.DEFAULT_FADEOUT:
                {
                    return new AudioSettings.MusicOnly.Volume.Fade(
                        new TweenManager.TweenPath(
                            new TweenManager.TweenPart_Start(1, 0, 2, TweenManager.CURVE_PRESET.LINEAR)
                        )
                    );
                }
        }
        return null;
    }



    // Stops the primary music instance
    public void StopMusic(AudioSettings.MusicOnly.Volume.Fade fadeOut = null)
    {
        if (musicInstances.Count != 0)
        {
            musicInstances[musicInstances.Count - 1].Stop(fadeOut);
        }
    }



    /// <summary>
    /// The primary music instance. This is the most recently created MusicInstance. 
    /// 
    /// returns null if no music instances
    /// </summary>
    /// <returns></returns>
    public MusicInstanceInterface GetPrimaryActiveMusicInstance()
    {
        if (musicInstances.Count != 0)
        {
            return new MusicInstanceInterface(musicInstances[musicInstances.Count - 1]);
        }
        return null;
    }


    /// <summary>
    /// All the music instances which are currently playing music. There is only one primary music instance, however there are multiple active when music is being cross-faded
    /// The list is in order of activation. 
    ///     Index 0: Oldest
    ///     Index Count - 1: Newest (Primary MusicInstance)
    /// </summary>
    /// <returns></returns>
    public List<MusicInstanceInterface> GetAllActiveMusicInstances()
    {
        List<MusicInstanceInterface> musicInterfaces = new List<MusicInstanceInterface>();

        for (int i = 0; i <  musicInstances.Count; i++)
        {
            musicInterfaces.Add(new MusicInstanceInterface(musicInstances[i]));
        }
        return musicInterfaces;
    }


    public class MusicInstance
    {
        public AudioMusic audioMUSIC;
        public AudioSource source;
        public bool isStopping = false;
        public bool isCompleted = false;

        public float audioMusicAssetVolume;
        AudioSettings.MusicOnly.Volume.Fade fadeOut;
        AudioSettings.MusicOnly.Volume.Fade fadeIn;
        public MusicInstance(AudioMusic audioMusic, AudioSource audioSource, AudioMixerGroup mixerGroup = null, AudioSettings.MusicOnly.Volume.Fade fadeIn_ = null)
        {

            audioMUSIC = audioMusic;
            audioMusicAssetVolume = audioMUSIC.Volume;
            source = audioSource;
            source.clip = audioMUSIC.Clip;
            source.volume = audioSource.volume;
            source.pitch = 1;
            source.spatialize = false;
            source.loop = true;
            source.outputAudioMixerGroup = mixerGroup;
            source.priority = 0;
            fadeIn = fadeIn_;
            if (fadeIn != null)
            {
                fadeIn = fadeIn_;
                source.volume = fadeIn.Value * audioMusicAssetVolume;
            }


            source.Play();
        }
        public void Update()
        {
            if (isStopping)
            {
                if (fadeOut != null)
                {
                    fadeOut.Update();
                    source.volume = fadeOut.Value * audioMusicAssetVolume;

                    if (fadeOut.Completed)
                    {
                        isCompleted = true;
                        source.Stop();
                    }
                }
            }
            else
            {
                if (fadeIn != null)
                {
                    fadeIn.Update();
                    source.volume = fadeIn.Value * audioMusicAssetVolume;
                }
            }
        }

        public void Stop(AudioSettings.MusicOnly.Volume.Fade fadeOut_ = null)
        {
            if (!isStopping)
            {
                isStopping = true;
                fadeOut = fadeOut_;

                if (fadeOut != null)
                {
                    fadeOut = fadeOut_;
                }
                else
                {
                    source.Stop();
                    isCompleted = true;
                }
            }
        }
    }

    public class MusicInstanceInterface
    {
        MusicInstance instance;
        public MusicInstanceInterface(MusicInstance instance_)
        {
            instance = instance_;
        }
        public AudioMusic SourceAudioMusic => instance.audioMUSIC;
        public string AudioMusicName => SourceAudioMusic.name;
        public bool IsStopping => instance.isStopping;
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
    /// 


    public SFXInstanceInterface PlaySFX(AudioSFX audioSFX, Transform sourceTransform, bool IsMenuSound = false, AudioSettings.AnyBoolSetting.BoolBase settingLoop = null, AudioSettings.AnyBoolSetting.BoolBase settingMute = null, AudioSettings.AnyFloatSetting.FloatBase settingPanning = null, AudioSettings.AnyFloatSetting.FloatBase settingPitch = null, AudioSettings.AnyIntSetting.IntBase settingPriority = null, AudioSettings.AnyFloatSetting.FloatBase settingSpatialBlend = null, AudioSettings.AnyFloatSetting.FloatBase settingVolume = null, object appliedID = null)
    {
        uniqueIDCount++;

        AudioMixerGroup mixerGroup = (IsMenuSound) ? masterFXMixerGroup : nonMenuFXMixerGroup;

        SFXInstanceInterface instanceInterface = null;
        if (availableSFXInstanceObjs.Count != 0)
        {
            GameObject sfxInstanceObj = availableSFXInstanceObjs[availableSFXInstanceObjs.Count - 1];
            availableSFXInstanceObjs.RemoveAt(availableSFXInstanceObjs.Count - 1);

            sfxInstanceObj.transform.SetParent(sourceTransform,false);
            sfxInstanceObj.SetActive(true);
            SFXInstance instance = new SFXInstance(audioSFX, sfxInstanceObj.GetComponent<AudioSource>(), uniqueIDCount, mixerGroup, settingLoop, settingMute, settingPanning, settingPitch, settingPriority, settingSpatialBlend, settingVolume, appliedID);
            sfxInstances.Add(instance);
            instanceInterface = new SFXInstanceInterface(instance);
        }
        else // all sfx objs in the pool are being used
        {
            int myPriority = (settingPriority == null) ? audioSFX.Priority : settingPriority.Value;

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
                    SFXInstance instance = new SFXInstance(audioSFX, sfxInstanceObj.GetComponent<AudioSource>(), uniqueIDCount, mixerGroup, settingLoop, settingMute, settingPanning, settingPitch, settingPriority, settingSpatialBlend, settingVolume, appliedID);
                    sfxInstances.Add(instance);
                    instanceInterface = new SFXInstanceInterface(instance);
                    break;
                }
            }
        }

        return instanceInterface;
    }
    public SFXInstanceInterface PlaySFX(SFXArgs args) // args wrapper
    {
        return PlaySFX(args.audioSFX, args.sourceTransform, args.IsMenuSound, args.settingLoop, args.settingMute, args.settingPanning, args.settingPitch, args.settingPriority, args.settingSpatialBlend, args.settingVolume);
    }

    public class SFXArgs
    {
        public readonly AudioSFX audioSFX;
        public readonly Transform sourceTransform;
        public readonly bool IsMenuSound;
        public readonly AudioSettings.AnyBoolSetting.BoolBase settingLoop;
        public readonly AudioSettings.AnyBoolSetting.BoolBase settingMute;
        public readonly AudioSettings.AnyFloatSetting.FloatBase settingPanning;
        public readonly AudioSettings.AnyFloatSetting.FloatBase settingPitch;
        public readonly AudioSettings.AnyIntSetting.IntBase settingPriority;
        public readonly AudioSettings.AnyFloatSetting.FloatBase settingSpatialBlend;
        public readonly AudioSettings.AnyFloatSetting.FloatBase settingVolume;
        public SFXArgs(AudioSFX audioSFX_, Transform sourceTransform_, bool IsMenuSound_ = false, AudioSettings.AnyBoolSetting.BoolBase settingLoop_ = null, AudioSettings.AnyBoolSetting.BoolBase settingMute_ = null, AudioSettings.AnyFloatSetting.FloatBase settingPanning_ = null, AudioSettings.AnyFloatSetting.FloatBase settingPitch_ = null, AudioSettings.AnyIntSetting.IntBase settingPriority_ = null, AudioSettings.AnyFloatSetting.FloatBase settingSpatialBlend_ = null, AudioSettings.AnyFloatSetting.FloatBase settingVolume_ = null)
        {
            audioSFX = audioSFX_;
            sourceTransform = sourceTransform_;
            IsMenuSound = IsMenuSound_;
            settingLoop = settingLoop_;
            settingMute = settingMute_;
            settingPanning = settingPanning_;
            settingPitch = settingPitch_;
            settingPriority = settingPriority_;
            settingSpatialBlend = settingSpatialBlend_;
            settingVolume = settingVolume_;
        }
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
            if (instance != null)
            {
                if (instance.source != null)
                {
                    instance.source.Stop();
                }
            }
           
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

        public object AppliedID
        { 
         get
            {
                return instance.appliedID;
            }        
        }
        public int UniqueId
        { 
        get
            {
                return instance.uniqueID;
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

        public AudioSettings.AnyBoolSetting.BoolBase settingLoop = null;
        public AudioSettings.AnyBoolSetting.BoolBase settingMute = null;
        public AudioSettings.AnyFloatSetting.FloatBase settingPanning = null;
        public AudioSettings.AnyFloatSetting.FloatBase settingPitch = null;
        public AudioSettings.AnyIntSetting.IntBase settingPriority = null;
        public AudioSettings.AnyFloatSetting.FloatBase settingSpatialBlend = null;
        public AudioSettings.AnyFloatSetting.FloatBase settingVolume = null;


        public object appliedID;
        public int uniqueID;

        public SFXInstance(AudioSFX audioSFX_,AudioSource source_, int uniqueID_, AudioMixerGroup mixerGroup = null, AudioSettings.AnyBoolSetting.BoolBase settingLoop_ = null, AudioSettings.AnyBoolSetting.BoolBase settingMute_ = null, AudioSettings.AnyFloatSetting.FloatBase settingPanning_ = null, AudioSettings.AnyFloatSetting.FloatBase settingPitch_ = null, AudioSettings.AnyIntSetting.IntBase settingPriority_ = null, AudioSettings.AnyFloatSetting.FloatBase settingSpatialBlend_ = null, AudioSettings.AnyFloatSetting.FloatBase settingVolume_ = null, object appliedID_ = null)
        {
            
            isCompleted = false;
            appliedID = appliedID_;
            uniqueID = uniqueID_;
            audioSFX = audioSFX_;
            source = source_;
            settingLoop = settingLoop_;
            settingMute = settingMute_;
            settingPanning = settingPanning_;
            settingPitch = settingPitch_;
            settingPriority = settingPriority_;
            settingSpatialBlend = settingSpatialBlend_;
            settingVolume = settingVolume_;


            source.spatialize = true;


            if (settingPriority != null)
            {
                priority = settingPriority.Value;
                if (settingPriority.GetType() == typeof(AudioSettings.AnyIntSetting.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingPriority = null;
            }
            else
            {
                priority = audioSFX.Priority;
            }

            if (settingLoop != null)
            {
                isLooping = settingLoop.Value;
                if (settingLoop.GetType() == typeof(AudioSettings.AnyBoolSetting.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingLoop = null;
            }
            else
            {
                isLooping = audioSFX.IsLooping;
            }

            if (settingPitch != null)
            {
                pitch = settingPitch.Value;
                if (settingPitch.GetType() == typeof(AudioSettings.AnyFloatSetting.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingPitch = null;
            }
            else
            {
                pitch = audioSFX.Pitch;
            }

            if (settingSpatialBlend != null)
            {
                spatialBlend2DTo3D = settingSpatialBlend.Value;
                if (settingSpatialBlend.GetType() == typeof(AudioSettings.AnyFloatSetting.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingSpatialBlend = null;
            }
            else
            {
                spatialBlend2DTo3D = audioSFX.SpatialBlend2DTo3D;
            }

            if (settingVolume != null)
            {
                volume = settingVolume.Value;
                if (settingVolume.GetType() == typeof(AudioSettings.AnyFloatSetting.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingVolume = null;
            }
            else
            {
                volume = audioSFX.Volume;
            }

            if (settingPanning != null)
            {
                panning = settingPanning.Value;
                if (settingPanning.GetType() == typeof(AudioSettings.AnyFloatSetting.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
                    settingPanning = null;
            }
            else
            {
                panning = audioSFX.Panning;
            }

            if (settingMute != null)
            {
                isMuted = settingMute.Value;
                if (settingMute.GetType() == typeof(AudioSettings.AnyBoolSetting.Constant)) // reduce unessicary update cycles and memory by nulling pointer when the settings is constant
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
            source.dopplerLevel = audioSFX.DopplerEffect;
            source.maxDistance = audioSFX.AudioMaxDistance;
            source.minDistance = audioSFX.MinDistance;



            if (audioSFX.VolumeFallOffCurve.enabled)
            {
                source.SetCustomCurve(AudioSourceCurveType.CustomRolloff,audioSFX.VolumeFallOffCurve.animationCurve);
            }
            else
            {
                source.rolloffMode = AudioRolloffMode.Logarithmic;
            }
            if (audioSFX.AudioSpreadCurve.enabled)
            {
                source.SetCustomCurve(AudioSourceCurveType.Spread, audioSFX.AudioSpreadCurve.animationCurve);
            }
            else
            {
                source.spread = 0.0f;  
            }



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
                priority = settingPriority.Value;
                source.priority = priority;

                if (settingPriority.IsCompleted)
                    settingPriority = null;
            }

            if (settingLoop != null)
            {
                settingLoop.Update();
                isLooping = settingLoop.Value;
                source.loop = isLooping;

                if (settingLoop.IsCompleted)
                    settingLoop = null;
            }

            if (settingPitch != null)
            {
                settingPitch.Update();
                pitch = settingPitch.Value;
                source.pitch = pitch;

                if (settingPitch.IsCompleted)
                    settingPitch = null;
            }

            if (settingSpatialBlend != null)
            {
                settingSpatialBlend.Update();
                spatialBlend2DTo3D = settingSpatialBlend.Value;
                source.spatialBlend = spatialBlend2DTo3D;

                if (settingSpatialBlend.IsCompleted)
                    settingSpatialBlend = null;
            }

            if (settingVolume != null)
            {
                settingVolume.Update();
                volume = settingVolume.Value;
                source.volume = volume;

                if (settingVolume.IsCompleted)
                    settingVolume = null;
            }

            if (settingPanning != null)
            {
                settingPanning.Update();
                panning = settingPanning.Value;
                source.panStereo = panning;
                if (settingPanning.IsCompleted)
                    settingPanning = null;
            }

            if (settingMute != null)
            {
                settingMute.Update();
                isMuted = settingMute.Value;
                source.mute = isMuted;

                if (settingMute.IsCompleted)
                    settingMute = null;
            }

            if (!source.isPlaying)
                isCompleted = true;
        }   
    }


}
