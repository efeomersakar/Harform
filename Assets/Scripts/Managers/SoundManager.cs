using UnityEngine;
using System.Collections;
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
    }

    private void OnEnable()
    {
        EventManager.Instance.onRewardBoxTouched += RewardBoxSoundPlay;
        EventManager.Instance.OnPlayerKilled += PlayerDeathSoundPlay;
        EventManager.Instance.OnLevelCompleted += LevelCompletedSoundPlay;
        EventManager.Instance.OnGameContinue += MainThemeSoundPlay;

    }

    //=================================================================================================
    public void OnDisable()
    {

        EventManager.Instance.onRewardBoxTouched -= RewardBoxSoundPlay;
        EventManager.Instance.OnPlayerKilled -= PlayerDeathSoundPlay;
        EventManager.Instance.OnLevelCompleted -= LevelCompletedSoundPlay;
        EventManager.Instance.OnGameContinue -= MainThemeSoundPlay;

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
    private void MainThemeSoundPlay()
    {
        audioSource.Play();
    }
    //==================================================================================

    private void PlayerDeathSoundPlay()
    {
        if (DeathSound != null)
        {
            audioSource.Pause();
            AudioSource.PlayClipAtPoint(DeathSound, transform.position);


        }


    }
    //==================================================================================

    private void LevelCompletedSoundPlay()
    {
        if (LevelCompletedSound != null)
        {
            audioSource.Pause();
            AudioSource.PlayClipAtPoint(LevelCompletedSound, transform.position);
            ;
        }

    }
    //==================================================================================

}

