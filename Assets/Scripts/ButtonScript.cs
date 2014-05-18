using UnityEngine;

namespace Assets.Scripts
{
    public class ButtonScript : MonoBehaviour
    {
        #region Fields

        private float NewCameraPos;

		public AudioClip HoverSound;
        #endregion

        //private Collider2D col;

        #region Methods

        private void MoveCamera(int pos)
        {
            Camera.main.transform.position = new Vector3(pos, 0, -8.1f);
        }

		private void OnMouseEnter(){
			Camera.main.transform.audio.PlayOneShot(HoverSound);
		}

        private void OnMouseUpAsButton()
        {
            switch (this.gameObject.name)
            {
                case "backButtonLevelSelect":
                    this.MoveCamera(0);
                    Debug.Log("Backbutton pushed");
                    break;

                case "level1Button":
                    Debug.Log("Load level 1");
                    Application.LoadLevel(1);
                    break;

                case "level2Button":
                    Debug.Log("Load level 2");
                    Application.LoadLevel(2);
                    break;

                case "level3Button":
                    Debug.Log("Load level 3");
                    Application.LoadLevel(3);
                    break;

                case "exitButton":
                    Debug.Log("Quit");
                    Application.Quit();
                    break;

                case "levelSelectButton":
                    Debug.Log("Level Select pushed");
                    this.MoveCamera(20);
                    break;

                case "optionsButton":
                    Debug.Log("Options button pushed");
                    this.MoveCamera(-20);
                    break;

                case "startButton":
                    Debug.Log("Start");
                    Application.LoadLevel(1);
                    break;

                case "backButtonOptions":
                    this.MoveCamera(0);
                    break;

                case "soundButton":
                    break;
            }
        }

        private void Start()
        {
            //col = new Collider2D();
            this.gameObject.AddComponent<BoxCollider2D>();
        }

        #endregion
    }
}