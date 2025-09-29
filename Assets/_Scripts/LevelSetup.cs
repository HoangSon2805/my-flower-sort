using System.Collections.Generic;
using UnityEngine;

// Gắn script này vào GameManager
public class LevelSetup : MonoBehaviour {
    // Kéo các Prefab chậu hoa từ Hierarchy vào đây
    public List<VaseController> vases;

    // Kéo các Prefab bông hoa từ thư mục _Prefabs vào đây
    public List<GameObject> flowerPrefabs;

    void Start() {
        SetupLevel();
    }

    void SetupLevel() {
        // Định nghĩa level: mỗi số tương ứng với một màu hoa
        // Ví dụ: 0=đỏ, 1=vàng, 2=xanh. -1 là ô trống.
        List<int>[] levelData = new List<int>[]
        {
            new List<int> { 0, 1, 2, 0 }, // Dữ liệu cho chậu 1
            new List<int> { 1, 2, 1, 2 }, // Dữ liệu cho chậu 2
            new List<int> { 0, 2, 1, 0 }, // Dữ liệu cho chậu 3
            new List<int> { -1, -1, -1, -1 }, // Chậu 4 rỗng
            new List<int> { -1, -1, -1, -1 }  // Chậu 5 rỗng
        };

        for (int i = 0; i < vases.Count; i++)
        {
            foreach (int flowerIndex in levelData[i])
            {
                if (flowerIndex != -1)
                {
                    // Tạo ra một bông hoa mới từ Prefab
                    GameObject newFlower = Instantiate(flowerPrefabs[flowerIndex]);
                    // Thêm bông hoa vào chậu tương ứng
                    vases[i].AddFlower(newFlower);
                }
            }
        }
    }
}