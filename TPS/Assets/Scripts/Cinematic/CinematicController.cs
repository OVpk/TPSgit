using System.Collections;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    [SerializeField] private AnimationClip[] animations;

    [SerializeField] private Animation animationPlayer;

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
