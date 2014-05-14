namespace Assets
{
    using UnityEngine;

    public delegate void BirdStateEventHandler(BirdChangedEvent e);

    public class BirdBehaviour : MonoBehaviour
    {
        #region Fields

        private float deathTimer;

        private float loadCounter;

        private BirdState state = BirdState.Idle;

        #endregion

        #region Public Events

        public event BirdStateEventHandler Changed;

        #endregion

        #region Enums

        public enum BirdState
        {
            Idle,

            Loading,

            Ready,

            Primed,

            Flying,

            Landed
        }

        public enum BirdType
        {
            Normal,

            Special
        }

        #endregion

        // Use this for initialization

        #region Public Methods and Operators

        public BirdState GetState()
        {
            return this.state;
        }

        public bool SetState(BirdState newState)
        {
            switch (this.state)
            {
                case BirdState.Idle:
                    if (newState == BirdState.Loading)
                    {
                        this.state = newState;
                        this.OnChanged();
                        return true;
                    }
                    return this.state == newState;
                case BirdState.Loading:
                    if (newState == BirdState.Ready)
                    {
                        this.state = newState;
                        this.OnChanged();
                        return true;
                    }
                    return this.state == newState;
                case BirdState.Ready:
                    if (newState == BirdState.Primed)
                    {
                        this.state = newState;
                        this.OnChanged();
                        return true;
                    }
                    return newState == this.state;
                case BirdState.Primed:
                    if (newState == BirdState.Flying)
                    {
                        this.state = newState;
                        this.OnChanged();
                        return true;
                    }
                    return this.state == newState;
                case BirdState.Flying:
                    if (newState == BirdState.Landed)
                    {
                        this.state = newState;
                        this.OnChanged();
                        return true;
                    }
                    return this.state == newState;
                default:
                    return false;
            }
        }

        #endregion

        #region Methods

        protected virtual void OnChanged()
        {
            Debug.Log("Attempting to fire change event!");
            if (this.Changed != null)
            {
                Debug.Log("Fired event!");
                this.Changed(new BirdChangedEvent(this.gameObject, this.state));
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            Debug.Log("COLLISION!");
            if (this.state == BirdState.Flying)
            {
                this.SetState(BirdState.Landed);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.state == BirdState.Ready || this.state == BirdState.Primed || this.state == BirdState.Loading)
            {
                this.gameObject.rigidbody2D.gravityScale = 0;
            }
            else
            {
                this.gameObject.rigidbody2D.gravityScale = 1;
            }
            if (this.state == BirdState.Landed && this.transform.rigidbody2D.velocity.magnitude < 2f)
            {
                this.deathTimer += Time.deltaTime;
            }
            else
            {
                this.deathTimer = 0;
            }

            if (this.deathTimer >= 3)
            {
                Destroy(this.gameObject);
            }
        }

        #endregion
    }
}