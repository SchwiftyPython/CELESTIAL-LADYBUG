using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("<Agility>k__BackingField", "<Coordination>k__BackingField", "_physique", "_intellect", "<Acumen>k__BackingField", "_charisma")]
	public class ES3UserType_Attributes : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Attributes() : base(typeof(Assets.Scripts.Entities.Attributes)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.Entities.Attributes)obj;
			
			writer.WritePrivateField("<Agility>k__BackingField", instance);
			writer.WritePrivateField("<Coordination>k__BackingField", instance);
			writer.WritePrivateField("_physique", instance);
			writer.WritePrivateField("_intellect", instance);
			writer.WritePrivateField("<Acumen>k__BackingField", instance);
			writer.WritePrivateField("_charisma", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.Entities.Attributes)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "<Agility>k__BackingField":
					reader.SetPrivateField("<Agility>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "<Coordination>k__BackingField":
					reader.SetPrivateField("<Coordination>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "_physique":
					reader.SetPrivateField("_physique", reader.Read<System.Int32>(), instance);
					break;
					case "_intellect":
					reader.SetPrivateField("_intellect", reader.Read<System.Int32>(), instance);
					break;
					case "<Acumen>k__BackingField":
					reader.SetPrivateField("<Acumen>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "_charisma":
					reader.SetPrivateField("_charisma", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Scripts.Entities.Attributes();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_AttributesArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_AttributesArray() : base(typeof(Assets.Scripts.Entities.Attributes[]), ES3UserType_Attributes.Instance)
		{
			Instance = this;
		}
	}
}