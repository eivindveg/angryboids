using UnityEngine;

namespace Assets.Scripts
{
    public class OnGuiScript : MonoBehaviour {

        private Transform cameraObject;

        void Start() {
            this.cameraObject = Camera.main.transform;
        }

        // Update is called once per frame
        void Update () {
            this.transform.position = this.cameraObject.transform.position + new Vector3(0f, -0.2f, 4f);
        }
    }
}
