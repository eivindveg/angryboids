using UnityEngine;

namespace Assets.Scripts
{
    public class SlingshotSound : MonoBehaviour {

        public AudioClip SlingSound;
        public AudioClip FlyingSound;
	

        public void PlaySling(){
            this.audio.PlayOneShot(this.SlingSound);
        }

        public void PlayRelease(){
            this.audio.PlayOneShot (this.FlyingSound);
        }
    }
}
	