using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class GlobalHelper : MonoBehaviour
    {
        public const string DoubleSpace = "\n\n";
        public const string SingleSpace = "\n";

        #region EventNames  //todo would it be better to make these enums?

        public const string EncounterOptionSelected = "EncounterOptionSelected";
        public const string FourOptionEncounter = "FourOptionEncounter";
        public const string TwoOptionEncounter = "TwoOptionEncounter";
        public const string NoOptionEncounter = "NoOptionEncounter";
        public const string CombatEncounter = "CombatEncounter";
        public const string RetreatEncounterFailed = "RetreatEncounterFailed";
        public const string EncounterResult = "EncounterResult";
        public const string EncounterFinished = "EncounterFinished";
        public const string CampingEncounterFinished = "CampingEncounterFinished";
        public const string PartyEatAndHeal = "PartyEatAndHeal";
        public const string PlayerTurn = "PlayerTurn";
        public const string AiTurn = "AiTurn";
        public const string CombatSceneLoaded = "CombatSceneLoaded";
        public const string RefreshCombatUi = "RefreshCombatUi";
        public const string TileSelected = "TileSelected";
        public const string TileDeselected = "TileDeselected";
        public const string TileHovered = "TileHovered";
        public const string AbilityHovered = "AbilityHovered";
        public const string HidePopup = "HidePopup";
        public const string EndTurn = "EndTurn";
        public const string NextTarget = "NextTarget";
        public const string EntityTargeted = "EntityTargeted";
        public const string CombatFinished = "CombatFinished";
        public const string EntityDead = "EntityDead";
        public const string SendMessageToConsole = "SendMessageToConsole";
        public const string GameOver = "GameOver";
        public const string ButtonClick = "ButtonClick";
        public const string MeleeHit = "MeleeHit";

        #endregion EventNames

        #region SceneNames

        public const string CombatScene = "Combat";
        public const string TravelScene = "Travel";
        public const string TitleScreenScene = "TitleScreen";

        #endregion SceneNames

        private static readonly System.Random SysRandom = new System.Random();

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

        public static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26;   

            for (var i = 0; i < size; i++)
            {
                var @char = (char)SysRandom.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
