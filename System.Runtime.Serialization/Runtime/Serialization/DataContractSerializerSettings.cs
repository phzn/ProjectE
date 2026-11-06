using System;
using System.Collections.Generic;
using System.Xml;

namespace System.Runtime.Serialization
{
	/// <summary>Specifies data contract serializer settings.</summary>
	// Token: 0x020000C7 RID: 199
	public class DataContractSerializerSettings
	{
		/// <summary>Gets or sets the root name of the selected object.</summary>
		/// <returns>The root name of the selected object.</returns>
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x00030B17 File Offset: 0x0002ED17
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x00030B1F File Offset: 0x0002ED1F
		public XmlDictionaryString RootName { get; set; }

		/// <summary>Gets or sets the root namespace for the specified object.</summary>
		/// <returns>The root namespace for the specified object.</returns>
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x00030B28 File Offset: 0x0002ED28
		// (set) Token: 0x06000B81 RID: 2945 RVA: 0x00030B30 File Offset: 0x0002ED30
		public XmlDictionaryString RootNamespace { get; set; }

		/// <summary>Gets or sets a collection of types that may be present in the object graph serialized using this instance of the DataContractSerializerSettings.</summary>
		/// <returns>A collection of types that may be present in the object graph serialized using this instance of the DataContractSerializerSettings.</returns>
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x00030B39 File Offset: 0x0002ED39
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x00030B41 File Offset: 0x0002ED41
		public IEnumerable<Type> KnownTypes { get; set; }

		/// <summary>Gets or sets the maximum number of items in an object graph to serialize or deserialize.</summary>
		/// <returns>The maximum number of items in an object graph to serialize or deserialize.</returns>
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x00030B4A File Offset: 0x0002ED4A
		// (set) Token: 0x06000B85 RID: 2949 RVA: 0x00030B52 File Offset: 0x0002ED52
		public int MaxItemsInObjectGraph
		{
			get
			{
				return this.maxItemsInObjectGraph;
			}
			set
			{
				this.maxItemsInObjectGraph = value;
			}
		}

		/// <summary>Gets or sets a value that specifies whether to ignore data supplied by an extension of the class when the class is being serialized or deserialized.</summary>
		/// <returns>
		///   <see langword="true" /> to ignore data supplied by an extension of the class when the class is being serialized or deserialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00030B5B File Offset: 0x0002ED5B
		// (set) Token: 0x06000B87 RID: 2951 RVA: 0x00030B63 File Offset: 0x0002ED63
		public bool IgnoreExtensionDataObject { get; set; }

		/// <summary>Gets or sets a value that specifies whether to use non-standard XML constructs to preserve object reference data.</summary>
		/// <returns>
		///   <see langword="true" /> to use non-standard XML constructs to preserve object reference data; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x00030B6C File Offset: 0x0002ED6C
		// (set) Token: 0x06000B89 RID: 2953 RVA: 0x00030B74 File Offset: 0x0002ED74
		public bool PreserveObjectReferences { get; set; }

		/// <summary>Gets or sets a serialization surrogate.</summary>
		/// <returns>The serialization surrogate.</returns>
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x00030B7D File Offset: 0x0002ED7D
		// (set) Token: 0x06000B8B RID: 2955 RVA: 0x00030B85 File Offset: 0x0002ED85
		public IDataContractSurrogate DataContractSurrogate { get; set; }

		/// <summary>Gets or sets the component used to dynamically map xsi:type declarations to known contract types.</summary>
		/// <returns>The component used to dynamically map xsi:type declarations to known contract types.</returns>
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x00030B8E File Offset: 0x0002ED8E
		// (set) Token: 0x06000B8D RID: 2957 RVA: 0x00030B96 File Offset: 0x0002ED96
		public DataContractResolver DataContractResolver { get; set; }

		/// <summary>Gets or sets a value that specifies whether to serialize read only types.</summary>
		/// <returns>
		///   <see langword="true" /> to serialize read only types; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x00030B9F File Offset: 0x0002ED9F
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x00030BA7 File Offset: 0x0002EDA7
		public bool SerializeReadOnlyTypes { get; set; }

		// Token: 0x040004A3 RID: 1187
		private int maxItemsInObjectGraph = int.MaxValue;
	}
}
