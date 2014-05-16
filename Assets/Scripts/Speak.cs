namespace Assets.Scripts
{
    using UnityEngine;

    public class Speak : MonoBehaviour
    {
        #region Fields

        public float MaxPause = 8f; //Maksimum tid i sekunder før neste lyd spilles

        public float MinPause = 1f; //Minimum tid i sekunder før neste lyd kan spilles

        public AudioClip[] Voices; //Liste med lydklipp

        private float randomTime;

        private float timeCounter;

        #endregion

        #region Methods

        private void Start()
        {
            this.randomTime = Random.Range(this.MinPause, this.MaxPause);
        }

        private void Update()
        {
            if (this.timeCounter > this.randomTime)
            {
                this.UseVoice();
            }
            this.timeCounter += Time.deltaTime;
        }

        //Spiller av en tilfeldig lyd og venter et tilfeldig antall sekunder
        private void UseVoice()
        {
            if (audio.isPlaying)
            {
                return;
            }
            this.randomTime = Random.Range(this.MinPause, this.MaxPause);
            this.timeCounter = 0f;
            int soundIndex = Random.Range(0, this.Voices.Length);
            this.audio.PlayOneShot(this.Voices[soundIndex]);
        }

        #endregion
    }
}