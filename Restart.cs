using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Метод для перезагрузки сцены
    public void RestartGame()
    {
        SceneManager.LoadScene(0); // Загружаем сцену с индексом 0 (можно изменить на индекс нужной сцены)
    }
}