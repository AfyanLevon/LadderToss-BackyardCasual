using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class SceneSetup : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        SceneSetup[] setups = FindObjectsOfType<SceneSetup>();
        if (setups.Length > 0)
        {
            return;
        }

        GameObject setupObject = new GameObject("SceneSetup");
        setupObject.AddComponent<SceneSetup>();
    }

    private void Awake()
    {
        DestroyAllExistingCameras();
        DestroyAllExistingLights();
        DestroyAllExistingCanvases();
        DestroyAllExistingEventSystems();
        DestroyExistingThrowers();
        DestroyExistingInputs();
        DestroyExistingManagers();

        CreateMainCamera();
        CreateDirectionalLight();
        CreateGround();

        GameObject ladderManagerObject = new GameObject("LadderGameManager");
        ladderManagerObject.transform.position = new Vector3(0f, 0f, 4f);
        ladderManagerObject.AddComponent<LadderGameManager>();
        CreateLadder(ladderManagerObject.transform);

        BolasThrower thrower = CreateThrowPoint();
        CreateSwipeInput(thrower);
    }

    private void DestroyAllExistingCameras()
    {
        Camera[] cameras = FindObjectsOfType<Camera>();
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != null)
            {
                DestroyImmediate(cameras[i].gameObject);
            }
        }
    }

    private void DestroyAllExistingLights()
    {
        Light[] lights = FindObjectsOfType<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null)
            {
                DestroyImmediate(lights[i].gameObject);
            }
        }
    }

    private void DestroyAllExistingCanvases()
    {
        Behaviour[] behaviours = FindObjectsOfType<Behaviour>();
        for (int i = 0; i < behaviours.Length; i++)
        {
            Behaviour behaviour = behaviours[i];
            if (behaviour == null)
            {
                continue;
            }

            if (behaviour.GetType().Name == "Canvas")
            {
                DestroyImmediate(behaviour.gameObject);
            }
        }
    }

    private void DestroyAllExistingEventSystems()
    {
        MonoBehaviour[] behaviours = FindObjectsOfType<MonoBehaviour>();
        for (int i = 0; i < behaviours.Length; i++)
        {
            MonoBehaviour behaviour = behaviours[i];
            if (behaviour == null)
            {
                continue;
            }

            if (behaviour.GetType().Name == "EventSystem")
            {
                DestroyImmediate(behaviour.gameObject);
            }
        }
    }

    private void DestroyExistingThrowers()
    {
        BolasThrower[] throwers = FindObjectsOfType<BolasThrower>();
        for (int i = 0; i < throwers.Length; i++)
        {
            if (throwers[i] != null)
            {
                DestroyImmediate(throwers[i].gameObject);
            }
        }
    }

    private void DestroyExistingInputs()
    {
        SwipeInput[] inputs = FindObjectsOfType<SwipeInput>();
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i] != null)
            {
                DestroyImmediate(inputs[i].gameObject);
            }
        }
    }

    private void DestroyExistingManagers()
    {
        LadderGameManager[] managers = FindObjectsOfType<LadderGameManager>();
        for (int i = 0; i < managers.Length; i++)
        {
            if (managers[i] != null)
            {
                DestroyImmediate(managers[i].gameObject);
            }
        }
    }

    private void CreateMainCamera()
    {
        GameObject cameraObject = new GameObject("MainCamera");
        cameraObject.tag = "MainCamera";

        Camera cameraComponent = cameraObject.AddComponent<Camera>();
        cameraComponent.fieldOfView = 60f;
        cameraComponent.transform.position = new Vector3(0f, 6f, -12f);
        cameraComponent.transform.LookAt(new Vector3(0f, 2.5f, 4f));
    }

    private void CreateDirectionalLight()
    {
        GameObject lightObject = new GameObject("Directional Light");
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        Light lightComponent = lightObject.AddComponent<Light>();
        lightComponent.type = LightType.Directional;
        lightComponent.intensity = 1.15f;
    }

    private void CreateGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(10f, 1f, 10f);

        Renderer renderer = ground.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = CreateMaterial(new Color(0.18f, 0.65f, 0.22f));
        }
    }

    private void CreateLadder(Transform parent)
    {
        Material poleMaterial = CreateMaterial(new Color(0.08f, 0.08f, 0.08f));
        Material rungMaterial = CreateMaterial(new Color(0.95f, 0.48f, 0.10f));

        CreateLadderPiece(parent, "LeftPole", new Vector3(-0.9f, 2.5f, 0f), new Vector3(0.2f, 5f, 0.2f), poleMaterial);
        CreateLadderPiece(parent, "RightPole", new Vector3(0.9f, 2.5f, 0f), new Vector3(0.2f, 5f, 0.2f), poleMaterial);

        for (int i = 0; i < 5; i++)
        {
            float y = 0.8f + (i * 0.85f);
            CreateLadderPiece(parent, "Rung_" + (i + 1), new Vector3(0f, y, 0f), new Vector3(2f, 0.16f, 0.22f), rungMaterial);
        }
    }

    private void CreateLadderPiece(Transform parent, string pieceName, Vector3 localPosition, Vector3 localScale, Material material)
    {
        GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.name = pieceName;
        piece.transform.SetParent(parent, false);
        piece.transform.localPosition = localPosition;
        piece.transform.localScale = localScale;

        BoxCollider collider = piece.GetComponent<BoxCollider>();
        if (collider == null)
        {
            piece.AddComponent<BoxCollider>();
        }

        Renderer renderer = piece.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
        }
    }

    private BolasThrower CreateThrowPoint()
    {
        GameObject throwPoint = new GameObject("ThrowPoint");
        throwPoint.transform.position = new Vector3(0f, 1.5f, -3f);

        BolasThrower thrower = throwPoint.AddComponent<BolasThrower>();
        thrower.throwPoint = throwPoint.transform;
        return thrower;
    }

    private void CreateSwipeInput(BolasThrower thrower)
    {
        GameObject swipeObject = new GameObject("SwipeInput");
        SwipeInput swipeInput = swipeObject.AddComponent<SwipeInput>();
        swipeInput.thrower = thrower;
    }

    private Material CreateMaterial(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }
        if (shader == null)
        {
            shader = Shader.Find("Unlit/Color");
        }

        Material material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;

        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }
        if (material.HasProperty("_Color"))
        {
            material.color = color;
        }

        return material;
    }
}
