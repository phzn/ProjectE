using System;
using Unity.Properties;

namespace Unity.Serialization.Json
{
	// Token: 0x0200003F RID: 63
	public readonly struct JsonMigrationContext
	{
		// Token: 0x06000159 RID: 345 RVA: 0x00005480 File Offset: 0x00003680
		internal JsonMigrationContext(int serializedVersion, SerializedObjectView serializedObject, Type serializedType, object userData, JsonPropertyReader visitor)
		{
			this.SerializedVersion = serializedVersion;
			this.SerializedObject = serializedObject;
			this.SerializedType = serializedType;
			this.UserData = userData;
			this.m_Visitor = visitor;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000054A7 File Offset: 0x000036A7
		public bool TryRead<TValue>(out TValue value)
		{
			return this.TryRead<TValue>(this.SerializedObject, out value);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000054B6 File Offset: 0x000036B6
		public TValue Read<TValue>()
		{
			return this.Read<TValue>(this.SerializedObject);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000054CC File Offset: 0x000036CC
		public bool TryRead<TValue>(string name, out TValue value)
		{
			bool flag;
			try
			{
				value = this.Read<TValue>(name);
				flag = true;
			}
			catch
			{
				value = default(TValue);
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005508 File Offset: 0x00003708
		public TValue Read<TValue>(string name)
		{
			return this.Read<TValue>(this.SerializedObject[name]);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000551C File Offset: 0x0000371C
		public bool TryRead<TValue>(SerializedObjectView view, out TValue value)
		{
			bool flag;
			try
			{
				value = this.Read<TValue>(view);
				flag = true;
			}
			catch
			{
				value = default(TValue);
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000555C File Offset: 0x0000375C
		public TValue Read<TValue>(SerializedValueView view)
		{
			TValue tvalue = default(TValue);
			this.Read<TValue>(ref tvalue, view);
			return tvalue;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000557C File Offset: 0x0000377C
		public void Read<TValue>(ref TValue value, SerializedValueView view)
		{
			switch (view.Type)
			{
			case TokenType.Object:
			case TokenType.Array:
			{
				Type type = ((!TypeTraits<TValue>.IsAbstractOrInterface) ? typeof(TValue) : null);
				using (this.m_Visitor.CreateSerializedTypeScope(type))
				{
					using (this.m_Visitor.CreateViewScope(view.AsUnsafe()))
					{
						using (this.m_Visitor.CreateDisableRootMigrationScope(true))
						{
							PropertyWrapper<TValue> propertyWrapper = new PropertyWrapper<TValue>(value);
							PropertyContainer.TryAccept<PropertyWrapper<TValue>>(this.m_Visitor, ref propertyWrapper, default(VisitParameters));
							value = propertyWrapper.Value;
							return;
						}
					}
				}
				break;
			}
			case TokenType.String:
			{
				string text = view.AsStringView().ToString();
				TypeConversion.TryConvert<string, TValue>(ref text, out value);
				return;
			}
			case TokenType.Primitive:
			{
				SerializedPrimitiveView serializedPrimitiveView = view.AsPrimitiveView();
				if (serializedPrimitiveView.IsIntegral())
				{
					if (serializedPrimitiveView.IsSigned())
					{
						long num = serializedPrimitiveView.AsInt64();
						TypeConversion.TryConvert<long, TValue>(ref num, out value);
						return;
					}
					ulong num2 = serializedPrimitiveView.AsUInt64();
					TypeConversion.TryConvert<ulong, TValue>(ref num2, out value);
					return;
				}
				else
				{
					if (serializedPrimitiveView.IsDecimal() || serializedPrimitiveView.IsInfinity() || serializedPrimitiveView.IsNaN())
					{
						float num3 = serializedPrimitiveView.AsFloat();
						TypeConversion.TryConvert<float, TValue>(ref num3, out value);
						return;
					}
					if (serializedPrimitiveView.IsBoolean())
					{
						bool flag = serializedPrimitiveView.AsBoolean();
						TypeConversion.TryConvert<bool, TValue>(ref flag, out value);
						return;
					}
					if (serializedPrimitiveView.IsNull())
					{
						value = default(TValue);
						return;
					}
					return;
				}
				break;
			}
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x04000087 RID: 135
		public readonly int SerializedVersion;

		// Token: 0x04000088 RID: 136
		public readonly SerializedObjectView SerializedObject;

		// Token: 0x04000089 RID: 137
		public readonly Type SerializedType;

		// Token: 0x0400008A RID: 138
		public readonly object UserData;

		// Token: 0x0400008B RID: 139
		private readonly JsonPropertyReader m_Visitor;
	}
}
