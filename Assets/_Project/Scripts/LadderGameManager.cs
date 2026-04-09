using UnityEngine;

public class LadderGameManager : MonoBehaviour
{
    public int playerScore = 0;
    public int targetScore = 21;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bolas"))
        {
            // Որոշում ենք միավորները ladder-ի բարձրությամբ
            float height = collision.transform.position.y;
            int points = height > 2f ? 3 : (height > 1f ? 2 : 1);

            playerScore += points;
            Debug.Log($"🎯 +{points} points! Score: {playerScore}/{targetScore}");

            Destroy(collision.gameObject);

            if (playerScore >= targetScore)
            {
                Debug.Log("🏆 YOU WIN THE BACKYARD!");
                // Այստեղ կավելացնենք win screen հետո
            }
        }
    }
}
