using UnityEngine;
using Random = System.Random;

public class Barrier : MonoBehaviour
{
    public float MoveSpeed { get; set; }
    public float basePositionY { get; set; }
    public int CellsCountInColumn { get; set; }
    private Vector3 finishPosition;
    private Vector3 startPosition;
    
    
    void Start()
    {
        startPosition = transform.position;
        finishPosition = new Vector3(-10.28f, startPosition.y, startPosition.z);
    }

    void Update()
    {
        ReturnOnStartPositionIfNeeded();
        
        var step = -MoveSpeed * Time.deltaTime;
        var position = transform.position;
        transform.position = new Vector3(position.x + step, position.y, position.z);

    }
    
    private void ReturnOnStartPositionIfNeeded()
    {
        var currentPosition = transform.position;
        if (currentPosition.x <= finishPosition.x)
        {
            var random = new Random();
            var y = basePositionY - random.Next(0, CellsCountInColumn);
            transform.position = new Vector3(startPosition.x, y, startPosition.z);
        }
    }
}