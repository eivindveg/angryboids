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

        #region Static Fields

        public static LevelInfo LvlInfo;

        private static string levelPrefsKey;

        #endregion

        #region Fields

        public GUIText ScoreGuiText;

        public TextMesh WinOrLoose;

        private int highScore;

        private int remainingBirds;

        private int remainingPigs;

        #endregion

        #region Public Properties

        public bool IsClear { get; private set; }

        public int Score { get; private set; }

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
                if (this.remainingBirds <= 0 && this.remainingPigs >= 1)
                {
                    // LOOSE
                    this.WinOrLoose.text = "Level failed!";

                    // Instantiate levelFinish on camera
                    var g = (GameObject)Instantiate(Resources.Load("levelFinish"));
                    g.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    Destroy(GameObject.FindGameObjectWithTag("birdIcon"));
                    LvlInfo = new LevelInfo(false, this.remainingBirds, this.Score);
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
                    // WIN
                    this.WinOrLoose.text = "Level cleared!";
                    LvlInfo = new LevelInfo(true, this.remainingBirds, this.Score);
                }
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
        }

        private void Update()
        {
            this.ScoreGuiText.text = DefaultScoreText + this.Score;
        }

        private void StartLevelTransition (LevelInfo lvlInfo)
        {
            return false;
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
                this.Score = score;
            }

            #endregion

            #region Properties

            private int BirdsRemaining { get; set; }

            private bool LevelWin { get; set; }

            private int Score { get; set; }

            #endregion
        }
    }
}