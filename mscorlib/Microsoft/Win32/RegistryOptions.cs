using System;

namespace Microsoft.Win32
{
	/// <summary>Specifies options to use when creating a registry key.</summary>
	// Token: 0x020000A6 RID: 166
	[Flags]
	public enum RegistryOptions
	{
		/// <summary>A non-volatile key. This is the default.</summary>
		// Token: 0x04000F6F RID: 3951
		None = 0,
		/// <summary>A volatile key. The information is stored in memory and is not preserved when the corresponding registry hive is unloaded.</summary>
		// Token: 0x04000F70 RID: 3952
		Volatile = 1
	}
}
