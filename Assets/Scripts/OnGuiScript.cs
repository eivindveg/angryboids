namespace Assets.Scripts
{
    using UnityEngine;

    public class OnGuiScript : MonoBehaviour
    {
        #region Fields

        private Transform cameraObject;

        #endregion

        #region Methods

        private void Start()
        {
            this.cameraObject = Camera.main.transform;
            this.transform.position = this.cameraObject.transform.position + new Vector3(0f, -0.2f, 4f);
        }

        #endregion
    }
}