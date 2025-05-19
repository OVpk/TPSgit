using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    public AnimationClip[] animations;

    public Animation animationPlayer;

    private bool canContinue;

    public IEnumerator PlayCinematic()
    {
        foreach (AnimationClip anim in animations)
        {
            canContinue = false;
            animationPlayer.Play(anim.name);
            yield return new WaitUntil(() => canContinue);
        }
    }

    public void CanContinueEvent() => canContinue = true;
}
