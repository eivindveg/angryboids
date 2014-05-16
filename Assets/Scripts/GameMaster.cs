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

        public const float EndWait = 5f;

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
                    if (!IsClear)
                    {
                        
                    }
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
                    SceneOver
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

            if (!SceneOver)
            {
                return;
            }
            EndTimer += Time.deltaTime;
            if (!(this.EndTimer > EndWait))
            {
                return;
            }
            LevelInfo levelInfo = this.remainingPigs <= 0 ? new LevelInfo(true, this.remainingBirds, this.Score) : new LevelInfo(false, this.remainingBirds, this.Score);
            this.StartLevelTransition(levelInfo);
            Debug.Log("FINISHED LEVEL, INFO:");
            Debug.Log(levelInfo.LevelWin);
            Debug.Log(levelInfo.BirdsRemaining);
            Debug.Log(levelInfo.Score);
        }

        public float EndTimer { get; set; }

        public bool SceneOver { get; set; }

        private void StartLevelTransition (LevelInfo lvlInfo)
        {
           
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

            public int BirdsRemaining { get; set; }

            public bool LevelWin { get; set; }

            public int Score { get; set; }

            #endregion
        }
    }
}