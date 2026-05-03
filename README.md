# HamSatTune
Amateur Radio Satellite Uplink and Downlink with doppler calculation by Choke E29AHU

Lasted:
[Version 1.7.0 Download](https://github.com/chokelive/HamSatTune/releases/download/V1.7.0/HamSatTune.V1.7.0.zip)

<img width="1206" height="844" alt="image" src="https://github.com/user-attachments/assets/c390dc19-81d9-4711-985f-75a061df2266" />



โปรแกรมช่วยคำนวณค่าความถี่ Uplink และ Downlink ของดาวเทียมวิทยุสมัครเล่น โดยเฉพาะ Linear Satellite ซึ่งการจูนหาค่าความถี่ค่อนข้างยุ่งยาก เนื่องจาก Transponder ของดาวเทียมวิทยุสมัครเล่นส่วนใหญ่เป็นชนิด Invert หมายถึงความถี่และโหมดของ Uplink จะวิ่งสวนทางกับความถี่ Downlink ดังนั้นโปรแกรมนี้จะช่วยคำนวณค่าความถี่และโหมดของ Uplink ให้อัตโนมัติ ตามความถี่ Downlink ที่เราเปลี่ยนแปลง

และโปรแกรมยังสามารถใช้กับดาวเทียม FM รุ่นอื่น ๆ ได้ด้วย โดยสามารถดูรายการดาวเทียมที่รองรับได้ที่ไฟล์:
https://raw.githubusercontent.com/chokelive/HamSatTune/main/Doppler.sqf

วิทยุรุ่นที่ผ่านการทดสอบการใช้งาน

FT-817, IC-705
หากมีรุ่นอื่น ๆ และต้องการทดสอบ สามารถแจ้งเข้ามาได้ครับ

วิธีการติดตั้ง
ดาวน์โหลดโปรแกรมเวอร์ชันล่าสุดจากหน้า Release:
https://github.com/chokelive/HamSatTune/releases
แตกไฟล์ (Unzip) แล้วนำไปวางไว้ในโฟลเดอร์ที่ต้องการ (แนะนำ C:\HamSatTune)
ดาวน์โหลดโปรแกรม Omnirig V1.2 จาก:
http://dxatlas.com/omnirig/
 และติดตั้งให้เรียบร้อย เพื่อใช้ควบคุมวิทยุ
คอมพิวเตอร์ต้องติดตั้ง .NET Framework เวอร์ชัน 4.7.2 ขึ้นไป หากไม่มี โปรแกรมจะไม่สามารถเปิดใช้งานได้
ดาวน์โหลดได้ที่: https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472
คลิกเปิดโปรแกรมและเริ่มใช้งานได้เลยครับ
วิธีการใช้งาน
เลือกดาวเทียมที่ต้องการใช้งานจากเมนู Satellite
คลิกปุ่ม Omnirig และตั้งค่าวิทยุให้เรียบร้อย (ตามตัวอย่างของ FT-817 และ IC-705 ด้านล่าง)

FT-817

เลือกดาวเทียมและโหมดตามต้องการ จากนั้นติ๊กที่ช่อง Connect Radio
โปรแกรมจะคำนวณค่า Uplink และ Downlink ให้อัตโนมัติ ที่เหลือก็ใช้งานได้เลยครับ
หมายเหตุ

โปรแกรมจะตั้งค่าอัตโนมัติให้กับวิทยุเป็นค่า default ดังนี้

ตั้งค่า Split Mode
VFO A = Downlink และ VFO B = Uplink
สำหรับ FT-817: ทุกครั้งที่กด Spacebar โปรแกรมจะสลับ VFO และป้อนค่าความถี่ Uplink ให้กับวิทยุ --> ฟังก์ชั่นนี้เอาออกไปแล้วครับ
สำหรับ IC-705: โปรแกรมสามารถป้อนค่าความถี่ผ่าน VFO A และ VFO B ได้โดยตรง ดังนั้นไม่จำเป็นต้องกดปุ่ม Tune TX ขณะใช้งาน
ปุ่ม Manual RX ใช้ในกรณีที่ไม่ได้เชื่อมต่อวิทยุกับโปรแกรม แต่ต้องการให้โปรแกรมคำนวณ Doppler โดยสามารถป้อนความถี่ RX เช่น 436500 แล้วโปรแกรมจะคำนวณค่า Doppler ของ RX และ TX ให้

หากมีข้อสงสัยหรือต้องการติดต่อ สามารถสอบถามได้ที่
e29ahu@gmail.com

ขอบคุณครับ
73 de Choke E29AHU
