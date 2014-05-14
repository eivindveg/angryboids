namespace Assets
{
    using System;

    using UnityEngine;

    public class BirdChangedEvent : EventArgs
    {

        #region Fields

        private readonly GameObject bird;

        private readonly BirdBehaviour.BirdState state;

        #endregion

        #region Constructors and Destructors

        public BirdChangedEvent(GameObject bird, BirdBehaviour.BirdState state)
        {
            this.bird = bird;
            this.state = state;
        }

        #endregion

        #region Public Properties

        public GameObject Bird
        {
            get
            {
                return this.bird;
            }
        }

        public BirdBehaviour.BirdState State
        {
            get
            {
                return this.state;
            }
        }

        #endregion
    }
}