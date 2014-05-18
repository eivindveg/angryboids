namespace Assets.Scripts
{
    using UnityEngine;

    public class GameMaster : MonoBehaviour
    {
        #region Constants

        public const float EndWait = 5f;

        private const string DefaultHighScoreText = "Highscore: ";

        private const string DefaultScoreText = "Score: ";

        private const string Separator = "::";

        #endregion

        #region Static Fields

        public static bool LockControls = false;

        public static LevelInfo LvlInfo;

        private static string levelPrefsKey;

        #endregion

        #region Fields

        public TextMesh WinOrLoose;

        private GUIText HighScoreGuiText;

        private GUIText ScoreGuiText;

        private GameObject endScreen;

        private int highScore;

        private int remainingBirds;

        private int remainingPigs;

        #endregion

        #region Public Properties

        public float EndTimer { get; set; }

        public bool IsClear { get; private set; }

        public bool SceneOver { get; set; }

        public int Score { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static bool CheckHighscore(int score)
        {
            if (score > GetHighScore())
            {
                string key = levelPrefsKey + Separator + "HighScore";

                PlayerPrefs.SetInt(key, score);
                return true;
            }
            return false;
        }

        public static int GetHighScore()
        {
            string key = levelPrefsKey + Separator + "HighScore";

            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : 0;
        }

        public void LocateAndAssignBirds()
        {
            foreach (GameObject bird in GameObject.FindGameObjectsWithTag("bird"))
            {
                this.remainingBirds++;
                Debug.Log("Attaching to bird");
                var birdScript = bird.GetComponent<BirdBehaviour>();
                birdScript.Changed += this.OnBirdChange;
            }
        }

        public void OnBirdChange(BirdChangedEvent e)
        {
            if (e.State == BirdBehaviour.BirdState.Landed)
            {
                this.remainingBirds--;
                Debug.Log(this.remainingBirds + "left!");
                if (this.remainingBirds <= 0)
                {
                    this.SceneOver = true;
                }
            }
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
                    this.SceneOver = true;
                }
            }
        }

        private void Start()
        {
            var scoreObject = (GameObject)Instantiate(Resources.Load("OnScreenText"));
            scoreObject.transform.position = new Vector3(
                scoreObject.transform.position.x,
                0.86f,
                scoreObject.transform.position.z);
            this.ScoreGuiText = scoreObject.GetComponent<GUIText>();

            LockControls = false;
            int level = Application.loadedLevel - 1;
            levelPrefsKey = "level" + level;

            this.highScore = GetHighScore();
            if (this.highScore > 0)
            {
                this.IsClear = true;
                this.HighScoreGuiText =
                    ((GameObject)Instantiate(Resources.Load("OnScreenText"))).GetComponent<GUIText>();
            }
        }

        private void StartLevelTransition(LevelInfo lvlInfo)
        {
            LockControls = true;

            // Faux singleton implementation.
            if (this.endScreen != null)
            {
                return;
            }

            if (lvlInfo.LevelWin)
            {
                CheckHighscore(lvlInfo.Score);
                // WIN
                this.WinOrLoose.text = "Level cleared!";
                this.Score = lvlInfo.Score;
                // Instantiate levelFinish on camera
                this.endScreen = (GameObject)Instantiate(Resources.Load("levelFinish"));
                this.endScreen.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }
            else
            {
                // LOOSE
                this.WinOrLoose.text = "Level failed!";

                // Instantiate levelFinish on camera
                this.endScreen = (GameObject)Instantiate(Resources.Load("levelFinish"));
                this.endScreen.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }

            this.endScreen.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }

        private void Update()
        {
            this.ScoreGuiText.text = DefaultScoreText + this.Score;
            if (this.IsClear)
            {
                this.HighScoreGuiText.text = DefaultHighScoreText + this.highScore;
            }

            if (!this.SceneOver)
            {
                return;
            }
            this.EndTimer += Time.deltaTime;
            if (!(this.EndTimer > EndWait))
            {
                return;
            }
            LevelInfo levelInfo = this.remainingPigs <= 0
                                      ? new LevelInfo(true, this.remainingBirds, this.Score)
                                      : new LevelInfo(false, this.remainingBirds, this.Score);
            this.StartLevelTransition(levelInfo);
            Debug.Log("FINISHED LEVEL, INFO:");
            Debug.Log(levelInfo.LevelWin);
            Debug.Log(levelInfo.BirdsRemaining);
            Debug.Log(levelInfo.Score);
        }

        #endregion

        public struct LevelInfo
        {
            #region Constructors and Destructors

            public LevelInfo(bool levelWin, int birdsRemaining, int score)
                : this()
            {
                this.LevelWin = levelWin;
                this.BirdsRemaining = birdsRemaining;
                this.Score = score + birdsRemaining * 10000;
            }

            #endregion

            #region Public Properties

            public int BirdsRemaining { get; set; }

            public bool LevelWin { get; set; }

            public int Score { get; set; }

            #endregion
        }
    }
}