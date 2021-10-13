using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("<Name>k__BackingField", "<Description>k__BackingField", "<ApCost>k__BackingField", "<Range>k__BackingField", "<AbilityOwner>k__BackingField", "<TargetType>k__BackingField", "<IsPassive>k__BackingField", "<Icon>k__BackingField", "<UsesEquipment>k__BackingField", "Name", "Description", "ApCost", "Range", "AbilityOwner", "TargetType", "IsPassive", "Icon", "UsesEquipment")]
	public class ES3UserType_VampireBite : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_VampireBite() : base(typeof(Assets.Scripts.Abilities.VampireBite)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.Abilities.VampireBite)obj;
			
			writer.WritePrivateField("<Name>k__BackingField", instance);
			writer.WritePrivateField("<Description>k__BackingField", instance);
			writer.WritePrivateField("<ApCost>k__BackingField", instance);
			writer.WritePrivateField("<Range>k__BackingField", instance);
			writer.WritePrivateField("<AbilityOwner>k__BackingField", instance);
			writer.WritePrivateField("<TargetType>k__BackingField", instance);
			writer.WritePrivateField("<IsPassive>k__BackingField", instance);
			writer.WritePrivateFieldByRef("<Icon>k__BackingField", instance);
			writer.WritePrivateField("<UsesEquipment>k__BackingField", instance);
			writer.WritePrivateProperty("Name", instance);
			writer.WritePrivateProperty("Description", instance);
			writer.WritePrivateProperty("ApCost", instance);
			writer.WritePrivateProperty("Range", instance);
			writer.WritePrivateProperty("AbilityOwner", instance);
			writer.WritePrivateProperty("TargetType", instance);
			writer.WritePrivateProperty("IsPassive", instance);
			writer.WritePrivatePropertyByRef("Icon", instance);
			writer.WritePrivateProperty("UsesEquipment", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.Abilities.VampireBite)obj;
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
					case "Name":
					reader.SetPrivateProperty("Name", reader.Read<System.String>(), instance);
					break;
					case "Description":
					reader.SetPrivateProperty("Description", reader.Read<System.String>(), instance);
					break;
					case "ApCost":
					reader.SetPrivateProperty("ApCost", reader.Read<System.Int32>(), instance);
					break;
					case "Range":
					reader.SetPrivateProperty("Range", reader.Read<System.Int32>(), instance);
					break;
					case "AbilityOwner":
					reader.SetPrivateProperty("AbilityOwner", reader.Read<Assets.Scripts.Entities.Entity>(), instance);
					break;
					case "TargetType":
					reader.SetPrivateProperty("TargetType", reader.Read<Assets.Scripts.TargetType>(), instance);
					break;
					case "IsPassive":
					reader.SetPrivateProperty("IsPassive", reader.Read<System.Boolean>(), instance);
					break;
					case "Icon":
					reader.SetPrivateProperty("Icon", reader.Read<UnityEngine.Sprite>(), instance);
					break;
					case "UsesEquipment":
					reader.SetPrivateProperty("UsesEquipment", reader.Read<System.Boolean>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Scripts.Abilities.VampireBite(null);
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_VampireBiteArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_VampireBiteArray() : base(typeof(Assets.Scripts.Abilities.VampireBite[]), ES3UserType_VampireBite.Instance)
		{
			Instance = this;
		}
	}
}