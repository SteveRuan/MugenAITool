using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugenAITool
{
    class AtkStorageManager
    {
        // Variables
        private string defName = "", cmdName = "", airName = "", rl, dirRelativePath;
        private List<string> stNames = new List<string>();                                  // List of names of state files mentioned in def file
        private List<int> stateNos = new List<int>();                                       // List of state numbers from cmd file
        private List<int> animNos = new List<int>();                                        // List of anim numbers from cns file
        private Dictionary<int, List<int>> stateData = new Dictionary<int, List<int>>();    // Move distance and Anims
        private Dictionary<int, List<int>> animData = new Dictionary<int, List<int>>();     // Atk distance X, Y and time
        private StreamReader readFile;

        public AtkStorageManager(string fileName)
        {
            defName = fileName;
            dirRelativePath = fileName.Substring(0, fileName.LastIndexOf('\\') + 1);
        }

        // Read def file to look for related cmd, st and air file
        public void AtkStorageMake()
        {
            ReadDef();
            ReadCmd();
            // ReadSt();
            // ReadAir();
            // CreateCsvFile();
        }

        // Read def file to look for related cmd, st and air file
        private void ReadDef()
        {
            readFile = new StreamReader(defName);
            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                if (rl.IndexOf(';') >= 0) rl = rl.Substring(0, rl.IndexOf(';')).Trim();     // Remove comments
                if (rl.IndexOf('=') > 0)                                                    // File definitions
                {
                    string fileTpye = rl.Substring(0, rl.IndexOf('=')).Trim(), fileName = rl.Substring(rl.IndexOf('=') + 1).Trim();
                    if (fileTpye.Equals("cmd", StringComparison.CurrentCultureIgnoreCase)) cmdName = fileName;
                    if (fileTpye.Length >= 2 && fileTpye.Substring(0, 2).Equals("st", StringComparison.CurrentCultureIgnoreCase) && 
                        !(fileTpye.Length >= 8 && fileTpye.Substring(0, 8).Equals("stcommon", StringComparison.CurrentCultureIgnoreCase))) stNames.Add(fileName);
                    if (fileTpye.Equals("anim", StringComparison.CurrentCultureIgnoreCase)) airName = fileName;
                }
            }
            readFile.Close();
        }


        //Read cmd file to find out all atk states
        private void ReadCmd()
        {
            // Variables
            int getValue = -1;
            bool getType = false;
            readFile = new StreamReader(dirRelativePath + cmdName);

            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                if (rl.IndexOf(';') >= 0) rl = rl.Substring(0, rl.IndexOf(';')).Trim();     // Remove comments
                if (rl.IndexOf('[') >= 0 && rl.Equals("[Statedef -1]")) break;              // Find [Statedef -1] and stop
            }
            
            // Search for all atk stateNos
            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                if (rl.IndexOf(';') >= 0) rl = rl.Substring(0, rl.IndexOf(';')).Trim();     // Remove comments

                // Verify head of sctrl, type and value
                // Case 1: [State -N] or [Statedef -N]
                if (rl.Length > 9 && (rl.Substring(0, 9).Equals("[State -1", StringComparison.CurrentCultureIgnoreCase) || rl.Substring(0, 9).Equals("[Statedef", StringComparison.CurrentCultureIgnoreCase)))
                {
                    if (getType && getValue >= 0 && !stateNos.Contains(getValue)) stateNos.Add(getValue);
                }
                // Case 2: Type is "changestate" or "selfstate"
                else if (rl.IndexOf('=') >= 0 && rl.Substring(0, rl.IndexOf('=')).Trim().Equals("type", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (rl.Substring(rl.IndexOf('=') + 1).Trim().Equals("changestate", StringComparison.CurrentCultureIgnoreCase) || rl.Substring(rl.IndexOf('=') + 1).Trim().Equals("selfstate", StringComparison.CurrentCultureIgnoreCase))
                        getType = true;
                }
                // Case 3: Value is non-negative
                else if (rl.IndexOf('=') >= 0 && rl.Substring(0, rl.IndexOf('=')).Trim().Equals("value", StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        getValue = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                    } catch (Exception e) {
                        getValue = -1;
                    }
                }
            }
            // Reach the end of file
            if (getType && getValue >= 0 && !stateNos.Contains(getValue)) stateNos.Add(getValue);
            stateNos.Sort();

            readFile.Close();
        }

        // Read st file to find out related anim and movement for atk states
        private void Readst()
        {
            foreach (string stName in stNames)
            {
                // sctrl_anim, changestate, movement
                // Variables

                readFile = new StreamReader(dirRelativePath + stName);
                for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
                {
                    if (rl.IndexOf(';') >= 0) rl = rl.Substring(0, rl.IndexOf(';')).Trim();     // Remove comments
                    if (rl.IndexOf('[') >= 0 && rl.Length > 11 && rl.Substring(0, 9).Equals("[Statedef") && !rl[10].Equals('-'))   // Find [Statedef N] which N >= 0
                    {

                    }
                }

                // if (rl.Length > 9 && (rl.Substring(0, 9).Equals("[State -1", StringComparison.CurrentCultureIgnoreCase) || rl.Substring(0, 9).Equals("[Statedef", StringComparison.CurrentCultureIgnoreCase)))

                readFile.Close();
            }
        }

        /* Read air file

        // Combine and organize the data

        // Write into target file */

    }
}
