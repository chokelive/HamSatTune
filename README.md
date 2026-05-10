# 🚀 HamSatTune  
Amateur Radio Satellite Uplink and Downlink with Doppler Calculation  
by Choke E29AHU  

---

## 📥 Latest Release
👉 [Download Latest Version](https://github.com/chokelive/HamSatTune/releases/latest)

---

## 🖼️ Preview
<img width="1163" height="783" alt="image" src="https://github.com/user-attachments/assets/6436dd57-79e9-4772-b4e1-69a26c019ccd" />


---

## 📡 Overview
โปรแกรมช่วยคำนวณค่าความถี่ **Uplink และ Downlink** ของดาวเทียมวิทยุสมัครเล่น โดยเฉพาะ **Linear Satellite** ซึ่งการจูนหาค่าความถี่ค่อนข้างยุ่งยาก  

เนื่องจาก Transponder ของดาวเทียมส่วนใหญ่เป็นชนิด **Invert** หมายถึงความถี่และโหมดของ Uplink จะวิ่งสวนทางกับ Downlink  

โปรแกรมนี้จะช่วยคำนวณค่าความถี่และโหมดของ **Uplink ให้อัตโนมัติ** ตามความถี่ Downlink ที่ผู้ใช้ปรับเปลี่ยน  

รองรับการใช้งานกับ **ดาวเทียม FM รุ่นอื่น ๆ** ได้ด้วย  

📄 Satellite list:  
https://raw.githubusercontent.com/chokelive/HamSatTune/main/Doppler.sqf  

---

## 📻 Tested Radios
- Yaesu FT-817, FT-897
- Icom IC-705, IC-756 Pro, IC-9700


> หากมีรุ่นอื่น ๆ และต้องการทดสอบ สามารถแจ้งเข้ามาได้ครับ  
---

## ⚙️ Installation

1. ดาวน์โหลดโปรแกรมจากหน้า Release  
   👉 https://github.com/chokelive/HamSatTune/releases  

2. แตกไฟล์ (Unzip) ไปยังโฟลเดอร์ที่ต้องการ  
   (แนะนำ: `C:\HamSatTune`)  

3. ติดตั้ง **OmniRig V1.2** (ใช้สำหรับควบคุมวิทยุ)  
   👉 http://dxatlas.com/omnirig/  

4. ติดตั้ง **.NET Framework 4.7.2 หรือสูงกว่า**  
   👉 https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472  

5. เปิดโปรแกรมและเริ่มใช้งานได้เลย  

---

## ▶️ Usage

1. เลือกดาวเทียมจากเมนู **Satellite**  
2. ตั้งค่าวิทยุผ่าน **OmniRig**  
3. เลือกโหมดและค่าที่ต้องการ  
4. ติ๊ก **Connect Radio**  
5. โปรแกรมจะคำนวณ Uplink และ Downlink ให้อัตโนมัติ  

---

## ⚡ Features
- ✅ Automatic Doppler calculation  
- ✅ Auto Uplink frequency tracking  
- ✅ Supports Linear & FM satellites  
- ✅ Radio control via OmniRig  
- ✅ Manual RX mode  

---

## 🧠 Default Behavior

โปรแกรมจะตั้งค่าอัตโนมัติดังนี้:

- Split Mode เปิดใช้งาน  
- VFO A = Downlink  
- VFO B = Uplink  

### 🔹 IC-705
- ควบคุม VFO A/B ได้โดยตรง  
- ไม่จำเป็นต้องกด Tune TX  

### 🔹 Manual RX Mode
- ป้อนความถี่ RX เช่น `436500`  
- โปรแกรมจะคำนวณ Doppler ของ RX และ TX ให้  

> ⚠️ หมายเหตุ: ฟังก์ชันกด Spacebar เพื่อสลับ VFO (FT-817) ได้ถูกยกเลิกแล้ว  

---

## 📬 Contact
📧 e29ahu@gmail.com  

---

## 🙏 Thanks
ขอบคุณที่ใช้งานครับ  

**73 de Choke E29AHU**
