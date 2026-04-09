using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public GameObject bolasPrefab;

    private void Awake()
    {
        GameObject throwPoint = new GameObject("ThrowPoint");
        throwPoint.transform.position = new Vector3(0, 1, 0);

        GameObject ladder = new GameObject("Ladder");
        ladder.transform.position = new Vector3(5, 0, 0);

        BolasThrower thrower = throwPoint.AddComponent<BolasThrower>();
        thrower.bolasPrefab = bolasPrefab;
        thrower.throwPoint = throwPoint.transform;

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            SwipeInput swipe = mainCam.gameObject.AddComponent<SwipeInput>();
            swipe.thrower = thrower;
        }

        Debug.Log("✅ Scene fully auto-setup completed!");
    }
}
