using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

namespace Unity.Properties.Internal
{
	// Token: 0x020000CE RID: 206
	internal class ReflectedPropertyBagProvider
	{
		// Token: 0x06000408 RID: 1032 RVA: 0x0000C898 File Offset: 0x0000AA98
		public ReflectedPropertyBagProvider()
		{
			this.m_CreatePropertyMethod = typeof(ReflectedPropertyBagProvider).GetMethod("CreateProperty", BindingFlags.Instance | BindingFlags.NonPublic);
			this.m_CreatePropertyBagMethod = typeof(ReflectedPropertyBagProvider).GetMethods(BindingFlags.Instance | BindingFlags.Public).First<MethodInfo>((MethodInfo x) => x.Name == "CreatePropertyBag" && x.IsGenericMethod);
			this.m_CreateIndexedCollectionPropertyBagMethod = typeof(ReflectedPropertyBagProvider).GetMethod("CreateIndexedCollectionPropertyBag", BindingFlags.Instance | BindingFlags.NonPublic);
			this.m_CreateSetPropertyBagMethod = typeof(ReflectedPropertyBagProvider).GetMethod("CreateSetPropertyBag", BindingFlags.Instance | BindingFlags.NonPublic);
			this.m_CreateKeyValueCollectionPropertyBagMethod = typeof(ReflectedPropertyBagProvider).GetMethod("CreateKeyValueCollectionPropertyBag", BindingFlags.Instance | BindingFlags.NonPublic);
			this.m_CreateKeyValuePairPropertyBagMethod = typeof(ReflectedPropertyBagProvider).GetMethod("CreateKeyValuePairPropertyBag", BindingFlags.Instance | BindingFlags.NonPublic);
			this.m_CreateArrayPropertyBagMethod = typeof(ReflectedPropertyBagProvider).GetMethod("CreateArrayPropertyBag", BindingFlags.Instance | BindingFlags.NonPublic);
			this.m_CreateListPropertyBagMethod = typeof(ReflectedPropertyBagProvider).GetMethod("CreateListPropertyBag", BindingFlags.Instance | BindingFlags.NonPublic);
			this.m_CreateHashSetPropertyBagMethod = typeof(ReflectedPropertyBagProvider).GetMethod("CreateHashSetPropertyBag", BindingFlags.Instance | BindingFlags.NonPublic);
			this.m_CreateDictionaryPropertyBagMethod = typeof(ReflectedPropertyBagProvider).GetMethod("CreateDictionaryPropertyBag", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000C9E4 File Offset: 0x0000ABE4
		public IPropertyBag CreatePropertyBag(Type type)
		{
			bool isGenericTypeDefinition = type.IsGenericTypeDefinition;
			IPropertyBag propertyBag;
			if (isGenericTypeDefinition)
			{
				propertyBag = null;
			}
			else
			{
				propertyBag = (IPropertyBag)this.m_CreatePropertyBagMethod.MakeGenericMethod(new Type[] { type }).Invoke(this, null);
			}
			return propertyBag;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000CA28 File Offset: 0x0000AC28
		public IPropertyBag<TContainer> CreatePropertyBag<TContainer>()
		{
			bool flag = !TypeTraits<TContainer>.IsContainer || TypeTraits<TContainer>.IsObject;
			if (flag)
			{
				throw new InvalidOperationException("Invalid container type.");
			}
			bool isArray = typeof(TContainer).IsArray;
			IPropertyBag<TContainer> propertyBag;
			if (isArray)
			{
				bool flag2 = typeof(TContainer).GetArrayRank() != 1;
				if (flag2)
				{
					throw new InvalidOperationException("Properties does not support multidimensional arrays.");
				}
				propertyBag = (IPropertyBag<TContainer>)this.m_CreateArrayPropertyBagMethod.MakeGenericMethod(new Type[] { typeof(TContainer).GetElementType() }).Invoke(this, new object[0]);
			}
			else
			{
				bool flag3 = typeof(TContainer).IsGenericType && typeof(TContainer).GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
				if (flag3)
				{
					propertyBag = (IPropertyBag<TContainer>)this.m_CreateListPropertyBagMethod.MakeGenericMethod(new Type[] { typeof(TContainer).GetGenericArguments().First<Type>() }).Invoke(this, new object[0]);
				}
				else
				{
					bool flag4 = typeof(TContainer).IsGenericType && typeof(TContainer).GetGenericTypeDefinition().IsAssignableFrom(typeof(HashSet<>));
					if (flag4)
					{
						propertyBag = (IPropertyBag<TContainer>)this.m_CreateHashSetPropertyBagMethod.MakeGenericMethod(new Type[] { typeof(TContainer).GetGenericArguments().First<Type>() }).Invoke(this, new object[0]);
					}
					else
					{
						bool flag5 = typeof(TContainer).IsGenericType && typeof(TContainer).GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<, >));
						if (flag5)
						{
							propertyBag = (IPropertyBag<TContainer>)this.m_CreateDictionaryPropertyBagMethod.MakeGenericMethod(new Type[]
							{
								typeof(TContainer).GetGenericArguments().First<Type>(),
								typeof(TContainer).GetGenericArguments().ElementAt<Type>(1)
							}).Invoke(this, new object[0]);
						}
						else
						{
							bool flag6 = typeof(TContainer).IsGenericType && typeof(TContainer).GetGenericTypeDefinition().IsAssignableFrom(typeof(IList<>));
							if (flag6)
							{
								propertyBag = (IPropertyBag<TContainer>)this.m_CreateIndexedCollectionPropertyBagMethod.MakeGenericMethod(new Type[]
								{
									typeof(TContainer),
									typeof(TContainer).GetGenericArguments().First<Type>()
								}).Invoke(this, new object[0]);
							}
							else
							{
								bool flag7 = typeof(TContainer).IsGenericType && typeof(TContainer).GetGenericTypeDefinition().IsAssignableFrom(typeof(ISet<>));
								if (flag7)
								{
									propertyBag = (IPropertyBag<TContainer>)this.m_CreateSetPropertyBagMethod.MakeGenericMethod(new Type[]
									{
										typeof(TContainer),
										typeof(TContainer).GetGenericArguments().First<Type>()
									}).Invoke(this, new object[0]);
								}
								else
								{
									bool flag8 = typeof(TContainer).IsGenericType && typeof(TContainer).GetGenericTypeDefinition().IsAssignableFrom(typeof(IDictionary<, >));
									if (flag8)
									{
										propertyBag = (IPropertyBag<TContainer>)this.m_CreateKeyValueCollectionPropertyBagMethod.MakeGenericMethod(new Type[]
										{
											typeof(TContainer),
											typeof(TContainer).GetGenericArguments().First<Type>(),
											typeof(TContainer).GetGenericArguments().ElementAt<Type>(1)
										}).Invoke(this, new object[0]);
									}
									else
									{
										bool flag9 = typeof(TContainer).IsGenericType && typeof(TContainer).GetGenericTypeDefinition().IsAssignableFrom(typeof(KeyValuePair<, >));
										if (flag9)
										{
											Type[] array = typeof(TContainer).GetGenericArguments().ToArray<Type>();
											propertyBag = (IPropertyBag<TContainer>)this.m_CreateKeyValuePairPropertyBagMethod.MakeGenericMethod(new Type[]
											{
												array[0],
												array[1]
											}).Invoke(this, new object[0]);
										}
										else
										{
											ReflectedPropertyBag<TContainer> reflectedPropertyBag = new ReflectedPropertyBag<TContainer>();
											foreach (MemberInfo memberInfo in ReflectedPropertyBagProvider.GetPropertyMembers(typeof(TContainer)))
											{
												MemberInfo memberInfo2 = memberInfo;
												MemberInfo memberInfo3 = memberInfo2;
												FieldInfo fieldInfo = memberInfo3 as FieldInfo;
												IMemberInfo memberInfo4;
												if (fieldInfo == null)
												{
													PropertyInfo propertyInfo = memberInfo3 as PropertyInfo;
													if (propertyInfo == null)
													{
														throw new InvalidOperationException();
													}
													memberInfo4 = new PropertyMember(propertyInfo);
												}
												else
												{
													memberInfo4 = new FieldMember(fieldInfo);
												}
												this.m_CreatePropertyMethod.MakeGenericMethod(new Type[]
												{
													typeof(TContainer),
													memberInfo4.ValueType
												}).Invoke(this, new object[] { memberInfo4, reflectedPropertyBag });
											}
											propertyBag = reflectedPropertyBag;
										}
									}
								}
							}
						}
					}
				}
			}
			return propertyBag;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000CF64 File Offset: 0x0000B164
		[Preserve]
		private void CreateProperty<TContainer, TValue>(IMemberInfo member, ReflectedPropertyBag<TContainer> propertyBag)
		{
			bool isPointer = typeof(TValue).IsPointer;
			if (!isPointer)
			{
				propertyBag.AddProperty<TValue>(new ReflectedMemberProperty<TContainer, TValue>(member, member.Name));
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000CF9B File Offset: 0x0000B19B
		[Preserve]
		private IPropertyBag<TList> CreateIndexedCollectionPropertyBag<TList, TElement>() where TList : IList<TElement>
		{
			return new IndexedCollectionPropertyBag<TList, TElement>();
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000CFA2 File Offset: 0x0000B1A2
		[Preserve]
		private IPropertyBag<TSet> CreateSetPropertyBag<TSet, TValue>() where TSet : ISet<TValue>
		{
			return new SetPropertyBagBase<TSet, TValue>();
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000CFA9 File Offset: 0x0000B1A9
		[Preserve]
		private IPropertyBag<TDictionary> CreateKeyValueCollectionPropertyBag<TDictionary, TKey, TValue>() where TDictionary : IDictionary<TKey, TValue>
		{
			return new KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000CFB0 File Offset: 0x0000B1B0
		[Preserve]
		private IPropertyBag<KeyValuePair<TKey, TValue>> CreateKeyValuePairPropertyBag<TKey, TValue>()
		{
			return new KeyValuePairPropertyBag<TKey, TValue>();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000CFB7 File Offset: 0x0000B1B7
		[Preserve]
		private IPropertyBag<TElement[]> CreateArrayPropertyBag<TElement>()
		{
			return new ArrayPropertyBag<TElement>();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000CFBE File Offset: 0x0000B1BE
		[Preserve]
		private IPropertyBag<List<TElement>> CreateListPropertyBag<TElement>()
		{
			return new ListPropertyBag<TElement>();
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000CFC5 File Offset: 0x0000B1C5
		[Preserve]
		private IPropertyBag<HashSet<TElement>> CreateHashSetPropertyBag<TElement>()
		{
			return new HashSetPropertyBag<TElement>();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000CFCC File Offset: 0x0000B1CC
		[Preserve]
		private IPropertyBag<Dictionary<TKey, TValue>> CreateDictionaryPropertyBag<TKey, TValue>()
		{
			return new DictionaryPropertyBag<TKey, TValue>();
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000CFD3 File Offset: 0x0000B1D3
		private static IEnumerable<MemberInfo> GetPropertyMembers(Type type)
		{
			do
			{
				IOrderedEnumerable<MemberInfo> members = from x in type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					orderby x.MetadataToken
					select x;
				foreach (MemberInfo member in members)
				{
					bool flag = member.MemberType != MemberTypes.Field && member.MemberType != MemberTypes.Property;
					if (!flag)
					{
						bool flag2 = member.DeclaringType != type;
						if (!flag2)
						{
							bool flag3 = !ReflectedPropertyBagProvider.IsValidMember(member);
							if (!flag3)
							{
								bool hasDontCreatePropertyAttribute = member.GetCustomAttribute<DontCreatePropertyAttribute>() != null;
								bool hasCreatePropertyAttribute = member.GetCustomAttribute<CreatePropertyAttribute>() != null;
								bool hasNonSerializedAttribute = member.GetCustomAttribute<NonSerializedAttribute>() != null;
								bool hasSerializedFieldAttribute = member.GetCustomAttribute<SerializeField>() != null;
								bool flag4 = hasDontCreatePropertyAttribute;
								if (!flag4)
								{
									bool flag5 = hasCreatePropertyAttribute;
									if (flag5)
									{
										yield return member;
									}
									else
									{
										bool flag6 = hasNonSerializedAttribute;
										if (!flag6)
										{
											bool flag7 = hasSerializedFieldAttribute;
											if (flag7)
											{
												yield return member;
											}
											else
											{
												FieldInfo field = member as FieldInfo;
												bool flag8 = field != null && field.IsPublic;
												if (flag8)
												{
													yield return member;
												}
												field = null;
												member = null;
											}
										}
									}
								}
							}
						}
					}
				}
				IEnumerator<MemberInfo> enumerator = null;
				type = type.BaseType;
				members = null;
			}
			while (type != null && type != typeof(object));
			yield break;
			yield break;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		private static bool IsValidMember(MemberInfo memberInfo)
		{
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			bool flag;
			if (fieldInfo == null)
			{
				PropertyInfo propertyInfo = memberInfo as PropertyInfo;
				flag = propertyInfo != null && (null != propertyInfo.GetMethod && !propertyInfo.GetMethod.IsStatic) && ReflectedPropertyBagProvider.IsValidPropertyType(propertyInfo.PropertyType);
			}
			else
			{
				flag = !fieldInfo.IsStatic && ReflectedPropertyBagProvider.IsValidPropertyType(fieldInfo.FieldType);
			}
			return flag;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000D060 File Offset: 0x0000B260
		private static bool IsValidPropertyType(Type type)
		{
			bool isPointer = type.IsPointer;
			return !isPointer && (!type.IsGenericType || type.GetGenericArguments().All<Type>(new Func<Type, bool>(ReflectedPropertyBagProvider.IsValidPropertyType)));
		}

		// Token: 0x0400018C RID: 396
		private readonly MethodInfo m_CreatePropertyMethod;

		// Token: 0x0400018D RID: 397
		private readonly MethodInfo m_CreatePropertyBagMethod;

		// Token: 0x0400018E RID: 398
		private readonly MethodInfo m_CreateIndexedCollectionPropertyBagMethod;

		// Token: 0x0400018F RID: 399
		private readonly MethodInfo m_CreateSetPropertyBagMethod;

		// Token: 0x04000190 RID: 400
		private readonly MethodInfo m_CreateKeyValueCollectionPropertyBagMethod;

		// Token: 0x04000191 RID: 401
		private readonly MethodInfo m_CreateKeyValuePairPropertyBagMethod;

		// Token: 0x04000192 RID: 402
		private readonly MethodInfo m_CreateArrayPropertyBagMethod;

		// Token: 0x04000193 RID: 403
		private readonly MethodInfo m_CreateListPropertyBagMethod;

		// Token: 0x04000194 RID: 404
		private readonly MethodInfo m_CreateHashSetPropertyBagMethod;

		// Token: 0x04000195 RID: 405
		private readonly MethodInfo m_CreateDictionaryPropertyBagMethod;
	}
}
