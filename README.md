# 🚀 Game: Spaceship Vs Asteroids

## 🎮 Thể loại  
2D Unity – Bắn thiên thạch (Side-scrolling Shooter)

## 🖥️ Tỷ lệ màn hình  
- **Tỷ lệ khung hình:** 16:9  
- **Giao diện nằm ngang (Landscape)**

---

## 🌌 Bối cảnh & Bố cục

- **Phi thuyền** nằm **ở phía trái** màn hình.
- **Thiên thạch** xuất hiện **ngẫu nhiên ở bên phải**, **bay theo hướng vị trí hiện tại của phi thuyền**.
- **Background:** Vũ trụ với hiệu ứng **cuộn vô tận (endless scroll)**.

---

## 🧑‍🚀 Điều khiển Phi thuyền

- Di chuyển: **Lên / Xuống / Trái / Phải**
- Tấn công: **Bắn đạn thẳng về phía trước**

---

## 🪨 Thiên Thạch

- Spawn ngẫu nhiên ở phía phải màn hình.
- Bay theo **hướng của phi thuyền tại thời điểm spawn**.
- Nếu **bị bắn trúng** → Bị phá hủy → **+1 điểm**
- Nếu **va chạm vào phi thuyền** → **Trừ 1 mạng**
  - Sau **3 lần va chạm**, **Game Over**.

---

## ⭐ Ngôi Sao

- Xuất hiện ngẫu nhiên sau mỗi khoảng thời gian nhất định.
- Nếu phi thuyền **chạm vào** → **+3 điểm**

---

## 🧭 Giao diện Chính Trong Game

- **Góc trên trái:** Điểm số hiện tại.
- **Giữa trên:** Thời gian sinh tồn (đếm tăng).
- **Góc trên phải:** Nút **Pause Game**.

---

## 🏁 Màn hình chính (Main Menu)

- `Start Game` – Bắt đầu chơi
- `Quit Game` – Thoát khỏi game

---

## ☠️ Game Over

Sau khi phi thuyền **bị va chạm 3 lần**:
![image](https://github.com/user-attachments/assets/4c609d50-7adc-44b5-ad42-fa6fd649325a)

- Hiển thị:
  - **Tổng điểm**
  - **Thời gian sống sót**
  - Nút `Back to Menu`
## 🖼 Gameplay Image
![image](https://github.com/user-attachments/assets/42836afc-0dfa-408c-b73b-c5f0493f8efd)
![image](https://github.com/user-attachments/assets/24b4ed5b-9327-41e8-9755-4c0c919d39a2)
- Easy mode
![image](https://github.com/user-attachments/assets/a93e07b7-da46-49b3-b3d6-e75fe39a94cd)
- Medium mode
![image](https://github.com/user-attachments/assets/48ef4523-cf91-4ff9-9d13-d3862e5a4bfc)
- Hard mode
![image](https://github.com/user-attachments/assets/370d3eb9-df7f-43a7-9002-d2aac2ce0f16)

