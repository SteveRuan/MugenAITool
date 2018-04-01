using System.IO;
using System.Linq;

namespace MugenAITool
{
    class GuardManager
    {
        // Global variables
        private string cmdPath = "", stCommonPath = "", variableType, variableNo;

        public GuardManager(string cmdPath, string stCommonPath, string variableType, string variableNo)
        {
            // Initialize global variables
            this.cmdPath = cmdPath;
            this.stCommonPath = stCommonPath;
            this.variableType = variableType;
            this.variableNo = variableNo;
        }

        public void GuardCmdMake()
        {
            string rl;
            StreamReader readFile = new StreamReader(cmdPath);
            StreamWriter writeFile = new StreamWriter(cmdPath + ".tmp");

            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                // 
                writeFile.WriteLine(rl);
                rl = rl.RemoveMugenComment();

                // Add guard changestate
                if (rl.Contains('[') && rl.EqualsIgnoreCase("[Statedef -1]"))
                {
                    StreamReader readTemplate = new StreamReader("MugenTemplate/GuardCmd.st");
                    for (rl = readTemplate.ReadLine(); rl != null; rl = readTemplate.ReadLine())
                    {
                        if (rl.ContainsIgnoreCase("{VariableType}"))
                        {
                            rl = rl.Replace("{VariableType}", variableType);
                        }

                        if (rl.ContainsIgnoreCase("{VariableNo}"))
                        {
                            rl = rl.Replace("{VariableNo}", variableNo);
                        }

                        if (rl.ContainsIgnoreCase("{CanAirGuard}"))
                        {
                            rl = rl.Replace("{CanAirGuard}", "");
                            // rl_tmp = rl_tmp.Replace("{CanAirGuard}", "Null ;");
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

        public void GuardStCommonMake()
        {
            bool skipGuardStateDef = false;
            string rl, rl_tmp;
            StreamReader readFile = new StreamReader(stCommonPath);
            StreamWriter writeFile = new StreamWriter(stCommonPath + ".tmp");

            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                rl_tmp = rl.RemoveMugenComment();
                if (rl_tmp.Contains('[') && rl_tmp.EqualsIgnoreCase("[Statedef 120]"))
                {
                    skipGuardStateDef = true;                       //
                }
                else if (rl_tmp.Contains('[') && rl_tmp.ContainsIgnoreCase("[Statedef") && skipGuardStateDef)
                {
                    int stateNo = int.Parse(rl_tmp.Substring((rl_tmp.IndexOf(' ') + 1), 3));
                    if (stateNo != 120 && !(stateNo >= 130 && stateNo <= 132) && stateNo != 140 && !(stateNo >= 150 && stateNo <= 155))
                    {
                        // Add guard states
                        StreamReader readTemplate = new StreamReader("MugenTemplate/GuardCommon.st");
                        for (rl_tmp = readTemplate.ReadLine(); rl_tmp != null; rl_tmp = readTemplate.ReadLine())
                        {
                            if (rl_tmp.ContainsIgnoreCase("{VariableType}"))
                            {
                                rl_tmp = rl_tmp.Replace("{VariableType}", variableType);
                            }

                            if (rl_tmp.ContainsIgnoreCase("{VariableNo}"))
                            {
                                rl_tmp = rl_tmp.Replace("{VariableNo}", variableNo);
                            }

                            writeFile.WriteLine(rl_tmp);
                        }

                        readTemplate.Close();

                        // 
                        skipGuardStateDef = false;
                    }
                }

                // 
                if (!skipGuardStateDef) writeFile.WriteLine(rl);
            }

            readFile.Close();
            writeFile.Close();

            //
            File.Replace((stCommonPath + ".tmp"), stCommonPath, (stCommonPath + ".backup"));
        }
    }
}
