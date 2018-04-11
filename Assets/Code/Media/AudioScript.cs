#region Usings

using UnityEngine;

#endregion

[RequireComponent(typeof(AudioSource))]
public class AudioScript : MonoBehaviour {

    #region Public Properties

    public AudioObject audioObject;

    #endregion

    #region Private Properties

    private AudioClip audioClip;
    private AudioSource audioSource;

    #endregion

    #region Methods

    public void SetAudioSource(AudioObject audioObj)
    {
        this.audioObject = audioObj;

        audioClip = (AudioClip)Resources.Load(audioObject.GetAudioFileFullPath(), typeof(AudioClip));
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
    }


    #endregion
}
