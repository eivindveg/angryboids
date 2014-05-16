namespace Assets.Scripts 
{
    using UnityEditor;

    using UnityEngine;
	using System.Collections;

	public class ButtonClick : MonoBehaviour {

//		private TextMesh winOrLoose;
//		public bool EndState;
//
//	    public ButtonClick(TextMesh winOrLoose)
//	    {
//	        this.winOrLoose = winOrLoose;
//	    }

	    private int nextScene;

	    // Use this for initialization
		void Start ()
		{
		    nextScene = Application.loadedLevel+1;

		    if (nextScene >= Application.levelCount)
		    {
		        nextScene = 0;
		    }


		}
		
		// Update is called once per frame
		void Update () {


		}

		void OnMouseUpAsButton(){
			// this object was clicked - do something
			Debug.Log ("Button pushed");

			if (gameObject.name == "button_home") {
				Application.LoadLevel(0);

			} else if (gameObject.name == "button_replay") {
				Application.LoadLevel(Application.loadedLevel);

			} else if (gameObject.name == "button_next")
			{
			    Application.LoadLevel(nextScene);

			}
		}  
	}
}
