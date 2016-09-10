using UnityEngine;
using System.Collections;

public class RaycastCollisionDetection : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rect collisionRect;

    public bool onGround;
    public bool sideCollision;
    public bool playerCollision;

    [SerializeField]
    private int verticalRaycasts = 4;
    [SerializeField]
    private int horizontalRaycasts = 4;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        collisionRect = new Rect(
            boxCollider.bounds.min.x,
            boxCollider.bounds.min.y,
            boxCollider.bounds.size.x,
            boxCollider.bounds.size.y);
    }

    public void Move(Vector2 deltaMovement, GameObject player)
    {
        Vector2 playerPosition = player.transform.position;

        float deltaY = deltaMovement.y;
        float deltaX = deltaMovement.x;
        deltaY = YAxisCollisionDetection(deltaY, Mathf.Abs(deltaX), playerPosition);
    }

    private float YAxisCollisionDetection(float deltaY, float dirX, Vector2 playerPosition)
    {
        onGround = false;

        float margin = 0.0f;

        Vector2 rayStartPoint = new Vector2(collisionRect.xMin + margin, collisionRect.center.y);
        Vector2 rayEndPoint = new Vector2(collisionRect.xMax - margin, collisionRect.center.y);
        float distance = (collisionRect.height / 2) + Mathf.Abs(deltaY);

        for (int i = 0; i < horizontalRaycasts; i++)
        {
            float lerpAmount = (float)i / ((float)horizontalRaycasts - 1);

            Vector2 origin = dirX == -1 ? Vector2.Lerp(rayStartPoint, rayEndPoint, lerpAmount) : Vector2.Lerp(rayEndPoint, rayStartPoint, lerpAmount);

            Ray ray = new Ray(origin, new Vector2(0, Mathf.Sign(deltaY)));
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
        }

        return deltaY;
    }


}
