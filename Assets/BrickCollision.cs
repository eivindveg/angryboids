using System;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using System.Collections;

public class BrickCollision : MonoBehaviour
{

	// Use this for initialization
    private double _currentHp;
    public double Maxhp;
    public Sprite[] sprite = new Sprite[5];
    private SpriteRenderer _renderer;

	void Start ()
	{
        _renderer = gameObject.AddComponent<SpriteRenderer>();
	    _renderer.sprite = sprite[0];
        _currentHp = Maxhp;	  
	}

    void Update () 
    {
	    
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Calculate damage made by the collision
        double magnitudeMass = (double)(collision.relativeVelocity.magnitude * collision.gameObject.rigidbody2D.mass);
        Debug.Log(magnitudeMass);
        
        if (magnitudeMass > 5)
        {
            _currentHp = _currentHp - magnitudeMass;        
        }

        if (_currentHp <= 0)
        {
            _renderer.sprite = sprite[4];
            Destroy(gameObject);
      
        }
        if (_currentHp < (Maxhp * 0.8) && _currentHp > (Maxhp * 0.6))
        {
            _renderer.sprite = sprite[1];
        }
        else if (_currentHp < (Maxhp*0.6) && _currentHp > (Maxhp*0.4))
        {
            _renderer.sprite = sprite[2];
        }
        if (_currentHp < (Maxhp * 0.4) && _currentHp > (Maxhp * 0.2))
        {
            _renderer.sprite = sprite[3];
        }
            
    }
}
