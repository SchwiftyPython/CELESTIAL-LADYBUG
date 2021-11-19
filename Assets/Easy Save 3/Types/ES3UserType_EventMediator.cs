using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_eventSubscriptions")]
	public class ES3UserType_EventMediator : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_EventMediator() : base(typeof(Assets.Scripts.EventMediator)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.EventMediator)obj;
			
			writer.WritePrivateField("_eventSubscriptions", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.EventMediator)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_eventSubscriptions":
					reader.SetPrivateField("_eventSubscriptions", reader.Read<System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<Assets.Scripts.ISubscriber>>>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_EventMediatorArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_EventMediatorArray() : base(typeof(Assets.Scripts.EventMediator[]), ES3UserType_EventMediator.Instance)
		{
			Instance = this;
		}
	}
}