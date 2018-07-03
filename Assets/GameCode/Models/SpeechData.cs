using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class SpeechData
{
    public List<string> Messages;
    public Sprite SpeakerPortrait;
    public float2 TextPosition;
}