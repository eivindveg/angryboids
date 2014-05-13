using UnityEngine;
using System.Collections;

using UnityEngine.SocialPlatforms;

public class BirdBehaviour : MonoBehaviour {

    float loadCounter;

    private float deathTimer;

    public enum BirdState
    {
        Idle, Loading, Ready, Primed, Flying, Landed
    }

    private BirdState state = BirdState.Idle;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (this.state == BirdState.Ready || this.state == BirdState.Primed || this.state == BirdState.Loading)
	    {
	        this.gameObject.rigidbody2D.gravityScale = 0;
	    }
	    else this.gameObject.rigidbody2D.gravityScale = 1;
	    if (this.state == BirdState.Landed && this.transform.rigidbody2D.velocity.magnitude < 2f)
	    {
	        deathTimer += Time.deltaTime;
	    }
	    else
	    {
	        deathTimer = 0;
	    }

	    if (deathTimer >= 5)
	    {
	        Destroy(this.gameObject);
	    }

	}

    public bool SetState(BirdState newState)
    {
        switch (this.state)
        {
            case BirdState.Idle:
                if (newState == BirdState.Loading)
                {
                    this.state = newState;
                    return true;
                }
                return this.state == newState;
            case BirdState.Loading:
                if (newState == BirdState.Ready)
                {
                    this.state = newState;
                    return true;
                }
                return this.state == newState;
            case BirdState.Ready:
                if (newState == BirdState.Primed)
                {
                    this.state = newState;
                    return true;
                }
                return newState == this.state;
            case BirdState.Primed:
                if (newState == BirdState.Flying)
                {
                    this.state = newState;
                    return true;
                }
                return this.state == newState;
            case BirdState.Flying:
                if (newState == BirdState.Landed)
                {
                    this.state = newState;
                    return true;
                }
                return this.state == newState;
            default:
                return false;
        }
    }

    public BirdState GetState()
    {
        return this.state;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log("COLLISION!");
        if (this.state == BirdState.Flying)
        {
            this.SetState(BirdState.Landed);
        }
    }
}
