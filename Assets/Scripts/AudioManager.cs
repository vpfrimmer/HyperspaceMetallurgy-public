using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AudioElement
{
    public string Name;
    public AudioClip Clip;
}

public enum SoundType
{
    RobotSpawn,
    RobotBeamed,
    RobotCollect,
    RobotLaser,
    RobotCombat,
    RobotDeath,
    SwitchMode
}

public class AudioManager : Singleton<AudioManager>
{
    public List<AudioElement> AudioElements = new List<AudioElement>();
    public AudioSource Source;

    public AudioClip GetClipFor(SoundType type)
    {
        switch (type)
        {
            case SoundType.RobotSpawn:
                return GetElementByName("RobotSpawn").Clip;
            case SoundType.RobotBeamed:
                return GetElementByName("RobotBeamed").Clip;
            case SoundType.RobotCollect:
                return GetElementByName("RobotCollect").Clip;
            case SoundType.RobotLaser:
                return GetElementByName("RobotLaser").Clip;
            case SoundType.RobotCombat:
                return GetElementByName("RobotCombat").Clip;
            case SoundType.RobotDeath:
                return GetElementByName("RobotDeath").Clip;
            case SoundType.SwitchMode:
                return GetElementByName("SwitchMode").Clip;
        }

        return null;
    }

    private AudioElement GetElementByName(string name)
    {
        foreach (var e in AudioElements.OrderBy(a => Guid.NewGuid()).ToList())
        {
            if (e.Name.Contains(name))
            {
                return e;
            }
        }

        return null;
    }

    public void PlayAudio(SoundType type)
    {
        AudioClip clip = GetClipFor(type);
        if (type == SoundType.RobotDeath)
        {
            Source.PlayOneShot(clip, 0.7f);
        }
        else
        {
            Source.PlayOneShot(clip);
        }
    }
}
