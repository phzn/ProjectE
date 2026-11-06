using System;

namespace System.Reflection
{
	/// <summary>Specifies the version of the assembly being attributed.</summary>
	// Token: 0x02000891 RID: 2193
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyVersionAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see langword="AssemblyVersionAttribute" /> class with the version number of the assembly being attributed.</summary>
		/// <param name="version">The version number of the attributed assembly.</param>
		// Token: 0x06004878 RID: 18552 RVA: 0x000EE1AF File Offset: 0x000EC3AF
		public AssemblyVersionAttribute(string version)
		{
			this.Version = version;
		}

		/// <summary>Gets the version number of the attributed assembly.</summary>
		/// <returns>A string containing the assembly version number.</returns>
		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06004879 RID: 18553 RVA: 0x000EE1BE File Offset: 0x000EC3BE
		public string Version { get; }
	}
}
