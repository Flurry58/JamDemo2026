using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layerPlayer = LayerMask.NameToLayer("Player");

        if (collision.gameObject.layer == layerPlayer)
        {
            LoadNextLevel();

        }
    }
}
