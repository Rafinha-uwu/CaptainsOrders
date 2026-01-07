using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Puzzle Order")]
    public Puzzle[] puzzles;

    [Header("Game End Objects")]
    public GameObject victoryObject;
    public GameObject failureObject;

    [Header("Failure Rules")]
    public int maxFailures = 3;

    private int currentPuzzleIndex = -1;
    private int failureCount = 0;
    private bool gameEnded = false;

    private void Start()
    {
        victoryObject.SetActive(false);
        failureObject.SetActive(false);

        foreach (Puzzle puzzle in puzzles)
        {
            puzzle.DeactivatePuzzle();
            puzzle.OnPuzzleEnded += HandlePuzzleEnded;
        }
    }

    public void StartGame()
    {
        gameEnded = false;
        currentPuzzleIndex = -1;
        failureCount = 0;

        victoryObject.SetActive(false);
        failureObject.SetActive(false);

        StartNextPuzzle();
    }

    private void HandlePuzzleEnded(bool success)
    {
        if (gameEnded) return;

        if (!success)
        {
            failureCount++;
            Debug.Log($"Failures: {failureCount}/{maxFailures}");

            if (failureCount >= maxFailures)
            {
                EndGame(false);
            }

            // STAY on the current puzzle
            return;
        }

        // SUCCESS → move forward
        StartNextPuzzle();
    }


    private void StartNextPuzzle()
    {
        if (currentPuzzleIndex >= 0 && currentPuzzleIndex < puzzles.Length)
        {
            puzzles[currentPuzzleIndex].DeactivatePuzzle();
        }

        currentPuzzleIndex++;

        if (currentPuzzleIndex < puzzles.Length)
        {
            puzzles[currentPuzzleIndex].ActivatePuzzle();
        }
        else
        {
            EndGame(true);
        }
    }

    private void EndGame(bool victory)
    {
        gameEnded = true;

        if (victory)
        {
            Debug.Log("GAME WON");
            victoryObject.SetActive(true);
        }
        else
        {
            Debug.Log("GAME LOST");
            failureObject.SetActive(true);
        }
    }
}
