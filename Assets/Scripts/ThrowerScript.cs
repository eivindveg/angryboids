namespace Assets.Scripts
{
    using System;
    using System.Linq;

    using UnityEngine;

    public class ThrowerScript : MonoBehaviour
    {
        #region Constants

        private const float LoadTime = 2f;

        #endregion

        #region Fields

        public BirdBehaviour.BirdType[] Birds;

        public int ShotStrengthModifier = 23;

        private GameObject[] availableBirds;

        private GameObject loadedBird;

        private int loadedBirdIndex;

        private float shotPotency;

        #endregion

        #region Methods

        private void LoadBird(int index)
        {
            var birdScript = this.availableBirds[index].GetComponent<BirdBehaviour>();
            if (birdScript.SetState(BirdBehaviour.BirdState.Loading))
            {
                this.loadedBird = this.availableBirds[index];
            }
        }

        private void Rotate(float rotation)
        {
            Vector3 oldRotation = this.loadedBird.transform.rotation.eulerAngles;
            Quaternion newRotation = Quaternion.Euler(new Vector3(0, 0, oldRotation.z + rotation));

            float angle = Quaternion.Angle(Quaternion.Euler(new Vector3(0,0,0)), newRotation);
            Debug.Log(angle);
            if (angle <= 80) { 
                this.loadedBird.transform.rotation = newRotation;
            }
            
            Debug.Log("Rotating to " + this.loadedBird.transform.rotation.y);
        }

        private void Start()
        {
            this.availableBirds = new GameObject[this.Birds.Length];

            // TODO Populate and sort bird collection
            int i = 0;

            foreach (
                string birdToLoad in this.Birds.Select(bird => bird == BirdBehaviour.BirdType.Special ? "SpecialBird" : "NormalBird"))
            {
                this.availableBirds[i] = (GameObject)Instantiate(Resources.Load(birdToLoad));

                var birdPosition = new Vector3(
                    this.transform.position.x - 1 - (1 * i),
                    this.transform.position.y,
                    this.transform.position.z);
                this.availableBirds[i].transform.position = birdPosition;
                i++;
            }

            foreach (GameObject birds in this.availableBirds)
            {
                Debug.Log(birds.name);
            }

            // TODO Load first bird
            var firstBirdScript = this.availableBirds[0].GetComponent<BirdBehaviour>();
            if (firstBirdScript.SetState(BirdBehaviour.BirdState.Loading))
            {
                Debug.Log("loading a bird");

                // TODO Replace with firing movement instead of teleport
                this.loadedBirdIndex = 0;
                this.LoadBird(this.loadedBirdIndex);
            }

            var camScript = FindObjectOfType<CameraController>();
            camScript.LocateAndAssignBirds();
        }

        @ImplicitlyUsed

        private void Update()
        {
            if (this.loadedBird == null)
            {
                this.loadedBirdIndex++;
                this.LoadBird(this.loadedBirdIndex);
            }

            var birdScript = this.loadedBird.GetComponent<BirdBehaviour>();
            Debug.Log(birdScript.GetState());
            switch (birdScript.GetState())
            {
                case BirdBehaviour.BirdState.Loading:
                    this.loadedBird.transform.position = Vector3.Lerp(
                        this.loadedBird.transform.position,
                        this.transform.position,
                        Time.deltaTime);
                    if (Vector3.Distance(this.loadedBird.transform.position, this.transform.position) < 0.5f)
                    {
                        birdScript.SetState(BirdBehaviour.BirdState.Ready);
                    }
                    else
                    {
                        Debug.Log(Vector3.Distance(this.loadedBird.transform.position, this.transform.position));
                    }
                    break;
                case BirdBehaviour.BirdState.Ready:
                    if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
                    {
                        this.Rotate(Input.GetAxis("Vertical"));
                    }
                    if (Input.GetAxis("Fire1") > 0.1f)
                    {
                        birdScript.SetState(BirdBehaviour.BirdState.Primed);
                    }
                    break;
                    // If bird primed
                case BirdBehaviour.BirdState.Primed:
                    // Check for fired input;
                    this.Translate();
                    if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
                    {
                        this.Rotate(Input.GetAxis("Vertical"));  
                    }
                    if (Input.GetAxis("Fire1") > 0.1f)
                    {
                        if (this.shotPotency <= 1.0f)
                        {
                            this.shotPotency += Time.deltaTime / LoadTime;
                            Debug.Log("Potency: " + this.shotPotency);
                        }
                    }
                    else
                    {
                        if (birdScript.SetState(BirdBehaviour.BirdState.Flying))
                        {
                            this.loadedBird.rigidbody2D.velocity = this.loadedBird.transform.right * this.shotPotency
                                                                   * this.ShotStrengthModifier;
                            this.shotPotency = 0;
                        }
                    }
                    break;
                case BirdBehaviour.BirdState.Flying:
                    Debug.Log(this.loadedBird.rigidbody2D.velocity.magnitude);
                    break;
            }
        }

        private void Translate()
        {
            Vector3 relativeDirection = -loadedBird.transform.right;
            this.loadedBird.transform.position = this.transform.position + 1.2f*shotPotency*relativeDirection;
        }

        #endregion
    }
}