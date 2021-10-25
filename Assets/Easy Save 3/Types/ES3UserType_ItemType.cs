using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Name", "Description", "Parent", "_melee", "_ranged", "_defense", "_abilities", "_range", "_sprite", "Stackable", "Price", "TwoHanded", "Group")]
	public class ES3UserType_ItemType : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ItemType() : base(typeof(Assets.Scripts.Items.ItemType)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.Items.ItemType)obj;
			
			writer.WriteProperty("Name", instance.Name, ES3Type_string.Instance);
			writer.WriteProperty("Description", instance.Description, ES3Type_string.Instance);
			writer.WriteProperty("Parent", instance.Parent, ES3Type_string.Instance);
			writer.WritePrivateField("_melee", instance);
			writer.WritePrivateField("_ranged", instance);
			writer.WritePrivateField("_defense", instance);
			writer.WritePrivateField("_abilities", instance);
			writer.WritePrivateField("_range", instance);
			writer.WritePrivateFieldByRef("_sprite", instance);
			writer.WriteProperty("Stackable", instance.Stackable, ES3Type_bool.Instance);
			writer.WriteProperty("Price", instance.Price, ES3Type_int.Instance);
			writer.WriteProperty("TwoHanded", instance.TwoHanded, ES3Type_bool.Instance);
			writer.WriteProperty("Group", instance.Group);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.Items.ItemType)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Name":
						instance.Name = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Description":
						instance.Description = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Parent":
						instance.Parent = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "_melee":
					reader.SetPrivateField("_melee", reader.Read<Assets.Scripts.Items.Components.Attack>(), instance);
					break;
					case "_ranged":
					reader.SetPrivateField("_ranged", reader.Read<Assets.Scripts.Items.Components.Attack>(), instance);
					break;
					case "_defense":
					reader.SetPrivateField("_defense", reader.Read<Assets.Scripts.Items.Components.Defense>(), instance);
					break;
					case "_abilities":
					reader.SetPrivateField("_abilities", reader.Read<System.Collections.Generic.List<System.String>>(), instance);
					break;
					case "_range":
					reader.SetPrivateField("_range", reader.Read<System.Int32>(), instance);
					break;
					case "_sprite":
					reader.SetPrivateField("_sprite", reader.Read<UnityEngine.Sprite>(), instance);
					break;
					case "Stackable":
						instance.Stackable = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Price":
						instance.Price = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "TwoHanded":
						instance.TwoHanded = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Group":
						instance.Group = reader.Read<Assets.Scripts.Items.ItemGroup>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Scripts.Items.ItemType();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_ItemTypeArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ItemTypeArray() : base(typeof(Assets.Scripts.Items.ItemType[]), ES3UserType_ItemType.Instance)
		{
			Instance = this;
		}
	}
}