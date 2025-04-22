using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;


namespace ENMod.Main
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "Cadenza.TROW.EnMod";
        public const string pluginName = "FJ ENMod Continued";
        public const string pluginVersion = "0.5";


        StoryFlow[] source2 = ResourceManager.LoadAllAssets<StoryFlow>(ResourceType.SideStories, string.Empty);
        public static Dictionary<string, string> translationDict;
        public string TranslationDictPath = Path.Combine(Paths.PluginPath, "Translations/translations.txt");
        public string FailedRegistry = Path.Combine(Paths.PluginPath, "Translations/failed.txt");

        public void Awake()
        {
            //This should be kept if necessary, it is used to fetch all Random Events Options Buttons Text (and results).
            /*               OptionQiyuAssset[] array = ResourceManager.LoadAllAssets<OptionQiyuAssset>(ResourceType.Qiyu, string.Empty);

                            foreach (OptionQiyuAssset x in array)
                            {
                            //    Debug.Log("GIMMEMOARSTRINGS QiyuOption = " + x.QiyuOption);

                                foreach(QiyuOption f in x.QiyuOption)
                            {
                                Debug.Log("GIMMEMOARSTRINGS optionDes = " + f.optionDes);
                                    foreach(QiyuOptionResult v in f.OptionResults)
                                {
                                    Debug.Log("What is this s*** ? = " + v.resultDes);
                                }

                            }
                        }
            */
            Debug.Log("CadenzaKun is Awake!");
            UnityEngine.Object[] gameobjects = ResourceManager.LoadAllAssets<UnityEngine.Object>(ResourceType.MainStories, "");

            Debug.Log("TranslationDictPath = " + TranslationDictPath);
            translationDict = FileToDictionary(TranslationDictPath);
            string dictdump = Path.Combine(Paths.PluginPath, "dictdump.txt");
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            File.WriteAllLines($"{currentPath}\\dictdump.txt", translationDict.Select(kvp => $"{ kvp.Key};{ kvp.Value}"));
            Logger.LogInfo("Hello World ! Welcome to Cadenza's plugin !");
            Harmony harmony = new Harmony("Cadenza.TROW.EnMod");
            harmony.PatchAll();
        }

        public void Prepare()
        {
            using (StreamWriter sw = File.AppendText(FailedRegistry))
            {
                string hello = "Hey ! Below, you'll find any untranslated lines in the .dll hardcoded lines (tooltips only) !";
                sw.Write($"{hello}\n---\n\n\n");
            }
        }

        public static Dictionary<string, string> FileToDictionary(string dir)
        {
            Debug.Log(Paths.PluginPath);

            Dictionary<string, string> dict = new Dictionary<string, string>();

            IEnumerable<string> lines = File.ReadLines(Path.Combine(Paths.PluginPath, "Translations", dir));

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
        }
    }

    //Keeping This In Case I Need To Re-Export The Main & Side Quests Desc
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
}
