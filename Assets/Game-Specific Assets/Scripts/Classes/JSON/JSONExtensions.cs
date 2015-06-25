using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace SimpleJSON
{
    public static class JSONExtensions
    {
        #region Extensions

        public static JSONClass ExportAsJson(this Vector3 vector)
        {
            JSONClass result = new JSONClass();

            result["x"] = new JSONData(vector.x);
            result["y"] = new JSONData(vector.y);
            result["z"] = new JSONData(vector.z);

            return result;
        }

        public static Vector3 ImportVector3(this JSONNode jsonObject)
        {
            Vector3 result = new Vector3();

            result.x = jsonObject["x"].AsFloat;
            result.y = jsonObject["y"].AsFloat;
            result.z = jsonObject["z"].AsFloat;

            return result;
        }

        public static JSONArray FoldList<T>(this List<T> list)
            where T : IJsonSavable
        {
            JSONArray array = new JSONArray();

            for(int i = 0; i < list.Count; i++)
            {
                JSONClass item = list[i].ExportState();
                array.Add(item);
            }

            return array;
        }

        public static List<T> UnfoldJsonArray<T>(this JSONArray array)
            where T : IJsonSavable, new()
        {
            List<T> result = new List<T>();

            foreach (JSONNode child in array.Childs)
            {
                T newItem = new T();
                newItem.ImportState(child.AsObject);

                result.Add(newItem);
            }

            return result;
        }

        public static T ToEnum<T>(this JSONNode node)
        {
            return (T)Enum.Parse(typeof(T), node);
        }

        #endregion Extensions
    }
}