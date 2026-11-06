using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;

namespace ProjectExhibitionist.Diagnostics
{
	// Token: 0x02000A13 RID: 2579
	public static class MemoryReport
	{
		// Token: 0x06004968 RID: 18792
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GlobalMemoryStatusEx(ref MemoryReport.MemoryStatusEx buffer);

		// Token: 0x06004969 RID: 18793 RVA: 0x001C23E5 File Offset: 0x001C05E5
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void LogBoot()
		{
			MemoryReport.Log("boot");
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x001C23F4 File Offset: 0x001C05F4
		public static bool TryReadSystem(out long physicalBytes, out long commitLimitBytes, out long commitAvailableBytes)
		{
			physicalBytes = (commitLimitBytes = (commitAvailableBytes = -1L));
			try
			{
				MemoryReport.MemoryStatusEx memoryStatusEx = default(MemoryReport.MemoryStatusEx);
				memoryStatusEx.dwLength = (uint)Marshal.SizeOf(typeof(MemoryReport.MemoryStatusEx));
				if (!MemoryReport.GlobalMemoryStatusEx(ref memoryStatusEx))
				{
					return false;
				}
				if (memoryStatusEx.ullTotalPhys == 0UL || memoryStatusEx.ullTotalPageFile == 0UL || memoryStatusEx.ullTotalPhys > 9223372036854775807UL || memoryStatusEx.ullTotalPageFile > 9223372036854775807UL || memoryStatusEx.ullAvailPageFile > 9223372036854775807UL)
				{
					return false;
				}
				physicalBytes = (long)memoryStatusEx.ullTotalPhys;
				commitLimitBytes = (long)memoryStatusEx.ullTotalPageFile;
				commitAvailableBytes = (long)memoryStatusEx.ullAvailPageFile;
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

		// Token: 0x0600496B RID: 18795 RVA: 0x001C24D4 File Offset: 0x001C06D4
		public static void Log(string phase)
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				stringBuilder.Append("[Memory] ").Append(string.IsNullOrEmpty(phase) ? "-" : phase);
				long num;
				long num2;
				long num3;
				if (MemoryReport.TryReadSystem(out num, out num2, out num3))
				{
					MemoryReport.Append(stringBuilder, "ram", num);
					MemoryReport.Append(stringBuilder, "commitLimit", num2);
					MemoryReport.Append(stringBuilder, "commitAvail", num3);
					if (num2 >= num)
					{
						MemoryReport.Append(stringBuilder, "pageFile", num2 - num);
					}
				}
				long num4;
				long num5;
				long num6;
				long num7;
				if (RunSoakProcessMemory.TryReadWindows(out num4, out num5, out num6, out num7))
				{
					MemoryReport.Append(stringBuilder, "procCommit", num5);
					MemoryReport.Append(stringBuilder, "ws", num4);
					if (num6 > 0L)
					{
						MemoryReport.Append(stringBuilder, "peakWs", num6);
					}
					if (num7 > 0L)
					{
						MemoryReport.Append(stringBuilder, "peakCommit", num7);
					}
				}
				MemoryReport.AppendGraphics(stringBuilder);
				Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "{0}", new object[] { stringBuilder.ToString() });
			}
			catch
			{
			}
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x001C25D8 File Offset: 0x001C07D8
		private static void AppendGraphics(StringBuilder line)
		{
			try
			{
				GraphicsSettingsController instance = GraphicsSettingsController.Instance;
				if (instance != null && instance.RuntimePipeline != null)
				{
					GraphicsSettingsModel model = instance.Model;
					line.Append(" textureQuality=").Append(model.textureQuality.ToString(CultureInfo.InvariantCulture)).Append(" mapLighting=")
						.Append(model.mapLighting.ToString());
				}
			}
			catch
			{
			}
			try
			{
				int width = Screen.width;
				int height = Screen.height;
				line.Append(" res=").Append(width.ToString(CultureInfo.InvariantCulture)).Append('x')
					.Append(height.ToString(CultureInfo.InvariantCulture));
			}
			catch
			{
			}
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x001C26B4 File Offset: 0x001C08B4
		private static void Append(StringBuilder line, string key, long bytes)
		{
			line.Append(' ').Append(key).Append('=')
				.Append((bytes / 1048576L).ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x04004A0A RID: 18954
		private const long Megabyte = 1048576L;

		// Token: 0x02000A14 RID: 2580
		private struct MemoryStatusEx
		{
			// Token: 0x04004A0B RID: 18955
			public uint dwLength;

			// Token: 0x04004A0C RID: 18956
			public uint dwMemoryLoad;

			// Token: 0x04004A0D RID: 18957
			public ulong ullTotalPhys;

			// Token: 0x04004A0E RID: 18958
			public ulong ullAvailPhys;

			// Token: 0x04004A0F RID: 18959
			public ulong ullTotalPageFile;

			// Token: 0x04004A10 RID: 18960
			public ulong ullAvailPageFile;

			// Token: 0x04004A11 RID: 18961
			public ulong ullTotalVirtual;

			// Token: 0x04004A12 RID: 18962
			public ulong ullAvailVirtual;

			// Token: 0x04004A13 RID: 18963
			public ulong ullAvailExtendedVirtual;
		}
	}
}
