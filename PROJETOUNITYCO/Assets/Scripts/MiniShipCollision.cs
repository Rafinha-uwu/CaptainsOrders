using UnityEngine;

public class MiniShipCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Danger"))
        {
            Debug.Log("Player hit a danger zone! Strike!");
        }
        else if (other.CompareTag("Goal"))
        {
            Debug.Log("Player reached the goal! You win!");
        }
    }
}
