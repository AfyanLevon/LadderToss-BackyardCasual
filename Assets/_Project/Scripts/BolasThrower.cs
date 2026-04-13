using UnityEngine;

public class BolasThrower : MonoBehaviour
{
    public Transform throwPoint;
    public float ropeLength = 0.65f;
    public float ballRadius = 0.18f;
    public float ballMass = 0.45f;
    public int maxActiveBolas = 10;

    private GameObject[] activeBolas;
    private int nextBolasIndex;

    private static Material ballMaterialA;
    private static Material ballMaterialB;
    private static Material ropeMaterial;

    private void Awake()
    {
        if (throwPoint == null)
        {
            throwPoint = transform;
        }

        activeBolas = new GameObject[maxActiveBolas];
    }

    private void Update()
    {
        for (int i = 0; i < activeBolas.Length; i++)
        {
            GameObject bolasRoot = activeBolas[i];
            if (bolasRoot == null || bolasRoot.transform.childCount < 2)
            {
                continue;
            }

            LineRenderer rope = bolasRoot.GetComponent<LineRenderer>();
            if (rope == null)
            {
                continue;
            }

            rope.SetPosition(0, bolasRoot.transform.GetChild(0).position);
            rope.SetPosition(1, bolasRoot.transform.GetChild(1).position);
        }
    }

    public void Throw(Vector3 direction, float power)
    {
        if (throwPoint == null)
        {
            throwPoint = transform;
        }

        direction = direction.sqrMagnitude > 0.001f ? direction.normalized : new Vector3(0f, 0.35f, 1f).normalized;
        power = Mathf.Max(1f, power);

        GameObject oldBolas = activeBolas[nextBolasIndex];
        if (oldBolas != null)
        {
            Destroy(oldBolas);
        }

        GameObject bolasRoot = new GameObject("Bolas");
        bolasRoot.transform.position = throwPoint.position;

        LineRenderer rope = bolasRoot.AddComponent<LineRenderer>();
        rope.positionCount = 2;
        rope.widthMultiplier = 0.05f;
        rope.numCapVertices = 4;
        rope.material = GetRopeMaterial();
        rope.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        rope.receiveShadows = false;
        rope.startColor = new Color(0.20f, 0.14f, 0.09f);
        rope.endColor = new Color(0.20f, 0.14f, 0.09f);

        GameObject ballA = CreateBall("BallA", bolasRoot.transform, new Vector3(-ropeLength * 0.5f, 0f, 0f), GetBallMaterialA());
        GameObject ballB = CreateBall("BallB", bolasRoot.transform, new Vector3(ropeLength * 0.5f, 0f, 0f), GetBallMaterialB());

        rope.SetPosition(0, ballA.transform.position);
        rope.SetPosition(1, ballB.transform.position);

        Rigidbody rbA = ballA.GetComponent<Rigidbody>();
        Rigidbody rbB = ballB.GetComponent<Rigidbody>();
        Collider colA = ballA.GetComponent<Collider>();
        Collider colB = ballB.GetComponent<Collider>();

        Physics.IgnoreCollision(colA, colB, true);

        SpringJoint spring = ballB.AddComponent<SpringJoint>();
        spring.connectedBody = rbA;
        spring.autoConfigureConnectedAnchor = false;
        spring.anchor = Vector3.zero;
        spring.connectedAnchor = Vector3.zero;
        spring.minDistance = ropeLength * 0.8f;
        spring.maxDistance = ropeLength;
        spring.spring = 80f;
        spring.damper = 6f;
        spring.enableCollision = false;

        Vector3 side = Vector3.Cross(direction, Vector3.up);
        if (side.sqrMagnitude < 0.001f)
        {
            side = Vector3.right;
        }

        side.Normalize();

        Vector3 launchForce = direction * power;
        Vector3 splitForce = side * 1.2f;

        rbA.AddForce(launchForce + splitForce, ForceMode.Impulse);
        rbB.AddForce(launchForce - splitForce, ForceMode.Impulse);
        rbA.AddTorque(-side * 4f, ForceMode.Impulse);
        rbB.AddTorque(side * 4f, ForceMode.Impulse);

        activeBolas[nextBolasIndex] = bolasRoot;
        nextBolasIndex = (nextBolasIndex + 1) % activeBolas.Length;
    }

    private GameObject CreateBall(string objectName, Transform parent, Vector3 localPosition, Material material)
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = objectName;
        ball.transform.SetParent(parent, false);
        ball.transform.localPosition = localPosition;
        ball.transform.localScale = Vector3.one * (ballRadius * 2f);

        Renderer renderer = ball.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        Rigidbody rb = ball.AddComponent<Rigidbody>();
        rb.mass = ballMass;
        rb.linearDamping = 0.12f;
        rb.angularDamping = 0.05f;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        return ball;
    }

    private static Material GetBallMaterialA()
    {
        if (ballMaterialA == null)
        {
            ballMaterialA = CreateMaterial(new Color(0.96f, 0.46f, 0.12f));
        }

        return ballMaterialA;
    }

    private static Material GetBallMaterialB()
    {
        if (ballMaterialB == null)
        {
            ballMaterialB = CreateMaterial(new Color(0.22f, 0.58f, 0.96f));
        }

        return ballMaterialB;
    }

    private static Material GetRopeMaterial()
    {
        if (ropeMaterial == null)
        {
            ropeMaterial = CreateMaterial(new Color(0.22f, 0.16f, 0.10f));
        }

        return ropeMaterial;
    }

    private static Material CreateMaterial(Color color)
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
