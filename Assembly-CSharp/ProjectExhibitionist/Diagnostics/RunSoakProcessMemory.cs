using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ProjectExhibitionist.Diagnostics
{
	// Token: 0x02000A1E RID: 2590
	public static class RunSoakProcessMemory
	{
		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06004994 RID: 18836 RVA: 0x001C3F53 File Offset: 0x001C2153
		public static int NativeLayoutBytes
		{
			get
			{
				return Marshal.SizeOf(typeof(RunSoakProcessMemory.ProcessMemoryCountersEx));
			}
		}

		// Token: 0x06004995 RID: 18837
		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern IntPtr GetCurrentProcess();

		// Token: 0x06004996 RID: 18838
		[DllImport("psapi.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetProcessMemoryInfo(IntPtr process, ref RunSoakProcessMemory.ProcessMemoryCountersEx counters, uint size);

		// Token: 0x06004997 RID: 18839 RVA: 0x001C3F64 File Offset: 0x001C2164
		public static long PositiveOrUnavailable(long value)
		{
			if (value <= 0L)
			{
				return -1L;
			}
			return value;
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x001C3F70 File Offset: 0x001C2170
		private static long PositiveOrUnavailable(UIntPtr value)
		{
			ulong num = value.ToUInt64();
			if (num <= 0UL || num > 9223372036854775807UL)
			{
				return -1L;
			}
			return (long)num;
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x001C3F9C File Offset: 0x001C219C
		public static bool TryReadWindows(out long workingSetBytes, out long privateBytes)
		{
			long num;
			long num2;
			return RunSoakProcessMemory.TryReadWindows(out workingSetBytes, out privateBytes, out num, out num2);
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x001C3FB4 File Offset: 0x001C21B4
		public static bool TryReadWindows(out long workingSetBytes, out long privateBytes, out long peakWorkingSetBytes, out long peakPrivateBytes)
		{
			workingSetBytes = (privateBytes = (peakWorkingSetBytes = (peakPrivateBytes = -1L)));
			try
			{
				RunSoakProcessMemory.ProcessMemoryCountersEx processMemoryCountersEx = default(RunSoakProcessMemory.ProcessMemoryCountersEx);
				processMemoryCountersEx.cb = (uint)RunSoakProcessMemory.NativeLayoutBytes;
				if (!RunSoakProcessMemory.GetProcessMemoryInfo(RunSoakProcessMemory.GetCurrentProcess(), ref processMemoryCountersEx, processMemoryCountersEx.cb))
				{
					return false;
				}
				ulong num = processMemoryCountersEx.WorkingSetSize.ToUInt64();
				ulong num2 = processMemoryCountersEx.PrivateUsage.ToUInt64();
				if (num == 0UL || num2 == 0UL || num > 9223372036854775807UL || num2 > 9223372036854775807UL)
				{
					return false;
				}
				workingSetBytes = (long)num;
				privateBytes = (long)num2;
				peakWorkingSetBytes = RunSoakProcessMemory.PositiveOrUnavailable(processMemoryCountersEx.PeakWorkingSetSize);
				peakPrivateBytes = RunSoakProcessMemory.PositiveOrUnavailable(processMemoryCountersEx.PeakPagefileUsage);
				return true;
			}
			catch (DllNotFoundException)
			{
			}
			catch (EntryPointNotFoundException)
			{
			}
			catch (BadImageFormatException)
			{
			}
			return false;
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x001C4098 File Offset: 0x001C2298
		public static int Read(Process process, out long workingSetBytes, out long privateBytes)
		{
			if (RunSoakProcessMemory.TryReadWindows(out workingSetBytes, out privateBytes))
			{
				return 1;
			}
			if (process == null)
			{
				return 0;
			}
			int num;
			try
			{
				process.Refresh();
				workingSetBytes = RunSoakProcessMemory.PositiveOrUnavailable(process.WorkingSet64);
				privateBytes = RunSoakProcessMemory.PositiveOrUnavailable(process.PrivateMemorySize64);
				num = ((workingSetBytes > 0L || privateBytes > 0L) ? 2 : 0);
			}
			catch
			{
				workingSetBytes = (privateBytes = -1L);
				num = 0;
			}
			return num;
		}

		// Token: 0x04004A92 RID: 19090
		public const int Unavailable = 0;

		// Token: 0x04004A93 RID: 19091
		public const int WindowsPsapi = 1;

		// Token: 0x04004A94 RID: 19092
		public const int ManagedProcess = 2;

		// Token: 0x02000A1F RID: 2591
		private struct ProcessMemoryCountersEx
		{
			// Token: 0x04004A95 RID: 19093
			public uint cb;

			// Token: 0x04004A96 RID: 19094
			public uint PageFaultCount;

			// Token: 0x04004A97 RID: 19095
			public UIntPtr PeakWorkingSetSize;

			// Token: 0x04004A98 RID: 19096
			public UIntPtr WorkingSetSize;

			// Token: 0x04004A99 RID: 19097
			public UIntPtr QuotaPeakPagedPoolUsage;

			// Token: 0x04004A9A RID: 19098
			public UIntPtr QuotaPagedPoolUsage;

			// Token: 0x04004A9B RID: 19099
			public UIntPtr QuotaPeakNonPagedPoolUsage;

			// Token: 0x04004A9C RID: 19100
			public UIntPtr QuotaNonPagedPoolUsage;

			// Token: 0x04004A9D RID: 19101
			public UIntPtr PagefileUsage;

			// Token: 0x04004A9E RID: 19102
			public UIntPtr PeakPagefileUsage;

			// Token: 0x04004A9F RID: 19103
			public UIntPtr PrivateUsage;
		}
	}
}
