using UnityEngine;
using UnityEngine.InputSystem;

public class BolasThrower : MonoBehaviour
{
    public GameObject bolasPrefab;
    public Transform throwPoint;
    public float throwForce = 15f;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void Throw(Vector2 screenPosition)
    {
        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
        GameObject bolas = Instantiate(bolasPrefab, throwPoint.position, Quaternion.identity);
        
        Vector3 direction = (worldPos - throwPoint.position).normalized;
        bolas.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
    }
}
