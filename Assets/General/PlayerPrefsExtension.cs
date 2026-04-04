using System.Collections.Generic;   
using UnityEngine;

namespace BenScr.UnityStack
{
    /*
    * Extensions for PlayerPrefs to support more data types such as bool, Color, Vector2, Vector3, List<T>, and any other serializable object.
    */

    public static class PlayerPrefsExtension
    {
        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key + "_bool", value ? 1 : 0);
        }
        public static bool GetBool(string key, bool defaultValue = default)
        {
            return PlayerPrefs.GetInt(key + "_bool", defaultValue ? 1 : 0) == 0 ? false : true;
        }

        public static void SetColor(Color color, string key)
        {
            PlayerPrefs.SetFloat(key + "_red", color.r);
            PlayerPrefs.SetFloat(key + "_green", color.g);
            PlayerPrefs.SetFloat(key + "_blue", color.b);
            PlayerPrefs.SetFloat(key + "_alpha", color.a);
        }
        public static Color GetColor(string key, Color defaultValue = default)
        {
            float r = PlayerPrefs.GetFloat(key + "_red", defaultValue.r);
            float g = PlayerPrefs.GetFloat(key + "_green", defaultValue.g);
            float b = PlayerPrefs.GetFloat(key + "_blue", defaultValue.b);
            float a = PlayerPrefs.GetFloat(key + "_alpha", defaultValue.a);
            return new Color(r, g, b, a);
        }

        public static T SetObject<T>(string key, T obj)
        {
            string json = JsonUtility.ToJson(obj);
            PlayerPrefs.SetString(key + "_object", json);
            return obj;
        }
        public static T GetObject<T>(string key, T defaultValue = default)
        {
            string json = PlayerPrefs.GetString(key + "_object", JsonUtility.ToJson(defaultValue));
            return JsonUtility.FromJson<T>(json);
        }

        public static void SetVector2(Vector2 vector, string key)
        {
            PlayerPrefs.SetFloat(key + "_vec2X", vector.x);
            PlayerPrefs.SetFloat(key + "_vec2Y", vector.y);
        }
        public static Vector2 GetVector2(string key, Vector2 defaultValue = default)
        {
            float x = PlayerPrefs.GetFloat(key + "_vec2X", defaultValue.x);
            float y = PlayerPrefs.GetFloat(key + "_vec2Y", defaultValue.y);
            return new Vector2(x, y);
        }

        public static void SetVector3(Vector3 vector, string key)
        {
            PlayerPrefs.SetFloat(key + "_vec3X", vector.x);
            PlayerPrefs.SetFloat(key + "_vec3Y", vector.y);
            PlayerPrefs.SetFloat(key + "_vec3Z", vector.z);
        }
        public static Vector3 GetVector3(string key, Vector3 defaultValue = default)
        {
            float x = PlayerPrefs.GetFloat(key + "_vec3X", defaultValue.x);
            float y = PlayerPrefs.GetFloat(key + "_vec3Y", defaultValue.y);
            float z = PlayerPrefs.GetFloat(key + "_vec3Z", defaultValue.z);
            return new Vector3(x, y, z);
        }

        public static void SetList<T>(string key,List<T> list)
        {
            string json = JsonUtility.ToJson(new ListWrapper<T>(list));
            PlayerPrefs.SetString(key + "_list", json);
        }
        public static List<T> GetList<T>(string key, List<T> defaultValue = default)
        {
            string json = PlayerPrefs.GetString(key + "_list", JsonUtility.ToJson(new ListWrapper<T>(defaultValue)));
            return JsonUtility.FromJson<ListWrapper<T>>(json).list;
        }

        public class ListWrapper<T>
        {
            public List<T> list;
            public ListWrapper(List<T> list)
            {
                this.list = list;
            }
        }
    }
}
