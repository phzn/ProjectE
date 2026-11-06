using System;
using System.Collections.Generic;

namespace System.Runtime.Serialization.Json
{
	/// <summary>Specifies <see cref="T:System.Runtime.Serialization.Json.DataContractJsonSerializer" /> settings.</summary>
	// Token: 0x02000166 RID: 358
	public class DataContractJsonSerializerSettings
	{
		/// <summary>Gets or sets the root name of the selected object.</summary>
		/// <returns>The root name of the selected object.</returns>
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x00049DEC File Offset: 0x00047FEC
		// (set) Token: 0x060012ED RID: 4845 RVA: 0x00049DF4 File Offset: 0x00047FF4
		public string RootName { get; set; }

		/// <summary>Gets or sets a collection of types that may be present in the object graph serialized using this instance the DataContractJsonSerializerSettings.</summary>
		/// <returns>A collection of types that may be present in the object graph serialized using this instance the DataContractJsonSerializerSettings.</returns>
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x00049DFD File Offset: 0x00047FFD
		// (set) Token: 0x060012EF RID: 4847 RVA: 0x00049E05 File Offset: 0x00048005
		public IEnumerable<Type> KnownTypes { get; set; }

		/// <summary>Gets or sets the maximum number of items in an object graph to serialize or deserialize.</summary>
		/// <returns>The maximum number of items in an object graph to serialize or deserialize.</returns>
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x00049E0E File Offset: 0x0004800E
		// (set) Token: 0x060012F1 RID: 4849 RVA: 0x00049E16 File Offset: 0x00048016
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
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x00049E1F File Offset: 0x0004801F
		// (set) Token: 0x060012F3 RID: 4851 RVA: 0x00049E27 File Offset: 0x00048027
		public bool IgnoreExtensionDataObject { get; set; }

		/// <summary>Gets or sets a surrogate type that is currently active for given IDataContractSurrogate instance.</summary>
		/// <returns>The surrogate type that is currently active for given IDataContractSurrogate instance.</returns>
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x00049E30 File Offset: 0x00048030
		// (set) Token: 0x060012F5 RID: 4853 RVA: 0x00049E38 File Offset: 0x00048038
		public IDataContractSurrogate DataContractSurrogate { get; set; }

		/// <summary>Gets or sets the data contract JSON serializer settings to emit type information.</summary>
		/// <returns>The data contract JSON serializer settings to emit type information.</returns>
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x00049E41 File Offset: 0x00048041
		// (set) Token: 0x060012F7 RID: 4855 RVA: 0x00049E49 File Offset: 0x00048049
		public EmitTypeInformation EmitTypeInformation { get; set; }

		/// <summary>Gets or sets a DateTimeFormat that defines the culturally appropriate format of displaying dates and times.</summary>
		/// <returns>The DateTimeFormat that defines the culturally appropriate format of displaying dates and times.</returns>
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x00049E52 File Offset: 0x00048052
		// (set) Token: 0x060012F9 RID: 4857 RVA: 0x00049E5A File Offset: 0x0004805A
		public DateTimeFormat DateTimeFormat { get; set; }

		/// <summary>Gets or sets a value that specifies whether to serialize read only types.</summary>
		/// <returns>
		///   <see langword="true" /> to serialize read only types; otherwise <see langword="false" />.</returns>
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x00049E63 File Offset: 0x00048063
		// (set) Token: 0x060012FB RID: 4859 RVA: 0x00049E6B File Offset: 0x0004806B
		public bool SerializeReadOnlyTypes { get; set; }

		/// <summary>Gets or sets a value that specifies whether to use a simple dictionary format.</summary>
		/// <returns>
		///   <see langword="true" /> to use a simple dictionary format; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x00049E74 File Offset: 0x00048074
		// (set) Token: 0x060012FD RID: 4861 RVA: 0x00049E7C File Offset: 0x0004807C
		public bool UseSimpleDictionaryFormat { get; set; }

		// Token: 0x0400097F RID: 2431
		private int maxItemsInObjectGraph = int.MaxValue;
	}
}
