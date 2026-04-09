using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public GameObject bolasPrefab;

    private void Awake()
    {
        // ThrowPoint
        GameObject throwPoint = new GameObject("ThrowPoint");
        throwPoint.transform.position = new Vector3(0, 1, 0);

        // Ladder + Collider + Game Manager
        GameObject ladder = new GameObject("Ladder");
        ladder.transform.position = new Vector3(4, 0, 0);
        ladder.AddComponent<BoxCollider>().size = new Vector3(0.3f, 3f, 0.3f);
        LadderGameManager manager = ladder.AddComponent<LadderGameManager>();

        // BolasThrower
        BolasThrower thrower = throwPoint.AddComponent<BolasThrower>();
        thrower.bolasPrefab = bolasPrefab;
        thrower.throwPoint = throwPoint.transform;

        // SwipeInput Main Camera-ին
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            SwipeInput swipe = mainCam.gameObject.AddComponent<SwipeInput>();
            swipe.thrower = thrower;
        }

        Debug.Log("✅ Full auto-setup with Ladder collider completed!");
    }
}
