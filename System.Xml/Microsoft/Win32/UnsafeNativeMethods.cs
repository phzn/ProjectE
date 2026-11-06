using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32
{
	// Token: 0x02000006 RID: 6
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		// Token: 0x0600000B RID: 11
		[SecurityCritical]
		[DllImport("kernel32.dll", EntryPoint = "GetCurrentPackageId")]
		[return: MarshalAs(UnmanagedType.I4)]
		private static extern int _GetCurrentPackageId(ref int pBufferLength, byte[] pBuffer);

		// Token: 0x0600000C RID: 12
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string methodName);

		// Token: 0x0600000D RID: 13
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string moduleName);

		// Token: 0x0600000E RID: 14 RVA: 0x000020BC File Offset: 0x000002BC
		[SecurityCritical]
		private static bool DoesWin32MethodExist(string moduleName, string methodName)
		{
			IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(moduleName);
			return !(moduleHandle == IntPtr.Zero) && UnsafeNativeMethods.GetProcAddress(moduleHandle, methodName) != IntPtr.Zero;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000020F0 File Offset: 0x000002F0
		[SecuritySafeCritical]
		private static bool _IsPackagedProcess()
		{
			OperatingSystem osversion = Environment.OSVersion;
			if (osversion.Platform == PlatformID.Win32NT && osversion.Version >= new Version(6, 2, 0, 0) && UnsafeNativeMethods.DoesWin32MethodExist("kernel32.dll", "GetCurrentPackageId"))
			{
				int num = 0;
				return UnsafeNativeMethods._GetCurrentPackageId(ref num, null) == 122;
			}
			return false;
		}

		// Token: 0x0400048D RID: 1165
		internal const string KERNEL32 = "kernel32.dll";

		// Token: 0x0400048E RID: 1166
		internal const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x0400048F RID: 1167
		internal const int ERROR_NO_PACKAGE_IDENTITY = 15700;

		// Token: 0x04000490 RID: 1168
		[SecuritySafeCritical]
		internal static Lazy<bool> IsPackagedProcess = new Lazy<bool>(() => UnsafeNativeMethods._IsPackagedProcess());
	}
}
