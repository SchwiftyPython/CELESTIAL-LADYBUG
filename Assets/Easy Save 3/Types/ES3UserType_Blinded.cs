using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("LocationDependent", "Stacks", "Description", "TargetType", "<Icon>k__BackingField", "<Name>k__BackingField", "_duration")]
	public class ES3UserType_Blinded : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Blinded() : base(typeof(Assets.Scripts.Effects.Blinded)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.Effects.Blinded)obj;
			
			writer.WritePrivateField("LocationDependent", instance);
			writer.WritePrivateField("Stacks", instance);
			writer.WritePrivateField("Description", instance);
			writer.WritePrivateField("TargetType", instance);
			writer.WritePrivateFieldByRef("<Icon>k__BackingField", instance);
			writer.WritePrivateField("<Name>k__BackingField", instance);
			writer.WritePrivateField("_duration", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.Effects.Blinded)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "LocationDependent":
					reader.SetPrivateField("LocationDependent", reader.Read<System.Boolean>(), instance);
					break;
					case "Stacks":
					reader.SetPrivateField("Stacks", reader.Read<System.Boolean>(), instance);
					break;
					case "Description":
					reader.SetPrivateField("Description", reader.Read<System.String>(), instance);
					break;
					case "TargetType":
					reader.SetPrivateField("TargetType", reader.Read<Assets.Scripts.TargetType>(), instance);
					break;
					case "<Icon>k__BackingField":
					reader.SetPrivateField("<Icon>k__BackingField", reader.Read<UnityEngine.Sprite>(), instance);
					break;
					case "<Name>k__BackingField":
					reader.SetPrivateField("<Name>k__BackingField", reader.Read<System.String>(), instance);
					break;
					case "_duration":
					reader.SetPrivateField("_duration", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Scripts.Effects.Blinded();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_BlindedArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_BlindedArray() : base(typeof(Assets.Scripts.Effects.Blinded[]), ES3UserType_Blinded.Instance)
		{
			Instance = this;
		}
	}
}