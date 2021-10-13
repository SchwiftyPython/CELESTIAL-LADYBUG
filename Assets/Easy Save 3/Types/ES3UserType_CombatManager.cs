using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_currentCombatState", "<ActiveEntity>k__BackingField", "<CurrentTurnNumber>k__BackingField", "PrototypePawnHighlighterPrefab")]
	public class ES3UserType_CombatManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_CombatManager() : base(typeof(Assets.Scripts.Combat.CombatManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Assets.Scripts.Combat.CombatManager)obj;
			
			writer.WritePrivateField("_currentCombatState", instance);
			writer.WritePrivateField("<ActiveEntity>k__BackingField", instance);
			writer.WritePrivateField("<CurrentTurnNumber>k__BackingField", instance);
			writer.WritePropertyByRef("PrototypePawnHighlighterPrefab", instance.PrototypePawnHighlighterPrefab);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Assets.Scripts.Combat.CombatManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_currentCombatState":
					reader.SetPrivateField("_currentCombatState", reader.Read<Assets.Scripts.Combat.CombatState>(), instance);
					break;
					case "<ActiveEntity>k__BackingField":
					reader.SetPrivateField("<ActiveEntity>k__BackingField", reader.Read<Assets.Scripts.Entities.Entity>(), instance);
					break;
					case "<CurrentTurnNumber>k__BackingField":
					reader.SetPrivateField("<CurrentTurnNumber>k__BackingField", reader.Read<System.Int32>(), instance);
					break;
					case "PrototypePawnHighlighterPrefab":
						instance.PrototypePawnHighlighterPrefab = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_CombatManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_CombatManagerArray() : base(typeof(Assets.Scripts.Combat.CombatManager[]), ES3UserType_CombatManager.Instance)
		{
			Instance = this;
		}
	}
}