using System;

namespace System.ComponentModel.Composition
{
	/// <summary>Specifies metadata for a part.</summary>
	// Token: 0x02000051 RID: 81
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class PartMetadataAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Composition.PartMetadataAttribute" /> class with the specified name and metadata value.</summary>
		/// <param name="name">A string that contains the name of the metadata value or <see langword="null" /> to use an empty string ("").</param>
		/// <param name="value">An object that contains the metadata value. This can be <see langword="null" />.</param>
		// Token: 0x06000224 RID: 548 RVA: 0x000069E3 File Offset: 0x00004BE3
		public PartMetadataAttribute(string name, object value)
		{
			this.Name = name ?? string.Empty;
			this.Value = value;
		}

		/// <summary>Gets the name of the metadata value.</summary>
		/// <returns>A string that contains the name of the metadata value.</returns>
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00006A02 File Offset: 0x00004C02
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00006A0A File Offset: 0x00004C0A
		public string Name { get; private set; }

		/// <summary>Gets the metadata value.</summary>
		/// <returns>An object that contains the metadata value.</returns>
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00006A13 File Offset: 0x00004C13
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00006A1B File Offset: 0x00004C1B
		public object Value { get; private set; }
	}
}
