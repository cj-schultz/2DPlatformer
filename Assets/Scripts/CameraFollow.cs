using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float followSpeed = 10f;

    private float minHeight;

    void Awake()
    {
        minHeight = transform.position.y;
    }

    void Update()
    {
        Vector3 targetPos = new Vector3(transform.position.x, Mathf.Clamp(player.position.y, minHeight, Mathf.Infinity), transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
    }

}