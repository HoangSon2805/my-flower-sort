using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// Script này được gắn vào một GameObject rỗng trong Scene
public class GameManager : MonoBehaviour {
    // Lưu lại chậu hoa đang được người chơi chọn
    private VaseController selectedVase = null;
    public UIManager uiManager;
    public List<VaseController> allVases;
    // Update được gọi mỗi khung hình
    void Update() {
        // Chỉ xử lý khi người chơi nhấp chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            // Chuyển đổi tọa độ màn hình của chuột thành tọa độ trong thế giới game
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Bắn một tia từ vị trí chuột để xem nó chạm vào vật thể nào
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Nếu tia chạm vào một vật thể có Collider
            if (hit.collider != null)
            {
                // Thử lấy component VaseController từ vật thể đó
                VaseController clickedVase = hit.collider.GetComponent<VaseController>();

                // Nếu vật thể đó thực sự là một chậu hoa
                if (clickedVase != null)
                {
                    HandleVaseClick(clickedVase);
                }
            }
        }
    }

    /// <summary>
    /// Xử lý logic khi một chậu hoa được nhấp vào.
    /// </summary>
    private void HandleVaseClick(VaseController clickedVase) {
        // TRƯỜNG HỢP 1: CHƯA CÓ CHẬU NÀO ĐƯỢC CHỌN
        if (selectedVase == null)
        {
            // Nếu chậu được click không rỗng, chúng ta sẽ chọn nó
            if (!clickedVase.IsEmpty())
            {
                selectedVase = clickedVase;
                // Hiệu ứng nhỏ: Nhấc chậu hoa lên một chút để người chơi biết nó đã được chọn
                selectedVase.transform.position += Vector3.up * 0.5f;
            }
        }
        // TRƯỜNG HỢP 2: ĐÃ CÓ MỘT CHẬU ĐƯỢC CHỌN
        else
        {
            // Nếu người chơi click lại chính chậu đã chọn -> Bỏ chọn
            if (selectedVase == clickedVase)
            {
                // Trả chậu hoa về vị trí cũ
                selectedVase.transform.position -= Vector3.up * 0.5f;
                selectedVase = null;
            }
            // Nếu người chơi click vào một chậu khác (chậu mục tiêu)
            else
            {
                // Thực hiện di chuyển bông hoa
                MoveFlower(selectedVase, clickedVase);
            }
        }
    }

    /// <summary>
    /// Kiểm tra luật chơi và di chuyển bông hoa nếu hợp lệ.
    /// </summary>
    private void MoveFlower(VaseController fromVase, VaseController toVase) {
        // ĐIỀU KIỆN DI CHUYỂN:
        // 1. Chậu mục tiêu (toVase) không được đầy.
        // 2. VÀ (chậu mục tiêu rỗng HOẶC màu hoa trên cùng của 2 chậu phải giống nhau).
        if (!toVase.IsFull() && (toVase.IsEmpty() || toVase.GetTopFlowerColor() == fromVase.GetTopFlowerColor()))
        {
            // Lấy bông hoa trên cùng từ chậu gốc
            GameObject flowerToMove = fromVase.RemoveFlower();
            // Thêm bông hoa đó vào chậu mục tiêu
            // DÙNG DOTWEEN ĐỂ TẠO ANIMATION
            // 1. Tạo một chuỗi animation
            Sequence sequence = DOTween.Sequence();
            // 2. Thêm bước di chuyển hoa lên trên chậu cũ
            sequence.Append(flowerToMove.transform.DOMove(fromVase.transform.position + Vector3.up * 1.5f, 0.2f));
            // 3. Thêm bước di chuyển hoa đến phía trên chậu mới
            sequence.Append(flowerToMove.transform.DOMove(toVase.transform.position + Vector3.up * 1.5f, 0.3f));
            // 4. Khi animation hoàn thành, gọi hàm để đặt hoa vào chậu mới
            sequence.OnComplete(() =>
            {
                toVase.AddFlower(flowerToMove);

                // TODO: Kiểm tra điều kiện thắng sau khi di chuyển thành công
                CheckWinCondition();
            });
        }

        // Dù di chuyển thành công hay không, luôn bỏ chọn chậu gốc và trả nó về vị trí cũ
        fromVase.transform.position -= Vector3.up * 0.5f;
        selectedVase = null;
    }

    private void CheckWinCondition() {
        foreach (var vase in allVases)
        {
            // Nếu có bất kỳ một chậu nào chưa rỗng và chưa hoàn thành -> game chưa thắng
            if (!vase.IsEmpty() && !vase.IsCompleted())
            {
                return; // Dừng hàm ngay lập tức
            }
        }

        // Nếu vòng lặp chạy hết mà không bị return -> tất cả các chậu đã xong -> THẮNG!
        Debug.Log("YOU WIN!");
        uiManager.ShowWinScreen();
    }
}