using System;
using GoRogue;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_terrain")]
	public class ES3UserType_Map : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Map() : base(typeof(GoRogue.GameFramework.Map)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (GoRogue.GameFramework.Map)obj;
			
			writer.WritePrivateField("_terrain", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (GoRogue.GameFramework.Map)obj;
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
			var instance = new GoRogue.GameFramework.Map(32, 24, 1, Distance.CHEBYSHEV, 4294967295, 4294967295, 0);
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_MapArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_MapArray() : base(typeof(GoRogue.GameFramework.Map[]), ES3UserType_Map.Instance)
		{
			Instance = this;
		}
	}
}