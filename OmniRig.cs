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

        public OmniRig()
        {
            omniRig = new OmniRigX();
            
        }

        public void rigConnect()
        {
            rig = omniRig.Rig1; // Default at Rig1
        }

        public void OmniRigConfig()
        {
            omniRig.DialogVisible = true;
        }

        public void disConnectRig()
        {
            rig = null;
        }

        public void setFreq(int freq)
        {
            rig.Freq = freq;
        }

        public int getFreq()
        {
            return rig.Freq;
        }

        public void setModeFM()
        {
            rig.Mode = RigParamX.PM_FM;
        }

        public void setModeLSB()
        {
            rig.Mode = RigParamX.PM_SSB_L;
        }

        public void setModeUSB()
        {
            rig.Mode = RigParamX.PM_SSB_U;
        }

        public void setModeCW()
        {
            rig.Mode = RigParamX.PM_CW_U;
        }

        public void setSplit()
        {
            rig.Split = RigParamX.PM_SPLITON;
        }

        public void toggleVfo()
        {
            rig.Vfo = RigParamX.PM_VFOSWAP;
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
