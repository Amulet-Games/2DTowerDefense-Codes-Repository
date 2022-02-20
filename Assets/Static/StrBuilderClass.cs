using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public static class StrBuilderClass
    {
        /// Tool tips builder
        public static StringBuilder strBuilder;

        #region Build

        #region Selector Tips.
        public static string Build_SelectorTips_WithCost(BuildingSO referBuildingType)
        {
            strBuilder.Clear();

            strBuilder.Append(referBuildingType.build_Name);
            strBuilder.Append("\n");

            Additive_AddBuildingCost(referBuildingType);

            return strBuilder.ToString();
        }
        #endregion

        #region Construction Error Message.
        public static string Build_CannotAffordCost(BuildingSO referBuildingType)
        {
            strBuilder.Clear();

            strBuilder.Append("Cannot afford ");

            Additive_AddBuildingCost(referBuildingType);

            return strBuilder.ToString();
        }
        #endregion

        #region Enemy Wave Message.
        public static string Build_NextWaveSeconds(float seconds)
        {
            strBuilder.Clear();

            strBuilder.Append("Next Wave in ").Append(seconds.ToString("F1")).Append("s");

            return strBuilder.ToString();
        }

        public static string Build_WaveNumberText(int waveNumb)
        {
            strBuilder.Clear();

            strBuilder.Append("Wave ").Append(waveNumb);

            return strBuilder.ToString();
        }
        #endregion

        #region GameOver Result Message.
        public static string Build_GameOverResultMsg()
        {
            strBuilder.Clear();

            strBuilder.Append("You Survived ");
            strBuilder.Append(AISessionManager.singleton.waveNumber);
            strBuilder.Append(" Waves!");

            return strBuilder.ToString();
        }
        #endregion

        #region Repair Error Message.
        public static string Build_InsufficientRepairCost(int insufficientAmt)
        {
            strBuilder.Clear();

            strBuilder.Append("You need ").Append(insufficientAmt).Append(" more Gold to repair building!");
            
            return strBuilder.ToString();
        }
        #endregion

        #endregion

        #region Additive
        static void Additive_AddBuildingCost(BuildingSO referBuildingType)
        {
            int costArrayLength = referBuildingType.costArray.Length;
            if (costArrayLength == 1)
            {
                ResourceCost _resourceCost = referBuildingType.costArray[0];

                strBuilder
                    .Append("<color=#").Append(_resourceCost.referedResource.colorHex).Append(">")
                    .Append(_resourceCost.referedResource.nameString)
                    .Append(": ")
                    .Append(_resourceCost.cost)
                    .Append("</color>");
            }
            else
            {
                ResourceCost _resourceCost = null;
                for (int i = 0; i < costArrayLength; i++)
                {
                    _resourceCost = referBuildingType.costArray[i];

                    strBuilder
                        .Append("<color=#").Append(_resourceCost.referedResource.colorHex).Append(">")
                        .Append(_resourceCost.referedResource.nameString)
                        .Append(": ")
                        .Append(_resourceCost.cost)
                        .Append("</color>");

                    // if this is not the last element.
                    if (i != (costArrayLength - 1))
                    {
                        strBuilder.Append("   ");
                    }
                }
            }
        }
        #endregion

        #region Setup.
        public static void Setup()
        {
            strBuilder = new StringBuilder();
        }
        #endregion
    }
}