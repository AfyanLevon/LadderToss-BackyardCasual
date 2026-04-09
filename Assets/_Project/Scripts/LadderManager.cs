using UnityEngine;

public class LadderManager : MonoBehaviour
{
    public int points = 1;
    private ScoringManager scoringManager;

    private void Start()
    {
        scoringManager = FindObjectOfType<ScoringManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bolas"))
        {
            if (scoringManager != null)
            {
                scoringManager.AddScore(points);
            }
            Destroy(collision.gameObject);
        }
    }
}
