using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;

namespace System.Runtime.Remoting
{
	/// <summary>Provides several methods for using and publishing remoted objects in SOAP format.</summary>
	// Token: 0x02000574 RID: 1396
	[ComVisible(true)]
	public class SoapServices
	{
		// Token: 0x060036CE RID: 14030 RVA: 0x0000259F File Offset: 0x0000079F
		private SoapServices()
		{
		}

		/// <summary>Gets the XML namespace prefix for common language runtime types.</summary>
		/// <returns>The XML namespace prefix for common language runtime types.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060036CF RID: 14031 RVA: 0x000C5DE0 File Offset: 0x000C3FE0
		public static string XmlNsForClrType
		{
			get
			{
				return "http://schemas.microsoft.com/clr/";
			}
		}

		/// <summary>Gets the default XML namespace prefix that should be used for XML encoding of a common language runtime class that has an assembly, but no native namespace.</summary>
		/// <returns>The default XML namespace prefix that should be used for XML encoding of a common language runtime class that has an assembly, but no native namespace.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x060036D0 RID: 14032 RVA: 0x000C5DE7 File Offset: 0x000C3FE7
		public static string XmlNsForClrTypeWithAssembly
		{
			get
			{
				return "http://schemas.microsoft.com/clr/assem/";
			}
		}

		/// <summary>Gets the XML namespace prefix that should be used for XML encoding of a common language runtime class that is part of the mscorlib.dll file.</summary>
		/// <returns>The XML namespace prefix that should be used for XML encoding of a common language runtime class that is part of the mscorlib.dll file.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x060036D1 RID: 14033 RVA: 0x000C5DEE File Offset: 0x000C3FEE
		public static string XmlNsForClrTypeWithNs
		{
			get
			{
				return "http://schemas.microsoft.com/clr/ns/";
			}
		}

		/// <summary>Gets the default XML namespace prefix that should be used for XML encoding of a common language runtime class that has both a common language runtime namespace and an assembly.</summary>
		/// <returns>The default XML namespace prefix that should be used for XML encoding of a common language runtime class that has both a common language runtime namespace and an assembly.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x060036D2 RID: 14034 RVA: 0x000C5DF5 File Offset: 0x000C3FF5
		public static string XmlNsForClrTypeWithNsAndAssembly
		{
			get
			{
				return "http://schemas.microsoft.com/clr/nsassem/";
			}
		}

		/// <summary>Returns the common language runtime type namespace name from the provided namespace and assembly names.</summary>
		/// <param name="typeNamespace">The namespace that is to be coded.</param>
		/// <param name="assemblyName">The name of the assembly that is to be coded.</param>
		/// <returns>The common language runtime type namespace name from the provided namespace and assembly names.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="assemblyName" /> and <paramref name="typeNamespace" /> parameters are both either <see langword="null" /> or empty.</exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036D3 RID: 14035 RVA: 0x000C5DFC File Offset: 0x000C3FFC
		public static string CodeXmlNamespaceForClrTypeNamespace(string typeNamespace, string assemblyName)
		{
			if (assemblyName == string.Empty)
			{
				return SoapServices.XmlNsForClrTypeWithNs + typeNamespace;
			}
			if (typeNamespace == string.Empty)
			{
				return SoapServices.EncodeNs(SoapServices.XmlNsForClrTypeWithAssembly + assemblyName);
			}
			return SoapServices.EncodeNs(SoapServices.XmlNsForClrTypeWithNsAndAssembly + typeNamespace + "/" + assemblyName);
		}

		/// <summary>Decodes the XML namespace and assembly names from the provided common language runtime namespace.</summary>
		/// <param name="inNamespace">The common language runtime namespace.</param>
		/// <param name="typeNamespace">When this method returns, contains a <see cref="T:System.String" /> that holds the decoded namespace name. This parameter is passed uninitialized.</param>
		/// <param name="assemblyName">When this method returns, contains a <see cref="T:System.String" /> that holds the decoded assembly name. This parameter is passed uninitialized.</param>
		/// <returns>
		///   <see langword="true" /> if the namespace and assembly names were successfully decoded; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="inNamespace" /> parameter is <see langword="null" /> or empty.</exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036D4 RID: 14036 RVA: 0x000C5E58 File Offset: 0x000C4058
		public static bool DecodeXmlNamespaceForClrTypeNamespace(string inNamespace, out string typeNamespace, out string assemblyName)
		{
			if (inNamespace == null)
			{
				throw new ArgumentNullException("inNamespace");
			}
			inNamespace = SoapServices.DecodeNs(inNamespace);
			typeNamespace = null;
			assemblyName = null;
			if (inNamespace.StartsWith(SoapServices.XmlNsForClrTypeWithNsAndAssembly))
			{
				int length = SoapServices.XmlNsForClrTypeWithNsAndAssembly.Length;
				if (length >= inNamespace.Length)
				{
					return false;
				}
				int num = inNamespace.IndexOf('/', length + 1);
				if (num == -1)
				{
					return false;
				}
				typeNamespace = inNamespace.Substring(length, num - length);
				assemblyName = inNamespace.Substring(num + 1);
				return true;
			}
			else
			{
				if (inNamespace.StartsWith(SoapServices.XmlNsForClrTypeWithNs))
				{
					int length2 = SoapServices.XmlNsForClrTypeWithNs.Length;
					typeNamespace = inNamespace.Substring(length2);
					return true;
				}
				if (inNamespace.StartsWith(SoapServices.XmlNsForClrTypeWithAssembly))
				{
					int length3 = SoapServices.XmlNsForClrTypeWithAssembly.Length;
					assemblyName = inNamespace.Substring(length3);
					return true;
				}
				return false;
			}
		}

		/// <summary>Retrieves field type from XML attribute name, namespace, and the <see cref="T:System.Type" /> of the containing object.</summary>
		/// <param name="containingType">The <see cref="T:System.Type" /> of the object that contains the field.</param>
		/// <param name="xmlAttribute">The XML attribute name of the field type.</param>
		/// <param name="xmlNamespace">The XML namespace of the field type.</param>
		/// <param name="type">When this method returns, contains a <see cref="T:System.Type" /> of the field. This parameter is passed uninitialized.</param>
		/// <param name="name">When this method returns, contains a <see cref="T:System.String" /> that holds the name of the field. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036D5 RID: 14037 RVA: 0x000C5F18 File Offset: 0x000C4118
		public static void GetInteropFieldTypeAndNameFromXmlAttribute(Type containingType, string xmlAttribute, string xmlNamespace, out Type type, out string name)
		{
			SoapServices.TypeInfo typeInfo = (SoapServices.TypeInfo)SoapServices._typeInfos[containingType];
			SoapServices.GetInteropFieldInfo((typeInfo != null) ? typeInfo.Attributes : null, xmlAttribute, xmlNamespace, out type, out name);
		}

		/// <summary>Retrieves the <see cref="T:System.Type" /> and name of a field from the provided XML element name, namespace, and the containing type.</summary>
		/// <param name="containingType">The <see cref="T:System.Type" /> of the object that contains the field.</param>
		/// <param name="xmlElement">The XML element name of field.</param>
		/// <param name="xmlNamespace">The XML namespace of the field type.</param>
		/// <param name="type">When this method returns, contains a <see cref="T:System.Type" /> of the field. This parameter is passed uninitialized.</param>
		/// <param name="name">When this method returns, contains a <see cref="T:System.String" /> that holds the name of the field. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036D6 RID: 14038 RVA: 0x000C5F4C File Offset: 0x000C414C
		public static void GetInteropFieldTypeAndNameFromXmlElement(Type containingType, string xmlElement, string xmlNamespace, out Type type, out string name)
		{
			SoapServices.TypeInfo typeInfo = (SoapServices.TypeInfo)SoapServices._typeInfos[containingType];
			SoapServices.GetInteropFieldInfo((typeInfo != null) ? typeInfo.Elements : null, xmlElement, xmlNamespace, out type, out name);
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x000C5F80 File Offset: 0x000C4180
		private static void GetInteropFieldInfo(Hashtable fields, string xmlName, string xmlNamespace, out Type type, out string name)
		{
			if (fields != null)
			{
				FieldInfo fieldInfo = (FieldInfo)fields[SoapServices.GetNameKey(xmlName, xmlNamespace)];
				if (fieldInfo != null)
				{
					type = fieldInfo.FieldType;
					name = fieldInfo.Name;
					return;
				}
			}
			type = null;
			name = null;
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x000C5FC5 File Offset: 0x000C41C5
		private static string GetNameKey(string name, string namspace)
		{
			if (namspace == null)
			{
				return name;
			}
			return name + " " + namspace;
		}

		/// <summary>Retrieves the <see cref="T:System.Type" /> that should be used during deserialization of an unrecognized object type with the given XML element name and namespace.</summary>
		/// <param name="xmlElement">The XML element name of the unknown object type.</param>
		/// <param name="xmlNamespace">The XML namespace of the unknown object type.</param>
		/// <returns>The <see cref="T:System.Type" /> of object associated with the specified XML element name and namespace.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036D9 RID: 14041 RVA: 0x000C5FD8 File Offset: 0x000C41D8
		public static Type GetInteropTypeFromXmlElement(string xmlElement, string xmlNamespace)
		{
			object syncRoot = SoapServices._xmlElements.SyncRoot;
			Type type;
			lock (syncRoot)
			{
				type = (Type)SoapServices._xmlElements[xmlElement + " " + xmlNamespace];
			}
			return type;
		}

		/// <summary>Retrieves the object <see cref="T:System.Type" /> that should be used during deserialization of an unrecognized object type with the given XML type name and namespace.</summary>
		/// <param name="xmlType">The XML type of the unknown object type.</param>
		/// <param name="xmlTypeNamespace">The XML type namespace of the unknown object type.</param>
		/// <returns>The <see cref="T:System.Type" /> of object associated with the specified XML type name and namespace.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036DA RID: 14042 RVA: 0x000C6034 File Offset: 0x000C4234
		public static Type GetInteropTypeFromXmlType(string xmlType, string xmlTypeNamespace)
		{
			object syncRoot = SoapServices._xmlTypes.SyncRoot;
			Type type;
			lock (syncRoot)
			{
				type = (Type)SoapServices._xmlTypes[xmlType + " " + xmlTypeNamespace];
			}
			return type;
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x000C6090 File Offset: 0x000C4290
		private static string GetAssemblyName(MethodBase mb)
		{
			if (mb.DeclaringType.Assembly == typeof(object).Assembly)
			{
				return string.Empty;
			}
			return mb.DeclaringType.Assembly.GetName().Name;
		}

		/// <summary>Returns the SOAPAction value associated with the method specified in the given <see cref="T:System.Reflection.MethodBase" />.</summary>
		/// <param name="mb">The <see cref="T:System.Reflection.MethodBase" /> that contains the method for which a SOAPAction is requested.</param>
		/// <returns>The SOAPAction value associated with the method specified in the given <see cref="T:System.Reflection.MethodBase" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036DC RID: 14044 RVA: 0x000C60CE File Offset: 0x000C42CE
		public static string GetSoapActionFromMethodBase(MethodBase mb)
		{
			return SoapServices.InternalGetSoapAction(mb);
		}

		/// <summary>Determines the type and method name of the method associated with the specified SOAPAction value.</summary>
		/// <param name="soapAction">The SOAPAction of the method for which the type and method names were requested.</param>
		/// <param name="typeName">When this method returns, contains a <see cref="T:System.String" /> that holds the type name of the method in question. This parameter is passed uninitialized.</param>
		/// <param name="methodName">When this method returns, contains a <see cref="T:System.String" /> that holds the method name of the method in question. This parameter is passed uninitialized.</param>
		/// <returns>
		///   <see langword="true" /> if the type and method name were successfully recovered; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">The SOAPAction value does not start and end with quotes.</exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036DD RID: 14045 RVA: 0x000C60D8 File Offset: 0x000C42D8
		public static bool GetTypeAndMethodNameFromSoapAction(string soapAction, out string typeName, out string methodName)
		{
			object syncRoot = SoapServices._soapActions.SyncRoot;
			lock (syncRoot)
			{
				MethodBase methodBase = (MethodBase)SoapServices._soapActionsMethods[soapAction];
				if (methodBase != null)
				{
					typeName = methodBase.DeclaringType.AssemblyQualifiedName;
					methodName = methodBase.Name;
					return true;
				}
			}
			typeName = null;
			methodName = null;
			int num = soapAction.LastIndexOf('#');
			if (num == -1)
			{
				return false;
			}
			methodName = soapAction.Substring(num + 1);
			string text;
			string text2;
			if (!SoapServices.DecodeXmlNamespaceForClrTypeNamespace(soapAction.Substring(0, num), out text, out text2))
			{
				return false;
			}
			if (text2 == null)
			{
				typeName = text + ", " + typeof(object).Assembly.GetName().Name;
			}
			else
			{
				typeName = text + ", " + text2;
			}
			return true;
		}

		/// <summary>Returns XML element information that should be used when serializing the given type.</summary>
		/// <param name="type">The object <see cref="T:System.Type" /> for which the XML element and namespace names were requested.</param>
		/// <param name="xmlElement">When this method returns, contains a <see cref="T:System.String" /> that holds the XML element name of the specified object type. This parameter is passed uninitialized.</param>
		/// <param name="xmlNamespace">When this method returns, contains a <see cref="T:System.String" /> that holds the XML namespace name of the specified object type. This parameter is passed uninitialized.</param>
		/// <returns>
		///   <see langword="true" /> if the requested values have been set flagged with <see cref="T:System.Runtime.Remoting.Metadata.SoapTypeAttribute" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036DE RID: 14046 RVA: 0x000C61C4 File Offset: 0x000C43C4
		public static bool GetXmlElementForInteropType(Type type, out string xmlElement, out string xmlNamespace)
		{
			SoapTypeAttribute soapTypeAttribute = (SoapTypeAttribute)InternalRemotingServices.GetCachedSoapAttribute(type);
			if (!soapTypeAttribute.IsInteropXmlElement)
			{
				xmlElement = null;
				xmlNamespace = null;
				return false;
			}
			xmlElement = soapTypeAttribute.XmlElementName;
			xmlNamespace = soapTypeAttribute.XmlNamespace;
			return true;
		}

		/// <summary>Retrieves the XML namespace used during remote calls of the method specified in the given <see cref="T:System.Reflection.MethodBase" />.</summary>
		/// <param name="mb">The <see cref="T:System.Reflection.MethodBase" /> of the method for which the XML namespace was requested.</param>
		/// <returns>The XML namespace used during remote calls of the specified method.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036DF RID: 14047 RVA: 0x000C61FE File Offset: 0x000C43FE
		public static string GetXmlNamespaceForMethodCall(MethodBase mb)
		{
			return SoapServices.CodeXmlNamespaceForClrTypeNamespace(mb.DeclaringType.FullName, SoapServices.GetAssemblyName(mb));
		}

		/// <summary>Retrieves the XML namespace used during the generation of responses to the remote call to the method specified in the given <see cref="T:System.Reflection.MethodBase" />.</summary>
		/// <param name="mb">The <see cref="T:System.Reflection.MethodBase" /> of the method for which the XML namespace was requested.</param>
		/// <returns>The XML namespace used during the generation of responses to a remote method call.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036E0 RID: 14048 RVA: 0x000C61FE File Offset: 0x000C43FE
		public static string GetXmlNamespaceForMethodResponse(MethodBase mb)
		{
			return SoapServices.CodeXmlNamespaceForClrTypeNamespace(mb.DeclaringType.FullName, SoapServices.GetAssemblyName(mb));
		}

		/// <summary>Returns XML type information that should be used when serializing the given <see cref="T:System.Type" />.</summary>
		/// <param name="type">The object <see cref="T:System.Type" /> for which the XML element and namespace names were requested.</param>
		/// <param name="xmlType">The XML type of the specified object <see cref="T:System.Type" />.</param>
		/// <param name="xmlTypeNamespace">The XML type namespace of the specified object <see cref="T:System.Type" />.</param>
		/// <returns>
		///   <see langword="true" /> if the requested values have been set flagged with <see cref="T:System.Runtime.Remoting.Metadata.SoapTypeAttribute" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036E1 RID: 14049 RVA: 0x000C6218 File Offset: 0x000C4418
		public static bool GetXmlTypeForInteropType(Type type, out string xmlType, out string xmlTypeNamespace)
		{
			SoapTypeAttribute soapTypeAttribute = (SoapTypeAttribute)InternalRemotingServices.GetCachedSoapAttribute(type);
			if (!soapTypeAttribute.IsInteropXmlType)
			{
				xmlType = null;
				xmlTypeNamespace = null;
				return false;
			}
			xmlType = soapTypeAttribute.XmlTypeName;
			xmlTypeNamespace = soapTypeAttribute.XmlTypeNamespace;
			return true;
		}

		/// <summary>Returns a Boolean value that indicates whether the specified namespace is native to the common language runtime.</summary>
		/// <param name="namespaceString">The namespace to check in the common language runtime.</param>
		/// <returns>
		///   <see langword="true" /> if the given namespace is native to the common language runtime; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036E2 RID: 14050 RVA: 0x000C6252 File Offset: 0x000C4452
		public static bool IsClrTypeNamespace(string namespaceString)
		{
			return namespaceString.StartsWith(SoapServices.XmlNsForClrType);
		}

		/// <summary>Determines if the specified SOAPAction is acceptable for a given <see cref="T:System.Reflection.MethodBase" />.</summary>
		/// <param name="soapAction">The SOAPAction to check against the given <see cref="T:System.Reflection.MethodBase" />.</param>
		/// <param name="mb">The <see cref="T:System.Reflection.MethodBase" /> the specified SOAPAction is checked against.</param>
		/// <returns>
		///   <see langword="true" /> if the specified SOAPAction is acceptable for a given <see cref="T:System.Reflection.MethodBase" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036E3 RID: 14051 RVA: 0x000C6260 File Offset: 0x000C4460
		public static bool IsSoapActionValidForMethodBase(string soapAction, MethodBase mb)
		{
			string text;
			string text2;
			SoapServices.GetTypeAndMethodNameFromSoapAction(soapAction, out text, out text2);
			if (text2 != mb.Name)
			{
				return false;
			}
			string assemblyQualifiedName = mb.DeclaringType.AssemblyQualifiedName;
			return text == assemblyQualifiedName;
		}

		/// <summary>Preloads every <see cref="T:System.Type" /> found in the specified <see cref="T:System.Reflection.Assembly" /> from the information found in the <see cref="T:System.Runtime.Remoting.Metadata.SoapTypeAttribute" /> associated with each type.</summary>
		/// <param name="assembly">The <see cref="T:System.Reflection.Assembly" /> for each type of which to call <see cref="M:System.Runtime.Remoting.SoapServices.PreLoad(System.Type)" />.</param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036E4 RID: 14052 RVA: 0x000C629C File Offset: 0x000C449C
		public static void PreLoad(Assembly assembly)
		{
			Type[] types = assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				SoapServices.PreLoad(types[i]);
			}
		}

		/// <summary>Preloads the given <see cref="T:System.Type" /> based on values set in a <see cref="T:System.Runtime.Remoting.Metadata.SoapTypeAttribute" /> on the type.</summary>
		/// <param name="type">The <see cref="T:System.Type" /> to preload.</param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036E5 RID: 14053 RVA: 0x000C62C8 File Offset: 0x000C44C8
		public static void PreLoad(Type type)
		{
			SoapServices.TypeInfo typeInfo = SoapServices._typeInfos[type] as SoapServices.TypeInfo;
			if (typeInfo != null)
			{
				return;
			}
			string text;
			string text2;
			if (SoapServices.GetXmlTypeForInteropType(type, out text, out text2))
			{
				SoapServices.RegisterInteropXmlType(text, text2, type);
			}
			if (SoapServices.GetXmlElementForInteropType(type, out text, out text2))
			{
				SoapServices.RegisterInteropXmlElement(text, text2, type);
			}
			object syncRoot = SoapServices._typeInfos.SyncRoot;
			lock (syncRoot)
			{
				typeInfo = new SoapServices.TypeInfo();
				foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					SoapFieldAttribute soapFieldAttribute = (SoapFieldAttribute)InternalRemotingServices.GetCachedSoapAttribute(fieldInfo);
					if (soapFieldAttribute.IsInteropXmlElement())
					{
						string nameKey = SoapServices.GetNameKey(soapFieldAttribute.XmlElementName, soapFieldAttribute.XmlNamespace);
						if (soapFieldAttribute.UseAttribute)
						{
							if (typeInfo.Attributes == null)
							{
								typeInfo.Attributes = new Hashtable();
							}
							typeInfo.Attributes[nameKey] = fieldInfo;
						}
						else
						{
							if (typeInfo.Elements == null)
							{
								typeInfo.Elements = new Hashtable();
							}
							typeInfo.Elements[nameKey] = fieldInfo;
						}
					}
				}
				SoapServices._typeInfos[type] = typeInfo;
			}
		}

		/// <summary>Associates the given XML element name and namespace with a run-time type that should be used for deserialization.</summary>
		/// <param name="xmlElement">The XML element name to use in deserialization.</param>
		/// <param name="xmlNamespace">The XML namespace to use in deserialization.</param>
		/// <param name="type">The run-time <see cref="T:System.Type" /> to use in deserialization.</param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036E6 RID: 14054 RVA: 0x000C63FC File Offset: 0x000C45FC
		public static void RegisterInteropXmlElement(string xmlElement, string xmlNamespace, Type type)
		{
			object syncRoot = SoapServices._xmlElements.SyncRoot;
			lock (syncRoot)
			{
				SoapServices._xmlElements[xmlElement + " " + xmlNamespace] = type;
			}
		}

		/// <summary>Associates the given XML type name and namespace with the run-time type that should be used for deserialization.</summary>
		/// <param name="xmlType">The XML type to use in deserialization.</param>
		/// <param name="xmlTypeNamespace">The XML namespace to use in deserialization.</param>
		/// <param name="type">The run-time <see cref="T:System.Type" /> to use in deserialization.</param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036E7 RID: 14055 RVA: 0x000C6454 File Offset: 0x000C4654
		public static void RegisterInteropXmlType(string xmlType, string xmlTypeNamespace, Type type)
		{
			object syncRoot = SoapServices._xmlTypes.SyncRoot;
			lock (syncRoot)
			{
				SoapServices._xmlTypes[xmlType + " " + xmlTypeNamespace] = type;
			}
		}

		/// <summary>Associates the specified <see cref="T:System.Reflection.MethodBase" /> with the SOAPAction cached with it.</summary>
		/// <param name="mb">The <see cref="T:System.Reflection.MethodBase" /> of the method to associate with the SOAPAction cached with it.</param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036E8 RID: 14056 RVA: 0x000C64AC File Offset: 0x000C46AC
		public static void RegisterSoapActionForMethodBase(MethodBase mb)
		{
			SoapServices.InternalGetSoapAction(mb);
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x000C64B8 File Offset: 0x000C46B8
		private static string InternalGetSoapAction(MethodBase mb)
		{
			object syncRoot = SoapServices._soapActions.SyncRoot;
			string text2;
			lock (syncRoot)
			{
				string text = (string)SoapServices._soapActions[mb];
				if (text == null)
				{
					text = ((SoapMethodAttribute)InternalRemotingServices.GetCachedSoapAttribute(mb)).SoapAction;
					SoapServices._soapActions[mb] = text;
					SoapServices._soapActionsMethods[text] = mb;
				}
				text2 = text;
			}
			return text2;
		}

		/// <summary>Associates the provided SOAPAction value with the given <see cref="T:System.Reflection.MethodBase" /> for use in channel sinks.</summary>
		/// <param name="mb">The <see cref="T:System.Reflection.MethodBase" /> to associate with the provided SOAPAction.</param>
		/// <param name="soapAction">The SOAPAction value to associate with the given <see cref="T:System.Reflection.MethodBase" />.</param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
		// Token: 0x060036EA RID: 14058 RVA: 0x000C6538 File Offset: 0x000C4738
		public static void RegisterSoapActionForMethodBase(MethodBase mb, string soapAction)
		{
			object syncRoot = SoapServices._soapActions.SyncRoot;
			lock (syncRoot)
			{
				SoapServices._soapActions[mb] = soapAction;
				SoapServices._soapActionsMethods[soapAction] = mb;
			}
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x000C6590 File Offset: 0x000C4790
		private static string EncodeNs(string ns)
		{
			ns = ns.Replace(",", "%2C");
			ns = ns.Replace(" ", "%20");
			return ns.Replace("=", "%3D");
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x000C65C6 File Offset: 0x000C47C6
		private static string DecodeNs(string ns)
		{
			ns = ns.Replace("%2C", ",");
			ns = ns.Replace("%20", " ");
			return ns.Replace("%3D", "=");
		}

		// Token: 0x04002561 RID: 9569
		private static Hashtable _xmlTypes = new Hashtable();

		// Token: 0x04002562 RID: 9570
		private static Hashtable _xmlElements = new Hashtable();

		// Token: 0x04002563 RID: 9571
		private static Hashtable _soapActions = new Hashtable();

		// Token: 0x04002564 RID: 9572
		private static Hashtable _soapActionsMethods = new Hashtable();

		// Token: 0x04002565 RID: 9573
		private static Hashtable _typeInfos = new Hashtable();

		// Token: 0x02000575 RID: 1397
		private class TypeInfo
		{
			// Token: 0x04002566 RID: 9574
			public Hashtable Attributes;

			// Token: 0x04002567 RID: 9575
			public Hashtable Elements;
		}
	}
}
