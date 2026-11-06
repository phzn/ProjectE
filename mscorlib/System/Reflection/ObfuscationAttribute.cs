using System;

namespace System.Reflection
{
	/// <summary>Instructs obfuscation tools to take the specified actions for an assembly, type, or member.</summary>
	// Token: 0x020008B4 RID: 2228
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
	public sealed class ObfuscationAttribute : Attribute
	{
		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value indicating whether the obfuscation tool should remove this attribute after processing.</summary>
		/// <returns>
		///   <see langword="true" /> if an obfuscation tool should remove the attribute after processing; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x060049B7 RID: 18871 RVA: 0x000EF1D3 File Offset: 0x000ED3D3
		// (set) Token: 0x060049B8 RID: 18872 RVA: 0x000EF1DB File Offset: 0x000ED3DB
		public bool StripAfterObfuscation { get; set; } = true;

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value indicating whether the obfuscation tool should exclude the type or member from obfuscation.</summary>
		/// <returns>
		///   <see langword="true" /> if the type or member to which this attribute is applied should be excluded from obfuscation; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x060049B9 RID: 18873 RVA: 0x000EF1E4 File Offset: 0x000ED3E4
		// (set) Token: 0x060049BA RID: 18874 RVA: 0x000EF1EC File Offset: 0x000ED3EC
		public bool Exclude { get; set; } = true;

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value indicating whether the attribute of a type is to apply to the members of the type.</summary>
		/// <returns>
		///   <see langword="true" /> if the attribute is to apply to the members of the type; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x060049BB RID: 18875 RVA: 0x000EF1F5 File Offset: 0x000ED3F5
		// (set) Token: 0x060049BC RID: 18876 RVA: 0x000EF1FD File Offset: 0x000ED3FD
		public bool ApplyToMembers { get; set; } = true;

		/// <summary>Gets or sets a string value that is recognized by the obfuscation tool, and which specifies processing options.</summary>
		/// <returns>A string value that is recognized by the obfuscation tool, and which specifies processing options. The default is "all".</returns>
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x000EF206 File Offset: 0x000ED406
		// (set) Token: 0x060049BE RID: 18878 RVA: 0x000EF20E File Offset: 0x000ED40E
		public string Feature { get; set; } = "all";
	}
}
