using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VoiceActing
{
    public AudioClip[] voice;
    public string[] lines;
    int i = 0;
    bool newLineStart = true;
    float lineEndTime;
    public bool finished = false;

    public void DoIt(Text text, AudioSource source)
    {
        if(i == voice.Length)
        {
            finished = true;
            text.text = " ";
        }
        if (finished == false)
        {
            if (newLineStart == true)
            {
                source.clip = voice[i];
                lineEndTime = Time.time + voice[i].length + 0.2f;
                newLineStart = false;
                source.Play();
                text.text = lines[i];
            }
            if (Time.time >= lineEndTime)
            {
                i++;
                newLineStart = true;
            }
        }
    }
}
