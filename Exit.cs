using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // Метод для выхода из игры
    public void ExitGame()
    {
        Application.Quit(); // Закрытие приложения
        Debug.Log("Exit button clicked."); // Для отладки (не работает в редакторе)
    }
}