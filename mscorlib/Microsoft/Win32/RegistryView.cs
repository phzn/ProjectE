using System;

namespace Microsoft.Win32
{
	/// <summary>Specifies which registry view to target on a 64-bit operating system.</summary>
	// Token: 0x020000A9 RID: 169
	public enum RegistryView
	{
		/// <summary>The default view.</summary>
		// Token: 0x04000F7E RID: 3966
		Default,
		/// <summary>The 64-bit view.</summary>
		// Token: 0x04000F7F RID: 3967
		Registry64 = 256,
		/// <summary>The 32-bit view.</summary>
		// Token: 0x04000F80 RID: 3968
		Registry32 = 512
	}
}
