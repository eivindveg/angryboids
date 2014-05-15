namespace Assets.Scripts
{
    using UnityEditor;

    using UnityEngine;

    public class GameMaster : MonoBehaviour
    {
        #region Constants

        private const string DefaultHighScoreText = "Highscore: ";

        private const string DefaultScoreText = "Score: ";

        private const string Separator = "::";

        #endregion

        #region Fields

        public GUIText ScoreGuItext;

        private int highScore;

        private bool isClear;

        private string levelPrefsKey;

        private int remainingPigs;

		private int remainingBirds;

		public struct LevelInfo
		{
			bool levelWin {get; private set;}
			int birdsRemaining {get; private set;}
			int score {get; private set;}

			public LevelInfo(bool levelWin, int birdsRemaining, int score)
			{
				this.levelWin = levelWin;
				this.birdsRemaining = birdsRemaining;
				this.score = score;
			}
		}

		public static LevelInfo levelInfo;

	 

        #endregion

        #region Public Properties

        public int Score { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static bool CheckHighscore(int score)
        {
            if (score > this.GetHighScore())
            {
                PlayerPrefs.SetInt("HighScore", score);
				return true;
			}
			return false;
        }

        public static int GetHighScore()
        {
            string key = this.levelPrefsKey + Separator + "HighScore";

            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : 0;
        }

        public void RegisterBrick(BrickCollision brick)
        {
            brick.Death += this.BlockDeathEventHandler;

            if (brick.Type == BrickCollision.BrickType.Pig)
            {
                this.remainingPigs++;
            }
        }

        #endregion

        #region Methods

        private void BlockDeathEventHandler(BlockDeathEvent e)
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

		private void OnBirdChange (BirdChangedEvent e)
		{
			if (e.GetType () == BirdBehaviour.BirdState.Landed)
			{
				remainingBirds--;

				if(this.remainingBirds <= 0 && this.remainingPigs >= 1) 
				{
					levelInfo = new LevelInfo(false, remainingBirds, this.Score);

				} else if(this.remainingPigs <= 0)
					levelInfo = new LevelInfo(true, remainingBirds, this.Score);
				{

				}
			}
		}

		public void LocateAndAssignBirds()
		{
			foreach (GameObject bird in GameObject.FindGameObjectsWithTag("bird"))
			{
				remainingBirds++;
				Debug.Log("Attaching to bird");
				var birdScript = bird.GetComponent<BirdBehaviour>();
				birdScript.Changed += this.OnBirdChange;
			}
		}

        private void Start()
        {
            string currentScene = EditorApplication.currentScene;
            currentScene = (currentScene.Substring((currentScene.Length - 1)));
            this.levelPrefsKey = "level" + currentScene;

            this.highScore = this.GetHighScore();
            if (this.highScore > 0)
            {
                this.isClear = true;
            }

            this.Score = 0;
            this.ScoreGuItext.text = DefaultScoreText + this.Score;
        }

        #endregion
    }
}