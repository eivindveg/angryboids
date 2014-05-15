using Assets;

using UnityEngine;
using System.Collections;

public class SpecialBirdBehaviour : MonoBehaviour
{

    private BirdBehaviour birdScript;

    public int SpecialModifier = 35;

    private bool fired;
    private float flyTimer = 0;

	// Use this for initialization
	void Start ()
	{
	    birdScript = this.gameObject.GetComponent<BirdBehaviour>();
	}

    
	
	// Update is called once per frame
	void Update () {
	    if (birdScript.GetState() == BirdBehaviour.BirdState.Flying && !fired)
	    {
	        flyTimer += Time.deltaTime;
	        if (flyTimer > 0.2 && Input.GetAxis("Fire1") > 0.1f)
	        {
                Debug.Log("SUPERSHOT");
	            this.gameObject.rigidbody2D.velocity = this.transform.right * SpecialModifier;
	            fired = true;
	        }
	    }
	}
}
