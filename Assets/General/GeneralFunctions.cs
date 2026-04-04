using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace BenScr.UnityStack
{
    public static class GeneralFunctions
    {
        public static string FormatPercent(float percent)
        {
            return percent.ToString("0.##").Replace(',', '.') + "%";
        }

        public static IEnumerator DoAfterXFrames(int frameCount, Action action)
        {
            for (int i = 0; i < frameCount; i++)
                yield return null;

            action();
        }

        public static IEnumerator DoAfterDelay(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }


        public static string ToFileSize(long fileSize)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
            double len = fileSize;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        public static IEnumerator SetScrollRectValue(ScrollRect scrollRect, float value = 1.0f)
        {
            yield return null;
            scrollRect.verticalNormalizedPosition = value;
        }


        public static string CollapseSpaces(string input)
        {
            if (input == null) return null;
            return Regex.Replace(input, " {2,}", " ");
        }

        public static Vector3 Snap(Vector3 pos, float snapVal, float z = 0) => new Vector3(Mathf.Floor(pos.x / snapVal) * snapVal, Mathf.Floor(pos.y / snapVal) * snapVal, z);
        public static float3 Snap(float3 pos, float snapVal, float z) => new float3(math.floor(pos.x / snapVal) * snapVal, math.floor(pos.y / snapVal) * snapVal, z);
    }
}