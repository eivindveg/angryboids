using UnityEngine;

namespace Assets.Scripts
{
    public class GameMaster : MonoBehaviour
    {

        private int Pigs;

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
                Pigs++;
            }
        }

        void BlockDeathEventHandler(BlockDeathEvent e)
        {
            this.Score += (int)e.Value;
            if (e.Type == BrickCollision.BrickType.Pig)
            {
                Pigs--;

                if (Pigs <= 0)
                {
                    // WIN-R, WIN-R, CHIC-N, DIN-R
                }
            }
        }
    }
}
