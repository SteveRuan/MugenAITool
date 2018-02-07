using System.IO;
using System.Linq;

namespace MugenAITool
{
    class AISwitchManager
    {
        // Global variables
        private string cmdPath = "", variableType, variableNo;
        private int checkedRadioButton = 0;

        public AISwitchManager(string cmdPath, string variableType, string variableNo, bool alwaysChecked, bool AIlevelExistsChecked)
        {
            // Initialize global variables
            this.cmdPath = cmdPath;
            this.variableType = variableType;
            this.variableNo = variableNo;
            if (alwaysChecked) checkedRadioButton = 1;
            else checkedRadioButton = 2;
        }

        public void AISwitchMake()
        {
            string rl;
            StreamReader readFile = new StreamReader(cmdPath);
            StreamWriter writeFile = new StreamWriter(cmdPath + ".tmp");
            
            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                // 
                writeFile.WriteLine(rl);
                rl = rl.RemoveMugenComment();
                
                // Add AI switch 
                if (rl.Contains('[') && rl.EqualsIgnoreCase("[Statedef -1]"))
                {
                    StreamReader readTemplate = new StreamReader("MugenTemplate/AISwitchTemplate.st");
                    for (rl = readTemplate.ReadLine(); rl != null; rl = readTemplate.ReadLine())
                    {
                        if (rl.ContainsIgnoreCase("{Condition}"))
                        {
                            if (checkedRadioButton == 1) rl = rl.Replace("{Condition}", "1");
                            else rl = rl.Replace("{Condition}", "AIlevel");
                        }

                        if (rl.ContainsIgnoreCase("{VariableType}"))
                        {
                            rl = rl.Replace("{VariableType}", variableType);
                        }

                        if (rl.ContainsIgnoreCase("{VariableNo}"))
                        {
                            rl = rl.Replace("{VariableNo}", variableNo);
                        }

                        writeFile.WriteLine(rl);
                    }

                    readTemplate.Close();
                }
            }
            
            readFile.Close();
            writeFile.Close();

            //
            File.Replace((cmdPath + ".tmp"), cmdPath, (cmdPath + ".backup"));
        }
    }
}