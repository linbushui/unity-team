using UnityEngine;

public class PlaySoundEvent : MonoBehaviour
{
    public AudioSource audioSource;

    // 该方法将会在动画事件中被调用
    public void PlayAnimationSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}