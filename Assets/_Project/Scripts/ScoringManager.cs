using UnityEngine;

public class ScoringManager : MonoBehaviour
{
    public static ScoringManager Instance;
    public int currentScore = 0;
    public int targetScore = 21;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int points)
    {
        currentScore += points;
        if (currentScore >= targetScore)
        {
            Debug.Log("Player wins!");
        }
    }
}
