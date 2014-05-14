using UnityEngine;
using System.Collections;

public class buttonClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnMouseUpAsButton(){
		// this object was clicked - do something
		Debug.Log ("Button pushed");

		if (gameObject.name == "button_home") {
			Application.LoadLevel("home");

		} else if (gameObject.name == "button_replay") {
			Application.LoadLevel(1);

		} else if (gameObject.name == "button_next") {
			Application.LoadLevel(2);

		}

	}  
}

