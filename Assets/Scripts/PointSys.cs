namespace Assets.Scripts
{
    using UnityEngine;

    public class PointSys : MonoBehaviour
    {
        #region Fields

        public int Highscore = 0;

        private int score;

        #endregion

        #region Public Methods and Operators


        //Legger til ny highscore hvis poengsummen er større enn den eksisterende highscore
        public void CheckHighscore()
        {
            if (this.score > this.Highscore)
            {
                this.Highscore = this.score;
                Debug.Log("New highscore: " + this.Highscore + "!");
            }
            else
            {
                Debug.Log("You scored " + this.score + " Score!");
                Debug.Log("Highscore: " + this.Highscore);
            }
        }

        public void CheckPoints(int a, int b, int c)
        {
            //a = sum for å få 1 stjerne
            //b = sum for å få 2 stjerner
            //c = sum for å få 3 stjerner

            if (this.score >= a)
            {
                Debug.Log("Earned 1 star.");
                //Mer kode her
            }
            else if (this.score >= b)
            {
                Debug.Log("Earned 2 stars!");
                //Mer kode her
            }
            else if (this.score >= c)
            {
                Debug.Log("Earned 3 stars!!!");
                //Mer kode her
            }
            else if (this.score < a)
            {
                Debug.Log("Earned no stars :-(");
                //Mer kode her
            }
        }

        public void SetHighscore(int h)
        {
            this.Highscore = h;
            Debug.Log("Highscore set to " + h);
        }

        #endregion

    }
}