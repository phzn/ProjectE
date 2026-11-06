using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32
{
	// Token: 0x02000132 RID: 306
	internal static class NativeMethods
	{
		// Token: 0x06000713 RID: 1811 RVA: 0x0001388C File Offset: 0x00011A8C
		public static bool DuplicateHandle(HandleRef hSourceProcessHandle, SafeHandle hSourceHandle, HandleRef hTargetProcess, out SafeWaitHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions)
		{
			bool flag = false;
			bool flag3;
			try
			{
				hSourceHandle.DangerousAddRef(ref flag);
				IntPtr intPtr;
				MonoIOError monoIOError;
				bool flag2 = MonoIO.DuplicateHandle(hSourceProcessHandle.Handle, hSourceHandle.DangerousGetHandle(), hTargetProcess.Handle, out intPtr, dwDesiredAccess, bInheritHandle ? 1 : 0, dwOptions, out monoIOError);
				if (monoIOError != MonoIOError.ERROR_SUCCESS)
				{
					throw MonoIO.GetException(monoIOError);
				}
				targetHandle = new SafeWaitHandle(intPtr, true);
				flag3 = flag2;
			}
			finally
			{
				if (flag)
				{
					hSourceHandle.DangerousRelease();
				}
			}
			return flag3;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00013900 File Offset: 0x00011B00
		public static bool DuplicateHandle(HandleRef hSourceProcessHandle, HandleRef hSourceHandle, HandleRef hTargetProcess, out SafeProcessHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions)
		{
			IntPtr intPtr;
			MonoIOError monoIOError;
			bool flag = MonoIO.DuplicateHandle(hSourceProcessHandle.Handle, hSourceHandle.Handle, hTargetProcess.Handle, out intPtr, dwDesiredAccess, bInheritHandle ? 1 : 0, dwOptions, out monoIOError);
			if (monoIOError != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(monoIOError);
			}
			targetHandle = new SafeProcessHandle(intPtr, true);
			return flag;
		}

		// Token: 0x06000715 RID: 1813
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetCurrentProcess();

		// Token: 0x06000716 RID: 1814
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetExitCodeProcess(IntPtr processHandle, out int exitCode);

		// Token: 0x06000717 RID: 1815 RVA: 0x0001394C File Offset: 0x00011B4C
		public static bool GetExitCodeProcess(SafeProcessHandle processHandle, out int exitCode)
		{
			bool flag = false;
			bool exitCodeProcess;
			try
			{
				processHandle.DangerousAddRef(ref flag);
				exitCodeProcess = NativeMethods.GetExitCodeProcess(processHandle.DangerousGetHandle(), out exitCode);
			}
			finally
			{
				if (flag)
				{
					processHandle.DangerousRelease();
				}
			}
			return exitCodeProcess;
		}

		// Token: 0x06000718 RID: 1816
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool TerminateProcess(IntPtr processHandle, int exitCode);

		// Token: 0x06000719 RID: 1817 RVA: 0x00013990 File Offset: 0x00011B90
		public static bool TerminateProcess(SafeProcessHandle processHandle, int exitCode)
		{
			bool flag = false;
			bool flag2;
			try
			{
				processHandle.DangerousAddRef(ref flag);
				flag2 = NativeMethods.TerminateProcess(processHandle.DangerousGetHandle(), exitCode);
			}
			finally
			{
				if (flag)
				{
					processHandle.DangerousRelease();
				}
			}
			return flag2;
		}

		// Token: 0x0600071A RID: 1818
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int WaitForInputIdle(IntPtr handle, int milliseconds);

		// Token: 0x0600071B RID: 1819 RVA: 0x000139D4 File Offset: 0x00011BD4
		public static int WaitForInputIdle(SafeProcessHandle handle, int milliseconds)
		{
			bool flag = false;
			int num;
			try
			{
				handle.DangerousAddRef(ref flag);
				num = NativeMethods.WaitForInputIdle(handle.DangerousGetHandle(), milliseconds);
			}
			finally
			{
				if (flag)
				{
					handle.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x0600071C RID: 1820
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetProcessWorkingSetSize(IntPtr handle, out IntPtr min, out IntPtr max);

		// Token: 0x0600071D RID: 1821 RVA: 0x00013A18 File Offset: 0x00011C18
		public static bool GetProcessWorkingSetSize(SafeProcessHandle handle, out IntPtr min, out IntPtr max)
		{
			bool flag = false;
			bool processWorkingSetSize;
			try
			{
				handle.DangerousAddRef(ref flag);
				processWorkingSetSize = NativeMethods.GetProcessWorkingSetSize(handle.DangerousGetHandle(), out min, out max);
			}
			finally
			{
				if (flag)
				{
					handle.DangerousRelease();
				}
			}
			return processWorkingSetSize;
		}

		// Token: 0x0600071E RID: 1822
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetProcessWorkingSetSize(IntPtr handle, IntPtr min, IntPtr max);

		// Token: 0x0600071F RID: 1823 RVA: 0x00013A5C File Offset: 0x00011C5C
		public static bool SetProcessWorkingSetSize(SafeProcessHandle handle, IntPtr min, IntPtr max)
		{
			bool flag = false;
			bool flag2;
			try
			{
				handle.DangerousAddRef(ref flag);
				flag2 = NativeMethods.SetProcessWorkingSetSize(handle.DangerousGetHandle(), min, max);
			}
			finally
			{
				if (flag)
				{
					handle.DangerousRelease();
				}
			}
			return flag2;
		}

		// Token: 0x06000720 RID: 1824
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetProcessTimes(IntPtr handle, out long creation, out long exit, out long kernel, out long user);

		// Token: 0x06000721 RID: 1825 RVA: 0x00013AA0 File Offset: 0x00011CA0
		public static bool GetProcessTimes(SafeProcessHandle handle, out long creation, out long exit, out long kernel, out long user)
		{
			bool flag = false;
			bool processTimes;
			try
			{
				handle.DangerousAddRef(ref flag);
				processTimes = NativeMethods.GetProcessTimes(handle.DangerousGetHandle(), out creation, out exit, out kernel, out user);
			}
			finally
			{
				if (flag)
				{
					handle.DangerousRelease();
				}
			}
			return processTimes;
		}

		// Token: 0x06000722 RID: 1826
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetCurrentProcessId();

		// Token: 0x06000723 RID: 1827
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetPriorityClass(IntPtr handle);

		// Token: 0x06000724 RID: 1828 RVA: 0x00013AE8 File Offset: 0x00011CE8
		public static int GetPriorityClass(SafeProcessHandle handle)
		{
			bool flag = false;
			int priorityClass;
			try
			{
				handle.DangerousAddRef(ref flag);
				priorityClass = NativeMethods.GetPriorityClass(handle.DangerousGetHandle());
			}
			finally
			{
				if (flag)
				{
					handle.DangerousRelease();
				}
			}
			return priorityClass;
		}

		// Token: 0x06000725 RID: 1829
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetPriorityClass(IntPtr handle, int priorityClass);

		// Token: 0x06000726 RID: 1830 RVA: 0x00013B28 File Offset: 0x00011D28
		public static bool SetPriorityClass(SafeProcessHandle handle, int priorityClass)
		{
			bool flag = false;
			bool flag2;
			try
			{
				handle.DangerousAddRef(ref flag);
				flag2 = NativeMethods.SetPriorityClass(handle.DangerousGetHandle(), priorityClass);
			}
			finally
			{
				if (flag)
				{
					handle.DangerousRelease();
				}
			}
			return flag2;
		}

		// Token: 0x06000727 RID: 1831
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CloseProcess(IntPtr handle);

		// Token: 0x040004F7 RID: 1271
		public const int E_ABORT = -2147467260;

		// Token: 0x040004F8 RID: 1272
		public const int PROCESS_TERMINATE = 1;

		// Token: 0x040004F9 RID: 1273
		public const int PROCESS_CREATE_THREAD = 2;

		// Token: 0x040004FA RID: 1274
		public const int PROCESS_SET_SESSIONID = 4;

		// Token: 0x040004FB RID: 1275
		public const int PROCESS_VM_OPERATION = 8;

		// Token: 0x040004FC RID: 1276
		public const int PROCESS_VM_READ = 16;

		// Token: 0x040004FD RID: 1277
		public const int PROCESS_VM_WRITE = 32;

		// Token: 0x040004FE RID: 1278
		public const int PROCESS_DUP_HANDLE = 64;

		// Token: 0x040004FF RID: 1279
		public const int PROCESS_CREATE_PROCESS = 128;

		// Token: 0x04000500 RID: 1280
		public const int PROCESS_SET_QUOTA = 256;

		// Token: 0x04000501 RID: 1281
		public const int PROCESS_SET_INFORMATION = 512;

		// Token: 0x04000502 RID: 1282
		public const int PROCESS_QUERY_INFORMATION = 1024;

		// Token: 0x04000503 RID: 1283
		public const int PROCESS_QUERY_LIMITED_INFORMATION = 4096;

		// Token: 0x04000504 RID: 1284
		public const int STANDARD_RIGHTS_REQUIRED = 983040;

		// Token: 0x04000505 RID: 1285
		public const int SYNCHRONIZE = 1048576;

		// Token: 0x04000506 RID: 1286
		public const int PROCESS_ALL_ACCESS = 2035711;

		// Token: 0x04000507 RID: 1287
		public const int DUPLICATE_CLOSE_SOURCE = 1;

		// Token: 0x04000508 RID: 1288
		public const int DUPLICATE_SAME_ACCESS = 2;

		// Token: 0x04000509 RID: 1289
		public const int STILL_ACTIVE = 259;

		// Token: 0x0400050A RID: 1290
		public const int WAIT_OBJECT_0 = 0;

		// Token: 0x0400050B RID: 1291
		public const int WAIT_FAILED = -1;

		// Token: 0x0400050C RID: 1292
		public const int WAIT_TIMEOUT = 258;

		// Token: 0x0400050D RID: 1293
		public const int WAIT_ABANDONED = 128;

		// Token: 0x0400050E RID: 1294
		public const int WAIT_ABANDONED_0 = 128;

		// Token: 0x0400050F RID: 1295
		public const int ERROR_FILE_NOT_FOUND = 2;

		// Token: 0x04000510 RID: 1296
		public const int ERROR_PATH_NOT_FOUND = 3;

		// Token: 0x04000511 RID: 1297
		public const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04000512 RID: 1298
		public const int ERROR_INVALID_HANDLE = 6;

		// Token: 0x04000513 RID: 1299
		public const int ERROR_SHARING_VIOLATION = 32;

		// Token: 0x04000514 RID: 1300
		public const int ERROR_INVALID_NAME = 123;

		// Token: 0x04000515 RID: 1301
		public const int ERROR_ALREADY_EXISTS = 183;

		// Token: 0x04000516 RID: 1302
		public const int ERROR_FILENAME_EXCED_RANGE = 206;
	}
}
