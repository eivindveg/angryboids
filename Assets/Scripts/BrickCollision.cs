namespace Assets.Scripts
{
    using UnityEngine;

    public delegate void BlockDeathEventHandler(BlockDeathEvent e);

    public class BrickCollision : MonoBehaviour
    {
        // Use this for initialization

        #region Fields

        public double Maxhp;

        public Sprite[] Sprites = new Sprite[5];

        public AudioClip[] Sounds;

        public BrickType Type;

        private double currentHp;

        private SpriteRenderer _renderer;

        #endregion

        #region Public Events

        public event BlockDeathEventHandler Death;

        #endregion

        #region Enums

        public enum BrickType
        {
            Pig,

            Block

        }
        

        #endregion

        #region Methods

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //Calculate damage made by the collision
            double magnitudeMass = collision.relativeVelocity.magnitude * (collision.gameObject.rigidbody2D.mass + this.rigidbody2D.mass);
            //play collision sound
            if (this.Type == BrickType.Block && Time.realtimeSinceStartup > 2f)
            {
                int soundIndex = Random.Range(0, this.Sounds.Length);
                this.audio.PlayOneShot(this.Sounds[soundIndex]);
            }   
            if (magnitudeMass >= 1.5)
            {
                this.currentHp = this.currentHp - magnitudeMass;
            }

            if (this.currentHp <= 0)
            {
                this._renderer.sprite = this.Sprites[4];
                this.OnDeath();
                Destroy(this.gameObject);
            }
            if (this.currentHp < (this.Maxhp * 0.8) && this.currentHp > (this.Maxhp * 0.6))
            {
                this._renderer.sprite = this.Sprites[1];
            }
            else if (this.currentHp < (this.Maxhp * 0.6) && this.currentHp > (this.Maxhp * 0.4))
            {
                this._renderer.sprite = this.Sprites[2];
            }
            if (this.currentHp < (this.Maxhp * 0.4) && this.currentHp > (this.Maxhp * 0.2))
            {
                this._renderer.sprite = this.Sprites[3];
            }
        }

        private void OnDeath()
        {
            GameObject poff = (GameObject)Instantiate(Resources.Load("poff"));
            poff.transform.position = this.transform.position;
            if (this.Death != null)
            {
                this.Death(new BlockDeathEvent(this.Type, this.gameObject));
            }
        }

        private void Start()
        {
            // Register with GameMaster
            FindObjectOfType<GameMaster>().RegisterBrick(this);
            this.gameObject.AddComponent<AudioSource>();
            this.gameObject.audio.volume = 0.3f;
            this._renderer = this.gameObject.GetComponent<SpriteRenderer>();
            this._renderer.sprite = this.Sprites[0];
            this.currentHp = this.Maxhp;
        }

        #endregion
    }
}