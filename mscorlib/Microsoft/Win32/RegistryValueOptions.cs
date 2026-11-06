using System;

namespace Microsoft.Win32
{
	/// <summary>Specifies optional behavior when retrieving name/value pairs from a registry key.</summary>
	// Token: 0x020000A8 RID: 168
	[Flags]
	public enum RegistryValueOptions
	{
		/// <summary>No optional behavior is specified.</summary>
		// Token: 0x04000F7B RID: 3963
		None = 0,
		/// <summary>A value of type <see cref="F:Microsoft.Win32.RegistryValueKind.ExpandString" /> is retrieved without expanding its embedded environment variables.</summary>
		// Token: 0x04000F7C RID: 3964
		DoNotExpandEnvironmentNames = 1
	}
}
