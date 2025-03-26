using UnityEngine;

public enum Sound
{
    PlayerBulletShot,
    EnemyDie,
    GetItem,
    Victory,
    GameOver
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance = null;

    AudioSource audioSource;

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

    public void PlaySFX(Sound sound)
    {
        audioSource.PlayOneShot(sfxClipArray[(int)sound]);
    }
}
