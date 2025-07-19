# 🏛️ UAS-VR03 - Labirin Buta

Proyek Final untuk mata kuliah Virtual Reality (VR03).

---

## 📖 Deskripsi

Diadaptasi dari game klasik "Honeycomb Maze", **Labirin Buta** adalah sebuah game *puzzle-thriller* dengan elemen *stealth* yang dibangun menggunakan Unity untuk platform VR. Pemain terjebak dalam sebuah labirin raksasa berbentuk sarang lebah dan hanya dapat melihat satu ruangan pada satu waktu karena perspektif yang terbatas.

Tujuan utama pemain adalah menemukan jalan keluar sambil menghindari berbagai rintangan seperti pintu yang salah, jebakan, dan Penjaga (Guardian) mematikan yang terus berpatroli. Game ini menantang daya ingat, strategi, kesabaran, dan keberanian pemain dalam menghadapi ketidakpastian.

---

## ✨ Fitur

### Gameplay
*   **Navigasi Labirin Buta:** Pemain harus menjelajahi labirin ruangan demi ruangan, mengandalkan ingatan dan petunjuk suara untuk membuat peta mental.
*   **Pilihan & Konsekuensi:** Setiap pintu adalah sebuah pilihan yang bisa berujung pada kemajuan, jalan buntu, atau jebakan mematikan.
*   **Ancaman Penjaga (Guardian):** Karakter AI penjaga berpatroli di labirin. Pemain harus bergerak diam-diam untuk menghindari deteksi, karena tertangkap berarti gagal.

### Teknis
*   **Interaksi Berbasis Raycast:** Interaksi dengan objek (seperti pintu) menggunakan *raycast* dari controller VR.
*   **AI Penjaga (Guard AI):** Implementasi AI sederhana untuk perilaku patroli penjaga.
*   **Sistem Pintu Interaktif:** Logika untuk mengelola status dan interaksi beberapa pintu di dalam labirin.

---

## 🚀 Cara Menjalankan Proyek

1.  Pastikan Anda telah menginstal [Unity Hub](https://unity.com/download).
2.  Instal versi Unity Editor yang sesuai (periksa `ProjectSettings/ProjectVersion.txt` untuk versi yang tepat).
3.  Clone atau unduh repositori ini.
4.  Buka Unity Hub, klik "Add project from disk", dan pilih folder root proyek ini.
5.  Setelah proyek dimuat di Unity Editor, buka scene utama (misalnya, di dalam folder `Assets/Scenes`).
6.  Klik tombol "Play" untuk menjalankan simulasi di Editor, atau build proyek untuk headset VR target Anda.

---

## 💻 Struktur Skrip

Proyek ini berisi beberapa skrip C# utama untuk fungsionalitas game:

*   `PlayerRaycast.cs`: Menangani input pemain dan interaksi berbasis raycast.
*   `Door.cs` & `DoorController.cs`: Mengelola logika untuk pintu yang dapat dioperasikan.
*   `GuardAI.cs`: Mengimplementasikan perilaku untuk karakter AI penjaga.

---

## ✍️ Author

*   **Nama:** Husnu Mulyadi
*   **NIM:** 24120300013
*   **Nama:** Febri Andri Yanto
*   **NIM:** 24130300002