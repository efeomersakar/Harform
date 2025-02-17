using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SoundManager : MonoBehaviour
{
    public AudioClip DeathSound;
    public AudioClip RewardBoxSound;
    public AudioClip LevelCompletedSound;
    public AudioSource audioSource;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
    private void OnEnable()
    {
        EventManager.Instance.onRewardBoxTouched += RewardBoxSoundPlay;
        EventManager.Instance.OnPlayerKilled += PlayerDeathSoundPlay;
        EventManager.Instance.OnLevelCompleted += LevelCompletedSoundPlay;

    }

    //=================================================================================================
    public void OnDisable()
    {

        EventManager.Instance.onRewardBoxTouched -= RewardBoxSoundPlay;
        EventManager.Instance.OnPlayerKilled -= PlayerDeathSoundPlay;
        EventManager.Instance.OnLevelCompleted -= LevelCompletedSoundPlay;

    }


    //==================================================================================
    private void RewardBoxSoundPlay(Vector3 spawnPosition)
    {
        if (RewardBoxSound != null)
        {
            AudioSource.PlayClipAtPoint(RewardBoxSound, spawnPosition);
        }
    }

    //==================================================================================
    private void PlayerDeathSoundPlay()
    {
        if (DeathSound != null)
        {
            audioSource.Stop();
            AudioSource.PlayClipAtPoint(DeathSound, transform.position);
        }
   
    }
    //==================================================================================

    private void LevelCompletedSoundPlay()
    {
        if (LevelCompletedSound != null)
        {
            audioSource.Stop();
            AudioSource.PlayClipAtPoint(LevelCompletedSound, transform.position);
        }
    }
    //==================================================================================

}
