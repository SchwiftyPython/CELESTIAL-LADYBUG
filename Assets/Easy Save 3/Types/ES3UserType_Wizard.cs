using System;
using Assets.Scripts.Entities;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_level", "_xp", "_isPlayer", "_aiController", "_moved", "_movedLastTurn", "<Name>k__BackingField", "<Portrait>k__BackingField", "<Abilities>k__BackingField", "<CombatSpritePrefab>k__BackingField", "<CombatSpriteInstance>k__BackingField", "IdleSkinSwap", "AttackSkinSwap", "HitSkinSwap", "DeadSkinSwap", "HurtSound", "DieSound", "AttackSound", "_parentObject", "_position", "_isWalkable", "<IsTransparent>k__BackingField", "<CurrentMap>k__BackingField", "_components")]
	public class ES3UserType_Wizard : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Wizard() : base(typeof(Assets.Scripts.Entities.Companions.Wizard)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.Entities.Companions.Wizard)obj;
			
			writer.WritePrivateField("_level", instance);
			writer.WritePrivateField("_xp", instance);
			writer.WritePrivateField("_isPlayer", instance);
			writer.WritePrivateFieldByRef("_aiController", instance);
			writer.WritePrivateField("_moved", instance);
			writer.WritePrivateField("_movedLastTurn", instance);
			writer.WritePrivateField("<Name>k__BackingField", instance);
			writer.WritePrivateField("<Portrait>k__BackingField", instance);
			writer.WritePrivateField("<Abilities>k__BackingField", instance);
			writer.WritePrivateFieldByRef("<CombatSpritePrefab>k__BackingField", instance);
			writer.WritePrivateFieldByRef("<CombatSpriteInstance>k__BackingField", instance);
			writer.WritePropertyByRef("IdleSkinSwap", instance.IdleSkinSwap);
			writer.WritePropertyByRef("AttackSkinSwap", instance.AttackSkinSwap);
			writer.WritePropertyByRef("HitSkinSwap", instance.HitSkinSwap);
			writer.WritePropertyByRef("DeadSkinSwap", instance.DeadSkinSwap);
			writer.WriteProperty("HurtSound", instance.HurtSound, ES3Type_string.Instance);
			writer.WriteProperty("DieSound", instance.DieSound, ES3Type_string.Instance);
			writer.WriteProperty("AttackSound", instance.AttackSound, ES3Type_string.Instance);
			writer.WritePrivateField("_parentObject", instance);
			writer.WritePrivateField("_position", instance);
			writer.WritePrivateField("_isWalkable", instance);
			writer.WritePrivateField("<IsTransparent>k__BackingField", instance);
			writer.WritePrivateField("<CurrentMap>k__BackingField", instance);
			writer.WritePrivateField("_components", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.Entities.Companions.Wizard)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_level":
					reader.SetPrivateField("_level", reader.Read<System.Int32>(), instance);
					break;
					case "_xp":
					reader.SetPrivateField("_xp", reader.Read<System.Int32>(), instance);
					break;
					case "_isPlayer":
					reader.SetPrivateField("_isPlayer", reader.Read<System.Boolean>(), instance);
					break;
					case "_aiController":
					reader.SetPrivateField("_aiController", reader.Read<Assets.Scripts.AI.AiController>(), instance);
					break;
					case "_moved":
					reader.SetPrivateField("_moved", reader.Read<System.Boolean>(), instance);
					break;
					case "_movedLastTurn":
					reader.SetPrivateField("_movedLastTurn", reader.Read<System.Boolean>(), instance);
					break;
					case "<Name>k__BackingField":
					reader.SetPrivateField("<Name>k__BackingField", reader.Read<System.String>(), instance);
					break;
					case "<Portrait>k__BackingField":
					reader.SetPrivateField("<Portrait>k__BackingField", reader.Read<System.Collections.Generic.Dictionary<Assets.Scripts.UI.Portrait.Slot, System.String>>(), instance);
					break;
					case "<Abilities>k__BackingField":
					reader.SetPrivateField("<Abilities>k__BackingField", reader.Read<System.Collections.Generic.Dictionary<System.Type, Assets.Scripts.Abilities.Ability>>(), instance);
					break;
					case "<CombatSpritePrefab>k__BackingField":
					reader.SetPrivateField("<CombatSpritePrefab>k__BackingField", reader.Read<UnityEngine.GameObject>(), instance);
					break;
					case "<CombatSpriteInstance>k__BackingField":
					reader.SetPrivateField("<CombatSpriteInstance>k__BackingField", reader.Read<UnityEngine.GameObject>(), instance);
					break;
					case "IdleSkinSwap":
						instance.IdleSkinSwap = reader.Read<UnityEngine.Texture>(ES3Type_Texture.Instance);
						break;
					case "AttackSkinSwap":
						instance.AttackSkinSwap = reader.Read<UnityEngine.Texture>(ES3Type_Texture.Instance);
						break;
					case "HitSkinSwap":
						instance.HitSkinSwap = reader.Read<UnityEngine.Texture>(ES3Type_Texture.Instance);
						break;
					case "DeadSkinSwap":
						instance.DeadSkinSwap = reader.Read<UnityEngine.Texture>(ES3Type_Texture.Instance);
						break;
					case "HurtSound":
						instance.HurtSound = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "DieSound":
						instance.DieSound = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "AttackSound":
						instance.AttackSound = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "_parentObject":
					reader.SetPrivateField("_parentObject", reader.Read<GoRogue.GameFramework.IGameObject>(), instance);
					break;
					case "_position":
					reader.SetPrivateField("_position", reader.Read<GoRogue.Coord>(), instance);
					break;
					case "_isWalkable":
					reader.SetPrivateField("_isWalkable", reader.Read<System.Boolean>(), instance);
					break;
					case "<IsTransparent>k__BackingField":
					reader.SetPrivateField("<IsTransparent>k__BackingField", reader.Read<System.Boolean>(), instance);
					break;
					case "<CurrentMap>k__BackingField":
					reader.SetPrivateField("<CurrentMap>k__BackingField", reader.Read<GoRogue.GameFramework.Map>(), instance);
					break;
					case "_components":
					reader.SetPrivateField("_components", reader.Read<System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.List<System.Object>>>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Scripts.Entities.Companions.Wizard(Race.RaceType.Human, true);
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_WizardArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_WizardArray() : base(typeof(Assets.Scripts.Entities.Companions.Wizard[]), ES3UserType_Wizard.Instance)
		{
			Instance = this;
		}
	}
}