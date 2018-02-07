using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace MugenAITool
{
    public partial class Main : Form
    {
        // Global variables
        CharFilesInfo charFilesInfo = new CharFilesInfo();
        Dictionary<int, List<int>> atkStorageTable;


        public Main()
        {
            InitializeComponent();
            for (int i = 0; i < Mainpage_checkedList.Items.Count; i += 1)
            {
                Mainpage_checkedList.SetItemChecked(i, true);
            }
        }

        private void ReplaceHelper_Replace()
        {
            ReplaceHelper_resultText.Text = ReplaceHelper_templateText.Text;
            if (ReplaceHelper_X1.Text.Length > 0 && ReplaceHelper_V1.Text.Length > 0) ReplaceHelper_resultText.Text = ReplaceHelper_resultText.Text.Replace(ReplaceHelper_X1.Text, ReplaceHelper_V1.Text);
            if (ReplaceHelper_X2.Text.Length > 0 && ReplaceHelper_V2.Text.Length > 0) ReplaceHelper_resultText.Text = ReplaceHelper_resultText.Text.Replace(ReplaceHelper_X2.Text, ReplaceHelper_V2.Text);
            if (ReplaceHelper_X3.Text.Length > 0 && ReplaceHelper_V3.Text.Length > 0) ReplaceHelper_resultText.Text = ReplaceHelper_resultText.Text.Replace(ReplaceHelper_X3.Text, ReplaceHelper_V3.Text);
            if (ReplaceHelper_X4.Text.Length > 0 && ReplaceHelper_V4.Text.Length > 0) ReplaceHelper_resultText.Text = ReplaceHelper_resultText.Text.Replace(ReplaceHelper_X4.Text, ReplaceHelper_V4.Text);
            if (ReplaceHelper_X5.Text.Length > 0 && ReplaceHelper_V5.Text.Length > 0) ReplaceHelper_resultText.Text = ReplaceHelper_resultText.Text.Replace(ReplaceHelper_X1.Text, ReplaceHelper_V5.Text);
        }

        private void ReplaceHelper_replaceButton_Click(object sender, EventArgs e)
        {
            ReplaceHelper_Replace();
        }

        private void AtkStorage_createButton_Click(object sender, EventArgs e)
        {
            AtkStorageManager atkStorageManager = new AtkStorageManager(charFilesInfo, AtkStorage_csvText.Text);
            atkStorageManager.AtkStorageMake();

            // Open the CSV file
            Process.Start(AtkStorage_csvText.Text + ".csv");
        }

        private void AtkStorage_chooseDefButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog readDef = new OpenFileDialog();
            if (readDef.ShowDialog() == DialogResult.OK)
            {
                string defPath = readDef.FileName;
                string charName = defPath.Substring(defPath.LastIndexOf('\\') + 1, defPath.LastIndexOf('.') - defPath.LastIndexOf('\\') - 1);
                AtkStorage_defText.Text = defPath;
                AtkStorage_csvText.Text = defPath.Substring(0, defPath.LastIndexOf('\\')) + "\\MugenAITool\\" + charFilesInfo.charName + "AtkDatas";

                // Update information
                charFilesInfo.UpdateAfterChooseDefFile(defPath);
            }
        }

        private void AISwitch_addButton_Click(object sender, EventArgs e)
        {
            AISwitchManager ASM = new AISwitchManager(AISwitch_cmdText.Text, AISwitch_comboButton.Text, AISwitch_numberUpDownButton.Text, AISwitch_radioButton1.Checked, AISwitch_radioButton2.Checked);
            ASM.AISwitchMake();
        }

        private void AISwitch_chooseCmdButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog readCmd = new OpenFileDialog();
            if (readCmd.ShowDialog() == DialogResult.OK)
            {
                string cmdPath = readCmd.FileName;
                AISwitch_cmdText.Text = cmdPath;
            }
        }

        private void Guard_addButton_Click(object sender, EventArgs e)
        {
            GuardManager GM = new GuardManager(Guard_cmdText.Text, Guard_stCommonText.Text, AISwitch_comboButton.Text, AISwitch_numberUpDownButton.Text);
            GM.GuardCmdMake();
            GM.GuardStCommonMake();
        }

        private void Guard_chooseCmdButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog readCmd = new OpenFileDialog();
            if (readCmd.ShowDialog() == DialogResult.OK)
            {
                string cmdPath = readCmd.FileName;
                Guard_cmdText.Text = cmdPath;
            }
        }

        private void Guard_chooseStCommonButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog readStCommon = new OpenFileDialog();
            if (readStCommon.ShowDialog() == DialogResult.OK)
            {
                string stCommonPath = readStCommon.FileName;
                Guard_stCommonText.Text = stCommonPath;
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void Mainpage_createButton_Click(object sender, EventArgs e)
        {
            string tempCsvFilePath = charFilesInfo.charDirPath + charFilesInfo.charName + "temp";

            // Atk Storage collect datas from character
            AtkStorageManager atkStorageManager = new AtkStorageManager(charFilesInfo, tempCsvFilePath);
            atkStorageManager.AtkStorageMake();

            // Read CSV file 
            // atkStorageTable = 

            // Add mugen code into character

        }

        private void Mainpage_chooseDefButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog readDef = new OpenFileDialog();
            if (readDef.ShowDialog() == DialogResult.OK)
            {
                string defPath = readDef.FileName;
                Mainpage_defText.Text = defPath;

                // Update charFilesInfo
                charFilesInfo.UpdateAfterChooseDefFile(defPath);
            }
        }

    }
}
