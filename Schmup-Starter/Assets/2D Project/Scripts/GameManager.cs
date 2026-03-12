using UnityEngine;
using TMPro;

// Track UI Score and Hi-score
// Hi-score saves with PlayerPrefs
// On player death reset score
public class GameManager : MonoBehaviour
{
    
    private const string HI_SCORE_KEY = "HI_SCORE";

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;

    private int score = 0;
    private int hiScore = 0;
    
    AudioSource audioSource;

    void Awake()
    {
        //_player = FindFirstObjectByType<Player>();
        // Save Hi-Score for future playthroughs
        hiScore = PlayerPrefs.GetInt(HI_SCORE_KEY, 0);
        
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        Enemy.onEnemyDied += onEnemyDied;
        Player.onPlayerDied += onPlayerDied;
    }

    void OnDisable()
    {
        Enemy.onEnemyDied -= onEnemyDied;
        Player.onPlayerDied -= onPlayerDied;
    }

    void Start()
    {
        // Initialize UI immediately so it shows values at start
        scoreText.text = $"Score {score.ToString("D4")}"; //Score 0000
        hiScoreText.text = $"Hi-Score {hiScore.ToString("D4")}"; //Hi-Score 0000
    }

    void onEnemyDied(int points)
    {
        AddScore(points);
    }

    public void onPlayerDied()
    {
        //From GameManager
        ResetScore();
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"Score {score.ToString("D4")}";

        if (score > hiScore)
        {
            hiScore = score;
            hiScoreText.text = $"Hi-Score {hiScore.ToString("D4")}";

            // Save Hi-Score for future playthroughs
            PlayerPrefs.SetInt(HI_SCORE_KEY, hiScore);
            PlayerPrefs.Save();
        }
    }

    // Call this to restart score
    public void ResetScore()
    {
        score = 0;
        if (scoreText != null)
        {
            scoreText.text = $"Score {score.ToString("D4")}";
        }
    }
    
    // void Start()
    // {
    //    // todo - sign up for notification about enemy death 
    //    Enemy.onEnemyDied += OnEnemyDied;
    // }
    //
    // void OnDestroy()
    // {
    //     Enemy.onEnemyDied -= OnEnemyDied;
    // }
    //
    // void OnEnemyDied(float score)
    // {
    //     Debug.Log($"Killed enemy worth {score}");
    // }
}
