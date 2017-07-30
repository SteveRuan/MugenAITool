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

        private void replace()
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
            replace();
        }
        
        private void RH_B2_Click(object sender, EventArgs e)
        {
            OpenFileDialog readDef = new OpenFileDialog();
            if (readDef.ShowDialog() == DialogResult.OK)
            {
                string defName = readDef.FileName;
                AS_T1.Text = defName;
                AS_T2.Text = defName.Substring(0, defName.LastIndexOf('.'));
            }
        }

        private void AS_B1_Click(object sender, EventArgs e)
        {
            AtkStorageManager ASM = new AtkStorageManager(AS_T1.Text);
            ASM.ReadDef();
            ASM.ReadCmd();
            // ASM.ReadSt();
            // ASM.ReadAir();
            // ASM.CreateCsvFile();
        }
    }
}
