using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Unity.Properties.Internal;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Scripting;

namespace Unity.Properties
{
	// Token: 0x02000090 RID: 144
	public static class TypeUtility
	{
		// Token: 0x0600030A RID: 778 RVA: 0x0000AFA8 File Offset: 0x000091A8
		static TypeUtility()
		{
			TypeUtility.SetExplicitInstantiationMethod<string>(() => string.Empty);
			foreach (MethodInfo methodInfo in typeof(TypeUtility).GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
			{
				bool flag = methodInfo.Name != "CreateTypeConstructor" || !methodInfo.IsGenericMethod;
				if (!flag)
				{
					TypeUtility.s_CreateTypeConstructor = methodInfo;
					break;
				}
			}
			bool flag2 = null == TypeUtility.s_CreateTypeConstructor;
			if (flag2)
			{
				throw new InvalidProgramException();
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000B088 File Offset: 0x00009288
		public static string GetTypeDisplayName(Type type)
		{
			string typeDisplayName;
			bool flag = TypeUtility.s_CachedResolvedName.TryGetValue(type, out typeDisplayName);
			string text;
			if (flag)
			{
				text = typeDisplayName;
			}
			else
			{
				int num = 0;
				typeDisplayName = TypeUtility.GetTypeDisplayName(type, type.GetGenericArguments(), ref num);
				TypeUtility.s_CachedResolvedName[type] = typeDisplayName;
				text = typeDisplayName;
			}
			return text;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000B0D0 File Offset: 0x000092D0
		private static string GetTypeDisplayName(Type type, IReadOnlyList<Type> args, ref int argIndex)
		{
			bool flag = type == typeof(int);
			string text;
			if (flag)
			{
				text = "int";
			}
			else
			{
				bool flag2 = type == typeof(uint);
				if (flag2)
				{
					text = "uint";
				}
				else
				{
					bool flag3 = type == typeof(short);
					if (flag3)
					{
						text = "short";
					}
					else
					{
						bool flag4 = type == typeof(ushort);
						if (flag4)
						{
							text = "ushort";
						}
						else
						{
							bool flag5 = type == typeof(byte);
							if (flag5)
							{
								text = "byte";
							}
							else
							{
								bool flag6 = type == typeof(char);
								if (flag6)
								{
									text = "char";
								}
								else
								{
									bool flag7 = type == typeof(bool);
									if (flag7)
									{
										text = "bool";
									}
									else
									{
										bool flag8 = type == typeof(long);
										if (flag8)
										{
											text = "long";
										}
										else
										{
											bool flag9 = type == typeof(ulong);
											if (flag9)
											{
												text = "ulong";
											}
											else
											{
												bool flag10 = type == typeof(float);
												if (flag10)
												{
													text = "float";
												}
												else
												{
													bool flag11 = type == typeof(double);
													if (flag11)
													{
														text = "double";
													}
													else
													{
														bool flag12 = type == typeof(string);
														if (flag12)
														{
															text = "string";
														}
														else
														{
															string text2 = type.Name;
															bool isGenericParameter = type.IsGenericParameter;
															if (isGenericParameter)
															{
																text = text2;
															}
															else
															{
																bool isNested = type.IsNested;
																if (isNested)
																{
																	text2 = TypeUtility.GetTypeDisplayName(type.DeclaringType, args, ref argIndex) + "." + text2;
																}
																bool flag13 = !type.IsGenericType;
																if (flag13)
																{
																	text = text2;
																}
																else
																{
																	int num = text2.IndexOf('`');
																	int num2 = type.GetGenericArguments().Length;
																	bool flag14 = num > -1;
																	if (flag14)
																	{
																		num2 = int.Parse(text2.Substring(num + 1));
																		text2 = text2.Remove(num);
																	}
																	StringBuilder stringBuilder = null;
																	object obj = TypeUtility.syncedPoolObject;
																	lock (obj)
																	{
																		stringBuilder = TypeUtility.s_Builders.Get();
																	}
																	try
																	{
																		int num3 = 0;
																		while (num3 < num2 && argIndex < args.Count)
																		{
																			bool flag16 = num3 != 0;
																			if (flag16)
																			{
																				stringBuilder.Append(", ");
																			}
																			stringBuilder.Append(TypeUtility.GetTypeDisplayName(args[argIndex]));
																			num3++;
																			argIndex++;
																		}
																		bool flag17 = stringBuilder.Length > 0;
																		if (flag17)
																		{
																			text2 = string.Format("{0}<{1}>", text2, stringBuilder);
																		}
																	}
																	finally
																	{
																		object obj2 = TypeUtility.syncedPoolObject;
																		lock (obj2)
																		{
																			TypeUtility.s_Builders.Release(stringBuilder);
																		}
																	}
																	text = text2;
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return text;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000B414 File Offset: 0x00009614
		public static Type GetRootType(this Type type)
		{
			bool isInterface = type.IsInterface;
			Type type2;
			if (isInterface)
			{
				type2 = null;
			}
			else
			{
				Type type3 = (type.IsValueType ? typeof(ValueType) : typeof(object));
				while (type3 != type.BaseType)
				{
					type = type.BaseType;
				}
				type2 = type;
			}
			return type2;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000B470 File Offset: 0x00009670
		[Preserve]
		private static TypeUtility.ITypeConstructor CreateTypeConstructor(Type type)
		{
			IPropertyBag propertyBag = PropertyBagStore.GetPropertyBag(type);
			bool flag = propertyBag != null;
			TypeUtility.ITypeConstructor typeConstructor;
			if (flag)
			{
				TypeUtility.TypeConstructorVisitor typeConstructorVisitor = new TypeUtility.TypeConstructorVisitor();
				propertyBag.Accept(typeConstructorVisitor);
				typeConstructor = typeConstructorVisitor.TypeConstructor;
			}
			else
			{
				bool containsGenericParameters = type.ContainsGenericParameters;
				if (containsGenericParameters)
				{
					TypeUtility.NonConstructable nonConstructable = new TypeUtility.NonConstructable();
					TypeUtility.s_TypeConstructors[type] = nonConstructable;
					typeConstructor = nonConstructable;
				}
				else
				{
					typeConstructor = TypeUtility.s_CreateTypeConstructor.MakeGenericMethod(new Type[] { type }).Invoke(null, null) as TypeUtility.ITypeConstructor;
				}
			}
			return typeConstructor;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000B4F0 File Offset: 0x000096F0
		private static TypeUtility.ITypeConstructor<T> CreateTypeConstructor<T>()
		{
			TypeUtility.TypeConstructor<T> typeConstructor = new TypeUtility.TypeConstructor<T>();
			TypeUtility.Cache<T>.TypeConstructor = typeConstructor;
			TypeUtility.s_TypeConstructors[typeof(T)] = typeConstructor;
			return typeConstructor;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000B528 File Offset: 0x00009728
		private static TypeUtility.ITypeConstructor GetTypeConstructor(Type type)
		{
			TypeUtility.ITypeConstructor typeConstructor;
			return TypeUtility.s_TypeConstructors.TryGetValue(type, out typeConstructor) ? typeConstructor : TypeUtility.CreateTypeConstructor(type);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000B554 File Offset: 0x00009754
		private static TypeUtility.ITypeConstructor<T> GetTypeConstructor<T>()
		{
			return (TypeUtility.Cache<T>.TypeConstructor != null) ? TypeUtility.Cache<T>.TypeConstructor : TypeUtility.CreateTypeConstructor<T>();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000B579 File Offset: 0x00009779
		public static bool CanBeInstantiated(Type type)
		{
			return TypeUtility.GetTypeConstructor(type).CanBeInstantiated;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000B586 File Offset: 0x00009786
		public static bool CanBeInstantiated<T>()
		{
			return TypeUtility.GetTypeConstructor<T>().CanBeInstantiated;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000B592 File Offset: 0x00009792
		public static void SetExplicitInstantiationMethod<T>(Func<T> constructor)
		{
			TypeUtility.GetTypeConstructor<T>().SetExplicitConstructor(constructor);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000B5A0 File Offset: 0x000097A0
		public static T Instantiate<T>()
		{
			TypeUtility.ITypeConstructor<T> typeConstructor = TypeUtility.GetTypeConstructor<T>();
			TypeUtility.CheckCanBeInstantiated<T>(typeConstructor);
			return typeConstructor.Instantiate();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000B5C8 File Offset: 0x000097C8
		public static bool TryInstantiate<T>(out T instance)
		{
			TypeUtility.ITypeConstructor<T> typeConstructor = TypeUtility.GetTypeConstructor<T>();
			bool canBeInstantiated = typeConstructor.CanBeInstantiated;
			bool flag;
			if (canBeInstantiated)
			{
				instance = typeConstructor.Instantiate();
				flag = true;
			}
			else
			{
				instance = default(T);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000B604 File Offset: 0x00009804
		public static T Instantiate<T>(Type derivedType)
		{
			TypeUtility.ITypeConstructor typeConstructor = TypeUtility.GetTypeConstructor(derivedType);
			TypeUtility.CheckIsAssignableFrom(typeof(T), derivedType);
			TypeUtility.CheckCanBeInstantiated(typeConstructor, derivedType);
			return (T)((object)typeConstructor.Instantiate());
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000B644 File Offset: 0x00009844
		public static bool TryInstantiate<T>(Type derivedType, out T value)
		{
			bool flag = !typeof(T).IsAssignableFrom(derivedType);
			bool flag2;
			if (flag)
			{
				value = default(T);
				value = default(T);
				flag2 = false;
			}
			else
			{
				TypeUtility.ITypeConstructor typeConstructor = TypeUtility.GetTypeConstructor(derivedType);
				bool flag3 = !typeConstructor.CanBeInstantiated;
				if (flag3)
				{
					value = default(T);
					flag2 = false;
				}
				else
				{
					value = (T)((object)typeConstructor.Instantiate());
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000B6B4 File Offset: 0x000098B4
		public static TArray InstantiateArray<TArray>(int count = 0)
		{
			bool flag = count < 0;
			if (flag)
			{
				throw new ArgumentException(string.Format("{0}: Cannot construct an array with {1}={2}", "TypeUtility", "count", count));
			}
			IPropertyBag<TArray> propertyBag = PropertyBagStore.GetPropertyBag<TArray>();
			IConstructorWithCount<TArray> constructorWithCount = propertyBag as IConstructorWithCount<TArray>;
			bool flag2 = constructorWithCount != null;
			TArray tarray;
			if (flag2)
			{
				tarray = constructorWithCount.InstantiateWithCount(count);
			}
			else
			{
				Type typeFromHandle = typeof(TArray);
				bool flag3 = !typeFromHandle.IsArray;
				if (flag3)
				{
					throw new ArgumentException("TypeUtility: Cannot construct an array, since " + typeof(TArray).Name + " is not an array type.");
				}
				Type elementType = typeFromHandle.GetElementType();
				bool flag4 = null == elementType;
				if (flag4)
				{
					throw new ArgumentException("TypeUtility: Cannot construct an array, since " + typeof(TArray).Name + ".GetElementType() returned null.");
				}
				tarray = (TArray)((object)Array.CreateInstance(elementType, count));
			}
			return tarray;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public static bool TryInstantiateArray<TArray>(int count, out TArray instance)
		{
			bool flag = count < 0;
			bool flag2;
			if (flag)
			{
				instance = default(TArray);
				flag2 = false;
			}
			else
			{
				IPropertyBag<TArray> propertyBag = PropertyBagStore.GetPropertyBag<TArray>();
				IConstructorWithCount<TArray> constructorWithCount = propertyBag as IConstructorWithCount<TArray>;
				bool flag3 = constructorWithCount != null;
				if (flag3)
				{
					try
					{
						instance = constructorWithCount.InstantiateWithCount(count);
						return true;
					}
					catch
					{
					}
				}
				Type typeFromHandle = typeof(TArray);
				bool flag4 = !typeFromHandle.IsArray;
				if (flag4)
				{
					instance = default(TArray);
					flag2 = false;
				}
				else
				{
					Type elementType = typeFromHandle.GetElementType();
					bool flag5 = null == elementType;
					if (flag5)
					{
						instance = default(TArray);
						flag2 = false;
					}
					else
					{
						instance = (TArray)((object)Array.CreateInstance(elementType, count));
						flag2 = true;
					}
				}
			}
			return flag2;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000B86C File Offset: 0x00009A6C
		public static TArray InstantiateArray<TArray>(Type derivedType, int count = 0)
		{
			bool flag = count < 0;
			if (flag)
			{
				throw new ArgumentException(string.Format("{0}: Cannot instantiate an array with {1}={2}", "TypeUtility", "count", count));
			}
			IPropertyBag propertyBag = PropertyBagStore.GetPropertyBag(derivedType);
			IConstructorWithCount<TArray> constructorWithCount = propertyBag as IConstructorWithCount<TArray>;
			bool flag2 = constructorWithCount != null;
			TArray tarray;
			if (flag2)
			{
				tarray = constructorWithCount.InstantiateWithCount(count);
			}
			else
			{
				Type typeFromHandle = typeof(TArray);
				bool flag3 = !typeFromHandle.IsArray;
				if (flag3)
				{
					throw new ArgumentException("TypeUtility: Cannot instantiate an array, since " + typeof(TArray).Name + " is not an array type.");
				}
				Type elementType = typeFromHandle.GetElementType();
				bool flag4 = null == elementType;
				if (flag4)
				{
					throw new ArgumentException("TypeUtility: Cannot instantiate an array, since " + typeof(TArray).Name + ".GetElementType() returned null.");
				}
				tarray = (TArray)((object)Array.CreateInstance(elementType, count));
			}
			return tarray;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000B958 File Offset: 0x00009B58
		private static void CheckIsAssignableFrom(Type type, Type derivedType)
		{
			bool flag = !type.IsAssignableFrom(derivedType);
			if (flag)
			{
				throw new ArgumentException(string.Concat(new string[] { "Could not create instance of type `", derivedType.Name, "` and convert to `", type.Name, "`: The given type is not assignable to target type." }));
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000B9B0 File Offset: 0x00009BB0
		private static void CheckCanBeInstantiated<T>(TypeUtility.ITypeConstructor<T> constructor)
		{
			bool flag = !constructor.CanBeInstantiated;
			if (flag)
			{
				throw new InvalidOperationException("Type `" + typeof(T).Name + "` could not be instantiated. A parameter-less constructor or an explicit construction method is required.");
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000B9F0 File Offset: 0x00009BF0
		private static void CheckCanBeInstantiated(TypeUtility.ITypeConstructor constructor, Type type)
		{
			bool flag = !constructor.CanBeInstantiated;
			if (flag)
			{
				throw new InvalidOperationException("Type `" + type.Name + "` could not be instantiated. A parameter-less constructor or an explicit construction method is required.");
			}
		}

		// Token: 0x04000178 RID: 376
		private static readonly ConcurrentDictionary<Type, TypeUtility.ITypeConstructor> s_TypeConstructors = new ConcurrentDictionary<Type, TypeUtility.ITypeConstructor>();

		// Token: 0x04000179 RID: 377
		private static readonly MethodInfo s_CreateTypeConstructor;

		// Token: 0x0400017A RID: 378
		private static readonly ConcurrentDictionary<Type, string> s_CachedResolvedName = new ConcurrentDictionary<Type, string>();

		// Token: 0x0400017B RID: 379
		private static readonly ObjectPool<StringBuilder> s_Builders = new ObjectPool<StringBuilder>(() => new StringBuilder(), null, delegate(StringBuilder sb)
		{
			sb.Clear();
		}, null, true, 10, 10000);

		// Token: 0x0400017C RID: 380
		private static readonly object syncedPoolObject = new object();

		// Token: 0x02000091 RID: 145
		private interface ITypeConstructor
		{
			// Token: 0x17000063 RID: 99
			// (get) Token: 0x0600031F RID: 799
			bool CanBeInstantiated { get; }

			// Token: 0x06000320 RID: 800
			object Instantiate();
		}

		// Token: 0x02000092 RID: 146
		private interface ITypeConstructor<T> : TypeUtility.ITypeConstructor
		{
			// Token: 0x06000321 RID: 801
			T Instantiate();

			// Token: 0x06000322 RID: 802
			void SetExplicitConstructor(Func<T> constructor);
		}

		// Token: 0x02000093 RID: 147
		private class TypeConstructor<T> : TypeUtility.ITypeConstructor<T>, TypeUtility.ITypeConstructor
		{
			// Token: 0x17000064 RID: 100
			// (get) Token: 0x06000323 RID: 803 RVA: 0x0000BA28 File Offset: 0x00009C28
			bool TypeUtility.ITypeConstructor.CanBeInstantiated
			{
				get
				{
					bool flag = this.m_ExplicitConstructor != null;
					bool flag2;
					if (flag)
					{
						flag2 = true;
					}
					else
					{
						bool flag3 = this.m_OverrideConstructor != null;
						if (flag3)
						{
							bool flag4 = this.m_OverrideConstructor.InstantiationKind == InstantiationKind.NotInstantiatable;
							if (flag4)
							{
								return false;
							}
							bool flag5 = this.m_OverrideConstructor.InstantiationKind == InstantiationKind.PropertyBagOverride;
							if (flag5)
							{
								return true;
							}
						}
						flag2 = this.m_ImplicitConstructor != null;
					}
					return flag2;
				}
			}

			// Token: 0x06000324 RID: 804 RVA: 0x0000BA91 File Offset: 0x00009C91
			public TypeConstructor()
			{
				this.m_OverrideConstructor = PropertyBagStore.GetPropertyBag<T>() as IConstructor<T>;
				this.SetImplicitConstructor();
			}

			// Token: 0x06000325 RID: 805 RVA: 0x0000BAB4 File Offset: 0x00009CB4
			private void SetImplicitConstructor()
			{
				Type typeFromHandle = typeof(T);
				bool isValueType = typeFromHandle.IsValueType;
				if (isValueType)
				{
					this.m_ImplicitConstructor = new Func<T>(TypeUtility.TypeConstructor<T>.CreateValueTypeInstance);
				}
				else
				{
					bool isAbstract = typeFromHandle.IsAbstract;
					if (!isAbstract)
					{
						bool flag = typeof(ScriptableObject).IsAssignableFrom(typeFromHandle);
						if (flag)
						{
							this.m_ImplicitConstructor = new Func<T>(TypeUtility.TypeConstructor<T>.CreateScriptableObjectInstance);
						}
						else
						{
							bool flag2 = null != typeFromHandle.GetConstructor(Array.Empty<Type>());
							if (flag2)
							{
								this.m_ImplicitConstructor = new Func<T>(TypeUtility.TypeConstructor<T>.CreateClassInstance);
							}
						}
					}
				}
			}

			// Token: 0x06000326 RID: 806 RVA: 0x0000BB50 File Offset: 0x00009D50
			private static T CreateValueTypeInstance()
			{
				return default(T);
			}

			// Token: 0x06000327 RID: 807 RVA: 0x0000BB6C File Offset: 0x00009D6C
			private static T CreateScriptableObjectInstance()
			{
				return (T)((object)ScriptableObject.CreateInstance(typeof(T)));
			}

			// Token: 0x06000328 RID: 808 RVA: 0x0000BB94 File Offset: 0x00009D94
			private static T CreateClassInstance()
			{
				return Activator.CreateInstance<T>();
			}

			// Token: 0x06000329 RID: 809 RVA: 0x0000BBAB File Offset: 0x00009DAB
			public void SetExplicitConstructor(Func<T> constructor)
			{
				this.m_ExplicitConstructor = constructor;
			}

			// Token: 0x0600032A RID: 810 RVA: 0x0000BBB8 File Offset: 0x00009DB8
			T TypeUtility.ITypeConstructor<T>.Instantiate()
			{
				bool flag = this.m_ExplicitConstructor != null;
				T t;
				if (flag)
				{
					t = this.m_ExplicitConstructor();
				}
				else
				{
					bool flag2 = this.m_OverrideConstructor != null;
					if (flag2)
					{
						bool flag3 = this.m_OverrideConstructor.InstantiationKind == InstantiationKind.NotInstantiatable;
						if (flag3)
						{
							throw new InvalidOperationException("The type '" + typeof(T).Name + "' is not constructable.");
						}
						bool flag4 = this.m_OverrideConstructor.InstantiationKind == InstantiationKind.PropertyBagOverride;
						if (flag4)
						{
							return this.m_OverrideConstructor.Instantiate();
						}
					}
					bool flag5 = this.m_ImplicitConstructor != null;
					if (!flag5)
					{
						throw new InvalidOperationException("The type '" + typeof(T).Name + "' is not constructable.");
					}
					t = this.m_ImplicitConstructor();
				}
				return t;
			}

			// Token: 0x0600032B RID: 811 RVA: 0x0000BC8E File Offset: 0x00009E8E
			object TypeUtility.ITypeConstructor.Instantiate()
			{
				return ((TypeUtility.ITypeConstructor<T>)this).Instantiate();
			}

			// Token: 0x0400017D RID: 381
			private Func<T> m_ExplicitConstructor;

			// Token: 0x0400017E RID: 382
			private Func<T> m_ImplicitConstructor;

			// Token: 0x0400017F RID: 383
			private IConstructor<T> m_OverrideConstructor;
		}

		// Token: 0x02000094 RID: 148
		private class NonConstructable : TypeUtility.ITypeConstructor
		{
			// Token: 0x17000065 RID: 101
			// (get) Token: 0x0600032C RID: 812 RVA: 0x000057E1 File Offset: 0x000039E1
			bool TypeUtility.ITypeConstructor.CanBeInstantiated
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600032D RID: 813 RVA: 0x0000BC9B File Offset: 0x00009E9B
			public object Instantiate()
			{
				throw new InvalidOperationException("The type is not instantiatable.");
			}
		}

		// Token: 0x02000095 RID: 149
		private struct Cache<T>
		{
			// Token: 0x04000180 RID: 384
			public static TypeUtility.ITypeConstructor<T> TypeConstructor;
		}

		// Token: 0x02000096 RID: 150
		private class TypeConstructorVisitor : ITypeVisitor
		{
			// Token: 0x0600032F RID: 815 RVA: 0x0000BCA7 File Offset: 0x00009EA7
			public void Visit<TContainer>()
			{
				this.TypeConstructor = TypeUtility.CreateTypeConstructor<TContainer>();
			}

			// Token: 0x04000181 RID: 385
			public TypeUtility.ITypeConstructor TypeConstructor;
		}
	}
}
