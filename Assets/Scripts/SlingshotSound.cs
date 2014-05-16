using UnityEngine;
using System.Collections;

public class SlingshotSound : MonoBehaviour {

	public AudioClip slingSound;
	public AudioClip flyingSound;
	

	public void playSling(){
		gameObject.GetComponent<SoundSys>().PlaySoundOnce (slingSound);
	}

	public void playSvoosj(){
		gameObject.GetComponent<SoundSys>().PlaySoundOnce (flyingSound);
	}
}
	