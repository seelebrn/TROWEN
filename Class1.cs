using System;
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


        StoryFlow[] source2 = ResourceManager.LoadAllAssets<StoryFlow>(ResourceType.SideStories, string.Empty);
        public static Dictionary<string, string> translationDict;
        public string TranslationDictPath = Path.Combine(BepInEx.Paths.PluginPath, "Translations/translations.txt");
        public string FailedRegistry = Path.Combine(BepInEx.Paths.PluginPath, "Translations/failed.txt");

       

     //   [SerializeField]
     //   public GameObject stepPrefab = GameObject.Find("StepPart_Mission");



        public void Awake()
        {
            Debug.Log("CadenzaKun is Awake!");
            UnityEngine.Object[] gameobjects = ResourceManager.LoadAllAssets<UnityEngine.Object>(ResourceType.MainStories, "");

            Debug.Log("Count = " + gameobjects.Count());

            foreach (UnityEngine.Object n in gameobjects)
            {
                
                Debug.Log("Names = " + n.name);
                Debug.Log("Type = " + n.GetType());

            }
            StoryFlow[] source2 = ResourceManager.LoadAllAssets<StoryFlow>(ResourceType.SideStories, string.Empty);
            foreach(StoryFlow x in source2)
            {
                for(int i=0; i<x.nodes.Count; i++)
                { 
                Debug.Log("Node = " + x.nodes[i].ToString());

                }
            }

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
    }/*
    [HarmonyPatch(typeof(StepPart_MissionWindow), "GetAvailableStepStruct")]
    static class Mumu
    {
        static AccessTools.FieldRef<StepPart_MissionWindow, GameObject> stepPrefabRef =
        AccessTools.FieldRefAccess<StepPart_MissionWindow, GameObject>("stepPrefab");
        static AccessTools.FieldRef<StepPart_MissionWindow, Transform> stepRootRef =
        AccessTools.FieldRefAccess<StepPart_MissionWindow, Transform>("stepRoot");
        static AccessTools.FieldRef<StepPart_MissionWindow, List<StepPart_MissionWindow.StepStruct>> stepsRef = 
        AccessTools.FieldRefAccess<StepPart_MissionWindow, List<StepPart_MissionWindow.StepStruct>>("steps");



        static void Prefix(StepPart_MissionWindow __instance)
        {
            List<StepPart_MissionWindow.StepStruct> steps = new List<StepPart_MissionWindow.StepStruct>();
            var stepPrefab = stepPrefabRef(__instance);
            var stepRoot = stepRootRef(__instance);

            Debug.Log("HarmonyStepRoot = " + stepRoot);
            Debug.Log("HarmonyStepPrefab = " + stepPrefab);
            

            StepPart_MissionWindow.StepStruct zaza = stepPrefab.GetComponent<StepPart_MissionWindow.StepStruct>();
            if(zaza.stepDes.text != null)
            { 
            Debug.Log("zaza = " + zaza.stepDes);
            }
            StepPart_MissionWindow.StepStruct zaza1 = stepPrefab.GetComponentInChildren<StepPart_MissionWindow.StepStruct>();
            if (zaza1.stepDes.text != null)
            {
                Debug.Log("zaza1 = " + zaza.stepDes);
            }

            // stepStruct2 = new StepPart_MissionWindow.StepStruct(gameObject2.GetComponent<UnityEngine.UI.Text>(), gameObject2.GetComponentInChildren<UnityEngine.UI.Image>(true));


        }
    }*/
    /*
    [HarmonyPatch(typeof(FlowProgress), "NotReallyActivated")]
    static class FlowProgress_patch
    {
        static AccessTools.FieldRef<FlowProgress, List<string>> completedListRef =
            AccessTools.FieldRefAccess<FlowProgress, List<string>>("completedList");

        static void Postfix(FlowProgress __instance)
        {
            //GameData GameData = Singleton<GameManager>.Main
            var completedList = completedListRef(__instance);
            foreach(string s in completedList)
            {
                StoryFlow[] source = ResourceManager.LoadAllAssets<StoryFlow>(ResourceType.SideStories, string.Empty);
                //FlowProgress gamedata = Singleton<GameManager>.Instance.GameData.MainPlotProgress;
                //Debug.Log("MainPlotProgress = " + gamedata);
                foreach (StoryFlow x in source)
                { 

                    Debug.Log("flow = " + x);
                    foreach(FlowNode t in x.nodes)
                    {
                        if (t.baseInfo != null && !t.baseInfo.updateLogInfo.IsNullOrEmpty())
                        { 
                            Debug.Log("GIMMESTRINGS = " + t.baseInfo.updateLogInfo);
                        }
                    }
                }
                Debug.Log("Harmonycompletedlist = " + s);
            }
        }
    }
    */

    public static class Helpers
    {
        public static readonly Regex cjkCharRegex = new Regex(@"\p{IsCJKUnifiedIdeographs}");
        public static bool IsChinese(string s)
        {
            return cjkCharRegex.IsMatch(s);
        }
    }
}
 


