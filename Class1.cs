﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using BepInEx;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Sirenix.Serialization;

namespace ENMod
{


    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {


        public const string pluginGuid = "Cadenza.TROW.EnMod";
        public const string pluginName = "FJ ENMod Continued";
        public const string pluginVersion = "0.5";

        public static Dictionary<string, string> translationDict;
        public string TranslationDictPath = Path.Combine(BepInEx.Paths.PluginPath, "Translations/translations.txt");
        public string FailedRegistry = Path.Combine(BepInEx.Paths.PluginPath, "Translations/failed.txt");
        public Transform stepRoot;

        public void Awake()
        {   
            Debug.Log("CadenzaKun is Awake!");
            StoryFlow[] source = ResourceManager.LoadAllAssets<StoryFlow>(ResourceType.MainStories, string.Empty);
            StoryFlow[] source2 = ResourceManager.LoadAllAssets<StoryFlow>(ResourceType.SideStories, string.Empty);
            GameObject[] source3 = ResourceManager.LoadAllAssets<GameObject>(ResourceType.Prefab, string.Empty);
            

            source.ToList<StoryFlow>().ForEach(delegate (StoryFlow x)
            {
                //StoryFlowSystem.AllMainStories.Add(x.flowName, x);
               
            });
            source2.ToList<StoryFlow>().ForEach(delegate (StoryFlow x)
            {
                //StoryFlowSystem.AllSideStories.Add(x.flowName, x);
                
            });
            StepPart_MissionWindow.StepStruct availableStepStruct;

            source3.ToList<GameObject>().ForEach(delegate (GameObject n)
            {
                Debug.Log("n.name = " + n.name);
                
            });
            




            Debug.Log("TranslationDictPath = " + TranslationDictPath);
            translationDict = FileToDictionary(TranslationDictPath);
            string dictdump = Path.Combine(BepInEx.Paths.PluginPath, "dictdump.txt");

            File.WriteAllLines(
           "C:\\Program Files (x86)\\Steam\\steamapps\\common\\江湖余生\\BepInEx\\plugins\\dictdump.txt",
           translationDict.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
            Logger.LogInfo("Hello World ! Welcome to Cadenza's plugin !");
            var harmony = new Harmony("Cadenza.TROW.EnMod");
            harmony.PatchAll();

        }


        public void Prepare()
        {
            using (StreamWriter sw = File.AppendText(FailedRegistry))
            {
                string Hello = "Hey ! Below, you'll find any untranslated lines in the .dll hardcoded lines (tooltips only) !";
                sw.Write(Hello);
                sw.Write(Environment.NewLine);
                sw.Write("---");
                sw.Write(Environment.NewLine);
                sw.Write(Environment.NewLine);
            }
        }






        public static Dictionary<string, string> FileToDictionary(string dir)
        {
            Debug.Log(BepInEx.Paths.PluginPath);

            Dictionary<string, string> dict = new Dictionary<string, string>();

            IEnumerable<string> lines = File.ReadLines(Path.Combine(BepInEx.Paths.PluginPath, "Translations", dir));

            foreach (string line in lines)
            {
                var arr = line.Split('¤');
                if (arr[0] != arr[1])
                {
                    var pair = new KeyValuePair<string, string>(Regex.Replace(arr[0], @"\t|\n|\r", ""), arr[1]);
                    if (!dict.ContainsKey(pair.Key))
                        dict.Add(pair.Key, pair.Value);
                    else
                        Debug.Log($"Found a duplicated line while parsing {dir}: {pair.Key}");
                }
            }

            return dict;

            //return File.ReadLines(Path.Combine(BepInEx.Paths.PluginPath, "Translations", dir))
            //    .Select(line =>
            //    {
            //        var arr = line.Split('¤');
            //        return new KeyValuePair<string, string>(Regex.Replace(arr[0], @"\t|\n|\r", ""), arr[1]);
            //    })
            //    .GroupBy(kvp => kvp.Key)
            //    .Select(x => x.First())
            //    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, comparer);
        }
        public void Hello()
        {

        }

        //return File.ReadLines(Path.Combine(BepInEx.Paths.PluginPath, "Translations", dir))
        //    .Select(line =>
        //    {
        //        var arr = line.Split('¤');
        //        return new KeyValuePair<string, string>(Regex.Replace(arr[0], @"\t|\n|\r", ""), arr[1]);
        //    })
        //    .GroupBy(kvp => kvp.Key)
        //    .Select(x => x.First())
        //    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, comparer);


    }





        



    [HarmonyPatch]

    static class LogTooltips
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(BaseScene), "UnlockButton", new Type[] { typeof(WindowType), typeof(bool) });
            yield return AccessTools.Method(typeof(BlackSmithWindow), "OnClickAddBtn");
            yield return AccessTools.Method(typeof(CardChooseInBattle), "MeetCardCondition");
            yield return AccessTools.Method(typeof(ExploreWindow), "OnClickExploreButton");
            yield return AccessTools.Method(typeof(FellowPack_CharWindow), "GetRewardItem");
            yield return AccessTools.Method(typeof(FellowStateInfo), "ChangeAffinity");
            yield return AccessTools.Method(typeof(FlowResult), "ResultEffect");
            yield return AccessTools.Method(typeof(FunctionScript), "CertainLoot");
            yield return AccessTools.Method(typeof(FunctionScript), "PossibleRewardItemByBag");
            yield return AccessTools.Method(typeof(FunctionScript), "PossibleRewardMijiOrAcc");
            yield return AccessTools.Method(typeof(GameData), "AddCharacters");
            yield return AccessTools.Method(typeof(GameData), "AddFacAffinity");
            yield return AccessTools.Method(typeof(GameData), "AddRecipe");
            yield return AccessTools.Method(typeof(GameData), "ChangeHunger");
            yield return AccessTools.Method(typeof(GameManager), "NextDay");
            yield return AccessTools.Method(typeof(InventoryWindow), "UseMiji");
            yield return AccessTools.Method(typeof(InventoryWindowInBattle), "UseItem");
            yield return AccessTools.Method(typeof(InventoryWindowInBattle), "UseItem", new Type[] { typeof(ItemInInventory) });
            yield return AccessTools.Method(typeof(ItemPack_CraftSytem), "SuccesfullyCraftingItem");
            yield return AccessTools.Method(typeof(MiddlePack_PractiseWindow), "TrainCo");
            yield return AccessTools.Method(typeof(PlayerCardHolderInBattle), "MeetCondition");
            yield return AccessTools.Method(typeof(PlayerCardHolderInBattle), "OnRelease");
            yield return AccessTools.Method(typeof(QiyuResult), "Result");
            yield return AccessTools.Method(typeof(ShopWindowBase), "OnClickBuyButton");
            yield return AccessTools.Method(typeof(SingleFactionItem), "OnClickGetReward");
            yield return AccessTools.Method(typeof(TrainWindow), "Train");
            yield return AccessTools.Method(typeof(YiZhanWindow), "SetOut");
            

        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode == OpCodes.Ldstr)
                {
                    Debug.Log("operand = " + codes[i].operand.ToString());
                }
                if(codes[i].opcode == OpCodes.Ldstr && codes[i].operand != null && codes[i].operand != "")
                { 

                    if (codes[i].opcode == OpCodes.Ldstr && Main.translationDict.ContainsKey(codes[i].operand.ToString()))
                    {
                    codes[i].operand = Main.translationDict[codes[i].operand.ToString()];


                    }
                    
                else
                   {
                    if (codes[i].opcode == OpCodes.Ldstr && !Main.translationDict.ContainsKey(codes[i].operand.ToString()) && Helpers.IsChinese(codes[i].operand.ToString()))
                    {
                        string FailedRegistry = Path.Combine(BepInEx.Paths.PluginPath, "failed.txt");
                        Debug.Log(FailedRegistry);
                        using (StreamWriter sw = File.AppendText(FailedRegistry))
                        {
                            sw.Write(codes[i].operand.ToString());
                            sw.Write(Environment.NewLine);
                        }  
                    }
                    else
                    {
                        Debug.Log("Null");
                    }
                }
                }

            }
            return codes.AsEnumerable();

        }
    }

    [HarmonyPatch]

    static class SkillDesc
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(FunctionScript), "GetSkillCost");
            yield return AccessTools.Method(typeof(FunctionScript), "GetCastTimesCost");
            yield return AccessTools.Method(typeof(FunctionScript), "GetSkillTargetDes");
            yield return AccessTools.Method(typeof(FunctionScript), "GetAccDes");
            yield return AccessTools.Method(typeof(DesPack_PractiseWindow), "GetCost");
            yield return AccessTools.Method(typeof(DesPack_PractiseWindow), "GetCastTimes");
            yield return AccessTools.Method(typeof(DesPack_PractiseWindow), "GetTargetDes");

        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode == OpCodes.Ldstr)
                {
                    Debug.Log("operand = " + codes[i].operand.ToString());
                }
                if (codes[i].opcode == OpCodes.Ldstr)
                {

                    if (Main.translationDict.ContainsKey(codes[i].operand.ToString()))
                    {
                        Debug.Log("Found Matching String ! = " + Main.translationDict[codes[i].operand.ToString()]);
                        codes[i].operand = Main.translationDict[codes[i].operand.ToString()];
                        Debug.Log("Updated String ! = " + codes[i].operand.ToString());


                    }
                    else
                    {
                        Debug.Log("Not found");
                    }
                    /*
                    else
                    {
                        if (codes[i].opcode == OpCodes.Ldstr && !Main.translationDict.ContainsKey(codes[i].operand.ToString()) && Helpers.IsChinese(codes[i].operand.ToString()))
                        {
                            string FailedRegistry = Path.Combine(BepInEx.Paths.PluginPath, "failed.txt");
                            Debug.Log(FailedRegistry);
                            using (StreamWriter sw = File.AppendText(FailedRegistry))
                            {
                                sw.Write(codes[i].operand.ToString());
                                sw.Write(Environment.NewLine);
                            }
                        }
                        else
                        {
                            Debug.Log("Null");
                        }
                    }*/
                
                }


            }
            return codes.AsEnumerable();


        }
    }


    public static class Helpers
    {
        public static readonly Regex cjkCharRegex = new Regex(@"\p{IsCJKUnifiedIdeographs}");
        public static bool IsChinese(string s)
        {
            return cjkCharRegex.IsMatch(s);
        }
    }
}
 


