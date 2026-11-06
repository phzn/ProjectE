using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization
{
	/// <summary>When given a class representing a data contract, and metadata representing a member of the contract, produces an XPath query for the member.</summary>
	// Token: 0x02000130 RID: 304
	public static class XPathQueryGenerator
	{
		/// <summary>Creates an XPath from a data contract using the specified data contract type, array of metadata elements, and namespaces.</summary>
		/// <param name="type">The type that represents a data contract.</param>
		/// <param name="pathToMember">The metadata, generated using the <see cref="Overload:System.Type.GetMember" /> method of the <see cref="T:System.Type" /> class, that points to the specific data member used to generate the query.</param>
		/// <param name="namespaces">The XML namespaces and their prefixes found in the data contract.</param>
		/// <returns>
		///   <see cref="T:System.String" />  
		///
		/// The XPath generated from the type and member data.</returns>
		// Token: 0x06000F02 RID: 3842 RVA: 0x0003CD5C File Offset: 0x0003AF5C
		public static string CreateFromDataContractSerializer(Type type, MemberInfo[] pathToMember, out XmlNamespaceManager namespaces)
		{
			return XPathQueryGenerator.CreateFromDataContractSerializer(type, pathToMember, null, out namespaces);
		}

		/// <summary>Creates an XPath from a data contract using the specified contract data type, array of metadata elements, the top level element, and namespaces.</summary>
		/// <param name="type">The type that represents a data contract.</param>
		/// <param name="pathToMember">The metadata, generated using the <see cref="Overload:System.Type.GetMember" /> method of the <see cref="T:System.Type" /> class, that points to the specific data member used to generate the query.</param>
		/// <param name="rootElementXpath">The top level element in the xpath.</param>
		/// <param name="namespaces">The XML namespaces and their prefixes found in the data contract.</param>
		/// <returns>
		///   <see cref="T:System.String" />  
		///
		/// The XPath generated from the type and member data.</returns>
		// Token: 0x06000F03 RID: 3843 RVA: 0x0003CD68 File Offset: 0x0003AF68
		public static string CreateFromDataContractSerializer(Type type, MemberInfo[] pathToMember, StringBuilder rootElementXpath, out XmlNamespaceManager namespaces)
		{
			if (type == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("type"));
			}
			if (pathToMember == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("pathToMember"));
			}
			DataContract dataContract = DataContract.GetDataContract(type);
			XPathQueryGenerator.ExportContext exportContext;
			if (rootElementXpath == null)
			{
				exportContext = new XPathQueryGenerator.ExportContext(dataContract);
			}
			else
			{
				exportContext = new XPathQueryGenerator.ExportContext(rootElementXpath);
			}
			for (int i = 0; i < pathToMember.Length; i++)
			{
				dataContract = XPathQueryGenerator.ProcessDataContract(dataContract, exportContext, pathToMember[i]);
			}
			namespaces = exportContext.Namespaces;
			return exportContext.XPath;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0003CDE2 File Offset: 0x0003AFE2
		private static DataContract ProcessDataContract(DataContract contract, XPathQueryGenerator.ExportContext context, MemberInfo memberNode)
		{
			if (contract is ClassDataContract)
			{
				return XPathQueryGenerator.ProcessClassDataContract((ClassDataContract)contract, context, memberNode);
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(global::System.Runtime.Serialization.SR.GetString("The path to member was not found for XPath query generator.")));
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0003CE10 File Offset: 0x0003B010
		private static DataContract ProcessClassDataContract(ClassDataContract contract, XPathQueryGenerator.ExportContext context, MemberInfo memberNode)
		{
			string text = context.SetNamespace(contract.Namespace.Value);
			foreach (DataMember dataMember in XPathQueryGenerator.GetDataMembers(contract))
			{
				if (dataMember.MemberInfo.Name == memberNode.Name && dataMember.MemberInfo.DeclaringType.IsAssignableFrom(memberNode.DeclaringType))
				{
					context.WriteChildToContext(dataMember, text);
					return dataMember.MemberTypeContract;
				}
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(global::System.Runtime.Serialization.SR.GetString("The path to member was not found for XPath query generator.")));
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0003CEC0 File Offset: 0x0003B0C0
		private static IEnumerable<DataMember> GetDataMembers(ClassDataContract contract)
		{
			if (contract.BaseContract != null)
			{
				foreach (DataMember dataMember in XPathQueryGenerator.GetDataMembers(contract.BaseContract))
				{
					yield return dataMember;
				}
				IEnumerator<DataMember> enumerator = null;
			}
			if (contract.Members != null)
			{
				foreach (DataMember dataMember2 in contract.Members)
				{
					yield return dataMember2;
				}
				List<DataMember>.Enumerator enumerator2 = default(List<DataMember>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04000685 RID: 1669
		private const string XPathSeparator = "/";

		// Token: 0x04000686 RID: 1670
		private const string NsSeparator = ":";

		// Token: 0x02000131 RID: 305
		private class ExportContext
		{
			// Token: 0x06000F07 RID: 3847 RVA: 0x0003CED0 File Offset: 0x0003B0D0
			public ExportContext(DataContract rootContract)
			{
				this.namespaces = new XmlNamespaceManager(new NameTable());
				string text = this.SetNamespace(rootContract.TopLevelElementNamespace.Value);
				this.xPathBuilder = new StringBuilder("/" + text + ":" + rootContract.TopLevelElementName.Value);
			}

			// Token: 0x06000F08 RID: 3848 RVA: 0x0003CF2B File Offset: 0x0003B12B
			public ExportContext(StringBuilder rootContractXPath)
			{
				this.namespaces = new XmlNamespaceManager(new NameTable());
				this.xPathBuilder = rootContractXPath;
			}

			// Token: 0x06000F09 RID: 3849 RVA: 0x0003CF4A File Offset: 0x0003B14A
			public void WriteChildToContext(DataMember contextMember, string prefix)
			{
				this.xPathBuilder.Append("/" + prefix + ":" + contextMember.Name);
			}

			// Token: 0x17000348 RID: 840
			// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0003CF6E File Offset: 0x0003B16E
			public XmlNamespaceManager Namespaces
			{
				get
				{
					return this.namespaces;
				}
			}

			// Token: 0x17000349 RID: 841
			// (get) Token: 0x06000F0B RID: 3851 RVA: 0x0003CF76 File Offset: 0x0003B176
			public string XPath
			{
				get
				{
					return this.xPathBuilder.ToString();
				}
			}

			// Token: 0x06000F0C RID: 3852 RVA: 0x0003CF84 File Offset: 0x0003B184
			public string SetNamespace(string ns)
			{
				string text = this.namespaces.LookupPrefix(ns);
				if (text == null || text.Length == 0)
				{
					string text2 = "xg";
					int num = this.nextPrefix;
					this.nextPrefix = num + 1;
					text = text2 + num.ToString(NumberFormatInfo.InvariantInfo);
					this.Namespaces.AddNamespace(text, ns);
				}
				return text;
			}

			// Token: 0x04000687 RID: 1671
			private XmlNamespaceManager namespaces;

			// Token: 0x04000688 RID: 1672
			private int nextPrefix;

			// Token: 0x04000689 RID: 1673
			private StringBuilder xPathBuilder;
		}
	}
}
