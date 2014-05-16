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

        private static string levelPrefsKey;

        private int remainingPigs;

		private int remainingBirds;

		public struct LevelInfo
		{
		    bool LevelWin {get; set;}
			int BirdsRemaining {get; set;}

		    private int score { get; set; }

		    public LevelInfo(bool levelWin, int birdsRemaining, int score)
			    : this()
			{
				this.LevelWin = levelWin;
				this.BirdsRemaining = birdsRemaining;
				this.score = score;
			}
		}

		public static LevelInfo LvlInfo;

	 

        #endregion

        #region Public Properties

        public int Score { get; private set; }

        public bool IsClear { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static bool CheckHighscore(int score)
        {
            if (score > GetHighScore())
            {
                PlayerPrefs.SetInt("HighScore", score);
				return true;
			}
			return false;
        }

        public static int GetHighScore()
        {
            string key = levelPrefsKey + Separator + "HighScore";

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
			if (e.State == BirdBehaviour.BirdState.Landed)
			{
				remainingBirds--;

				if(this.remainingBirds <= 0 && this.remainingPigs >= 1) 
				{
					LvlInfo = new LevelInfo(false, remainingBirds, this.Score);

				} else if(this.remainingPigs <= 0)
					LvlInfo = new LevelInfo(true, remainingBirds, this.Score);
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
            levelPrefsKey = "level" + currentScene;

            this.highScore = GetHighScore();
            if (this.highScore > 0)
            {
                this.IsClear = true;
            }

            this.Score = 0;
            this.ScoreGuItext.text = DefaultScoreText + this.Score;
        }

        #endregion
    }
}