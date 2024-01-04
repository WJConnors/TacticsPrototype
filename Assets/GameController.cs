using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    private float checkInterval = 1.0f; // Check every 1 second

    void Start()
    {
        StartCoroutine(CheckForEndGameCoroutine());
    }

    IEnumerator CheckForEndGameCoroutine()
    {
        while (true)
        {
            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
            if (units.Length == 0)
            {
                EndGame();
                yield break; // Stop the coroutine
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Over! No units left.");
        // Close the game
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // If running in the Unity Editor
#else
        Application.Quit(); // If running in a build
#endif
    }
}