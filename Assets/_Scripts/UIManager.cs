using UnityEngine;
using UnityEngine.SceneManagement; // Cần dùng để load lại màn chơi

public class UIManager : MonoBehaviour {
    // Kéo Panel WinScreen từ Hierarchy vào đây
    public GameObject winScreen;

    public void ShowWinScreen() {
        winScreen.SetActive(true);
    }

    // Hàm này sẽ được gọi khi nhấn nút Replay
    public void ReplayLevel() {
        // Load lại màn chơi hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}