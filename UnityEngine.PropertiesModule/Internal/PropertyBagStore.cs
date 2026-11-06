using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Unity.Properties.Internal
{
	// Token: 0x020000C8 RID: 200
	internal static class PropertyBagStore
	{
		// Token: 0x060003F2 RID: 1010 RVA: 0x0000C28B File Offset: 0x0000A48B
		static PropertyBagStore()
		{
			DefaultPropertyBagInitializer.Initialize();
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060003F3 RID: 1011 RVA: 0x0000C2B8 File Offset: 0x0000A4B8
		// (remove) Token: 0x060003F4 RID: 1012 RVA: 0x0000C2EC File Offset: 0x0000A4EC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<Type, IPropertyBag> NewTypeRegistered;

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0000C31F File Offset: 0x0000A51F
		internal static bool HasProvider
		{
			get
			{
				return PropertyBagStore.s_PropertyBagProvider != null;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000C329 File Offset: 0x0000A529
		internal static List<Type> AllTypes
		{
			get
			{
				return PropertyBagStore.s_RegisteredTypes;
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000C330 File Offset: 0x0000A530
		internal static void AddPropertyBag<TContainer>(IPropertyBag<TContainer> propertyBag)
		{
			bool flag = !TypeTraits<TContainer>.IsContainer;
			if (flag)
			{
				throw new Exception(string.Format("PropertyBagStore Type=[{0}] is not a valid container type. Type can not be primitive, enum or string.", typeof(TContainer)));
			}
			bool isAbstractOrInterface = TypeTraits<TContainer>.IsAbstractOrInterface;
			if (isAbstractOrInterface)
			{
				throw new Exception(string.Format("PropertyBagStore Type=[{0}] is not a valid container type. Type can not be abstract or interface.", typeof(TContainer)));
			}
			bool flag2 = PropertyBagStore.TypedStore<TContainer>.PropertyBag != null;
			if (flag2)
			{
				IPropertyBag<TContainer> propertyBag2 = PropertyBagStore.TypedStore<TContainer>.PropertyBag;
				bool flag3 = propertyBag2.GetType().Assembly == typeof(TContainer).Assembly;
				if (flag3)
				{
					return;
				}
				bool flag4 = propertyBag.GetType().GetCustomAttributes<CompilerGeneratedAttribute>().Any<CompilerGeneratedAttribute>();
				if (flag4)
				{
					bool flag5 = propertyBag.GetType().Assembly != typeof(TContainer).Assembly;
					if (flag5)
					{
						return;
					}
				}
			}
			PropertyBagStore.TypedStore<TContainer>.PropertyBag = propertyBag;
			bool flag6 = !PropertyBagStore.s_PropertyBags.ContainsKey(typeof(TContainer));
			if (flag6)
			{
				PropertyBagStore.s_RegisteredTypes.Add(typeof(TContainer));
			}
			PropertyBagStore.s_PropertyBags[typeof(TContainer)] = propertyBag;
			Action<Type, IPropertyBag> newTypeRegistered = PropertyBagStore.NewTypeRegistered;
			if (newTypeRegistered != null)
			{
				newTypeRegistered(typeof(TContainer), propertyBag);
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000C474 File Offset: 0x0000A674
		internal static IPropertyBag<TContainer> GetPropertyBag<TContainer>()
		{
			bool flag = PropertyBagStore.TypedStore<TContainer>.PropertyBag != null;
			IPropertyBag<TContainer> propertyBag;
			if (flag)
			{
				propertyBag = PropertyBagStore.TypedStore<TContainer>.PropertyBag;
			}
			else
			{
				IPropertyBag propertyBag2 = PropertyBagStore.GetPropertyBag(typeof(TContainer));
				bool flag2 = propertyBag2 == null;
				if (flag2)
				{
					propertyBag = null;
				}
				else
				{
					IPropertyBag<TContainer> propertyBag3 = propertyBag2 as IPropertyBag<TContainer>;
					bool flag3 = propertyBag3 == null;
					if (flag3)
					{
						throw new InvalidOperationException("PropertyBag type container type mismatch.");
					}
					propertyBag = propertyBag3;
				}
			}
			return propertyBag;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000C4DC File Offset: 0x0000A6DC
		internal static IPropertyBag GetPropertyBag(Type type)
		{
			IPropertyBag propertyBag;
			bool flag = PropertyBagStore.s_PropertyBags.TryGetValue(type, out propertyBag);
			IPropertyBag propertyBag2;
			if (flag)
			{
				propertyBag2 = propertyBag;
			}
			else
			{
				bool flag2 = !TypeTraits.IsContainer(type);
				if (flag2)
				{
					propertyBag2 = null;
				}
				else
				{
					bool flag3 = type.IsArray && type.GetArrayRank() != 1;
					if (flag3)
					{
						propertyBag2 = null;
					}
					else
					{
						bool flag4 = type.IsInterface || type.IsAbstract;
						if (flag4)
						{
							propertyBag2 = null;
						}
						else
						{
							bool flag5 = type == typeof(object);
							if (flag5)
							{
								propertyBag2 = null;
							}
							else
							{
								bool flag6 = PropertyBagStore.s_PropertyBagProvider != null;
								if (flag6)
								{
									propertyBag = PropertyBagStore.s_PropertyBagProvider.CreatePropertyBag(type);
									bool flag7 = propertyBag == null;
									if (!flag7)
									{
										IPropertyBagRegister propertyBagRegister = propertyBag as IPropertyBagRegister;
										if (propertyBagRegister != null)
										{
											propertyBagRegister.Register();
										}
										return propertyBag;
									}
									PropertyBagStore.s_PropertyBags.TryAdd(type, null);
								}
								propertyBag2 = null;
							}
						}
					}
				}
			}
			return propertyBag2;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000C5C8 File Offset: 0x0000A7C8
		internal static bool Exists<TContainer>()
		{
			return PropertyBagStore.TypedStore<TContainer>.PropertyBag != null;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
		internal static bool Exists(Type type)
		{
			return PropertyBagStore.s_PropertyBags.ContainsKey(type);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000C604 File Offset: 0x0000A804
		internal static bool Exists<TContainer>(ref TContainer value)
		{
			bool flag = !TypeTraits<TContainer>.CanBeNull;
			bool flag2;
			if (flag)
			{
				flag2 = PropertyBagStore.GetPropertyBag<TContainer>() != null;
			}
			else
			{
				bool flag3 = EqualityComparer<TContainer>.Default.Equals(value, default(TContainer));
				flag2 = !flag3 && PropertyBagStore.GetPropertyBag(value.GetType()) != null;
			}
			return flag2;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000C664 File Offset: 0x0000A864
		internal static bool TryGetPropertyBagForValue<TValue>(ref TValue value, out IPropertyBag propertyBag)
		{
			bool flag = !TypeTraits<TValue>.IsContainer;
			bool flag2;
			if (flag)
			{
				propertyBag = null;
				flag2 = false;
			}
			else
			{
				bool canBeNull = TypeTraits<TValue>.CanBeNull;
				if (canBeNull)
				{
					bool flag3 = EqualityComparer<TValue>.Default.Equals(value, default(TValue));
					if (flag3)
					{
						propertyBag = PropertyBagStore.GetPropertyBag<TValue>();
						return propertyBag != null;
					}
				}
				bool isValueType = TypeTraits<TValue>.IsValueType;
				if (isValueType)
				{
					propertyBag = PropertyBagStore.GetPropertyBag<TValue>();
					flag2 = propertyBag != null;
				}
				else
				{
					propertyBag = PropertyBagStore.GetPropertyBag(value.GetType());
					flag2 = propertyBag != null;
				}
			}
			return flag2;
		}

		// Token: 0x04000183 RID: 387
		private static readonly ConcurrentDictionary<Type, IPropertyBag> s_PropertyBags = new ConcurrentDictionary<Type, IPropertyBag>();

		// Token: 0x04000184 RID: 388
		private static readonly List<Type> s_RegisteredTypes = new List<Type>();

		// Token: 0x04000186 RID: 390
		private static ReflectedPropertyBagProvider s_PropertyBagProvider = new ReflectedPropertyBagProvider();

		// Token: 0x020000C9 RID: 201
		internal struct TypedStore<TContainer>
		{
			// Token: 0x04000187 RID: 391
			public static IPropertyBag<TContainer> PropertyBag;
		}
	}
}
