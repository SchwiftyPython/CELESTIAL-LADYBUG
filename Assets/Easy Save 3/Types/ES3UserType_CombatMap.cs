using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_terrain")]
	public class ES3UserType_CombatMap : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_CombatMap() : base(typeof(Assets.Scripts.Combat.CombatMap)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.Combat.CombatMap)obj;
			
			writer.WritePrivateField("_terrain", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.Combat.CombatMap)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_terrain":
					reader.SetPrivateField("_terrain", reader.Read<GoRogue.MapViews.ISettableMapView<GoRogue.GameFramework.IGameObject>>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Assets.Scripts.Combat.CombatMap(32, 24);
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_CombatMapArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_CombatMapArray() : base(typeof(Assets.Scripts.Combat.CombatMap[]), ES3UserType_CombatMap.Instance)
		{
			Instance = this;
		}
	}
}