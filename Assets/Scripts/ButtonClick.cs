namespace Assets.Scripts 
{

	using UnityEngine;
	using System.Collections;

	public class buttonClick : MonoBehaviour {

		public TextMesh winOrLoose;
		public bool endState;

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
				// Application.LoadLevel("home");
				winOrLoose.text="Home"; 

			} else if (gameObject.name == "button_replay") {
				// Application.LoadLevel(1);
				winOrLoose.text="Replay";

			} else if (gameObject.name == "button_next") {
				// Application.LoadLevel(2);
				winOrLoose.text="Next";

			}
		}  
	}
}
