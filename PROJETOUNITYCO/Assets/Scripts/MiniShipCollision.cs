using UnityEngine;

public class MiniShipCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Puzzle puzzle = GetComponent<Puzzle>();

        if (other.CompareTag("Danger"))
        {
            Debug.Log("Player hit a danger zone! Strike!");
            puzzle.FailPuzzle();
        }
        else if (other.CompareTag("Goal"))
        {
            Debug.Log("Player reached the goal! You win!");
            puzzle.CompletePuzzle();
        }
    }
}
