using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Diagnostics;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.AttributedModel
{
	// Token: 0x02000104 RID: 260
	internal class AttributedPartCreationInfo : IReflectionPartCreationInfo, ICompositionElement
	{
		// Token: 0x060006CF RID: 1743 RVA: 0x0001514A File Offset: 0x0001334A
		public AttributedPartCreationInfo(Type type, PartCreationPolicyAttribute partCreationPolicy, bool ignoreConstructorImports, ICompositionElement origin)
		{
			Assumes.NotNull<Type>(type);
			this._type = type;
			this._ignoreConstructorImports = ignoreConstructorImports;
			this._partCreationPolicy = partCreationPolicy;
			this._origin = origin;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00015175 File Offset: 0x00013375
		public Type GetPartType()
		{
			return this._type;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001517D File Offset: 0x0001337D
		public Lazy<Type> GetLazyPartType()
		{
			return new Lazy<Type>(new Func<Type>(this.GetPartType), LazyThreadSafetyMode.PublicationOnly);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00015192 File Offset: 0x00013392
		public ConstructorInfo GetConstructor()
		{
			if (this._constructor == null && !this._ignoreConstructorImports)
			{
				this._constructor = AttributedPartCreationInfo.SelectPartConstructor(this._type);
			}
			return this._constructor;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000151C1 File Offset: 0x000133C1
		public IDictionary<string, object> GetMetadata()
		{
			return this._type.GetPartMetadataForType(this.CreationPolicy);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000151D4 File Offset: 0x000133D4
		public IEnumerable<ExportDefinition> GetExports()
		{
			this.DiscoverExportsAndImports();
			return this._exports;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000151E2 File Offset: 0x000133E2
		public IEnumerable<ImportDefinition> GetImports()
		{
			this.DiscoverExportsAndImports();
			return this._imports;
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x000151F0 File Offset: 0x000133F0
		public bool IsDisposalRequired
		{
			get
			{
				return typeof(IDisposable).IsAssignableFrom(this.GetPartType());
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00015207 File Offset: 0x00013407
		public bool IsPartDiscoverable()
		{
			if (this._type.IsAttributeDefined<PartNotDiscoverableAttribute>())
			{
				CompositionTrace.DefinitionMarkedWithPartNotDiscoverableAttribute(this._type);
				return false;
			}
			if (!this.HasExports())
			{
				CompositionTrace.DefinitionContainsNoExports(this._type);
				return false;
			}
			return this.AllExportsHaveMatchingArity();
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00015243 File Offset: 0x00013443
		private bool HasExports()
		{
			return this.GetExportMembers(this._type).Any<MemberInfo>() || this.GetInheritedExports(this._type).Any<Type>();
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001526C File Offset: 0x0001346C
		private bool AllExportsHaveMatchingArity()
		{
			bool flag = true;
			if (this._type.ContainsGenericParameters)
			{
				int pureGenericArity = this._type.GetPureGenericArity();
				foreach (MemberInfo memberInfo in this.GetExportMembers(this._type).Concat<MemberInfo>(this.GetInheritedExports(this._type)))
				{
					if (memberInfo.MemberType == MemberTypes.Method && ((MethodInfo)memberInfo).ContainsGenericParameters)
					{
						flag = false;
						CompositionTrace.DefinitionMismatchedExportArity(this._type, memberInfo);
					}
					else if (memberInfo.GetDefaultTypeFromMember().GetPureGenericArity() != pureGenericArity)
					{
						flag = false;
						CompositionTrace.DefinitionMismatchedExportArity(this._type, memberInfo);
					}
				}
			}
			return flag;
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0001532C File Offset: 0x0001352C
		string ICompositionElement.DisplayName
		{
			get
			{
				return this.GetDisplayName();
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00015334 File Offset: 0x00013534
		ICompositionElement ICompositionElement.Origin
		{
			get
			{
				return this._origin;
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001532C File Offset: 0x0001352C
		public override string ToString()
		{
			return this.GetDisplayName();
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001533C File Offset: 0x0001353C
		private string GetDisplayName()
		{
			return this.GetPartType().GetDisplayName();
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0001534C File Offset: 0x0001354C
		private CreationPolicy CreationPolicy
		{
			get
			{
				if (this._partCreationPolicy == null)
				{
					this._partCreationPolicy = this._type.GetFirstAttribute<PartCreationPolicyAttribute>() ?? PartCreationPolicyAttribute.Default;
				}
				if (this._partCreationPolicy.CreationPolicy == CreationPolicy.NewScope)
				{
					throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidPartCreationPolicyOnPart, this._partCreationPolicy.CreationPolicy), this._origin);
				}
				return this._partCreationPolicy.CreationPolicy;
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x000153C0 File Offset: 0x000135C0
		private static ConstructorInfo SelectPartConstructor(Type type)
		{
			Assumes.NotNull<Type>(type);
			if (type.IsAbstract)
			{
				return null;
			}
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			ConstructorInfo[] constructors = type.GetConstructors(bindingFlags);
			if (constructors.Length == 0)
			{
				return null;
			}
			if (constructors.Length == 1 && constructors[0].GetParameters().Length == 0)
			{
				return constructors[0];
			}
			ConstructorInfo constructorInfo = null;
			ConstructorInfo constructorInfo2 = null;
			foreach (ConstructorInfo constructorInfo3 in constructors)
			{
				if (constructorInfo3.IsAttributeDefined<ImportingConstructorAttribute>())
				{
					if (constructorInfo != null)
					{
						return null;
					}
					constructorInfo = constructorInfo3;
				}
				else if (constructorInfo2 == null && constructorInfo3.GetParameters().Length == 0)
				{
					constructorInfo2 = constructorInfo3;
				}
			}
			return constructorInfo ?? constructorInfo2;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00015459 File Offset: 0x00013659
		private void DiscoverExportsAndImports()
		{
			if (this._exports != null && this._imports != null)
			{
				return;
			}
			this._exports = this.GetExportDefinitions();
			this._imports = this.GetImportDefinitions();
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00015484 File Offset: 0x00013684
		private IEnumerable<ExportDefinition> GetExportDefinitions()
		{
			List<ExportDefinition> list = new List<ExportDefinition>();
			this._contractNamesOnNonInterfaces = new HashSet<string>();
			foreach (MemberInfo memberInfo in this.GetExportMembers(this._type))
			{
				foreach (ExportAttribute exportAttribute in memberInfo.GetAttributes<ExportAttribute>())
				{
					AttributedExportDefinition attributedExportDefinition = this.CreateExportDefinition(memberInfo, exportAttribute);
					if (exportAttribute.GetType() == CompositionServices.InheritedExportAttributeType)
					{
						if (!this._contractNamesOnNonInterfaces.Contains(attributedExportDefinition.ContractName))
						{
							list.Add(new ReflectionMemberExportDefinition(memberInfo.ToLazyMember(), attributedExportDefinition, this));
							this._contractNamesOnNonInterfaces.Add(attributedExportDefinition.ContractName);
						}
					}
					else
					{
						list.Add(new ReflectionMemberExportDefinition(memberInfo.ToLazyMember(), attributedExportDefinition, this));
					}
				}
			}
			foreach (Type type in this.GetInheritedExports(this._type))
			{
				foreach (InheritedExportAttribute inheritedExportAttribute in type.GetAttributes<InheritedExportAttribute>())
				{
					AttributedExportDefinition attributedExportDefinition2 = this.CreateExportDefinition(type, inheritedExportAttribute);
					if (!this._contractNamesOnNonInterfaces.Contains(attributedExportDefinition2.ContractName))
					{
						list.Add(new ReflectionMemberExportDefinition(type.ToLazyMember(), attributedExportDefinition2, this));
						if (!type.IsInterface)
						{
							this._contractNamesOnNonInterfaces.Add(attributedExportDefinition2.ContractName);
						}
					}
				}
			}
			this._contractNamesOnNonInterfaces = null;
			return list;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00015638 File Offset: 0x00013838
		private AttributedExportDefinition CreateExportDefinition(MemberInfo member, ExportAttribute exportAttribute)
		{
			string text = null;
			Type type = null;
			member.GetContractInfoFromExport(exportAttribute, out type, out text);
			return new AttributedExportDefinition(this, member, exportAttribute, type, text);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001565E File Offset: 0x0001385E
		private IEnumerable<MemberInfo> GetExportMembers(Type type)
		{
			BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			if (type.IsAbstract)
			{
				flags &= ~BindingFlags.Instance;
			}
			else if (AttributedPartCreationInfo.IsExport(type))
			{
				yield return type;
			}
			foreach (FieldInfo fieldInfo in type.GetFields(flags))
			{
				if (AttributedPartCreationInfo.IsExport(fieldInfo))
				{
					yield return fieldInfo;
				}
			}
			FieldInfo[] array = null;
			foreach (PropertyInfo propertyInfo in type.GetProperties(flags))
			{
				if (AttributedPartCreationInfo.IsExport(propertyInfo))
				{
					yield return propertyInfo;
				}
			}
			PropertyInfo[] array2 = null;
			foreach (MethodInfo methodInfo in type.GetMethods(flags))
			{
				if (AttributedPartCreationInfo.IsExport(methodInfo))
				{
					yield return methodInfo;
				}
			}
			MethodInfo[] array3 = null;
			yield break;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001566E File Offset: 0x0001386E
		private IEnumerable<Type> GetInheritedExports(Type type)
		{
			if (type.IsAbstract)
			{
				yield break;
			}
			Type currentType = type.BaseType;
			if (currentType == null)
			{
				yield break;
			}
			while (currentType != null && currentType.UnderlyingSystemType != CompositionServices.ObjectType)
			{
				if (AttributedPartCreationInfo.IsInheritedExport(currentType))
				{
					yield return currentType;
				}
				currentType = currentType.BaseType;
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				if (AttributedPartCreationInfo.IsInheritedExport(type2))
				{
					yield return type2;
				}
			}
			Type[] array = null;
			yield break;
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001567E File Offset: 0x0001387E
		private static bool IsExport(ICustomAttributeProvider attributeProvider)
		{
			return attributeProvider.IsAttributeDefined<ExportAttribute>(false);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00015687 File Offset: 0x00013887
		private static bool IsInheritedExport(ICustomAttributeProvider attributedProvider)
		{
			return attributedProvider.IsAttributeDefined<InheritedExportAttribute>(false);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00015690 File Offset: 0x00013890
		private IEnumerable<ImportDefinition> GetImportDefinitions()
		{
			List<ImportDefinition> list = new List<ImportDefinition>();
			foreach (MemberInfo memberInfo in this.GetImportMembers(this._type))
			{
				ReflectionMemberImportDefinition reflectionMemberImportDefinition = AttributedModelDiscovery.CreateMemberImportDefinition(memberInfo, this);
				list.Add(reflectionMemberImportDefinition);
			}
			ConstructorInfo constructor = this.GetConstructor();
			if (constructor != null)
			{
				ParameterInfo[] parameters = constructor.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					ReflectionParameterImportDefinition reflectionParameterImportDefinition = AttributedModelDiscovery.CreateParameterImportDefinition(parameters[i], this);
					list.Add(reflectionParameterImportDefinition);
				}
			}
			return list;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00015730 File Offset: 0x00013930
		private IEnumerable<MemberInfo> GetImportMembers(Type type)
		{
			if (type.IsAbstract)
			{
				yield break;
			}
			foreach (MemberInfo memberInfo in this.GetDeclaredOnlyImportMembers(type))
			{
				yield return memberInfo;
			}
			IEnumerator<MemberInfo> enumerator = null;
			if (type.BaseType != null)
			{
				Type baseType = type.BaseType;
				while (baseType != null && baseType.UnderlyingSystemType != CompositionServices.ObjectType)
				{
					foreach (MemberInfo memberInfo2 in this.GetDeclaredOnlyImportMembers(baseType))
					{
						yield return memberInfo2;
					}
					enumerator = null;
					baseType = baseType.BaseType;
				}
				baseType = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00015747 File Offset: 0x00013947
		private IEnumerable<MemberInfo> GetDeclaredOnlyImportMembers(Type type)
		{
			BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			foreach (FieldInfo fieldInfo in type.GetFields(flags))
			{
				if (AttributedPartCreationInfo.IsImport(fieldInfo))
				{
					yield return fieldInfo;
				}
			}
			FieldInfo[] array = null;
			foreach (PropertyInfo propertyInfo in type.GetProperties(flags))
			{
				if (AttributedPartCreationInfo.IsImport(propertyInfo))
				{
					yield return propertyInfo;
				}
			}
			PropertyInfo[] array2 = null;
			yield break;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00015757 File Offset: 0x00013957
		private static bool IsImport(ICustomAttributeProvider attributeProvider)
		{
			return attributeProvider.IsAttributeDefined<IAttributedImport>(false);
		}

		// Token: 0x040002EB RID: 747
		private readonly Type _type;

		// Token: 0x040002EC RID: 748
		private readonly bool _ignoreConstructorImports;

		// Token: 0x040002ED RID: 749
		private readonly ICompositionElement _origin;

		// Token: 0x040002EE RID: 750
		private PartCreationPolicyAttribute _partCreationPolicy;

		// Token: 0x040002EF RID: 751
		private ConstructorInfo _constructor;

		// Token: 0x040002F0 RID: 752
		private IEnumerable<ExportDefinition> _exports;

		// Token: 0x040002F1 RID: 753
		private IEnumerable<ImportDefinition> _imports;

		// Token: 0x040002F2 RID: 754
		private HashSet<string> _contractNamesOnNonInterfaces;
	}
}
