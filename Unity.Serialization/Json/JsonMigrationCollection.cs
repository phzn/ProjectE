using System;
using System.Collections.Generic;
using Unity.Serialization.Json.Unsafe;
using UnityEngine;

namespace Unity.Serialization.Json
{
	// Token: 0x0200003E RID: 62
	internal struct JsonMigrationCollection
	{
		// Token: 0x06000156 RID: 342 RVA: 0x00005220 File Offset: 0x00003420
		public bool TryGetSerializedVersion<TValue>(out int version)
		{
			IJsonMigration migrationForType = this.GetMigrationForType<TValue>(out version);
			if (migrationForType == null)
			{
				return false;
			}
			if (version > 0)
			{
				return true;
			}
			Debug.LogError(string.Format("An error occured while serializing Type=[{0}] using IJsonMigration=[{1}]. Serialized version must be greater than 0.", typeof(TValue), migrationForType.GetType()));
			return false;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005264 File Offset: 0x00003464
		public bool TryMigrate<TValue>(UnsafeObjectView view, out TValue value, JsonPropertyReader reader, List<DeserializationEvent> events)
		{
			int num;
			IJsonMigration migrationForType = this.GetMigrationForType<TValue>(out num);
			if (migrationForType == null)
			{
				value = default(TValue);
				return false;
			}
			int num2 = 0;
			UnsafeValueView unsafeValueView;
			if (view.TryGetValue("$version", out unsafeValueView))
			{
				if (unsafeValueView.Type != TokenType.Primitive)
				{
					events.Add(new DeserializationEvent(EventType.Exception, new Exception(string.Format("An error occured while deserializing Type=[{0}]. Property=[{1}] is expected to be an int value.", typeof(TValue), "$version"))));
					value = default(TValue);
					return false;
				}
				num2 = unsafeValueView.AsInt32();
				if (num == num2)
				{
					value = default(TValue);
					return false;
				}
			}
			JsonMigrationContext jsonMigrationContext = new JsonMigrationContext(num2, view.AsSafe(), typeof(TValue), this.UserData, reader);
			IJsonMigration<TValue> jsonMigration = migrationForType as IJsonMigration<TValue>;
			if (jsonMigration == null)
			{
				IContravariantJsonMigration<TValue> contravariantJsonMigration = migrationForType as IContravariantJsonMigration<TValue>;
				if (contravariantJsonMigration == null)
				{
					throw new Exception("An internal error has occured.");
				}
				value = (TValue)((object)contravariantJsonMigration.Migrate(in jsonMigrationContext));
			}
			else
			{
				value = jsonMigration.Migrate(in jsonMigrationContext);
			}
			return true;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005358 File Offset: 0x00003558
		private IJsonMigration GetMigrationForType<TValue>(out int version)
		{
			if (this.UserDefined != null && this.UserDefined.Count > 0)
			{
				foreach (IJsonMigration jsonMigration in this.UserDefined)
				{
					IJsonMigration<TValue> jsonMigration2 = jsonMigration as IJsonMigration<TValue>;
					if (jsonMigration2 != null)
					{
						version = jsonMigration2.Version;
						return jsonMigration2;
					}
					IContravariantJsonMigration<TValue> contravariantJsonMigration = jsonMigration as IContravariantJsonMigration<TValue>;
					if (contravariantJsonMigration != null)
					{
						version = contravariantJsonMigration.Version;
						return contravariantJsonMigration;
					}
				}
			}
			if (this.Global != null && this.Global.Count > 0)
			{
				foreach (IJsonMigration jsonMigration3 in this.Global)
				{
					IJsonMigration<TValue> jsonMigration4 = jsonMigration3 as IJsonMigration<TValue>;
					if (jsonMigration4 != null)
					{
						version = jsonMigration4.Version;
						return jsonMigration4;
					}
					IContravariantJsonMigration<TValue> contravariantJsonMigration2 = jsonMigration3 as IContravariantJsonMigration<TValue>;
					if (contravariantJsonMigration2 != null)
					{
						version = contravariantJsonMigration2.Version;
						return contravariantJsonMigration2;
					}
				}
			}
			version = 0;
			return null;
		}

		// Token: 0x04000084 RID: 132
		public List<IJsonMigration> Global;

		// Token: 0x04000085 RID: 133
		public List<IJsonMigration> UserDefined;

		// Token: 0x04000086 RID: 134
		public object UserData;
	}
}
