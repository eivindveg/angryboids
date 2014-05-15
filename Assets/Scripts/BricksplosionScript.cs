namespace Assets.Scripts
{
    using UnityEngine;

    public class BricksplosionScript : MonoBehaviour
    {
        #region Fields

        public double LifeTime = 1.0;

        private float currentLifetime;

        #endregion

        // Use this for initialization

        #region Methods

        private void Start()
        {
            this.currentLifetime = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            this.currentLifetime += Time.deltaTime;
            if (this.currentLifetime >= this.LifeTime)
            {
                DestroyImmediate(this.gameObject);
            }
        }

        #endregion
    }
}