namespace Assets.Scripts
{
    using UnityEngine;

    public class ButtonClick : MonoBehaviour
    {
        #region Fields

        private int nextScene;

        #endregion

        #region Methods

        private void OnMouseUpAsButton()
        {
            // this object was clicked - do something
            Debug.Log("Button pushed");

            if (this.gameObject.name == "button_home")
            {
                Application.LoadLevel(0);
            }
            else if (this.gameObject.name == "button_replay")
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            else if (this.gameObject.name == "button_next")
            {
                Application.LoadLevel(this.nextScene);
            }
        }

        // Use this for initialization
        private void Start()
        {
            this.nextScene = Application.loadedLevel + 1;

            if (this.nextScene >= Application.levelCount)
            {
                this.nextScene = 0;
            }
        }

        #endregion
    }
}