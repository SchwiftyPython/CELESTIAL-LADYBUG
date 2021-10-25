using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("<Melee>k__BackingField", "<Ranged>k__BackingField", "<Lockpicking>k__BackingField", "<Endurance>k__BackingField", "<Healing>k__BackingField", "<Survival>k__BackingField", "_persuasion")]
	public class ES3UserType_Skills : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Skills() : base(typeof(Assets.Scripts.Entities.Skills)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.Entities.Skills)obj;
			
			writer.WritePrivateField("<Melee>k__BackingField", instance);
			writer.WritePrivateField("<Ranged>k__BackingField", instance);
			writer.WritePrivateField("<Lockpicking>k__BackingField", instance);
			writer.WritePrivateField("<Endurance>k__BackingField", instance);
			writer.WritePrivateField("<Healing>k__BackingField", instance);
			writer.WritePrivateField("<Survival>k__BackingField", instance);
			writer.WritePrivateField("_persuasion", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.Entities.Skills)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "<Melee>k__BackingField":
					reader.SetPrivateField("<Melee>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "<Ranged>k__BackingField":
					reader.SetPrivateField("<Ranged>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "<Lockpicking>k__BackingField":
					reader.SetPrivateField("<Lockpicking>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "<Endurance>k__BackingField":
					reader.SetPrivateField("<Endurance>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "<Healing>k__BackingField":
					reader.SetPrivateField("<Healing>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "<Survival>k__BackingField":
					reader.SetPrivateField("<Survival>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "_persuasion":
					reader.SetPrivateField("_persuasion", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Scripts.Entities.Skills();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_SkillsArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SkillsArray() : base(typeof(Assets.Scripts.Entities.Skills[]), ES3UserType_Skills.Instance)
		{
			Instance = this;
		}
	}
}