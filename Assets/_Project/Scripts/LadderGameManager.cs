using UnityEngine;

public class LadderGameManager : MonoBehaviour
{
    public static LadderGameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
