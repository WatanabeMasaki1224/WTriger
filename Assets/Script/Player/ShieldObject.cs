using UnityEngine;

public class ShieldObject : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitSE;

    public void PlayHitSE()
    {
        _audioSource.PlayOneShot(_hitSE);
    }
}
