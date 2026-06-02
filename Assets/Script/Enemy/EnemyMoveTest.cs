using UnityEngine;

public class EnemyMoveTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 moveDirection;
    private float changeTimer;

    private void Start()
    {
        ChooseDirection();
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        changeTimer -= Time.deltaTime;

        if (changeTimer <= 0)
        {
            ChooseDirection();
        }
    }

    private void ChooseDirection()
    {
        int random = Random.Range(0, 5);

        switch (random)
        {
            case 0:
                moveDirection = transform.forward;
                break;

            case 1:
                moveDirection = -transform.forward;
                break;

            case 2:
                moveDirection = transform.right;
                break;

            case 3:
                moveDirection = -transform.right;
                break;

            case 4:
                moveDirection = Vector3.zero;
                break;
        }

        changeTimer = Random.Range(1f, 3f);
    }
}
