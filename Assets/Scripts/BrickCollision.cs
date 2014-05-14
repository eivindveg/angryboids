namespace Assets.Scripts
{
    using UnityEngine;

    public delegate void BlockDeathEventHandler(BlockDeathEvent e);

    public class BrickCollision : MonoBehaviour
    {
        // Use this for initialization

        #region Fields

        public double Maxhp;

        public Sprite[] sprite = new Sprite[5];

        public BrickType type;

        private double _currentHp;

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
            double magnitudeMass = collision.relativeVelocity.magnitude * collision.gameObject.rigidbody2D.mass;
            Debug.Log(magnitudeMass);

            if (magnitudeMass >= 2)
            {
                this._currentHp = this._currentHp - magnitudeMass;
            }

            if (this._currentHp <= 0)
            {
                this._renderer.sprite = this.sprite[4];
                this.OnDeath();
                Destroy(this.gameObject);
            }
            if (this._currentHp < (this.Maxhp * 0.8) && this._currentHp > (this.Maxhp * 0.6))
            {
                this._renderer.sprite = this.sprite[1];
            }
            else if (this._currentHp < (this.Maxhp * 0.6) && this._currentHp > (this.Maxhp * 0.4))
            {
                this._renderer.sprite = this.sprite[2];
            }
            if (this._currentHp < (this.Maxhp * 0.4) && this._currentHp > (this.Maxhp * 0.2))
            {
                this._renderer.sprite = this.sprite[3];
            }
        }

        private void OnDeath()
        {
            if (this.Death != null)
            {
                this.Death(new BlockDeathEvent(this.type, this.gameObject));
            }
        }

        private void Start()
        {
            this._renderer = this.gameObject.GetComponent<SpriteRenderer>();
            this._renderer.sprite = this.sprite[0];
            this._currentHp = this.Maxhp;
        }

        #endregion
    }
}