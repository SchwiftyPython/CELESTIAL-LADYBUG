using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class GlobalHelper : MonoBehaviour
    {
        public const string DoubleSpace = "\n\n";
        public const string SingleSpace = "\n";

        #region EventNames

        public const string EncounterOptionSelected = "EncounterOptionSelected";
        public const string FourOptionEncounter = "FourOptionEncounter";
        public const string TwoOptionEncounter = "TwoOptionEncounter";
        public const string NoOptionEncounter = "NoOptionEncounter";
        public const string EncounterResult = "EncounterResult";
        public const string EncounterFinished = "EncounterFinished";

        #endregion EventNames

        public static void DestroyAllChildren(GameObject parent)
        {
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                Destroy(parent.transform.GetChild(i).gameObject);
            }
        }

        public static void DestroyAllChildren(RectTransform parent)
        {
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                Destroy(parent.transform.GetChild(i).gameObject);
            }
        }

        public static void DestroyObject(GameObject go)
        {
            Destroy(go);
        }

        public static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string CapitalizeAllWords(string s)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static string SplitStringByCapitalLetters(string s)
        {
            var splitString = "";

            foreach (var letter in s)
            {
                if (char.IsUpper(letter) && splitString.Length > 0)
                {
                    splitString += " " + letter;
                }
                else
                {
                    splitString += letter;
                }
            }

            return splitString;
        }

        //<Summary>
        // Returns a random enum value of type T
        //</Summary>
        public static T GetRandomEnumValue<T>()
        {
            var values = Enum.GetValues(typeof(T));

            return (T)values.GetValue(Random.Range(0, values.Length));
        }

        public static string GetEnumDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? value.ToString();
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }
            throw new ArgumentException($@"Not found: {description}", nameof(description));
        }

        public static T NextEnum<T>(T src) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
            }

            var arr = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf(arr, src) + 1;
            return arr.Length == j ? arr[0] : arr[j];
        }
    }
}
