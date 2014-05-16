using UnityEngine;
using System.Collections;



public class SoundSys : MonoBehaviour {
	
	public float volume = 10;
	public bool mute = false;
	

	public void PlaySoundOnce(AudioClip a){
		//audio.clip = a;
		audio.volume = volume;
		if (!mute) audio.PlayOneShot (a);
	}

	void SoundDisabled (bool a) {
		mute = a;
	}

	
}


