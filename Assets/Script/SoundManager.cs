using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    #region audioclips
    public AudioClip GameMusic = new AudioClip();

    public AudioClip MenuButtonSound = new AudioClip();

    public AudioClip whiteMoveSound1 = new AudioClip();
    public AudioClip whiteMoveSound2 = new AudioClip();
    public AudioClip whiteMoveSound3 = new AudioClip();
    public AudioClip whiteMoveSound4 = new AudioClip();
    public AudioClip whiteMoveSound5 = new AudioClip();
    public AudioClip whiteMoveSound6 = new AudioClip();
    public AudioClip whiteMoveSound7 = new AudioClip();

    public AudioClip blackMoveSound1 = new AudioClip();
    public AudioClip blackMoveSound2 = new AudioClip();
    public AudioClip blackMoveSound3 = new AudioClip();
    public AudioClip blackMoveSound4 = new AudioClip();
    public AudioClip blackMoveSound5 = new AudioClip();
    public AudioClip blackMoveSound6 = new AudioClip();

    public AudioClip EatOrbSound = new AudioClip();
    public AudioClip EatOrbSoundRare = new AudioClip();

    public AudioClip connectSoundNormal = new AudioClip();
    public AudioClip connectSoundSilent = new AudioClip();

    public AudioClip openGateSound = new AudioClip();
    public AudioClip closeGateSound = new AudioClip();

    public AudioClip BlackChosen = new AudioClip();
    public AudioClip WhiteChosen = new AudioClip();

    public AudioClip PortalWhiteSound1 = new AudioClip();
    public AudioClip PortalWhiteSound2 = new AudioClip();

    public AudioClip PortalBlackSound1 = new AudioClip();
    public AudioClip PortalBlackSound2 = new AudioClip();

    public AudioClip LevelFailSound = new AudioClip();

    public AudioClip LevelSuccessSound1 = new AudioClip();
    public AudioClip LevelSuccessSound2 = new AudioClip();

    #endregion

    #region initializers and mutes
    public static SoundManager soundManager;
    private AudioSource[] sources;
    private AudioSource soundFX, gameSound, soundFXBackup, soundFXBackupBackup;
    private float musicVolume = 0.8f;
    public GameObject SoundSwitchGameObject, MusicSwitchGameObject;
    public Sprite SoundSwitchOnSprite, SoundSwitchOffSprite;

    void Awake()
    {
        if (soundManager == null)
        {
            DontDestroyOnLoad(gameObject);
            soundManager = this;
        }
        else if (soundManager != this)
        {
            Destroy(gameObject);
        }
        sources = GetComponents<AudioSource>();
        gameSound = sources[0];
        soundFX = sources[1];
        soundFXBackup = sources[2];
        soundFXBackupBackup = sources[3];
    }
    // Use this for initialization
    void Start()
    {
        //LevelManager.manager.AllSoundsMuted = false;

        playGameMusic();
    }

    void Update()
    {
        ingameSoundActivation();
    }

    public void activateGameMusicButton()
    {
        LevelManager.manager.MusicMuted = !LevelManager.manager.MusicMuted;
        ingameSoundActivation();
        if (LevelManager.manager.MusicMuted && LevelManager.manager.SoundFxMuted)
        {
            LevelManager.manager.AllSoundsMuted = true;
        }
        else if (!LevelManager.manager.MusicMuted && !LevelManager.manager.SoundFxMuted)
        {
            LevelManager.manager.AllSoundsMuted = false;
        }
        updateSoundButtonPosition();
    }
    public void ActivateSoundFxButton()
    {
        LevelManager.manager.SoundFxMuted = !LevelManager.manager.SoundFxMuted;
        ingameSoundActivation();
        if (LevelManager.manager.MusicMuted && LevelManager.manager.SoundFxMuted)
        {
            LevelManager.manager.AllSoundsMuted = true;
        }
        else if (!LevelManager.manager.MusicMuted && !LevelManager.manager.SoundFxMuted)
        {
            LevelManager.manager.AllSoundsMuted = false;
        }
        updateSoundButtonPosition();
    }

    public void AllMutePressed()
    {
        LevelManager.manager.AllSoundsMuted = !LevelManager.manager.AllSoundsMuted;
        ingameSoundActivation();
        updateSoundButtonPosition();
    }
    public void ingameSoundActivation()
    {
        gameSound.mute = LevelManager.manager.MusicMuted;
        soundFX.mute = LevelManager.manager.SoundFxMuted;
        soundFXBackup.mute = LevelManager.manager.SoundFxMuted;
        soundFXBackupBackup.mute = LevelManager.manager.SoundFxMuted;

    }
    public void updateSoundButtonPosition()
    {
        SoundSwitchGameObject.GetComponent<Image>().sprite =
    LevelManager.manager.SoundFxMuted ? SoundSwitchOffSprite : SoundSwitchOnSprite;

        MusicSwitchGameObject.GetComponent<Image>().sprite =
    LevelManager.manager.MusicMuted ? SoundSwitchOffSprite : SoundSwitchOnSprite;

        ingameSoundActivation();
    }

    public void locateSoundButtons()
    {
        SoundSwitchGameObject = GameObject.Find("SoundOn");
        MusicSwitchGameObject = GameObject.Find("musicOn");
    }

    #endregion

    #region Main Game Music
    public void playGameMusic()
    {
        gameSound.clip = GameMusic;
        gameSound.loop = true;
        gameSound.Play();
    }
    #endregion

    #region Sound Activation

    public void playMenuButtonSound()
    {
        playSoundEffect(MenuButtonSound);
    }
    public void PlayBlackMoveSound()
    {
        int randomNumber = Random.Range(1, 6);
        AudioClip MovementSound = blackMoveSound1;
        switch (randomNumber)
        {
            case 2:
                MovementSound = blackMoveSound2;
                break;
            case 3:
                MovementSound = blackMoveSound3;
                break;
            case 4:
                MovementSound = blackMoveSound4;
                break;
            case 5:
                MovementSound = blackMoveSound5;
                break;
            case 6:
                MovementSound = blackMoveSound6;
                break;
        }
        playSoundEffect(MovementSound);
    }
    public void PlayWhiteMoveSound()
    {
        int randomNumber = Random.Range(1, 7);
        AudioClip MovementSound = whiteMoveSound1;
        switch (randomNumber)
        {
            case 2:
                MovementSound = whiteMoveSound2;
                break;
            case 3:
                MovementSound = whiteMoveSound3;
                break;
            case 4:
                MovementSound = whiteMoveSound4;
                break;
            case 5:
                MovementSound = whiteMoveSound5;
                break;
            case 6:
                MovementSound = whiteMoveSound6;
                break;
            case 7:
                MovementSound = whiteMoveSound7;
                break;
        }
        playSoundEffect(MovementSound);
    }
    public void playEatOrbSound()
    {
        int randomNumber = Random.Range(1, 7);
        AudioClip MovementSound = EatOrbSound;
        if (randomNumber == 5)
        {
            MovementSound = EatOrbSoundRare;
        }
        playSoundEffect(MovementSound);
    }
    public void playConnectingCharactersSound()
    {
        int randomNumber = Random.Range(1, 3);
        AudioClip MovementSound = connectSoundNormal;
        if (randomNumber == 2)
        {
            MovementSound = connectSoundSilent;
        }
        playSoundEffect(MovementSound);
    }
    public void playOpenGateSound()
    {
        playSoundEffect(openGateSound);
    }
    public void playCloseGateSound()
    {
        playSoundEffect(closeGateSound);
    }
    public void playBlackChosen()
    {
        playSoundEffect(BlackChosen);
    }
    public void playWhiteChosen()
    {
        playSoundEffect(WhiteChosen);
    }
    public void playWhiteCharacterPortalSound()
    {
        int randomNumber = Random.Range(1, 2);
        AudioClip MovementSound = PortalWhiteSound1;
        if (randomNumber == 2)
        {
            MovementSound = PortalWhiteSound2;
        }
        playSoundEffect(MovementSound);
    }
    public void playBlackCharacterPortalSound()
    {
        int randomNumber = Random.Range(1, 2);
        AudioClip MovementSound = PortalBlackSound1;
        if (randomNumber == 2)
        {
            MovementSound = PortalBlackSound2;
        }
        playSoundEffect(MovementSound);
    }
    public void playLevelFailSound()
    {
        playSoundEffect(LevelFailSound);
    }
    public void playLevelWinSound()
    {
        int randomNumber = Random.Range(1, 2);
        AudioClip MovementSound = LevelSuccessSound1;
        if (randomNumber == 2)
        {
            MovementSound = LevelSuccessSound2;
        }
        playSoundEffect(MovementSound);
    }

    private void playSoundEffect(AudioClip playSound)
    {
        if (!soundFX.isPlaying)
        {
            soundFX.PlayOneShot(playSound, musicVolume);
        }
        else if (!soundFXBackup.isPlaying)
        {
            soundFXBackup.PlayOneShot(playSound, musicVolume);
        }
        else
        {
            soundFXBackupBackup.PlayOneShot(playSound, musicVolume);
        }
    }
    #endregion
}
