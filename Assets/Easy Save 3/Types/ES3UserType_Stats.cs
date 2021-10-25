using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_maxHealth", "_currentHealth", "_maxEnergy", "_currentEnergy", "_maxMorale", "_currentMorale", "_maxActionPoints", "_currentActionPoints", "<Initiative>k__BackingField", "<Armor>k__BackingField", "<Critical>k__BackingField")]
	public class ES3UserType_Stats : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Stats() : base(typeof(Assets.Scripts.Entities.Stats)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.Entities.Stats)obj;
			
			writer.WritePrivateField("_maxHealth", instance);
			writer.WritePrivateField("_currentHealth", instance);
			writer.WritePrivateField("_maxEnergy", instance);
			writer.WritePrivateField("_currentEnergy", instance);
			writer.WritePrivateField("_maxMorale", instance);
			writer.WritePrivateField("_currentMorale", instance);
			writer.WritePrivateField("_maxActionPoints", instance);
			writer.WritePrivateField("_currentActionPoints", instance);
			writer.WritePrivateField("<Initiative>k__BackingField", instance);
			writer.WritePrivateField("<Armor>k__BackingField", instance);
			writer.WritePrivateField("<Critical>k__BackingField", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.Entities.Stats)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_maxHealth":
					reader.SetPrivateField("_maxHealth", reader.Read<System.Int32>(), instance);
					break;
					case "_currentHealth":
					reader.SetPrivateField("_currentHealth", reader.Read<System.Int32>(), instance);
					break;
					case "_maxEnergy":
					reader.SetPrivateField("_maxEnergy", reader.Read<System.Int32>(), instance);
					break;
					case "_currentEnergy":
					reader.SetPrivateField("_currentEnergy", reader.Read<System.Int32>(), instance);
					break;
					case "_maxMorale":
					reader.SetPrivateField("_maxMorale", reader.Read<System.Int32>(), instance);
					break;
					case "_currentMorale":
					reader.SetPrivateField("_currentMorale", reader.Read<System.Int32>(), instance);
					break;
					case "_maxActionPoints":
					reader.SetPrivateField("_maxActionPoints", reader.Read<System.Int32>(), instance);
					break;
					case "_currentActionPoints":
					reader.SetPrivateField("_currentActionPoints", reader.Read<System.Int32>(), instance);
					break;
					case "<Initiative>k__BackingField":
					reader.SetPrivateField("<Initiative>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "<Armor>k__BackingField":
					reader.SetPrivateField("<Armor>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "<Critical>k__BackingField":
					reader.SetPrivateField("<Critical>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Scripts.Entities.Stats();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_StatsArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_StatsArray() : base(typeof(Assets.Scripts.Entities.Stats[]), ES3UserType_Stats.Instance)
		{
			Instance = this;
		}
	}
}