using UnityEngine;
using System.Collections;

public class Speak : MonoBehaviour {
	
	public AudioClip voice_1; //Lydklippene det skal velges fra
	public AudioClip voice_2;
	public AudioClip voice_3;
	public AudioClip voice_4;
	
	public float volume = 10;

	public float minPause = 1; //Minimum tid i sekunder før neste lyd kan spilles
	public float maxPause = 8; //Maksimum tid i sekunder før neste lyd spilles
	

	private double timeCounter=0.0;
	private double randomTime = 0.0;

	AudioClip[] voices; //Liste med lydklipp


	void Start () {
		voices = new AudioClip[] {voice_1,voice_2,voice_3,voice_4};
	}
	

	void Update () {
		useVoice ();
		timeCounter += Time.deltaTime;
	}

	//Spiller av en tilfeldig lyd og venter et tilfeldig antall sekunder
	void useVoice(){
		if (timeCounter > randomTime) {
			SoundSys soundSys = GetComponent<SoundSys>();
			randomTime = Random.Range (minPause, maxPause);
			timeCounter = 0.0;
			soundSys.PlaySoundOnce (voices [Random.Range (0, voices.Length)]);
		}

	}


}
