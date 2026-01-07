using UnityEngine;
using UnityEngine.SceneManagement;

public class VRMenuButtons : MonoBehaviour
{
    // Restart the currently active scene
    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // Quit the application
    public void QuitGame()
    {
        Debug.Log("Quit Game called");

        Application.Quit();

        // If running in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
