using System.Collections.Generic;
using UnityEngine;

// Script này được gắn vào mỗi Prefab Chậu hoa (Vase)
public class VaseController : MonoBehaviour {
    // Dùng Stack để lưu các bông hoa. Stack hoạt động theo nguyên tắc "vào sau, ra trước" (LIFO),
    // hoàn toàn phù hợp với logic của game: chỉ có thể lấy bông hoa trên cùng.
    public Stack<GameObject> flowers = new Stack<GameObject>();

    // Sức chứa tối đa của một chậu hoa
    public int vaseCapacity = 4;

    // ----- CÁC HÀM CÔNG KHAI ĐỂ GAMEMANAGER GỌI -----

    /// <summary>
    /// Lấy bông hoa trên cùng ra khỏi chậu.
    /// </summary>
    /// <returns>Đối tượng GameObject của bông hoa được lấy ra.</returns>
    public GameObject RemoveFlower() {
        if (flowers.Count > 0)
        {
            GameObject flower = flowers.Pop();
            return flower;
        }
        return null;
    }

    /// <summary>
    /// Thêm một bông hoa vào chậu và cập nhật vị trí của nó.
    /// </summary>
    /// <param name="flower">Đối tượng GameObject của bông hoa cần thêm.</param>
    public void AddFlower(GameObject flower) {
        // Thêm hoa vào stack
        flowers.Push(flower);

        // Cập nhật để bông hoa trở thành "con" của chậu hoa này
        flower.transform.SetParent(this.transform);

        // Tính toán và đặt vị trí mới cho bông hoa bên trong chậu
        // Các con số này có thể cần điều chỉnh để phù hợp với kích thước sprite của bạn
        float yOffset = -0.6f + (flowers.Count - 1) * 0.5f;
        flower.transform.localPosition = new Vector3(0, yOffset, 0);
    }

    /// <summary>
    /// Lấy màu của bông hoa trên cùng mà không lấy nó ra.
    /// </summary>
    /// <returns>Tên sprite, dùng để định danh màu sắc.</returns>
    public string GetTopFlowerColor() {
        if (flowers.Count > 0)
        {
            // Chúng ta dùng tên file sprite để xác định màu sắc. Ví dụ: "flower_red"
            return flowers.Peek().GetComponent<SpriteRenderer>().sprite.name;
        }
        return null;
    }

    /// <summary>
    /// Kiểm tra xem chậu hoa đã đầy chưa.
    /// </summary>
    public bool IsFull() {
        return flowers.Count >= vaseCapacity;
    }

    /// <summary>
    /// Kiểm tra xem chậu hoa có rỗng không.
    /// </summary>
    public bool IsEmpty() {
        return flowers.Count == 0;
    }

    /// <summary>
    /// Kiểm tra xem tất cả hoa trong chậu có cùng màu và đã đầy chưa.
    /// </summary>
    public bool IsCompleted() {
        if (flowers.Count < vaseCapacity)
        {
            return false;
        }

        string firstFlowerColor = GetTopFlowerColor();
        foreach (var flower in flowers)
        {
            if (flower.GetComponent<SpriteRenderer>().sprite.name != firstFlowerColor)
            {
                return false;
            }
        }

        return true;
    }
}