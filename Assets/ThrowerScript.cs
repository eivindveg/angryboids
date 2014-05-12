using UnityEngine;
using System.Collections;

public class ThrowerScript : MonoBehaviour {

    private GameObject loadedBird;
    private GameObject[] availableBirds;

    public enum BirdType
    {
        NORMAL, SPECIAL
    }


    public BirdType[] Birds;

	// Use this for initialization
	void Start () {
        availableBirds = new GameObject[Birds.Length];

        // TODO Populate and sort bird collection
        int i = 0;
        foreach (BirdType bird in Birds)
        {
            string birdToLoad;
            if (bird == BirdType.SPECIAL)
            {
                birdToLoad = "SpecialBird";
            } else {
                birdToLoad = "NormalBird";
            }
            availableBirds[i] = (GameObject)Instantiate(Resources.Load(birdToLoad));
            i++;
        }



        foreach (GameObject birds in availableBirds)
        {
            Debug.Log(birds.name);
        }
	    // TODO Load first bird
        
	}
	
	// Update is called once per frame
	void Update () {

        // Get loaded bird state;
            // If bird loaded
	            // Check input
                    // Set state primed.
            // If bird primed
                // Check for fired input;
                    // Fire bird
            // If no bird
                // Check if bird is loading
                    // If not loading, request bird to load.
                        // If no bird to load, out of birds.
	}
}
