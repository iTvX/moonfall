using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAniEvent : MonoBehaviour
{
    public void Audio_Footstep1()
    {
        AudioManager.Instance.Play("fs1");
    }
    public void Audio_Footstep2()
    {
        AudioManager.Instance.Play("fs2");
    }
    public void Audio_Footstep3()
    {
        AudioManager.Instance.Play("fs3");
    }
    public void Audio_Jump()
    {
        AudioManager.Instance.Play("jump");

    }
}
