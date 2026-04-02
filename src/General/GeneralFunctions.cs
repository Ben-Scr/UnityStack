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
        public static Vector2 GetMousePosition2D()
        {
            return Camera.main?.ScreenToWorldPoint(Input.mousePosition) ?? Vector2.zero;
        }

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
        public static Vector3 GetMousePosition()
        {
            bool isOrtho = Camera.main?.orthographic ?? true;

            if (isOrtho) return GetMousePosition2D();
            else
                return GetMousePosition3D();
        }

        public static Vector3 GetMousePosition3D()
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                return hit.point;
            }

            return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(cam.transform.position.z)));
        }
        public static Vector3 GetMousePositionOnGridPlane()
        {
            var cam = Camera.main;
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            var plane = new Plane(Vector3.up, Vector3.zero);

            if (plane.Raycast(ray, out float enter))
                return ray.GetPoint(enter);

            return Vector3.zero;
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

        public static bool Contains<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            foreach (var item in items)
            {
                if (predicate(item)) return true;
            }

            return false;
        }
        public static int Count<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            int i = 0;

            foreach (var item in items)
            {
                if (predicate(item)) i++;
            }

            return i;
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