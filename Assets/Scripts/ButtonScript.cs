using System;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private float NewCameraPos;
    private Collider2D col;

    private void Start()
    {
        col = new Collider2D();
        col = gameObject.AddComponent<BoxCollider2D>();
    }

    
    private void OnMouseUpAsButton()
    {
        switch (gameObject.name)
        {
            case "backButtonLevelSelect":
                MoveCamera(0);
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
                MoveCamera(20);
                break;

            case "optionsButton":
                Debug.Log("Options button pushed");
                MoveCamera(-20);
                break;

            case "startButton":
                Debug.Log("Start");
                Application.LoadLevel(1);
                break;

            case "backButtonOptions":
                MoveCamera(0);
                break;


            case "soundButton":
                break;
        }
    }

    private void MoveCamera(int pos)
    {
        Camera.main.transform.position = new Vector3(pos, 0, -8.1f);
    }
}