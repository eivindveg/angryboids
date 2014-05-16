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

		    int Score { get; set; }

		    public LevelInfo(bool levelWin, int birdsRemaining, int score)
			    : this()
			{
				this.LevelWin = levelWin;
				this.BirdsRemaining = birdsRemaining;
				this.Score = score;
			}
		}

		public static LevelInfo LvlInfo;

	 

        #endregion

        #region Public Properties

        public int Score { get; private set; }

        public bool IsClear { get; private set; }

		public TextMesh WinOrLoose;

        #endregion

        #region Public Methods and Operators

		private void Update(){
			this.ScoreGuItext.text = DefaultScoreText + this.Score;
		}

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
					// WIN
					WinOrLoose.text="Level cleared!"; 
					LvlInfo = new LevelInfo(true, remainingBirds, this.Score);
                }
            }
        }

		public void OnBirdChange (BirdChangedEvent e)
		{
			if (e.State == BirdBehaviour.BirdState.Landed)
			{
				remainingBirds--;
			    Debug.Log(remainingBirds + "left!");
				if(this.remainingBirds <= 0 && this.remainingPigs >= 1) 
				{
					// LOOSE
					WinOrLoose.text="Level failed!";

                    // Instantiate levelFinish on camera?!
					GameObject g = (GameObject)Instantiate(Resources.Load("levelFinish"));
				    
                    g.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
					Destroy(GameObject.FindGameObjectWithTag("birdIcon"));
					LvlInfo = new LevelInfo(false, remainingBirds, this.Score);
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

        private bool LevelTransition (LevelInfo lvlInfo) {
                    
        }

        #endregion
    }
}