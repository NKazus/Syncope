using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    private SceneController _sceneController;

    private void Start()
    {
        _sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            _sceneController.StartNextLevel();
        }
    }
}
