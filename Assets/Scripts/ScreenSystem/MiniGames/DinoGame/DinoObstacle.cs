using UnityEngine;

public class DinoObstacle : MonoBehaviour
{
    public DinoSpawner spawner;
    public void UpdateObstacle(float speed)
    {
        transform.position += Vector3.left * speed;    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeathWall"))
        {
            spawner.DestroyObstacle(this);
        }
    }
}

