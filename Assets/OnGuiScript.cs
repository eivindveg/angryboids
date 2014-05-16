using UnityEngine;
using System.Collections;

public class OnGuiScript : MonoBehaviour {

    public var cameraObject : Transform;
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(0f, -0.2f, -9f);
	}
}
