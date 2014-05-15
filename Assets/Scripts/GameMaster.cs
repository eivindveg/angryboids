using UnityEngine;

namespace Assets.Scripts
{
    public class GameMaster : MonoBehaviour
    {

        private int remainingPigs;

        public int Score { get; private set; }

        // Use this for initialization
        void Start ()
        {
            this.Score = 0;
        }

        public void RegisterBrick(BrickCollision brick)
        {
            brick.Death += this.BlockDeathEventHandler;

            if (brick.Type == BrickCollision.BrickType.Pig)
            {
                this.remainingPigs++;
            }
        }

        /*
        public void CheckHighscore()
        {
            if (this.Score > this.Highscore)
            {
                this.Highscore = this.Score;
                Debug.Log("New highscore: " + this.Highscore + "!");
            }
            else
            {
                Debug.Log("You scored " + this.Score + " Score!");
                Debug.Log("Highscore: " + this.Highscore);
            }
        }
        */
        void BlockDeathEventHandler(BlockDeathEvent e)
        {
            this.Score += (int)e.Value;
            if (e.Type == BrickCollision.BrickType.Pig)
            {
                this.remainingPigs--;

                if (this.remainingPigs <= 0)
                {
                    // WIN-R, WIN-R, CHIC-N, DIN-R
                }
            }
        }
    }
}
