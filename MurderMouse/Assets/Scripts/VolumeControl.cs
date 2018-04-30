using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour {

    public AudioMixer audioMixer;

	public void SetMasterVolume(float masterVolume)
    {
        audioMixer.SetFloat("masterVol", masterVolume);
    }
}
