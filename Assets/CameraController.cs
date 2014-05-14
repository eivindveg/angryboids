using Assets;

using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Fields

    public float SmoothTime = 0.15f;

    public Transform Target;

    private Vector3 velocity = Vector3.zero;

    #endregion

    #region Public Methods and Operators

    public void OnBirdChange(BirdChangedEvent e)
    {
        if (e.State == BirdBehaviour.BirdState.Primed) { 
            this.Target = e.Bird.transform;
        }
        else if (e.State == BirdBehaviour.BirdState.Landed)
        {
            this.Target = null;
        }
    }

    #endregion

    #region Methods

    private void Start()
    {
        foreach (GameObject bird in GameObject.FindGameObjectsWithTag("bird"))
        {
            var birdScript = bird.GetComponent<BirdBehaviour>();
            birdScript.Changed += this.OnBirdChange;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.Target)
        {
            Vector3 point = this.camera.WorldToViewportPoint(this.Target.position);
            Vector3 delta = this.Target.position - this.camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = this.transform.position + delta;
            this.transform.position = Vector3.SmoothDamp(
                this.transform.position,
                destination,
                ref this.velocity,
                this.SmoothTime);
        }
    }

    #endregion
}