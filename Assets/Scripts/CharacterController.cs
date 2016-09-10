using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(RaycastCollisionDetection))]
public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rb;
    private RaycastCollisionDetection raycastColliderDetection;

    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float jumpSpeed = 10f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        raycastColliderDetection = GetComponent<RaycastCollisionDetection>();
    }

    void Update()
    {
        Vector2 movement = Vector2.zero;

        float horizontalSpeed = Input.GetAxis("Horizontal");
        float verticalSpeed = Input.GetAxis("Vertical");

        movement.x = horizontalSpeed * speed * Time.deltaTime;
        movement.y = verticalSpeed * jumpSpeed * Time.deltaTime;

        raycastColliderDetection.Move(movement, this.gameObject);
    }
}
