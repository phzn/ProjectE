using System;

namespace System.Reflection
{
	/// <summary>Defines a friendly default alias for an assembly manifest.</summary>
	// Token: 0x02000883 RID: 2179
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyDefaultAliasAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyDefaultAliasAttribute" /> class.</summary>
		/// <param name="defaultAlias">The assembly default alias information.</param>
		// Token: 0x06004859 RID: 18521 RVA: 0x000EE058 File Offset: 0x000EC258
		public AssemblyDefaultAliasAttribute(string defaultAlias)
		{
			this.DefaultAlias = defaultAlias;
		}

		/// <summary>Gets default alias information.</summary>
		/// <returns>A string containing the default alias information.</returns>
		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x0600485A RID: 18522 RVA: 0x000EE067 File Offset: 0x000EC267
		public string DefaultAlias { get; }
	}
}
