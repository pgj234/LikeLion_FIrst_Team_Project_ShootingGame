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

    AudioSource audio;

    [SerializeField] AudioClip[] sfxClipArray;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audio = GetComponent<AudioSource>();
    }

    public void PlaySFX(Sound sound)
    {
        audio.PlayOneShot(sfxClipArray[(int)sound]);
    }
}
