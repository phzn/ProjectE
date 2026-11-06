using System;

namespace Microsoft.Win32
{
	/// <summary>Represents the possible values for a top-level node on a foreign machine.</summary>
	// Token: 0x020000A2 RID: 162
	public enum RegistryHive
	{
		/// <summary>Represents the HKEY_CLASSES_ROOT base key on another computer. This value can be passed to the <see cref="M:Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive,System.String)" /> method, to open this node remotely.</summary>
		// Token: 0x04000F4E RID: 3918
		ClassesRoot = -2147483648,
		/// <summary>Represents the HKEY_CURRENT_USER base key on another computer. This value can be passed to the <see cref="M:Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive,System.String)" /> method, to open this node remotely.</summary>
		// Token: 0x04000F4F RID: 3919
		CurrentUser,
		/// <summary>Represents the HKEY_LOCAL_MACHINE base key on another computer. This value can be passed to the <see cref="M:Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive,System.String)" /> method, to open this node remotely.</summary>
		// Token: 0x04000F50 RID: 3920
		LocalMachine,
		/// <summary>Represents the HKEY_USERS base key on another computer. This value can be passed to the <see cref="M:Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive,System.String)" /> method, to open this node remotely.</summary>
		// Token: 0x04000F51 RID: 3921
		Users,
		/// <summary>Represents the HKEY_PERFORMANCE_DATA base key on another computer. This value can be passed to the <see cref="M:Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive,System.String)" /> method, to open this node remotely.</summary>
		// Token: 0x04000F52 RID: 3922
		PerformanceData,
		/// <summary>Represents the HKEY_CURRENT_CONFIG base key on another computer. This value can be passed to the <see cref="M:Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive,System.String)" /> method, to open this node remotely.</summary>
		// Token: 0x04000F53 RID: 3923
		CurrentConfig,
		/// <summary>Represents the HKEY_DYN_DATA base key on another computer. This value can be passed to the <see cref="M:Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive,System.String)" /> method, to open this node remotely.</summary>
		// Token: 0x04000F54 RID: 3924
		DynData
	}
}
