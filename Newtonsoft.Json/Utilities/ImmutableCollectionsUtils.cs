using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005B RID: 91
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ImmutableCollectionsUtils
	{
		// Token: 0x06000531 RID: 1329 RVA: 0x00015A0C File Offset: 0x00013C0C
		internal static bool TryBuildImmutableForArrayContract(Type underlyingType, Type collectionItemType, [Nullable(2)] [NotNullWhen(true)] out Type createdType, [Nullable(new byte[] { 2, 1 })] [NotNullWhen(true)] out ObjectConstructor<object> parameterizedCreator)
		{
			if (underlyingType.IsGenericType())
			{
				Type genericTypeDefinition = underlyingType.GetGenericTypeDefinition();
				string name = genericTypeDefinition.FullName;
				ImmutableCollectionsUtils.ImmutableCollectionTypeInfo immutableCollectionTypeInfo = ImmutableCollectionsUtils.ArrayContractImmutableCollectionDefinitions.FirstOrDefault<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>((ImmutableCollectionsUtils.ImmutableCollectionTypeInfo d) => d.ContractTypeName == name);
				if (immutableCollectionTypeInfo != null)
				{
					Type type = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.CreatedTypeName);
					Type type2 = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.BuilderTypeName);
					if (type != null && type2 != null)
					{
						MethodInfo methodInfo = type2.GetMethods().FirstOrDefault<MethodInfo>((MethodInfo m) => m.Name == "CreateRange" && m.GetParameters().Length == 1);
						if (methodInfo != null)
						{
							createdType = type.MakeGenericType(new Type[] { collectionItemType });
							MethodInfo methodInfo2 = methodInfo.MakeGenericMethod(new Type[] { collectionItemType });
							parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(methodInfo2);
							return true;
						}
					}
				}
			}
			createdType = null;
			parameterizedCreator = null;
			return false;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00015B08 File Offset: 0x00013D08
		internal static bool TryBuildImmutableForDictionaryContract(Type underlyingType, Type keyItemType, Type valueItemType, [Nullable(2)] [NotNullWhen(true)] out Type createdType, [Nullable(new byte[] { 2, 1 })] [NotNullWhen(true)] out ObjectConstructor<object> parameterizedCreator)
		{
			if (underlyingType.IsGenericType())
			{
				Type genericTypeDefinition = underlyingType.GetGenericTypeDefinition();
				string name = genericTypeDefinition.FullName;
				ImmutableCollectionsUtils.ImmutableCollectionTypeInfo immutableCollectionTypeInfo = ImmutableCollectionsUtils.DictionaryContractImmutableCollectionDefinitions.FirstOrDefault<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>((ImmutableCollectionsUtils.ImmutableCollectionTypeInfo d) => d.ContractTypeName == name);
				if (immutableCollectionTypeInfo != null)
				{
					Type type = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.CreatedTypeName);
					Type type2 = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.BuilderTypeName);
					if (type != null && type2 != null)
					{
						MethodInfo methodInfo = type2.GetMethods().FirstOrDefault<MethodInfo>(delegate(MethodInfo m)
						{
							ParameterInfo[] parameters = m.GetParameters();
							return m.Name == "CreateRange" && parameters.Length == 1 && parameters[0].ParameterType.IsGenericType() && parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
						});
						if (methodInfo != null)
						{
							createdType = type.MakeGenericType(new Type[] { keyItemType, valueItemType });
							MethodInfo methodInfo2 = methodInfo.MakeGenericMethod(new Type[] { keyItemType, valueItemType });
							parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(methodInfo2);
							return true;
						}
					}
				}
			}
			createdType = null;
			parameterizedCreator = null;
			return false;
		}

		// Token: 0x040001F8 RID: 504
		private const string ImmutableListGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableList`1";

		// Token: 0x040001F9 RID: 505
		private const string ImmutableQueueGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableQueue`1";

		// Token: 0x040001FA RID: 506
		private const string ImmutableStackGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableStack`1";

		// Token: 0x040001FB RID: 507
		private const string ImmutableSetGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableSet`1";

		// Token: 0x040001FC RID: 508
		private const string ImmutableArrayTypeName = "System.Collections.Immutable.ImmutableArray";

		// Token: 0x040001FD RID: 509
		private const string ImmutableArrayGenericTypeName = "System.Collections.Immutable.ImmutableArray`1";

		// Token: 0x040001FE RID: 510
		private const string ImmutableListTypeName = "System.Collections.Immutable.ImmutableList";

		// Token: 0x040001FF RID: 511
		private const string ImmutableListGenericTypeName = "System.Collections.Immutable.ImmutableList`1";

		// Token: 0x04000200 RID: 512
		private const string ImmutableQueueTypeName = "System.Collections.Immutable.ImmutableQueue";

		// Token: 0x04000201 RID: 513
		private const string ImmutableQueueGenericTypeName = "System.Collections.Immutable.ImmutableQueue`1";

		// Token: 0x04000202 RID: 514
		private const string ImmutableStackTypeName = "System.Collections.Immutable.ImmutableStack";

		// Token: 0x04000203 RID: 515
		private const string ImmutableStackGenericTypeName = "System.Collections.Immutable.ImmutableStack`1";

		// Token: 0x04000204 RID: 516
		private const string ImmutableSortedSetTypeName = "System.Collections.Immutable.ImmutableSortedSet";

		// Token: 0x04000205 RID: 517
		private const string ImmutableSortedSetGenericTypeName = "System.Collections.Immutable.ImmutableSortedSet`1";

		// Token: 0x04000206 RID: 518
		private const string ImmutableHashSetTypeName = "System.Collections.Immutable.ImmutableHashSet";

		// Token: 0x04000207 RID: 519
		private const string ImmutableHashSetGenericTypeName = "System.Collections.Immutable.ImmutableHashSet`1";

		// Token: 0x04000208 RID: 520
		private static readonly IList<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo> ArrayContractImmutableCollectionDefinitions = new List<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>
		{
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableList`1", "System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableStack`1", "System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableSet`1", "System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableSortedSet`1", "System.Collections.Immutable.ImmutableSortedSet`1", "System.Collections.Immutable.ImmutableSortedSet"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableArray`1", "System.Collections.Immutable.ImmutableArray`1", "System.Collections.Immutable.ImmutableArray")
		};

		// Token: 0x04000209 RID: 521
		private const string ImmutableDictionaryGenericInterfaceTypeName = "System.Collections.Immutable.IImmutableDictionary`2";

		// Token: 0x0400020A RID: 522
		private const string ImmutableDictionaryTypeName = "System.Collections.Immutable.ImmutableDictionary";

		// Token: 0x0400020B RID: 523
		private const string ImmutableDictionaryGenericTypeName = "System.Collections.Immutable.ImmutableDictionary`2";

		// Token: 0x0400020C RID: 524
		private const string ImmutableSortedDictionaryTypeName = "System.Collections.Immutable.ImmutableSortedDictionary";

		// Token: 0x0400020D RID: 525
		private const string ImmutableSortedDictionaryGenericTypeName = "System.Collections.Immutable.ImmutableSortedDictionary`2";

		// Token: 0x0400020E RID: 526
		private static readonly IList<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo> DictionaryContractImmutableCollectionDefinitions = new List<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>
		{
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableSortedDictionary`2", "System.Collections.Immutable.ImmutableSortedDictionary`2", "System.Collections.Immutable.ImmutableSortedDictionary"),
			new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary")
		};

		// Token: 0x02000180 RID: 384
		[Nullable(0)]
		internal class ImmutableCollectionTypeInfo
		{
			// Token: 0x06000ECA RID: 3786 RVA: 0x0004120F File Offset: 0x0003F40F
			public ImmutableCollectionTypeInfo(string contractTypeName, string createdTypeName, string builderTypeName)
			{
				this.ContractTypeName = contractTypeName;
				this.CreatedTypeName = createdTypeName;
				this.BuilderTypeName = builderTypeName;
			}

			// Token: 0x17000292 RID: 658
			// (get) Token: 0x06000ECB RID: 3787 RVA: 0x0004122C File Offset: 0x0003F42C
			// (set) Token: 0x06000ECC RID: 3788 RVA: 0x00041234 File Offset: 0x0003F434
			public string ContractTypeName { get; set; }

			// Token: 0x17000293 RID: 659
			// (get) Token: 0x06000ECD RID: 3789 RVA: 0x0004123D File Offset: 0x0003F43D
			// (set) Token: 0x06000ECE RID: 3790 RVA: 0x00041245 File Offset: 0x0003F445
			public string CreatedTypeName { get; set; }

			// Token: 0x17000294 RID: 660
			// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0004124E File Offset: 0x0003F44E
			// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x00041256 File Offset: 0x0003F456
			public string BuilderTypeName { get; set; }
		}
	}
}
