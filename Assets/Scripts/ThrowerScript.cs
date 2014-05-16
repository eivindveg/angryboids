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

        private LineRenderer lineRenderer;

        #endregion

        #region Methods

        private void LoadBird(int index)
        {
            if (index >= availableBirds.Length)
            {
                return;
            }
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
            //Debug.Log(angle);
            if (angle <= 80) { 
                this.loadedBird.transform.rotation = newRotation;
            }
            
            //Debug.Log("Rotating to " + this.loadedBird.transform.rotation.y);
        }

        private void Start()
        {
            this.availableBirds = new GameObject[this.Birds.Length];
            lineRenderer = this.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, this.transform.position + new Vector3(0.35f, 0.25f, 0.02f));
            lineRenderer.SetPosition(2, this.transform.position + new Vector3(-0.1f, 0.3f, 0));

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

            // TODO Load first bird
            var firstBirdScript = this.availableBirds[0].GetComponent<BirdBehaviour>();
            if (firstBirdScript.SetState(BirdBehaviour.BirdState.Loading))
            {
                //Debug.Log("loading a bird");

                // TODO Replace with firing movement instead of teleport
                this.loadedBirdIndex = 0;
                this.LoadBird(this.loadedBirdIndex);
            }

            var camScript = FindObjectOfType<CameraController>();
            camScript.LocateAndAssignBirds();
            var gm = FindObjectOfType<GameMaster>();
            gm.LocateAndAssignBirds();
        }

        private void Update()
        {

            Vector2 point2;
            if (this.loadedBird == null)
            {
                this.loadedBirdIndex++;
                this.LoadBird(this.loadedBirdIndex);

                return;
            }

            var birdScript = this.loadedBird.GetComponent<BirdBehaviour>();

            // Draw Slingshot
            if (birdScript.GetState() == BirdBehaviour.BirdState.Primed)
            {
                point2 = loadedBird.transform.position;
            }
            else
            {
                point2 = this.transform.position;
            }

            lineRenderer.SetPosition(1, point2);

            //Debug.Log(birdScript.GetState());
            switch (birdScript.GetState())
            {
                case BirdBehaviour.BirdState.Loading:
                    this.loadedBird.transform.position = Vector3.Lerp(
                        this.loadedBird.transform.position,
                        this.transform.position,
                        Time.deltaTime);
                    if (Vector3.Distance(this.loadedBird.transform.position, this.transform.position) < 0.1f)
                    {
                        birdScript.SetState(BirdBehaviour.BirdState.Ready);
                    }
                    break;
                case BirdBehaviour.BirdState.Ready:
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
                            //Debug.Log("Potency: " + this.shotPotency);
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
                    //Debug.Log(this.loadedBird.rigidbody2D.velocity.magnitude);
                    break;
            }

            GameObject slingShotSeat = GameObject.FindGameObjectWithTag("slingshotseat");
            point2.x += -0.2f;
            point2.y += -0.15f;
            slingShotSeat.transform.position = point2;
            slingShotSeat.transform.rotation = loadedBird.transform.rotation;
        }

        private void Translate()
        {
            Vector3 relativeDirection = -loadedBird.transform.right;
            this.loadedBird.transform.position = this.transform.position + 1.2f*shotPotency*relativeDirection;
        }

        #endregion
    }
}