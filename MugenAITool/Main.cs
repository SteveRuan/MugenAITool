using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MugenAITool
{
    public partial class Main : Form
    {
        // Global variables
        CharFilesInfo charFilesInfo = new CharFilesInfo();
        Dictionary<int, string> commandTriggers;
        List <List<int>> atkStorageTable;
        
        public Main()
        {
            InitializeComponent();
            for (int i = 0; i < Mainpage_checkedList.Items.Count; i += 1)
            {
                Mainpage_checkedList.SetItemChecked(i, true);
            }
        }

        private void SetLanguage(string Lang)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Lang);
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Main));
            LanguageHandler languageHandler = new LanguageHandler();

            languageHandler.ApplyResourcesControl(resources, this);
            languageHandler.ApplyResourcesCheckedListBox(resources, Mainpage_checkedList);
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

        private CharFilesInfo CreateNewFiles(CharFilesInfo charFilesInfo, string newAIFolderPath)
        {
            // Variables
            string oldDefPath = charFilesInfo.charDirPath + charFilesInfo.defName,
                newDefPath = oldDefPath.Substring(0, oldDefPath.LastIndexOf('.')) + "(MugenAITool)" + oldDefPath.Substring(oldDefPath.LastIndexOf('.')),
                rl;
            StreamReader readFile = new StreamReader(oldDefPath);
            StreamWriter writeFile = new StreamWriter(newDefPath, false, Encoding.UTF8);

            // Create new def file and modify it
            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                if (rl.Contains('='))                                                   // File definitions
                {
                    string fileType = rl.Substring(0, rl.IndexOf('=')).Trim(), fileName = rl.Substring(rl.IndexOf('=') + 1).Trim();
                    string oldFilePath = charFilesInfo.charDirPath + fileName.RemoveMugenComment();
                    string newFilePath = charFilesInfo.charDirPath + "MugenAITool\\" + fileName.RemoveMugenComment();

                    // Move the files which are going to be modified into new folder
                    if (fileType.EqualsIgnoreCase("cmd") || fileType.EqualsIgnoreCase("cns")
                        || fileType.EqualsIgnoreCase("anim") || fileType.EqualsIgnoreCase("stcommon")
                        || (fileType.Length >= 2 && fileType.Substring(0, 2).EqualsIgnoreCase("st") && !(fileType.ContainsIgnoreCase("stcommon"))))
                    {

                        // Create new directory if not exists
                        if (fileName.Contains('\\'))
                        {
                            string newDirPath = charFilesInfo.charDirPath + "MugenAITool\\" + fileName.Substring(0, fileName.LastIndexOf('\\') + 1);
                            if (!Directory.Exists(newDirPath)) Directory.CreateDirectory(newDirPath);
                        }

                        if (File.Exists(newFilePath)) File.Delete(newFilePath);         // Remove files if they already exists
                        // If stcommon doesn't exist and it is common1.cns, then use the default common1.cns in data folder
                        if (!File.Exists(oldFilePath) && fileType.EqualsIgnoreCase("stcommon") && fileName.RemoveMugenComment().EqualsIgnoreCase("common1.cns"))
                        {
                            oldFilePath = charFilesInfo.charDirPath.Substring(0, charFilesInfo.charDirPath.IndexOf("char")) + "data\\common1.cns";
                        }
                        File.Copy(oldFilePath, newFilePath);

                        writeFile.WriteLine(fileType + " = MugenAITool\\" + fileName);

                    }
                    else
                    {
                        writeFile.WriteLine(rl);
                    }
                }
                else
                {
                    writeFile.WriteLine(rl);
                }
            }

            readFile.Close();
            writeFile.Close();

            // Update charFilesInfo and return it
            charFilesInfo.defName = newDefPath;
            charFilesInfo.cmdName = "MugenAITool\\" + charFilesInfo.cmdName;
            charFilesInfo.cnsName = "MugenAITool\\" + charFilesInfo.cnsName;
            charFilesInfo.airName = "MugenAITool\\" + charFilesInfo.airName;
            charFilesInfo.stcommonName = "MugenAITool\\" + charFilesInfo.stcommonName;
            for (int i = 0; i < charFilesInfo.stNames.Count; i += 1)
            {
                charFilesInfo.stNames[i] = "MugenAITool\\" + charFilesInfo.stNames[i];
            }
            return charFilesInfo;
        }

        private List<List<int>> ReadCSVfile(string CSVFilePath)
        {
            List<List<int>> table = new List<List<int>>();
            StreamReader readFile = new StreamReader(CSVFilePath);
            string rl = readFile.ReadLine();

            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                List<int> line = new List<int>();

                for (; rl.Length > 1 && rl.Contains(','); rl = rl.Substring(rl.IndexOf(',') + 1))
                {
                    line.Add(int.Parse(rl.Substring(0, rl.IndexOf(','))));
                }

                table.Add(line);
            }

            readFile.Close();
            return table;
        }

        private void AddAISwitchToOriginalCommands(string cmdFilePath)
        {
            // +++
        }

        // ==============================================================================================

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

            // Show message box after finished
            // MessageBox.Show("CSV file is created successfully.", "Mugen AI Tool", MessageBoxButtons.OK);
        }

        private void AtkStorage_chooseDefButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog readDef = new OpenFileDialog();
            if (readDef.ShowDialog() == DialogResult.OK)
            {
                string defPath = readDef.FileName;
                string charName = defPath.Substring(defPath.LastIndexOf('\\') + 1, defPath.LastIndexOf('.') - defPath.LastIndexOf('\\') - 1);

                // Update information
                charFilesInfo.UpdateAfterChooseDefFile(defPath);

                AtkStorage_defText.Text = defPath;
                AtkStorage_csvText.Text = defPath.Substring(0, defPath.LastIndexOf('\\')) + "\\MugenAITool\\" + charFilesInfo.charName + "AtkDatas";
            }
        }

        private void AISwitch_addButton_Click(object sender, EventArgs e)
        {
            AISwitchManager AISM = new AISwitchManager(AISwitch_cmdText.Text, AISwitch_comboButton.Text, AISwitch_numberUpDownButton.Text, AISwitch_radioButton1.Checked, AISwitch_radioButton2.Checked);
            AISM.AISwitchMake();
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
            // Update charFilesInfo
            charFilesInfo.UpdateAfterChooseDefFile(Mainpage_defText.Text);

            // Create new directory if not exists
            string newAIFolderPath = charFilesInfo.charDirPath + "MugenAITool\\";
            if (!Directory.Exists(newAIFolderPath)) Directory.CreateDirectory(newAIFolderPath);

            // Create new def, cmd, cns, st, stcommon and air files, and update charFilesInfo
            charFilesInfo = CreateNewFiles(charFilesInfo, newAIFolderPath);

            // Atk Storage collect datas from character
            string tempCsvFilePath = newAIFolderPath + charFilesInfo.charName + "temp";
            AtkStorageManager ASM = new AtkStorageManager(charFilesInfo, tempCsvFilePath);
            ASM.AtkStorageMake();

            // Read cmd file to record command triggers
            commandTriggers = ASM.commandTriggers;

            // Read CSV file and remove temporary CSV file
            atkStorageTable = ReadCSVfile(tempCsvFilePath + ".csv");
            File.Delete(tempCsvFilePath + ".csv");

            // Add AI switch to original commands to avoid AI move randomly
            AddAISwitchToOriginalCommands(charFilesInfo.charDirPath + charFilesInfo.cmdName);

            // Add mugen code into character, in reversed order
            // Guard part
            if (Mainpage_checkedList.GetItemChecked(7))
            {
                GuardManager GM = new GuardManager(charFilesInfo.charDirPath + charFilesInfo.cmdName, charFilesInfo.charDirPath + charFilesInfo.stcommonName,
                     AISwitch_comboButton.Text, AISwitch_numberUpDownButton.Text);
                GM.GuardCmdMake();
                GM.GuardStCommonMake();
            }

            // Neutral part
            if (Mainpage_checkedList.GetItemChecked(6))
            {
                //NeutralManager NM = new NeutralManager(charFilesInfo.charDirPath + charFilesInfo.cmdName);
                //NM.NeutralMake(atkStorageTable, commandTriggers);
            }

            // AI switch part
            if (Mainpage_checkedList.GetItemChecked(0))
            {
                AISwitchManager AISM = new AISwitchManager(charFilesInfo.charDirPath + charFilesInfo.cmdName,
                AISwitch_comboButton.Text, AISwitch_numberUpDownButton.Text, AISwitch_radioButton1.Checked, AISwitch_radioButton2.Checked);
                AISM.AISwitchMake();
            }

            // Popup message box after finished
            MessageBox.Show("Mugen AI is created successfully.", "Mugen AI Tool", MessageBoxButtons.OK);
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

        private void EnglishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage("");
        }

        private void SimplifiedChineseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage("zh-Hans");
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

    }
}
