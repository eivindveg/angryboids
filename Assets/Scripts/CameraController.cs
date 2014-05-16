using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        #region Fields

        public float SmoothTime = 0.05f;

        private Transform target;

        private Vector3 velocity = Vector3.zero;

        #endregion

        #region Public Methods and Operators

        public void LocateAndAssignBirds()
        {
            foreach (GameObject bird in GameObject.FindGameObjectsWithTag("bird"))
            {
                Debug.Log("Attaching to bird");
                var birdScript = bird.GetComponent<BirdBehaviour>();
                birdScript.Changed += this.OnBirdChange;
            }
        }

        public void OnBirdChange(BirdChangedEvent e)
        {
            Debug.Log("CameraController received a changed event");
            if (e.State != BirdBehaviour.BirdState.Landed)
            {
                this.target = e.Bird.transform;
            }
            else if (e.State == BirdBehaviour.BirdState.Landed)
            {
                this.target = null;
            }
        }

        #endregion

        // Update is called once per frame

        #region Methods

        private void FixedUpdate()
        {
            if (GameMaster.LockControls)
            {
                return;
            }

            Vector3 destination = this.transform.position;

            if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis("Horizontal") < -0.1f)
            {
                this.target = null;
                destination.x += Input.GetAxis("Horizontal");
            }

            if (this.target)
            {
                Transform transform1 = this.target;
                if (transform1 != null)
                {
                    Vector3 point = this.camera.WorldToViewportPoint(transform1.position);
                    Vector3 delta = transform1.position - this.camera.ViewportToWorldPoint(new Vector3(0.5f, point.y, point.z));
                    destination += delta;
                }
            }
            this.transform.position = Vector3.SmoothDamp(
                this.transform.position,
                destination,
                ref this.velocity,
                this.SmoothTime);
        }

        #endregion
    }
}