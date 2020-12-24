using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities.Names
{ public class NameStore : MonoBehaviour
    {
        #region FileInfo

        public TextAsset[] NameFiles;

        private readonly Dictionary<string, TextAsset> _nameFiles = new Dictionary<string, TextAsset>
        {
            {"generic_male_first_names", null},
            {"generic_female_first_names", null},
            {"generic_last_names", null}
        };

        #endregion FileInfo

        private static List<string> _genericMaleFirstNames;
        private static List<string> _genericFemaleFirstNames;
        private static List<string> _genericLastNames;

        private readonly List<List<string>> _nameLists = new List<List<string>>
        {
            _genericMaleFirstNames,
            _genericFemaleFirstNames,
            _genericLastNames
        };

        private  List<string> _firstNames;
        private  List<string> _lastNames;

        public static NameStore Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            LoadNamesFromFiles();
        }

        private void LoadNamesFromFiles()
        {
            try
            {
                var nameFilesIndex = 0;
                foreach (var file in _nameFiles.Keys.ToArray())
                {
                    _nameFiles[file] = NameFiles[nameFilesIndex];
                    nameFilesIndex++;
                }

                var nameListIndex = 0;
                foreach (var file in _nameFiles.Values)
                {
                    _nameLists[nameListIndex] = file.text.Split("\r\n"[0]).ToList();
                    nameListIndex++;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error processing name file" + e.Message);
            }
        
        }

        private void FilterPossibleNameListsBySex(IEnumerable<string> nameFiles, Sex sex)
        {
            _firstNames = new List<string>();
            _lastNames = new List<string>();
        
            foreach (var nameFile in nameFiles)
            {
                if (nameFile.Contains(sex.ToString().ToLower()))
                {
                    _firstNames.AddRange(_nameFiles[nameFile].text.Split("\r\n"[0]).ToList());
                }
                if (nameFile.Contains("last"))
                {
                    _lastNames.AddRange(_nameFiles[nameFile].text.Split("\r\n"[0]).ToList());
                }
            }
        }

        public string GenerateFullName(List<string> nameFiles, Sex sex)
        {
            if (nameFiles == null || nameFiles.Count < 2)
            {
                nameFiles = new List<string>(_nameFiles.Keys);
            }

            string firstName;
            string lastName;
            try
            {
                FilterPossibleNameListsBySex(nameFiles, sex);

                var index = Random.Range(0, _firstNames.Count);
                firstName = _firstNames[index].Trim('\n');

                index = Random.Range(0, _lastNames.Count);
                lastName = _lastNames[index].Trim('\n');
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                firstName = string.Empty;
                lastName = string.Empty;
            }

            return firstName + " " + lastName;
        }

        public  string GenerateFirstName(List<string> nameFiles, Sex sex)
        {
            FilterPossibleNameListsBySex(nameFiles, sex);

            var index = Random.Range(0, _firstNames.Count);
            return _firstNames[index].Trim('\n');
        }

        public string GenerateFullName()
        {
            var nameFiles = new List<string>();

            const int chanceToAddList = 79;
            for (var i = 0; i < _nameFiles.Count; i++)
            {
                var roll = Random.Range(0, 101);

                if (roll <= chanceToAddList)
                {
                    nameFiles.Add(_nameFiles.ElementAt(i).Key);
                }
            }

            const int sexChance = 50;
            var sexRoll = Random.Range(0, 101);

            var sex = sexRoll < sexChance ? Sex.Male : Sex.Female;

            return GenerateFullName(nameFiles, sex);
        }
    }
}
