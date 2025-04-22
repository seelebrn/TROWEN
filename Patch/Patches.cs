using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using ENMod.Helper;

namespace ENMod.Patch
{
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
                    Debug.Log($"operand1 = {codes[i].operand}");
                }
                if (codes[i].opcode == OpCodes.Ldstr && codes[i].operand != null && codes[i].operand.ToString() != "")
                {
                    if (Main.Main.translationDict.ContainsKey(codes[i].operand.ToString()))
                    {
                        Debug.Log($"Found String = {Main.Main.translationDict[codes[i].operand.ToString()]}");
                        codes[i].operand = Main.Main.translationDict[codes[i].operand.ToString()];

                        Debug.Log($"Updated String = {codes[i].operand}");
                    }
                    else
                    {
                        if (Helpers.IsChinese(codes[i].operand.ToString()))
                        {
                            string failedRegistry = Path.Combine(Paths.PluginPath, "failed.txt");
                            Debug.Log($"{failedRegistry}");
                            using (StreamWriter sw = File.AppendText(failedRegistry))
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
    static class SkillDesc_and_BlackSmith
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
            yield return AccessTools.Method(typeof(EventPack_RandomEventWindow), "ShowEventDes");
            yield return AccessTools.Method(typeof(StatUI), "ShowStat");
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode == OpCodes.Ldstr && codes[i].operand.ToString() != "\n")
                {
                    Debug.Log($"operand2 = {codes[i].operand}");

                    if (Main.Main.translationDict.ContainsKey(codes[i].operand.ToString().Replace("\n", "\\n")))
                    {
                        codes[i].operand = codes[i].operand.ToString().Replace("\n", "\\n");
                        Debug.Log($"Found Matching String ! = {Main.Main.translationDict[codes[i].operand.ToString()]}");
                        codes[i].operand = Main.Main.translationDict[codes[i].operand.ToString()];
                        codes[i].operand = codes[i].operand.ToString().Replace("\\n", "\n");

                        Debug.Log($"Updated String ! = {codes[i].operand}");
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
                    }
                    */
                }
            }
            return codes.AsEnumerable();
        }
    }

    [HarmonyPatch]
    static class Partners_CharacterSheet_Exploration
    {
        static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(FellowPack_CharWindow), "UpdateEquipSummary");
            yield return AccessTools.Method(typeof(BasePack_CharWindow), "InitPanel");
            yield return AccessTools.Method(typeof(ExploreWindow), "GetMoney");
            yield return AccessTools.Method(typeof(ExploreWindow), "RewardMiJiOrAcc");
            yield return AccessTools.Method(typeof(FactionPack_RandomEventWindow), "ShowEventDes");
            yield return AccessTools.Method(typeof(MilinWindow), "CalculateRewardStuff");
            yield return AccessTools.Method(typeof(MilinWindow), "RewardFood");
            yield return AccessTools.Method(typeof(BackMountWindow), "CalculateRewardStuff");
            yield return AccessTools.Method(typeof(BackMountWindow), "RewardMedi");
            yield return AccessTools.Method(typeof(BattleSummary), "StartShowAnimCo");
            yield return AccessTools.Method(typeof(BackMountWindow), "RewardMedi");
            yield return AccessTools.Method(typeof(BackMountWindow), "CalculateRewardStuff");
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count - 1; i++)
            {
                if (codes[i].opcode == OpCodes.Ldstr && codes[i].operand != null && codes[i].operand.ToString() != "\n" && codes[i].operand.ToString() != "")
                {
                    Debug.Log($"operand 3 = {codes[i].operand}");

                    if (Main.Main.translationDict.ContainsKey(codes[i].operand.ToString().Replace("\n", "\\n")))
                    {
                        codes[i].operand = codes[i].operand.ToString().Replace("\n", "\\n");
                        Debug.Log($"Found Matching String ! = {Main.Main.translationDict[codes[i].operand.ToString()]}");
                        codes[i].operand = Main.Main.translationDict[codes[i].operand.ToString()];
                        codes[i].operand = codes[i].operand.ToString().Replace("\\n", "\n");

                        Debug.Log($"Updated String ! = {codes[i].operand}");
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
                    }
                    */
                }
            }
            return codes.AsEnumerable();
        }
    }
}
