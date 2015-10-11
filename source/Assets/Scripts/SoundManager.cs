using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// Static instance of the class.
    /// </summary>
    public static SoundManager Instance = null;

    /// <summary>
    /// Drag a reference to the audio source which will play the sound effects.
    /// </summary>
    public AudioSource efxSource;

    /// <summary>
    /// Drag a reference to the audio source which will play the main menu music.
    /// </summary>
    public AudioSource mainMenuMusicSource;

    /// <summary>
    /// Drag a reference to the audio source which will play the game music.
    /// </summary>
    public AudioSource musicSource;

    /// <summary>
    /// Collection of tracks to be played as game music.
    /// </summary>
    public AudioClip[] playlist;

    /// <summary>
    /// The lowest a sound effect will be randomly pitched.
    /// </summary>
    public float lowPitchRange = 0.95f;

    /// <summary>
    /// The highest a sound effect will be randomly pitched.
    /// </summary>
    public float highPitchRange = 1.05f;

    /// <summary>
    /// Flag to keep track if game music should be playing or not.
    /// </summary>
    private bool _inGameMusicPlaying = false;

    /// <summary>
    /// Index of track being played.
    /// </summary>
    private int _currentTrack = 0;

    /// <summary>
    /// Index of next track to be played.
    /// </summary>
    private int _nextTrack = 0;

    /// <summary>
    /// Used to change the current track because track playing time never gets the full time.
    /// </summary>
    private float _paddingBetweenTracks = 0.01f;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    protected void Update()
    {
        // Check for track changes.
        if (musicSource.clip == null || (musicSource.time + _paddingBetweenTracks) >= playlist[_currentTrack].length)
        {
            ChangeTrack();
        }

        // Play/Pause.
        if (_inGameMusicPlaying && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
        else if (!_inGameMusicPlaying && musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    /// <summary>
    /// Used to play single sound clips.
    /// </summary>
    /// <param name="clip">Clip to play.</param>
    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    /// <summary>
    /// Chooses randomly between various audio clips and slightly changes their pitch.
    /// </summary>
    /// <param name="clips">Clips to play.</param>
    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    /// <summary>
    /// Used to play the main menu track.
    /// </summary>
    public void PlayMainMenuTrack()
    {
        if (!mainMenuMusicSource.isPlaying)
        {
            mainMenuMusicSource.Play();
        }
    }

    /// <summary>
    /// Pauses the main menu track.
    /// </summary>
    public void PauseMainMenuTrack()
    {
        if (mainMenuMusicSource.isPlaying)
        {
            mainMenuMusicSource.Pause();
        }
    }

    /// <summary>
    /// Used to play the game music sequentially.
    /// </summary>
    public void PlayMusic()
    {
        _inGameMusicPlaying = true;
    }

    /// <summary>
    /// Pauses the game music.
    /// </summary>
    public void PauseMusic()
    {
        _inGameMusicPlaying = false;
    }

    /// <summary>
    /// Changes the music track for the next one in the playlist.
    /// </summary>
    private void ChangeTrack()
    {
        musicSource.clip = playlist[_nextTrack];

        _currentTrack = _nextTrack;
        _nextTrack++;

        // Reset playlist?
        if (_nextTrack == playlist.Length)
        {
            _nextTrack = 0;
        }
    }
}
