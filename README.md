# HamSatTune
Amateur Radio Satellite Uplink and Downlink with doppler calculation by Choke E29AHU

Lasted:
[Version 1.2.0 Download](https://github.com/chokelive/HamSatTune/releases/download/V1.2.0/HamSatTune.V1.2.0.zip)

![image](https://github.com/chokelive/HamSatTune/assets/17312564/99920e7f-c206-4fbe-9041-7b5bd7414312)


โปรแกรมช่วยคำนวณค่าความถี่ Uplink และ Downlink ของดาวเทียมวิทยุสมัครเล่น โดยเฉพาะ Linear Satellite ที่การจูนหาค่าความถี่ค่อนข้างจะยุ่งยากเนื่องจาก เนื่องจาก Transponder ดาวเทียมวิทยุสมัครเล่นส่วนใหญ่จะเป็นชนิด Invert หมายถึงความถี่และโหมดของ Uplink จะวิ่งสวนทางกันกับความถี่ Downlink ดังนั้นโปรแกรมนี้จะช่วยทำหน้าที่คำนวณหาค่าความถี่และโหมดของ Uplink ให้อัตโนมัติ ตสใความถี่ downlink ที่เราเปลี่ยนแปลง 

และโปรแกรมยังใช้กับดาวเทียม FM ด้วงอื่นๆ ได้ด้วย ดูดาวเทียมที่ support ตอนนี้ได้ที่ไฟล์ https://raw.githubusercontent.com/chokelive/HamSatTune/main/Doppler.sqf

## วิทยุรุ่นที่ผ่านการทดสอบการใช้งาน
FT-817, IC-705
ถ้ามีรุ่นอื่นๆ จะต้องการทดสอบแจ้งเข้ามาได้ครับ

## วิธีการติดตั้ง
1. ดาวน์โหลดโปรแกรม version ล่าสุดจากหน้า Release https://github.com/chokelive/HamSatTune/releases
2. UnZip ไฟล์ ออกไปวางไว้ตาม Folder ที่ต้องการ ปกติจะวางไว้ที่ C:\HamSatTune
3. ดาวน์โหลดโปรแกรม Omnirig V1.2 จาก http://dxatlas.com/omnirig/ และติดตั้งให้เรียบร้อย เพื่อใช้ในการควบคุมวิทยุ
4. คอมพิวเตอร์ต้องติดตั้ง .NET Version 4.7.2 ไว้แล้วเป็นอย่างต่ำ ถ้าไม่มีเวลาเปิดโปรแกรมจะ error (ดาวน์โหลด .NET ได้ที่ https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472)
4. คลิกเปิดโปรแกรมและใช้งานได้เลยครับ

![image](https://github.com/chokelive/HamSatTune/assets/17312564/5d2db9a0-93ba-49ab-a4c3-208ae3ecd40b)


## วิธีการใช้งาน
1. เลือกดาวเทียมที่ต้องการใช้งานจากเมนู Satellite
2. คลิกปุ่ม Omnirig ตั้งค่าวิทยุให้เรียบร้อย ตามตัวอย่างของ FT-817 และ IC-705 ด้านล่าง

###FT817
![image](https://github.com/chokelive/HamSatTune/assets/17312564/e347c99f-cc15-4461-8dcc-3393a8d3e1fb)

4. หลังจากนั้นเปิดโปรแกรม HamSatTune ขึ้นมา เลือกดาวเทียมและโหมดตามที่ต้องการ หลังจากนั้นให้ติ๊กที่ช่อง Connect Radio
5. โปรแกรมจะทำการรับค่าความถี่จากวิทยุมาอัติโนมัติ และจะแสดงเป็นความถี่ downlink ตามความถี่ที่เราหมุนที่วิทยุ และจะคำนวณความถี่ Uplink กลับไปให้
6. กดปุ่ม Tune TX เพื่อส่งค่าความถี่ uplink กลับไปให้วิทยุ  หรือกด spacebare ที่คีย์บอร์ดก็ได้

หลังจากนั้น ก็เริ่มใช้งานติดต่อสื่อสารผ่านดาวเทียมวิทยุสมัครเล่นได้แล้วครับ

## หมายเหตุ:
โปรแกรมจะ Set ค่าอัตโนมัติให้กับวิทยุเป็นค่า default ตามนี้
- ตั้งค่า split mode
- VFO A คือ Downlink และ VFO B คือ Upink
- ทุกครั้งที่กด Tune TX หรือ Spacebar โปรแกรมจะสั่ง swtich VFO และป้อนค่าความถี่ Uplink ให้กับวิทยุ

73 de Choke E29AHU
