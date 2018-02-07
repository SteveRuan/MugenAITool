using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugenAITool
{
    class CharFilesInfo
    {
        // Files infomations
        public string defName, cmdName, cnsName, airName, stcommonName, charDirPath, charName;
        public List<string> stNames = new List<string>();


        // Read def file to look for related cmd, cns, st, stcommon and air files
        public void ReadDef()
        {
            string rl;
            StreamReader readFile = new StreamReader(charDirPath + defName);

            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                rl = rl.RemoveMugenComment();

                if (rl.Contains('='))                                                   // File definitions
                {
                    string fileType = rl.Substring(0, rl.IndexOf('=')).Trim(), fileName = rl.Substring(rl.IndexOf('=') + 1).Trim();

                    if (fileType.EqualsIgnoreCase("cmd")) cmdName = fileName;
                    if (fileType.EqualsIgnoreCase("cns")) cnsName = fileName;
                    if (fileType.Length >= 2 && fileType.Substring(0, 2).EqualsIgnoreCase("st") &&
                        !(fileType.ContainsIgnoreCase("stcommon"))) stNames.Add(fileName);
                    if (fileType.EqualsIgnoreCase("anim")) airName = fileName;
                    if (fileType.EqualsIgnoreCase("stcommon")) stcommonName = fileName;
                }
            }

            readFile.Close();
        }

        // Update CharFilesInfo after choose DEF file
        public void UpdateAfterChooseDefFile(string path)
        {
            charDirPath = path.Substring(0, path.LastIndexOf('\\') + 1);
            defName = path.Substring(path.LastIndexOf('\\') + 1);
            charName = path.Substring(path.LastIndexOf('\\') + 1, path.LastIndexOf('.') - path.LastIndexOf('\\') - 1);
            ReadDef();
        }
    }
}
