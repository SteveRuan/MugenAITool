using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugenAITool
{
    class NeutralManager
    {
        // Global variables
        private string cmdPath = "";
        
        public NeutralManager(string cmdPath)
        {
            // Initialize global variables
            this.cmdPath = cmdPath;
        }

        public void NeutralMake(List<List<int>> atkStorageTable, Dictionary<int, string> commandTriggers)
        {
            // 
            // atkStorageTable.Sort();

            string rl;
            StreamReader readFile = new StreamReader(cmdPath);
            StreamWriter writeFile = new StreamWriter(cmdPath + ".tmp");
            
            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                // 
                writeFile.WriteLine(rl);
                rl = rl.RemoveMugenComment();

                // Add neutral changestate
                if (rl.Contains('[') && rl.EqualsIgnoreCase("[Statedef -1]"))
                {
                    StreamReader readTemplate = new StreamReader("MugenTemplate/GuardCmd.st");




                    for (rl = readTemplate.ReadLine(); rl != null; rl = readTemplate.ReadLine())
                    {
                        if (rl.ContainsIgnoreCase("{VariableType}"))
                        {
                            rl = rl.Replace("{VariableType}", variableType);
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
