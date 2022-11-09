using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Corridor : MonoBehaviour
{
    public float MoveSpeed { get; set; }
    private Vector3 finishPosition;
    [SerializeField] private Transform secondCorridorElementTransform;

    void Start()
    {
        var initiationPosition = new Vector3(1.5f, -0.88f, -100);
        finishPosition = new Vector3(initiationPosition.x - 20, initiationPosition.y, initiationPosition.z);
    }

    void Update()
    {
        ReturnOnStartPositionIfNeeded(transform, secondCorridorElementTransform);
        ReturnOnStartPositionIfNeeded(secondCorridorElementTransform, transform);
        var step = -MoveSpeed * Time.deltaTime;
        
        var position = transform.position;
        transform.position = new Vector3(position.x + step, position.y, position.z);
        var position2 = secondCorridorElementTransform.position;
        secondCorridorElementTransform.position = new Vector3(position2.x + step, position2.y, position2.z);
    }

    private void ReturnOnStartPositionIfNeeded(Transform corridorFirstTransform, Transform corridorSecondTransform)
    {
        var corridorFirstTransformPosition = corridorFirstTransform.position;
        if (corridorFirstTransformPosition.x <= finishPosition.x)
        {
            var corridorSecondTransformPosition = corridorSecondTransform.position;
            corridorFirstTransform.position = new Vector3(corridorSecondTransformPosition.x + 20,
                corridorSecondTransformPosition.y, corridorSecondTransformPosition.z);
        }
    }
}
