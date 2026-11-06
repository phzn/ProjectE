using System;
using System.Collections.Generic;
using Unity.Properties;
using Unity.Serialization.Json.Unsafe;
using UnityEngine.Serialization;

namespace Unity.Serialization.Json
{
	// Token: 0x02000058 RID: 88
	internal class JsonPropertyReader : JsonPropertyVisitor, IPropertyBagVisitor, ISetPropertyBagVisitor, IListPropertyBagVisitor, IDictionaryPropertyBagVisitor, IPropertyVisitor
	{
		// Token: 0x06000226 RID: 550 RVA: 0x0000935D File Offset: 0x0000755D
		public void SetView(UnsafeValueView view)
		{
			this.m_View = view;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009366 File Offset: 0x00007566
		public void SetSerializedType(Type type)
		{
			this.m_SerializedType = type;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000936F File Offset: 0x0000756F
		public void SetDisableRootAdapters(bool disableRootAdapters)
		{
			this.m_DisableRootAdapters = disableRootAdapters;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00009378 File Offset: 0x00007578
		public void SetGlobalAdapters(List<IJsonAdapter> adapters)
		{
			this.m_Adapters.Global = adapters;
			this.m_HasPrimitiveOrStringGlobalAdapters = JsonAdapterCollection.ContainsPrimitiveOrStringAdapter(adapters);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00009392 File Offset: 0x00007592
		public void SetUserDefinedAdapters(List<IJsonAdapter> adapters)
		{
			this.m_Adapters.UserDefined = adapters;
			this.m_HasPrimitiveOrStringUserDefinedAdapters = JsonAdapterCollection.ContainsPrimitiveOrStringAdapter(adapters);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000093AC File Offset: 0x000075AC
		public void SetGlobalMigrations(List<IJsonMigration> migrations)
		{
			this.m_Migrations.Global = migrations;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000093BA File Offset: 0x000075BA
		public void SetUserDefinedMigrations(List<IJsonMigration> migrations)
		{
			this.m_Migrations.UserDefined = migrations;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000093C8 File Offset: 0x000075C8
		public void SetUserData(object userData)
		{
			this.m_Migrations.UserData = userData;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000093D6 File Offset: 0x000075D6
		public void SetEvents(List<DeserializationEvent> events)
		{
			this.m_SerializedTypeProvider.Events = events;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000093E4 File Offset: 0x000075E4
		public void SetSerializedReferences(SerializedReferences serializedReferences)
		{
			this.m_SerializedReferences = serializedReferences;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000093ED File Offset: 0x000075ED
		public JsonPropertyReader()
		{
			this.m_Adapters.InternalAdapter = new JsonAdapter();
			this.m_SerializedTypeProvider = new JsonPropertyReader.SerializedTypeProvider();
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00009410 File Offset: 0x00007610
		private static JsonPropertyReader.SerializedContainerMetadata GetSerializedContainerMetadata(UnsafeObjectView view)
		{
			JsonPropertyReader.SerializedContainerMetadata serializedContainerMetadata = default(JsonPropertyReader.SerializedContainerMetadata);
			foreach (UnsafeMemberView unsafeMemberView in view)
			{
				UnsafeStringView unsafeStringView = unsafeMemberView.Key().AsStringView();
				if (unsafeStringView.Length() == 0)
				{
					break;
				}
				if (unsafeStringView[0] != '$')
				{
					break;
				}
				if (unsafeStringView.Equals("$id"))
				{
					serializedContainerMetadata.HasSerializedId = true;
					serializedContainerMetadata.SerializedId = unsafeMemberView.Value().AsInt32();
				}
				if (unsafeStringView.Equals("$ref"))
				{
					serializedContainerMetadata.IsSerializedReference = true;
					serializedContainerMetadata.SerializedId = unsafeMemberView.Value().AsInt32();
				}
				serializedContainerMetadata.HasSerializedTypeInfo |= unsafeStringView.Equals("$type");
				serializedContainerMetadata.HasSerializedVersion |= unsafeStringView.Equals("$version");
				serializedContainerMetadata.HasElements |= unsafeStringView.Equals("$elements");
			}
			return serializedContainerMetadata;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00009534 File Offset: 0x00007734
		internal JsonPropertyReader.UnsafeViewScope CreateViewScope(UnsafeValueView view)
		{
			return new JsonPropertyReader.UnsafeViewScope(this, view);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000953D File Offset: 0x0000773D
		internal JsonPropertyReader.DisableRootMigrationScope CreateDisableRootMigrationScope(bool disableRootMigration)
		{
			return new JsonPropertyReader.DisableRootMigrationScope(this, disableRootMigration);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00009546 File Offset: 0x00007746
		internal JsonPropertyReader.SerializedTypeScope CreateSerializedTypeScope(Type serializedType)
		{
			return new JsonPropertyReader.SerializedTypeScope(this, serializedType);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00009550 File Offset: 0x00007750
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
		{
			if (properties is IPropertyWrapper)
			{
				using (PropertyCollection<TContainer>.Enumerator enumerator = properties.GetProperties().GetEnumerator())
				{
					if (enumerator.MoveNext() && enumerator.Current != null)
					{
						IProperty<TContainer> property = enumerator.Current;
						using (base.CreatePropertyScope(property))
						{
							property.Accept(this, ref container);
							return;
						}
					}
					throw new InvalidOperationException("IPropertyWrapper has no properties");
				}
			}
			if (this.m_View.Type != TokenType.Object)
			{
				throw new ArgumentException();
			}
			UnsafeObjectView unsafeObjectView = this.m_View.AsObjectView();
			TContainer tcontainer;
			if ((!(base.Property is IPropertyWrapper) || !this.m_DisableRootMigration) && this.m_Migrations.TryMigrate<TContainer>(unsafeObjectView, out tcontainer, this, this.m_SerializedTypeProvider.Events))
			{
				SerializedReferences serializedReferences = this.m_SerializedReferences;
				if (serializedReferences != null)
				{
					serializedReferences.AddDeserializedReference<TContainer>(this.m_Metadata.SerializedId, tcontainer);
				}
				container = tcontainer;
				return;
			}
			foreach (IProperty<TContainer> property2 in properties.GetProperties(ref container))
			{
				if (!PropertyChecks.IsPropertyExcludedFromSerialization(property2))
				{
					this.AcceptProperty<TContainer>(ref container, unsafeObjectView, property2);
				}
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000096B8 File Offset: 0x000078B8
		private void AcceptProperty<TContainer>(ref TContainer container, UnsafeObjectView view, IProperty<TContainer> property)
		{
			UnsafeValueView unsafeValueView;
			if (view.TryGetValue(property.Name, out unsafeValueView))
			{
				using (base.CreatePropertyScope(property))
				{
					using (this.CreateViewScope(unsafeValueView))
					{
						property.Accept(this, ref container);
					}
				}
				return;
			}
			foreach (FormerNameAttribute formerNameAttribute in property.GetAttributes<FormerNameAttribute>())
			{
				if (view.TryGetValue(formerNameAttribute.OldName, out unsafeValueView))
				{
					using (base.CreatePropertyScope(property))
					{
						using (this.CreateViewScope(unsafeValueView))
						{
							property.Accept(this, ref container);
							return;
						}
					}
				}
			}
			foreach (FormerlySerializedAsAttribute formerlySerializedAsAttribute in property.GetAttributes<FormerlySerializedAsAttribute>())
			{
				if (view.TryGetValue(formerlySerializedAsAttribute.oldName, out unsafeValueView))
				{
					using (base.CreatePropertyScope(property))
					{
						using (this.CreateViewScope(unsafeValueView))
						{
							property.Accept(this, ref container);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00009860 File Offset: 0x00007A60
		void ISetPropertyBagVisitor.Visit<TSet, TElement>(ISetPropertyBag<TSet, TElement> properties, ref TSet container)
		{
			UnsafeValueView unsafeValueView = (this.m_Metadata.HasElements ? this.m_View.AsObjectView()["$elements"] : this.m_View);
			if (unsafeValueView.Type != TokenType.Array)
			{
				throw new ArgumentException();
			}
			UnsafeArrayView unsafeArrayView = unsafeValueView.AsArrayView();
			container.Clear();
			foreach (UnsafeValueView unsafeValueView2 in unsafeArrayView)
			{
				TElement telement = default(TElement);
				this.ReadValue<TElement>(ref telement, unsafeValueView2, false);
				container.Add(telement);
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00009918 File Offset: 0x00007B18
		void IListPropertyBagVisitor.Visit<TList, TElement>(IListPropertyBag<TList, TElement> properties, ref TList container)
		{
			UnsafeValueView unsafeValueView = (this.m_Metadata.HasElements ? this.m_View.AsObjectView()["$elements"] : this.m_View);
			if (unsafeValueView.Type != TokenType.Array)
			{
				throw new ArgumentException();
			}
			UnsafeArrayView unsafeArrayView = unsafeValueView.AsArrayView();
			if (typeof(TList).IsArray)
			{
				int num = 0;
				using (IEnumerator<UnsafeValueView> enumerator = unsafeArrayView.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UnsafeValueView unsafeValueView2 = enumerator.Current;
						TElement telement = default(TElement);
						this.ReadValue<TElement>(ref telement, unsafeValueView2, false);
						container[num++] = telement;
					}
					return;
				}
			}
			container.Clear();
			foreach (UnsafeValueView unsafeValueView3 in unsafeArrayView)
			{
				TElement telement2 = default(TElement);
				this.ReadValue<TElement>(ref telement2, unsafeValueView3, false);
				container.Add(telement2);
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00009A44 File Offset: 0x00007C44
		void IDictionaryPropertyBagVisitor.Visit<TDictionary, TKey, TValue>(IDictionaryPropertyBag<TDictionary, TKey, TValue> properties, ref TDictionary container)
		{
			UnsafeValueView unsafeValueView = (this.m_Metadata.HasElements ? this.m_View.AsObjectView()["$elements"] : this.m_View);
			container.Clear();
			TokenType type = unsafeValueView.Type;
			if (type != TokenType.Object)
			{
				if (type != TokenType.Array)
				{
					return;
				}
				using (IEnumerator<UnsafeValueView> enumerator = unsafeValueView.AsArrayView().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UnsafeValueView unsafeValueView2 = enumerator.Current;
						if (unsafeValueView2.Type == TokenType.Object)
						{
							UnsafeObjectView unsafeObjectView = unsafeValueView2.AsObjectView();
							UnsafeValueView unsafeValueView3;
							UnsafeValueView unsafeValueView4;
							if (unsafeObjectView.TryGetValue("Key", out unsafeValueView3) && unsafeObjectView.TryGetValue("Value", out unsafeValueView4))
							{
								TKey tkey = default(TKey);
								this.ReadValue<TKey>(ref tkey, unsafeValueView3, false);
								TValue tvalue = default(TValue);
								this.ReadValue<TValue>(ref tvalue, unsafeValueView4, false);
								container.Add(tkey, tvalue);
							}
						}
					}
					return;
				}
			}
			foreach (UnsafeMemberView unsafeMemberView in unsafeValueView.AsObjectView())
			{
				TKey tkey2 = default(TKey);
				this.ReadValue<TKey>(ref tkey2, unsafeMemberView.Key(), false);
				TValue tvalue2 = default(TValue);
				this.ReadValue<TValue>(ref tvalue2, unsafeMemberView.Value(), false);
				container.Add(tkey2, tvalue2);
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00009BD4 File Offset: 0x00007DD4
		void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
		{
			TValue value = property.GetValue(ref container);
			bool flag = property is IPropertyWrapper;
			this.ReadValue<TValue>(ref value, this.m_View, flag);
			if (!property.IsReadOnly)
			{
				property.SetValue(ref container, value);
				return;
			}
			string text;
			if (PropertyChecks.CheckReadOnlyPropertyForDeserialization<TContainer, TValue>(property, ref container, ref value, out text))
			{
				this.m_SerializedTypeProvider.Events.Add(new DeserializationEvent(EventType.Exception, new SerializationException(text)));
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00009C3C File Offset: 0x00007E3C
		internal void ReadValue<TValue>(ref TValue value, UnsafeValueView view, bool isRoot = false)
		{
			JsonAdapterFilter jsonAdapterFilter = JsonAdapterFilter.All;
			if (isRoot && this.m_DisableRootAdapters)
			{
				jsonAdapterFilter = JsonAdapterFilter.None;
			}
			else if (TypeTraits<TValue>.IsPrimitiveOrString)
			{
				jsonAdapterFilter &= ~JsonAdapterFilter.Internal;
				if (!this.m_HasPrimitiveOrStringGlobalAdapters)
				{
					jsonAdapterFilter &= ~JsonAdapterFilter.Global;
				}
				if (!this.m_HasPrimitiveOrStringUserDefinedAdapters)
				{
					jsonAdapterFilter &= ~JsonAdapterFilter.UserDefined;
				}
			}
			this.ReadValueWithAdapters<TValue>(ref value, view, this.m_Adapters.GetEnumerator(jsonAdapterFilter), isRoot);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00009C98 File Offset: 0x00007E98
		internal void ReadValueWithAdapters<TValue>(ref TValue value, UnsafeValueView view, JsonAdapterCollection.Enumerator adapters, bool isRoot = false)
		{
			while (adapters.MoveNext())
			{
				IJsonAdapter jsonAdapter = adapters.Current;
				IJsonAdapter<TValue> jsonAdapter2 = jsonAdapter as IJsonAdapter<TValue>;
				if (jsonAdapter2 != null)
				{
					try
					{
						IJsonAdapter<TValue> jsonAdapter3 = jsonAdapter2;
						JsonDeserializationContext<TValue> jsonDeserializationContext = new JsonDeserializationContext<TValue>(this, adapters, value, view, isRoot);
						value = jsonAdapter3.Deserialize(in jsonDeserializationContext);
					}
					catch (Exception ex)
					{
						this.m_SerializedTypeProvider.Events.Add(new DeserializationEvent(EventType.Exception, ex));
					}
					return;
				}
				IContravariantJsonAdapter<TValue> contravariantJsonAdapter = jsonAdapter as IContravariantJsonAdapter<TValue>;
				if (contravariantJsonAdapter != null)
				{
					try
					{
						value = (TValue)((object)contravariantJsonAdapter.Deserialize(new JsonDeserializationContext<TValue>(this, adapters, value, view, isRoot)));
					}
					catch (Exception ex2)
					{
						this.m_SerializedTypeProvider.Events.Add(new DeserializationEvent(EventType.Exception, ex2));
					}
					return;
				}
			}
			this.ReadValueWithoutAdapters<TValue>(ref value, view, isRoot);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00009D84 File Offset: 0x00007F84
		internal void ReadValueWithoutAdapters<TValue>(ref TValue value, UnsafeValueView view, bool isRoot = false)
		{
			TokenType type = view.Type;
			if (type != TokenType.String)
			{
				if (type != TokenType.Primitive)
				{
					JsonPropertyReader.SerializedContainerMetadata serializedContainerMetadata = ((view.Type == TokenType.Object) ? JsonPropertyReader.GetSerializedContainerMetadata(view.AsObjectView()) : default(JsonPropertyReader.SerializedContainerMetadata));
					this.m_SerializedTypeProvider.View = view;
					this.m_SerializedTypeProvider.SerializedType = (isRoot ? this.m_SerializedType : null);
					if (TypeTraits<TValue>.IsNullable)
					{
						this.m_SerializedTypeProvider.SerializedType = Nullable.GetUnderlyingType(typeof(TValue));
					}
					if (serializedContainerMetadata.IsSerializedReference)
					{
						if (this.m_SerializedReferences == null)
						{
							this.m_SerializedTypeProvider.Events.Add(new DeserializationEvent(EventType.Exception, new Exception("Deserialization encountered a serialized object reference while running with DisableSerializedReferences.")));
							return;
						}
						value = (TValue)((object)this.m_SerializedReferences.GetDeserializedReference(serializedContainerMetadata.SerializedId));
						return;
					}
					else
					{
						try
						{
							DefaultTypeConstruction.Construct<TValue>(ref value, this.m_SerializedTypeProvider);
						}
						catch (ArgumentException ex)
						{
							this.m_SerializedTypeProvider.Events.Add(new DeserializationEvent(EventType.Exception, new ArgumentException(ex.Message)));
							return;
						}
						if (serializedContainerMetadata.HasSerializedId)
						{
							SerializedReferences serializedReferences = this.m_SerializedReferences;
							if (serializedReferences != null)
							{
								serializedReferences.AddDeserializedReference<TValue>(serializedContainerMetadata.SerializedId, value);
							}
						}
						using (new JsonPropertyReader.SerializedContainerMetadataScope(this, serializedContainerMetadata))
						{
							using (new JsonPropertyReader.UnsafeViewScope(this, view))
							{
								VisitReturnCode visitReturnCode2;
								if (TypeTraits<TValue>.IsNullable)
								{
									Type underlyingType = Nullable.GetUnderlyingType(typeof(TValue));
									object obj = Convert.ChangeType(value, underlyingType);
									VisitReturnCode visitReturnCode;
									if (!PropertyContainer.TryAccept<object>(this, ref obj, out visitReturnCode, default(VisitParameters)))
									{
										switch (visitReturnCode)
										{
										case VisitReturnCode.NullContainer:
											throw new ArgumentNullException("value");
										case VisitReturnCode.InvalidContainerType:
											throw new InvalidContainerTypeException(value.GetType());
										case VisitReturnCode.MissingPropertyBag:
											throw new MissingPropertyBagException(value.GetType());
										default:
											throw new Exception(string.Format("Unexpected {0}=[{1}]", "VisitReturnCode", visitReturnCode));
										}
									}
									else
									{
										value = (TValue)((object)obj);
									}
								}
								else if (!PropertyContainer.TryAccept<TValue>(this, ref value, out visitReturnCode2, default(VisitParameters)))
								{
									switch (visitReturnCode2)
									{
									case VisitReturnCode.NullContainer:
										throw new ArgumentNullException("value");
									case VisitReturnCode.InvalidContainerType:
										throw new InvalidContainerTypeException(value.GetType());
									case VisitReturnCode.MissingPropertyBag:
										throw new MissingPropertyBagException(value.GetType());
									default:
										throw new Exception(string.Format("Unexpected {0}=[{1}]", "VisitReturnCode", visitReturnCode2));
									}
								}
							}
						}
					}
				}
				else
				{
					UnsafePrimitiveView unsafePrimitiveView = view.AsPrimitiveView();
					if (unsafePrimitiveView.IsIntegral())
					{
						if (unsafePrimitiveView.IsSigned())
						{
							long num = unsafePrimitiveView.AsInt64();
							TypeConversion.TryConvert<long, TValue>(ref num, out value);
							return;
						}
						ulong num2 = unsafePrimitiveView.AsUInt64();
						TypeConversion.TryConvert<ulong, TValue>(ref num2, out value);
						return;
					}
					else
					{
						if (unsafePrimitiveView.IsDecimal() || unsafePrimitiveView.IsInfinity() || unsafePrimitiveView.IsNaN())
						{
							float num3 = unsafePrimitiveView.AsFloat();
							TypeConversion.TryConvert<float, TValue>(ref num3, out value);
							return;
						}
						if (unsafePrimitiveView.IsBoolean())
						{
							bool flag = unsafePrimitiveView.AsBoolean();
							TypeConversion.TryConvert<bool, TValue>(ref flag, out value);
							return;
						}
						if (unsafePrimitiveView.IsNull())
						{
							value = default(TValue);
							return;
						}
					}
				}
				return;
			}
			string text = view.AsStringView().ToString();
			TypeConversion.TryConvert<string, TValue>(ref text, out value);
		}

		// Token: 0x0400010E RID: 270
		private UnsafeValueView m_View;

		// Token: 0x0400010F RID: 271
		private Type m_SerializedType;

		// Token: 0x04000110 RID: 272
		private bool m_DisableRootAdapters;

		// Token: 0x04000111 RID: 273
		private bool m_DisableRootMigration;

		// Token: 0x04000112 RID: 274
		private JsonAdapterCollection m_Adapters;

		// Token: 0x04000113 RID: 275
		private JsonMigrationCollection m_Migrations;

		// Token: 0x04000114 RID: 276
		private SerializedReferences m_SerializedReferences;

		// Token: 0x04000115 RID: 277
		private JsonPropertyReader.SerializedContainerMetadata m_Metadata;

		// Token: 0x04000116 RID: 278
		private readonly JsonPropertyReader.SerializedTypeProvider m_SerializedTypeProvider;

		// Token: 0x04000117 RID: 279
		private bool m_HasPrimitiveOrStringGlobalAdapters;

		// Token: 0x04000118 RID: 280
		private bool m_HasPrimitiveOrStringUserDefinedAdapters;

		// Token: 0x02000059 RID: 89
		private class SerializedTypeProvider : ISerializedTypeProvider
		{
			// Token: 0x0600023E RID: 574 RVA: 0x0000A128 File Offset: 0x00008328
			public Type GetSerializedType()
			{
				if (this.SerializedType != null)
				{
					return this.SerializedType;
				}
				UnsafeValueView unsafeValueView;
				if (this.View.Type != TokenType.Object || !this.View.AsObjectView().TryGetValue("$type", out unsafeValueView))
				{
					return null;
				}
				if (unsafeValueView.Type != TokenType.String)
				{
					throw new ArgumentException("Failed to construct type. Property=[$type] is expected to be a string.");
				}
				string text = unsafeValueView.AsStringView().ToString();
				if (string.IsNullOrEmpty(text))
				{
					throw new ArgumentException("Failed to construct type. Property=[$type] is expected to be a fully qualified type name.");
				}
				Type type = Type.GetType(text);
				if (null == type)
				{
					string text2;
					if (FormerNameAttribute.TryGetCurrentTypeName(text, out text2))
					{
						type = Type.GetType(text2);
					}
					if (null == type)
					{
						throw new ArgumentException("Failed to construct type. Could not resolve type from TypeName=[" + text + "].");
					}
					this.Events.Add(new DeserializationEvent(EventType.Log, string.Concat(new string[] { "Type construction encountered a type name remap OldType=[", text, "] CurrentType=[", text2, "]" })));
				}
				return type;
			}

			// Token: 0x0600023F RID: 575 RVA: 0x0000A234 File Offset: 0x00008434
			public int GetArrayLength()
			{
				if (this.View.Type == TokenType.Array)
				{
					return this.View.AsArrayView().Count();
				}
				UnsafeValueView unsafeValueView;
				if (this.View.Type == TokenType.Object && this.View.AsObjectView().TryGetValue("$elements", out unsafeValueView))
				{
					return unsafeValueView.AsArrayView().Count();
				}
				return 0;
			}

			// Token: 0x06000240 RID: 576 RVA: 0x0000A2A0 File Offset: 0x000084A0
			public object GetDefaultObject()
			{
				TokenType type = this.View.Type;
				if (type == TokenType.Object)
				{
					return new JsonObject();
				}
				if (type != TokenType.Array)
				{
					return null;
				}
				return new JsonArray();
			}

			// Token: 0x04000119 RID: 281
			public List<DeserializationEvent> Events;

			// Token: 0x0400011A RID: 282
			public UnsafeValueView View;

			// Token: 0x0400011B RID: 283
			public Type SerializedType;
		}

		// Token: 0x0200005A RID: 90
		private struct SerializedContainerMetadata
		{
			// Token: 0x0400011C RID: 284
			public bool HasSerializedId;

			// Token: 0x0400011D RID: 285
			public bool HasSerializedTypeInfo;

			// Token: 0x0400011E RID: 286
			public bool HasSerializedVersion;

			// Token: 0x0400011F RID: 287
			public bool HasElements;

			// Token: 0x04000120 RID: 288
			public bool IsSerializedReference;

			// Token: 0x04000121 RID: 289
			public int SerializedId;
		}

		// Token: 0x0200005B RID: 91
		private struct SerializedContainerMetadataScope : IDisposable
		{
			// Token: 0x06000242 RID: 578 RVA: 0x0000A2D0 File Offset: 0x000084D0
			public SerializedContainerMetadataScope(JsonPropertyReader visitor, JsonPropertyReader.SerializedContainerMetadata metadata)
			{
				this.m_Visitor = visitor;
				this.m_Metadata = this.m_Visitor.m_Metadata;
				this.m_Visitor.m_Metadata = metadata;
			}

			// Token: 0x06000243 RID: 579 RVA: 0x0000A2F6 File Offset: 0x000084F6
			public void Dispose()
			{
				this.m_Visitor.m_Metadata = this.m_Metadata;
			}

			// Token: 0x04000122 RID: 290
			private readonly JsonPropertyReader m_Visitor;

			// Token: 0x04000123 RID: 291
			private readonly JsonPropertyReader.SerializedContainerMetadata m_Metadata;
		}

		// Token: 0x0200005C RID: 92
		internal readonly struct UnsafeViewScope : IDisposable
		{
			// Token: 0x06000244 RID: 580 RVA: 0x0000A309 File Offset: 0x00008509
			public UnsafeViewScope(JsonPropertyReader visitor, UnsafeValueView view)
			{
				this.m_View = visitor.m_View;
				this.m_Visitor = visitor;
				this.m_Visitor.m_View = view;
			}

			// Token: 0x06000245 RID: 581 RVA: 0x0000A32A File Offset: 0x0000852A
			public void Dispose()
			{
				this.m_Visitor.m_View = this.m_View;
			}

			// Token: 0x04000124 RID: 292
			private readonly JsonPropertyReader m_Visitor;

			// Token: 0x04000125 RID: 293
			private readonly UnsafeValueView m_View;
		}

		// Token: 0x0200005D RID: 93
		internal readonly struct DisableRootMigrationScope : IDisposable
		{
			// Token: 0x06000246 RID: 582 RVA: 0x0000A33D File Offset: 0x0000853D
			public DisableRootMigrationScope(JsonPropertyReader visitor, bool disableRootMigration)
			{
				this.m_DisableRootMigration = visitor.m_DisableRootMigration;
				this.m_Visitor = visitor;
				this.m_Visitor.m_DisableRootMigration = disableRootMigration;
			}

			// Token: 0x06000247 RID: 583 RVA: 0x0000A35E File Offset: 0x0000855E
			public void Dispose()
			{
				this.m_Visitor.m_DisableRootMigration = this.m_DisableRootMigration;
			}

			// Token: 0x04000126 RID: 294
			private readonly JsonPropertyReader m_Visitor;

			// Token: 0x04000127 RID: 295
			private readonly bool m_DisableRootMigration;
		}

		// Token: 0x0200005E RID: 94
		internal readonly struct SerializedTypeScope : IDisposable
		{
			// Token: 0x06000248 RID: 584 RVA: 0x0000A371 File Offset: 0x00008571
			public SerializedTypeScope(JsonPropertyReader visitor, Type type)
			{
				this.m_SerializedType = visitor.m_SerializedType;
				this.m_Visitor = visitor;
				this.m_Visitor.m_SerializedType = type;
			}

			// Token: 0x06000249 RID: 585 RVA: 0x0000A392 File Offset: 0x00008592
			public void Dispose()
			{
				this.m_Visitor.m_SerializedType = this.m_SerializedType;
			}

			// Token: 0x04000128 RID: 296
			private readonly JsonPropertyReader m_Visitor;

			// Token: 0x04000129 RID: 297
			private readonly Type m_SerializedType;
		}
	}
}
