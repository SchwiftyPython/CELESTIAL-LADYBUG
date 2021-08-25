using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class AudioStore : MonoBehaviour
    {
        private Dictionary<string, EntitySoundDto> _entitySounds;

        public struct EntitySoundDto
        {
            public string HitSound;
            public string DieSound;
        }

        [Header("Companion Sounds")]
        [FMODUnity.EventRef] public string companionHurt;
        [FMODUnity.EventRef] public string companionDie;

        [Header("Enemy Sounds")]
        [FMODUnity.EventRef] public string monsterHurt;
        [FMODUnity.EventRef] public string monsterDie;
        [FMODUnity.EventRef] public string skeletonHurt;
        [FMODUnity.EventRef] public string skeletonDie;
        [FMODUnity.EventRef] public string spiderHurt;
        [FMODUnity.EventRef] public string spiderDie;
        [FMODUnity.EventRef] public string spiderAttack;

        [Header("Neutral Sounds")]
        [FMODUnity.EventRef] public string genericAttack;
        [FMODUnity.EventRef] public string bowAttack;

        [Header("Interface Sounds")]
        [FMODUnity.EventRef] public string genericPopup;
        [FMODUnity.EventRef] public string buttonClick;
        [FMODUnity.EventRef] public string buttonHover;
        [FMODUnity.EventRef] public string resultPopup;
        [FMODUnity.EventRef] public string textWriting;
        [FMODUnity.EventRef] public string reward;
        [FMODUnity.EventRef] public string penalty;

        public EntitySoundDto GetEntitySound(string key)
        {
            if (!_entitySounds.ContainsKey(key))
            {
                Debug.LogWarning($"{key} does not exist in Entity Sounds!");

                return _entitySounds.First().Value;
            }

            return _entitySounds[key];
        }

        private void PopulateEntitySounds()
        {
            _entitySounds = new Dictionary<string, EntitySoundDto>
            {
                {"companion", new EntitySoundDto()},
                {"monster", new EntitySoundDto()},
                {"skeleton", new EntitySoundDto()},
            };
        }
    }
}
