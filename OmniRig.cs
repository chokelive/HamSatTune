using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniRig;

namespace HamSatTune
{
    class OmniRig
    {
        private OmniRigX omniRig;
        private IRigX rig;
        private IRigX rig2;

        public OmniRig()
        {
            omniRig = new OmniRigX();
            
        }

        public void rigConnect()
        {
            rig = omniRig.Rig1; // Default at Rig1
        }

        public void rig2Connect()
        {
            rig2 = omniRig.Rig2; // Default at Rig1
        }

        public string rigType()
        {
            return rig.RigType;
        }

        public string rig2Type()
        {
            return rig2.RigType;
        }

        public string rigStatus()
        {
            return rig.Status.ToString();
        }

        public string rig2Status()
        {
            return rig2.Status.ToString();
        }

        public void OmniRigConfig()
        {
            omniRig.DialogVisible = true;
        }

        public void disConnectRig()
        {
            rig = null;
        }

        public void disConnectRig2()
        {
            rig2 = null;
        }

        public void setFreq(int freq)
        {
            rig.Freq = freq;
        }

        public void setFreq_Rig2(int freq)
        {
            rig2.Freq = freq;
        }

        public void setFreqA(int freq)
        {
            rig.FreqA = freq;
        }

        public void setFreqB(int freq)
        {
            rig.FreqB = freq;
        }

        public int getFreq()
        {
            return rig.Freq;
        }

        public int getFreqA()
        {
            return rig.FreqA;
        }

        public int getFreqB()
        {
            return rig.FreqB;
        }

        public void setModeFM()
        {
            rig.Mode = RigParamX.PM_FM;
        }

        public void setModeFM_Rig2()
        {
            rig2.Mode = RigParamX.PM_FM;
        }

        public void setModeLSB()
        {
            rig.Mode = RigParamX.PM_SSB_L;
        }

        public void setModeLSB_Rig2()
        {
            rig2.Mode = RigParamX.PM_SSB_L;
        }

        public void setModeUSB()
        {
            rig.Mode = RigParamX.PM_SSB_U;
        }

        public void setModeUSB_Rig2()
        {
            rig2.Mode = RigParamX.PM_SSB_U;
        }

        public void setModeUSBData()
        {
            rig.Mode = RigParamX.PM_DIG_U;
        }

        public void setModeUSBData_Rig2()
        {
            rig2.Mode = RigParamX.PM_DIG_U;
        }

        public void setModeLSBData()
        {
            rig.Mode = RigParamX.PM_DIG_L;
        }

        public void setModeLSBData_Rig2()
        {
            rig2.Mode = RigParamX.PM_DIG_L;
        }

        public void setModeCW()
        {
            rig.Mode = RigParamX.PM_CW_U;
        }

        public void setModeCW_Rig2()
        {
            rig2.Mode = RigParamX.PM_CW_U;
        }

        public void setModeCW_RX() // Setup RX USB for CW for wide bandwidth receive
        {
            rig.Mode = RigParamX.PM_SSB_U;
        }

        public void setSplit()
        {
            rig.Split = RigParamX.PM_SPLITON;
        }

        public void toggleVfo()
        {
            rig.Vfo = RigParamX.PM_VFOSWAP;
        }

        public void setVFOA()
        {
            rig.Vfo = RigParamX.PM_VFOA;
        }

        public void setVFOB()
        {
            rig.Vfo = RigParamX.PM_VFOB;
        }

        public bool getTxStatus()
        {
            if (rig.Tx == RigParamX.PM_TX)
                return true;
            else
                return false;
        }

        


    }
}
