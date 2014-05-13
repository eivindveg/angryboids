using System.Linq;

using UnityEngine;

using System.Collections;

public class ThrowerScript : MonoBehaviour
{
    private GameObject[] availableBirds;

    private int loadedBirdIndex;

    private GameObject loadedBird;

    private float shotPotency;

    private int loadTime;

    public enum BirdType
    {
        Normal,

        Special
    }

    public BirdType[] Birds;

    // Use this for initialization
    private void Start()
    {
        loadTime = 0;
        availableBirds = new GameObject[Birds.Length];

        // TODO Populate and sort bird collection
        int i = 0;

        foreach (string birdToLoad in this.Birds.Select(bird => bird == BirdType.Special ? "SpecialBird" : "NormalBird"))
        {
            this.availableBirds[i] = (GameObject)Instantiate(Resources.Load(birdToLoad));
            
            Vector3 birdPosition = new Vector3(this.transform.position.x-2-(2*i), this.transform.position.y, this.transform.position.z);
            this.availableBirds[i].transform.position = birdPosition;
            i++;
        }

        foreach (GameObject birds in availableBirds)
        {
            Debug.Log(birds.name);
        }

        // TODO Load first bird
        BirdBehaviour firstBirdScript = availableBirds[0].GetComponent<BirdBehaviour>();
        if (firstBirdScript.SetState(BirdBehaviour.BirdState.Loading))
        {
            Debug.Log("loading a bird");

            // TODO Replace with firing movement instead of teleport
            loadedBirdIndex = 0;
            loadedBird = availableBirds[loadedBirdIndex];
        }
    }

    // Update is called once per frame
    private void Update()
    {
        BirdBehaviour birdScript = loadedBird.GetComponent<BirdBehaviour>();
        Debug.Log(birdScript.GetState());
        switch (birdScript.GetState())
        {
            case BirdBehaviour.BirdState.Loading:
                loadedBird.transform.position = Vector3.Lerp(loadedBird.transform.position, this.transform.position, Time.deltaTime);
                if (Vector3.Distance(loadedBird.transform.position,this.transform.position) < 1.0f)
                {
                    birdScript.SetState(BirdBehaviour.BirdState.Ready);
                }
                else Debug.Log(Vector3.Distance(loadedBird.transform.position, this.transform.position));
                break;
            case BirdBehaviour.BirdState.Ready:
                if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
                {
                    loadedBird.transform.position = transform.position;
                    this.Rotate(Input.GetAxis("Vertical"));
                    Debug.Log("Vertical axis is: " + Input.GetAxis("Vertical"));
                }
                if (Input.GetAxis("Fire1") > 0.1f)
                {
                    birdScript.SetState(BirdBehaviour.BirdState.Primed);
                }
                break;
                // If bird primed
            case BirdBehaviour.BirdState.Primed:
                // Check for fired input;
                if (Input.GetAxis("Fire1") > 0.1f)
                {
                    if (shotPotency <= 1.0f)
                    {
                        shotPotency += 0.1f * Time.deltaTime;
                        Debug.Log("Potency: " + shotPotency);
                    }
                }
                else
                {
                    if (birdScript.SetState(BirdBehaviour.BirdState.Flying))
                    {
                        Vector2 newVelocity = new Vector2(shotPotency * 100, 0);
                        loadedBird.rigidbody2D.velocity = newVelocity;
                    }
                }
                break;
            case BirdBehaviour.BirdState.Flying:
                Debug.Log(loadedBird.rigidbody2D.velocity.magnitude);
                break;
        }
        if (loadedBird == null)
        {
            if (availableBirds[loadedBirdIndex] == null)
            {
                // GAME OVER
            }
            else
            {
                this.LoadBird(loadedBirdIndex++);
            }
        }
    }

    private void LoadBird(int index)
    {

    }

    private void Rotate(float rotation)
    {
        Vector3 oldRotation = loadedBird.transform.rotation.eulerAngles;
        Quaternion newRotation = Quaternion.Euler(new Vector3(0, oldRotation.y + rotation, 0));
        loadedBird.transform.rotation = newRotation;
        Debug.Log("Rotating to " + loadedBird.transform.rotation.y);
    }
}