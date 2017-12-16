using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MugenAITool
{
    class AtkStorageManager
    {
        // Global variables
        private string defName = "", cmdName = "", cnsName = "", airName = "",
            rl, charDirPath, targetCSVFilePath;
        private List<string> stNames = new List<string>();                                  // List of names of state files mentioned in def file
        private List<int> stateNos = new List<int>();                                       // List of state numbers from cmd file
        private int groundFront = -1, airFront = -1;                                        // Ground.front and air.front
        private Dictionary<int, List<int>> stateProperties = new Dictionary<int, List<int>>();          // List of state properties, such as statetype, Juggle, attack_attribute and hitflag
        private Dictionary<int, List<int>> stateAnimDictionary = new Dictionary<int, List<int>>();      // Dictionary which match states and anims
        private Dictionary<int, List<List<int>>> animDatas = new Dictionary<int, List<List<int>>>();    // Atk anim number, distance X, Y and time
        private StreamReader readFile;
        private StreamWriter writeFile;

        public AtkStorageManager(string defPath, string targetCSVFilePath)
        {
            // Initialize global variables
            charDirPath = defPath.Substring(0, defPath.LastIndexOf('\\') + 1);
            defName = defPath.Substring(defPath.LastIndexOf('\\') + 1);
            this.targetCSVFilePath = targetCSVFilePath;
        }

        // Read def file to look for related cmd, st and air file
        public void AtkStorageMake()
        {
            ReadDef();
            ReadCns();
            ReadCmd();
            ReadSt();
            ReadAir();
            CreateCsvFile();
        }

        // Read def file to look for related cmd, st and air file
        private void ReadDef()
        {
            readFile = new StreamReader(charDirPath + defName);

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
                }
            }

            readFile.Close();
        }


        //Read cns file to look for the width of character
        private void ReadCns()
        {
            readFile = new StreamReader(charDirPath + cnsName);

            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                rl = rl.RemoveMugenComment();

                // Ground/air front width
                if (rl.ContainsIgnoreCase("ground.front")) groundFront = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                if (rl.ContainsIgnoreCase("air.front")) airFront = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());

                if (groundFront >= 0 && airFront >= 0) break;
            }

            readFile.Close();
        }

        //Read cmd file to find out all atk states
        private void ReadCmd()
        {
            // Variables
            int getValue = -1;
            bool getType = false;
            readFile = new StreamReader(charDirPath + cmdName);

            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                rl = rl.RemoveMugenComment();
                if (rl.Contains('[') && rl.EqualsIgnoreCase("[Statedef -1]")) break;    // Find [Statedef -1] and stop
            }
            
            // Search for all atk stateNos
            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                rl = rl.RemoveMugenComment();

                // Verify head of sctrl, type and value
                // Case 1: [State -N] or [Statedef -N]
                if (rl.Length > 9 && (rl.Substring(0, 9).EqualsIgnoreCase("[State -1") || 
                    rl.Substring(0, 9).EqualsIgnoreCase("[Statedef")))
                {
                    if (getType && getValue >= 0 && !stateNos.Contains(getValue)) stateNos.Add(getValue);
                }
                // Case 2: Type is "changestate" or "selfstate"
                else if (rl.Contains('=') && rl.Substring(0, rl.IndexOf('=')).Trim().EqualsIgnoreCase("type"))
                {
                    if (rl.Substring(rl.IndexOf('=') + 1).Trim().EqualsIgnoreCase("changestate") || 
                        rl.Substring(rl.IndexOf('=') + 1).Trim().EqualsIgnoreCase("selfstate"))
                        getType = true;
                }
                // Case 3: Value is non-negative integer
                else if (rl.Contains('=') && rl.Substring(0, rl.IndexOf('=')).Trim().EqualsIgnoreCase("value"))
                {
                    try
                    {
                        getValue = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                        // +++
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
        private void ReadSt()
        {
            foreach (string stName in stNames)
            {
                readFile = new StreamReader(charDirPath + stName);

                // Variables
                bool reaingHitdef = false;
                int stateNo = 0;

                for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
                {
                    rl = rl.RemoveMugenComment();
                    
                    if (rl.Contains("[Statedef") && !rl[10].Equals('-'))                            // Find [Statedef N] which N >= 0
                    {
                        stateNo = int.Parse(rl.Substring(10, rl.Length - 11).Trim());
                        if (stateNos.Contains(stateNo))
                        {
                            // Variables
                            //bool hitbyexisted;
                            int statetype = 0, juggle = -1;
                            
                            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
                            {
                                rl = rl.RemoveMugenComment();

                                // Loop for all properties
                                if (rl.Length > 4 && rl.Substring(0, 4).EqualsIgnoreCase("Anim"))
                                {
                                    List<int> stateData = new List<int>();
                                    stateData.Add(0);                                               // Add movement
                                    try
                                    {
                                        stateData.Add(int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim()));     // Add anim
                                        // +++ add mugen formula later
                                    }
                                    catch (Exception e)
                                    {
                                        stateData.Add(-1);
                                    }
                                    stateAnimDictionary.Add(stateNo, stateData);
                                }
                                if (rl.Length > 4 && rl.Substring(0, 4).EqualsIgnoreCase("Type"))
                                {
                                    // Statetype: S = 1, C = 2, A = 3, L = 4 (and U = 0)
                                    switch (rl.Substring(rl.IndexOf('=') + 1).Trim())
                                    {
                                        case "S":
                                            statetype = 1;
                                            break;
                                        case "C":
                                            statetype = 2;
                                            break;
                                        case "A":
                                            statetype = 3;
                                            break;
                                        case "L":
                                            statetype = 4;
                                            break;
                                    }
                                }
                                if (rl.Length > 6 && rl.Substring(0, 6).EqualsIgnoreCase("Juggle"))
                                {
                                    juggle = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                                }
                                if (rl.Length > 0 && rl[0] == '[') break;
                            }

                            // Put property list into stateProperties
                            List<int> propertiesList = new List<int>();
                            propertiesList.Add(statetype);
                            propertiesList.Add(juggle);
                            propertiesList.Add(0);                          // For attack attribute, start from 0
                            propertiesList.Add(0);                          // For combo available, start from 0
                            stateProperties.Add(stateNo, propertiesList);
                        }
                    }
                    if (stateNos.Contains(stateNo))
                    {
                        if (rl.Length > 0 && rl[0] == '[' && reaingHitdef) reaingHitdef = false;    // Reset reaingHitdef when we read new state controller

                        if (rl.ContainsIgnoreCase("type") && rl.ContainsIgnoreCase("hitdef"))
                        {
                            reaingHitdef = true;
                        }
                        if (rl.ContainsIgnoreCase("attr") && reaingHitdef)
                        {
                            switch (rl[rl.Length - 1])
                            {
                                case 'A':
                                    stateProperties[stateNo][2] |= 1;
                                    break;
                                case 'T':
                                    stateProperties[stateNo][2] |= 2;
                                    break;
                                case 'P':
                                    stateProperties[stateNo][2] |= 4;
                                    break;
                            }
                            stateProperties[stateNo][2] &= 7;                               // Remove the 4th bit which is helper
                        }
                        if (rl.ContainsIgnoreCase("hitflag") && reaingHitdef)
                        {
                            if (rl.Contains("+")) stateProperties[stateNo][3] = 1;          // Means can hit when emeny is hit
                            if (rl.Contains("-")) stateProperties[stateNo][3] = -1;         // Means can hit when emeny is not hit
                        }
                        if (rl.ContainsIgnoreCase("type") && rl.ContainsIgnoreCase("projectile"))
                        {
                            stateProperties[stateNo][2] |= 4;
                            stateProperties[stateNo][2] &= 7;                               // Remove the 4th bit which is helper
                        }
                        if (rl.ContainsIgnoreCase("type") && rl.ContainsIgnoreCase("helper") && (stateProperties[stateNo][2] & 7) == 0)
                        {
                            stateProperties[stateNo][2] |= 8;
                        }
                    }
                }
                
                readFile.Close();
            }
        }

        // Read air file
        private void ReadAir()
        {
            // Variables
            int airNo = 0, totalTime = 0, x1 = -999, x2 = 999, y1 = -999, y2 = 999;
            bool clsn1 = false;

            readFile = new StreamReader(charDirPath + airName);
            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                rl = rl.RemoveMugenComment();

                if (rl.ContainsIgnoreCase("[Begin Action"))                                     // If header of Begin Action
                {
                    // Total time 
                    if (animDatas.ContainsKey(airNo) && animDatas[airNo] != null) animDatas[airNo][0].Add(totalTime + 1);

                    // Reset all datas
                    airNo = int.Parse(rl.Substring(14, rl.Length - 15));
                    totalTime = 0;
                    x1 = -999;
                    x2 = 999;
                    y1 = -999;
                    y2 = 999;
                    clsn1 = false;
                } else if (Regex.Matches(rl, ",").Count >= 4) {                                 // If sprites and time

                    // Get attack persist time
                    for (int i = 0; i < 4; i += 1)
                    {
                        rl = rl.Substring(rl.IndexOf(',') + 1);
                    }
                    if (rl.Contains(',')) rl = rl.Substring(0, rl.IndexOf(','));
                    int persistTime = int.Parse(rl.Trim());

                    // Record datas if it has clsn1
                    if (clsn1)
                    {
                        if (!animDatas.Keys.Contains(airNo)) animDatas.Add(airNo, new List<List<int>>());
                        List<int> l = new List<int>();
                        l.Add(x1);
                        l.Add(x2);
                        l.Add(y1);
                        l.Add(y2);
                        l.Add(totalTime + 1);
                        l.Add(persistTime);
                        animDatas[airNo].Add(l);
                        clsn1 = false;
                    }

                    totalTime += persistTime;
                } else if (rl.ContainsIgnoreCase("Clsn1[")) {                                   // If hit frame
                    rl = rl.Substring(rl.IndexOf('=') + 1).Trim();
                    for (int i = 0; i < 4; i += 1)
                    {
                        int tmp;
                        if (rl.Contains(','))
                        {
                            tmp = int.Parse(rl.Substring(0, rl.IndexOf(',')).Trim());
                            rl = rl.Substring(rl.IndexOf(',') + 1);
                        }
                        else tmp = int.Parse(rl.Trim());

                        if (i % 2 == 0) {
                            if (tmp > x1) x1 = tmp;
                            if (tmp < x2) x2 = tmp;
                        } else {
                            if (tmp > y1) y1 = tmp;
                            if (tmp < y2) y2 = tmp;
                        }
                    }
                    clsn1 = true;
                }
            }

            // Total time 
            if (animDatas.ContainsKey(airNo) && animDatas[airNo] != null) animDatas[airNo][0].Add(totalTime + 1);

            readFile.Close();
        }

        // Combine and organize the data
        // Write into target CSV file
        private void CreateCsvFile()
        {
            // Create new directory if not exists
            string toolTempDirPath = charDirPath + "\\MugenAITool";
            if (!Directory.Exists(toolTempDirPath)) Directory.CreateDirectory(toolTempDirPath);

            // Create CSV file
            string csvName = targetCSVFilePath + ".csv";
            writeFile = new StreamWriter(csvName, false, Encoding.UTF8);

            // Write into target file
            writeFile.WriteLine("招式,StateNo,AnimNo,StateType,Juggle,AttackAttr,Hitflag,范围x1,范围x2,范围y1,范围y2,攻击帧,持续时间,总时长,");

            // loop for all statenos, find the anims they are using
            for (int i = 0; i < stateNos.Count; i += 1)
            {
                if (stateAnimDictionary.ContainsKey(stateNos[i]))
                {
                    List<int> animList = stateAnimDictionary[stateNos[i]];

                    // loop for all anims
                    for (int j = 0; j < animList.Count; j += 1)
                    {
                        if (animDatas.ContainsKey(animList[j]))
                        {
                            List<List<int>> stateDetails = animDatas[animList[j]];

                            // loop for all hits
                            for (int k = 0; k < stateDetails.Count; k += 1)
                            {
                                // StateNo, AnimNo and StateType
                                string writeStr = ',' + stateNos[i].ToString() + ',' + animList[j].ToString() + ',';

                                for (int l = 0; l < 4; l += 1)
                                {
                                    writeStr += stateProperties[stateNos[i]][l];
                                    writeStr += ',';
                                }

                                // loop for datas
                                int dataSize = 7;
                                for (int l = 0; l < dataSize; l += 1)
                                {
                                    if (l < stateDetails[k].Count)                                  // avoid null case
                                    {
                                        int value = stateDetails[k][l];

                                        // P2BodyDist X
                                        if (l <= 1)
                                        {
                                            // check on ground or in air, then minus related width
                                            if (stateProperties[stateNos[i]][0] == 3) {
                                                value -= airFront;
                                            } else {
                                                value -= groundFront;
                                            }
                                            value -= 1;
                                        }

                                        writeStr += value;
                                    }
                                    writeStr += ',';
                                }

                                writeFile.WriteLine(writeStr);
                            }
                        }
                    }
                }
            }

            writeFile.Close();

            // Open the CSV file
            Process.Start(csvName);
        }
    }
}

