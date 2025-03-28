using UnityEngine;

public enum BGM
{
    Main,
    Play
}

public enum Sound
{
    PlayerBulletImpact,
    EnemyDie,
    GetItem,
    Victory,
    GameOver,
    ButtonClick
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance = null;

    AudioSource audioSource;

    [SerializeField] AudioClip[] bgmClipArray;

    [Space(20)]
    [SerializeField] AudioClip[] sfxClipArray;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBGM(BGM bgm)
    {
        audioSource.clip = bgmClipArray[(int)bgm];
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void PlaySFX(Sound sound)
    {
        audioSource.PlayOneShot(sfxClipArray[(int)sound]);
    }
}
