using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("<Name>k__BackingField", "<Description>k__BackingField", "<ApCost>k__BackingField", "<Range>k__BackingField", "<AbilityOwner>k__BackingField", "<TargetType>k__BackingField", "<IsPassive>k__BackingField", "<Icon>k__BackingField", "<UsesEquipment>k__BackingField")]
	public class ES3UserType_WorksOut : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_WorksOut() : base(typeof(Assets.Scripts.Abilities.WorksOut)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.Abilities.WorksOut)obj;
			
			writer.WritePrivateField("<Name>k__BackingField", instance);
			writer.WritePrivateField("<Description>k__BackingField", instance);
			writer.WritePrivateField("<ApCost>k__BackingField", instance);
			writer.WritePrivateField("<Range>k__BackingField", instance);
			writer.WritePrivateField("<AbilityOwner>k__BackingField", instance);
			writer.WritePrivateField("<TargetType>k__BackingField", instance);
			writer.WritePrivateField("<IsPassive>k__BackingField", instance);
			writer.WritePrivateFieldByRef("<Icon>k__BackingField", instance);
			writer.WritePrivateField("<UsesEquipment>k__BackingField", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.Abilities.WorksOut)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "<Name>k__BackingField":
					reader.SetPrivateField("<Name>k__BackingField", reader.Read<System.String>(), instance);
					break;
					case "<Description>k__BackingField":
					reader.SetPrivateField("<Description>k__BackingField", reader.Read<System.String>(), instance);
					break;
					case "<ApCost>k__BackingField":
					reader.SetPrivateField("<ApCost>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "<Range>k__BackingField":
					reader.SetPrivateField("<Range>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "<AbilityOwner>k__BackingField":
					reader.SetPrivateField("<AbilityOwner>k__BackingField", reader.Read<Assets.Scripts.Entities.Entity>(), instance);
					break;
					case "<TargetType>k__BackingField":
					reader.SetPrivateField("<TargetType>k__BackingField", reader.Read<Assets.Scripts.TargetType>(), instance);
					break;
					case "<IsPassive>k__BackingField":
					reader.SetPrivateField("<IsPassive>k__BackingField", reader.Read<System.Boolean>(), instance);
					break;
					case "<Icon>k__BackingField":
					reader.SetPrivateField("<Icon>k__BackingField", reader.Read<UnityEngine.Sprite>(), instance);
					break;
					case "<UsesEquipment>k__BackingField":
					reader.SetPrivateField("<UsesEquipment>k__BackingField", reader.Read<System.Boolean>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Scripts.Abilities.WorksOut(null);
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_WorksOutArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_WorksOutArray() : base(typeof(Assets.Scripts.Abilities.WorksOut[]), ES3UserType_WorksOut.Instance)
		{
			Instance = this;
		}
	}
}