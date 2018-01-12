using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MugenAITool
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void RH_Replace()
        {
            RH_T2.Text = RH_T1.Text;
            if (RH_X1.Text.Length > 0 && RH_V1.Text.Length > 0) RH_T2.Text = RH_T2.Text.Replace(RH_X1.Text, RH_V1.Text);
            if (RH_X2.Text.Length > 0 && RH_V2.Text.Length > 0) RH_T2.Text = RH_T2.Text.Replace(RH_X2.Text, RH_V2.Text);
            if (RH_X3.Text.Length > 0 && RH_V3.Text.Length > 0) RH_T2.Text = RH_T2.Text.Replace(RH_X3.Text, RH_V3.Text);
            if (RH_X4.Text.Length > 0 && RH_V4.Text.Length > 0) RH_T2.Text = RH_T2.Text.Replace(RH_X4.Text, RH_V4.Text);
            if (RH_X5.Text.Length > 0 && RH_V5.Text.Length > 0) RH_T2.Text = RH_T2.Text.Replace(RH_X1.Text, RH_V5.Text);
        }

        private void RH_B1_Click(object sender, EventArgs e)
        {
            RH_Replace();
        }

        private void AS_B1_Click(object sender, EventArgs e)
        {
            AtkStorageManager ASM = new AtkStorageManager(AS_T1.Text, AS_T2.Text);
            ASM.AtkStorageMake();
        }

        private void AS_B2_Click(object sender, EventArgs e)
        {
            OpenFileDialog readDef = new OpenFileDialog();
            if (readDef.ShowDialog() == DialogResult.OK)
            {
                string defPath = readDef.FileName;
                string charName = defPath.Substring(defPath.LastIndexOf('\\') + 1, defPath.LastIndexOf('.') - defPath.LastIndexOf('\\') - 1);
                AS_T1.Text = defPath;
                AS_T2.Text = defPath.Substring(0, defPath.LastIndexOf('\\')) + "\\MugenAITool\\" + charName + "AtkDatas";
            }
        }

        private void ASM_B1_Click(object sender, EventArgs e)
        {
            AISwitchManager ASM = new AISwitchManager(ASM_T1.Text, ASM_CB1.Text, ASM_NUD1.Text, ASM_R1.Checked, ASM_R2.Checked);
            ASM.AISwitchMake();
        }

        private void ASM_B2_Click(object sender, EventArgs e)
        {
            OpenFileDialog readCmd = new OpenFileDialog();
            if (readCmd.ShowDialog() == DialogResult.OK)
            {
                string cmdPath = readCmd.FileName;
                ASM_T1.Text = cmdPath;
            }
        }

        private void GM_B1_Click(object sender, EventArgs e)
        {
            GuardManager GM = new GuardManager(GM_T1.Text, GM_T2.Text, ASM_CB1.Text, ASM_NUD1.Text);
            GM.GuardCmdMake();
            GM.GuardStCommonMake();
        }

        private void GM_B2_Click(object sender, EventArgs e)
        {
            OpenFileDialog readCmd = new OpenFileDialog();
            if (readCmd.ShowDialog() == DialogResult.OK)
            {
                string cmdPath = readCmd.FileName;
                GM_T1.Text = cmdPath;
            }
        }

        private void GM_B3_Click(object sender, EventArgs e)
        {
            OpenFileDialog readStCommon = new OpenFileDialog();
            if (readStCommon.ShowDialog() == DialogResult.OK)
            {
                string stCommonPath = readStCommon.FileName;
                GM_T2.Text = stCommonPath;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label0_Click(object sender, EventArgs e)
        {

        }
    }
}
