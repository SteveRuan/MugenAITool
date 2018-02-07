using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MugenAITool
{
    class AtkStorageManager
    {
        // Global variables
        private CharFilesInfo charFilesInfo;
        private string rl, targetCsvFilePath;
        private List<int> stateNos = new List<int>();                                               // List of state numbers from cmd file
        private int groundFront = -1, airFront = -1;                                                // Ground.front and air.front
        private const int pausetimeStart = 5;                                                       // The starting index of point pausetime in properties list
        private Dictionary<int, List<int>> stateProperties = new Dictionary<int, List<int>>();          // List of state properties, such as statetype, Juggle, attack_attribute, hitflag, and hit/guard time differences
        private Dictionary<int, List<int>> stateAnimDictionary = new Dictionary<int, List<int>>();      // Dictionary which match states and anims
        private Dictionary<int, List<List<int>>> animDatas = new Dictionary<int, List<List<int>>>();    // Atk anim number, distance X, Y and time
        private StreamReader readFile;
        private StreamWriter writeFile;

        public AtkStorageManager(CharFilesInfo charFilesInfo, string targetCsvFilePath)
        {
            // Initialize global variables
            this.charFilesInfo = charFilesInfo;
            this.targetCsvFilePath = targetCsvFilePath;
        }

        // Read def file to look for related cmd, st and air file
        public void AtkStorageMake()
        {
            ReadCns();
            ReadCmd();
            ReadSt();
            ReadAir();
            CreateCsvFile();

        }

        //Read cns file to look for the width of character
        private void ReadCns()
        {
            readFile = new StreamReader(charFilesInfo.charDirPath + charFilesInfo.cnsName);

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
            readFile = new StreamReader(charFilesInfo.charDirPath + charFilesInfo.cmdName);

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
            foreach (string stName in charFilesInfo.stNames)
            {
                readFile = new StreamReader(charFilesInfo.charDirPath + stName);

                // Variables
                string stateControllerType = "";
                int stateNo = 0, pausetimeLevel = 0, guardCtrltimeLevel = 0, guardHittimeLevel = 0, airGuardCtrltimeLevel = 0;

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

                            // Added new properties for attack attribute, combo available, guard flag, pausetime difference, guard pausetime difference, ground / air hittime / guardtime differences
                            for (int i = 0; i < 10; i +=1)
                            {
                                propertiesList.Add(i == 7 ? 20 : 0);
                            }

                            stateProperties.Add(stateNo, propertiesList);
                        }
                    }
                    if (stateNos.Contains(stateNo))
                    {
                        if (rl.Length > 0 && rl[0] == '[' && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            // Reset stateControllerType, guardCtrltimeLevel, guardHittimeLevel and airGuardCtrltimeLevelwhen we read new state controller
                            stateControllerType = "";
                            pausetimeLevel = 0;
                            guardCtrltimeLevel = 0;
                            guardHittimeLevel = 0;
                            airGuardCtrltimeLevel = 0;
                        }

                        if (rl.ContainsIgnoreCase("type") && rl.ContainsIgnoreCase("hitdef"))
                        {
                            stateControllerType = "hitdef";
                        }
                        else if (rl.ContainsIgnoreCase("attr") && stateControllerType.EqualsIgnoreCase("hitdef"))
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
                        else if (rl.ContainsIgnoreCase("hitflag") && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            if (rl.Contains("+")) stateProperties[stateNo][3] = 1;          // Means can hit when emeny is hit
                            if (rl.Contains("-")) stateProperties[stateNo][3] = -1;         // Means can hit when emeny is not hit
                        }
                        else if (rl.ContainsIgnoreCase("guardflag") && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            rl = rl.Substring(rl.IndexOf('=') + 1);

                            if (rl.ContainsIgnoreCase("H"))
                            {
                                stateProperties[stateNo][4] |= 1;
                            }
                            if (rl.ContainsIgnoreCase("L"))
                            {
                                stateProperties[stateNo][4] |= 2;
                            }
                            if (rl.ContainsIgnoreCase("A"))
                            {
                                stateProperties[stateNo][4] |= 4;
                            }
                            if (rl.ContainsIgnoreCase("M"))
                            {
                                stateProperties[stateNo][4] |= 3;
                            }
                        }
                        else if (rl.ContainsIgnoreCase("type") && rl.ContainsIgnoreCase("projectile"))
                        {
                            stateProperties[stateNo][2] |= 4;
                            stateProperties[stateNo][2] &= 7;                               // Remove the 4th bit which is helper
                        }
                        else if (rl.ContainsIgnoreCase("type") && rl.ContainsIgnoreCase("helper") && (stateProperties[stateNo][2] & 7) == 0)
                        {
                            stateProperties[stateNo][2] |= 8;
                        }

                        // For pausetime / guard.pausetime
                        else if (rl.ContainsIgnoreCase("pausetime") && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            stateProperties[stateNo][pausetimeStart] = int.Parse(rl.Substring(rl.IndexOf('=') + 1, rl.IndexOf(',') - rl.IndexOf('=') - 1).Trim()) - int.Parse(rl.Substring(rl.IndexOf(',') + 1).Trim());
                            if (pausetimeLevel < 1) stateProperties[stateNo][pausetimeStart+1] = stateProperties[stateNo][pausetimeStart];
                        }
                        else if (rl.ContainsIgnoreCase("guard.pausetime") && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            stateProperties[stateNo][pausetimeStart+1] = stateProperties[stateNo][pausetimeStart];
                            pausetimeLevel = 1;
                        }

                        // For ground/air.hittime
                        else if (rl.ContainsIgnoreCase("ground.hittime") && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            stateProperties[stateNo][pausetimeStart+2] = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                            if (guardHittimeLevel < 1) stateProperties[stateNo][pausetimeStart+5] = stateProperties[stateNo][pausetimeStart+2];
                        }
                        else if (rl.ContainsIgnoreCase("air.hittime") && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            stateProperties[stateNo][pausetimeStart+3] = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                        }

                        // For guard.ctrltime
                        else if (rl.ContainsIgnoreCase("guard.ctrltime") && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            stateProperties[stateNo][pausetimeStart+4] = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                            guardCtrltimeLevel = 2;
                            if (airGuardCtrltimeLevel < 1) stateProperties[stateNo][pausetimeStart+6] = stateProperties[stateNo][pausetimeStart+4];
                        }
                        else if (rl.ContainsIgnoreCase("guard.slidetime") && guardCtrltimeLevel < 2 && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            stateProperties[stateNo][pausetimeStart+4] = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                            guardCtrltimeLevel = 1;
                        }
                        else if (rl.ContainsIgnoreCase("ground.slidetime") && guardCtrltimeLevel < 1 && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            stateProperties[stateNo][pausetimeStart+4] = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                        }

                        // For guard.hittime
                        else if (rl.ContainsIgnoreCase("guard.hittime") && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            stateProperties[stateNo][pausetimeStart+5] = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                            guardHittimeLevel = 1;
                        }

                        // For airguard.ctrltime
                        else if (rl.ContainsIgnoreCase("airguard.ctrltime") && stateControllerType.EqualsIgnoreCase("hitdef"))
                        {
                            stateProperties[stateNo][pausetimeStart+6] = int.Parse(rl.Substring(rl.IndexOf('=') + 1).Trim());
                            airGuardCtrltimeLevel = 1;
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

            readFile = new StreamReader(charFilesInfo.charDirPath + charFilesInfo.airName);
            for (rl = readFile.ReadLine(); rl != null; rl = readFile.ReadLine())
            {
                rl = rl.RemoveMugenComment();

                if (rl.ContainsIgnoreCase("[Begin Action"))                                     // If header of Begin Action
                {
                    // Total time 
                    if (animDatas.ContainsKey(airNo) && animDatas[airNo] != null)
                    {
                        for (int i = 0; i < animDatas[airNo].Count; i += 1)
                        {
                            animDatas[airNo][i].Add(totalTime + 1);
                        }
                    }
                    
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
            
            // Total time after reading whole file if the last action is attack
            if (animDatas.ContainsKey(airNo) && animDatas[airNo] != null)
            {
                for (int i = 0; i < animDatas[airNo].Count; i += 1)
                {
                    animDatas[airNo][i].Add(totalTime + 1);
                }
            }

            readFile.Close();
        }

        // Combine and organize the data
        // Write into target CSV file
        private void CreateCsvFile()
        {
            // Create new directory if not exists
            string toolTempDirPath = targetCsvFilePath.Substring(0, targetCsvFilePath.LastIndexOf('\\') + 1);
            if (!Directory.Exists(toolTempDirPath)) Directory.CreateDirectory(toolTempDirPath);

            // Create CSV file
            string csvName = targetCsvFilePath + ".csv";
            writeFile = new StreamWriter(csvName, false, Encoding.UTF8);

            // Write into target file
            writeFile.WriteLine("招式,StateNo,AnimNo,StateType,Juggle,AttackAttr,Hitflag,Guardflag,范围x1,范围x2,范围y1,范围y2,攻击发生帧,持续时间,总时长,地面击中硬直差,空中击中硬直差,地面被防硬直差,空中被防硬直差,");

            // Loop for all statenos, find the anims they are using
            for (int i = 0; i < stateNos.Count; i += 1)
            {
                if (stateAnimDictionary.ContainsKey(stateNos[i]))
                {
                    List<int> animCountInState = stateAnimDictionary[stateNos[i]];

                    // Loop for all anims
                    for (int j = 0; j < animCountInState.Count; j += 1)
                    {
                        if (animDatas.ContainsKey(animCountInState[j]))
                        {
                            List<List<int>> stateDetails = animDatas[animCountInState[j]];

                            // Loop for all hits
                            for (int k = 0; k < stateDetails.Count; k += 1)
                            {
                                // StateNo, AnimNo, StateType, Juggle, AttackAttr, Hitflag and Guardflag
                                string writeStr = ',' + stateNos[i].ToString() + ',' + animCountInState[j].ToString() + ',';

                                for (int l = 0; l < pausetimeStart; l += 1)
                                {
                                    writeStr += stateProperties[stateNos[i]][l];
                                    writeStr += ',';
                                }

                                // Loop for datas
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

                                // We need to choose the smaller one from guard.ctrltime and guard.hittime, to be ground guard time, remove the unnecessary one
                                if (stateProperties[stateNos[i]].Count > pausetimeStart + 6)
                                {
                                    stateProperties[stateNos[i]].RemoveAt(pausetimeStart + 
                                        ((stateProperties[stateNos[i]][pausetimeStart + 4] < stateProperties[stateNos[i]][pausetimeStart + 5]) ? 5 : 4));
                                }
                                // Loop for hit / guard time differences
                                for (int l = pausetimeStart + 2; l < pausetimeStart + 6; l += 1)
                                {
                                        //
                                        int pausetimeDifference = (l < pausetimeStart + 4) ? stateProperties[stateNos[i]][pausetimeStart] : stateProperties[stateNos[i]][pausetimeStart+1];

                                        // Hit / guard time differences = hit / guard time + 1 + pausetime difference - (totaltime - 1 - attacktime)
                                        int value = stateProperties[stateNos[i]][l] + 1 + pausetimeDifference - (stateDetails[k][6] - 1 - stateDetails[k][4]);
                                        writeStr += value;
                                        writeStr += ',';
                                }

                                writeFile.WriteLine(writeStr);
                            }
                        }
                    }
                }
            }

            writeFile.Close();
        }
    }
}

