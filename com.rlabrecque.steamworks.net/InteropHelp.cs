using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Steamworks
{
	// Token: 0x02000179 RID: 377
	public class InteropHelp
	{
		// Token: 0x060008C7 RID: 2247 RVA: 0x0000D0BB File Offset: 0x0000B2BB
		public static void TestIfPlatformSupported()
		{
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0000D0BD File Offset: 0x0000B2BD
		public static void TestIfAvailableClient()
		{
			InteropHelp.TestIfPlatformSupported();
			if (CSteamAPIContext.GetSteamClient() == IntPtr.Zero && !CSteamAPIContext.Init())
			{
				throw new InvalidOperationException("Steamworks is not initialized.");
			}
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0000D0E7 File Offset: 0x0000B2E7
		public static void TestIfAvailableGameServer()
		{
			InteropHelp.TestIfPlatformSupported();
			if (CSteamGameServerAPIContext.GetSteamClient() == IntPtr.Zero && !CSteamGameServerAPIContext.Init())
			{
				throw new InvalidOperationException("Steamworks GameServer is not initialized.");
			}
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0000D114 File Offset: 0x0000B314
		public static string PtrToStringUTF8(IntPtr nativeUtf8)
		{
			if (nativeUtf8 == IntPtr.Zero)
			{
				return null;
			}
			int num = 0;
			while (Marshal.ReadByte(nativeUtf8, num) != 0)
			{
				num++;
			}
			if (num == 0)
			{
				return string.Empty;
			}
			byte[] array = new byte[num];
			Marshal.Copy(nativeUtf8, array, 0, array.Length);
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0000D168 File Offset: 0x0000B368
		public static string ByteArrayToStringUTF8(byte[] buffer)
		{
			int num = 0;
			while (num < buffer.Length && buffer[num] != 0)
			{
				num++;
			}
			return Encoding.UTF8.GetString(buffer, 0, num);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0000D198 File Offset: 0x0000B398
		public static void StringToByteArrayUTF8(string str, byte[] outArrayBuffer, int outArrayBufferSize)
		{
			outArrayBuffer = new byte[outArrayBufferSize];
			int bytes = Encoding.UTF8.GetBytes(str, 0, str.Length, outArrayBuffer, 0);
			outArrayBuffer[bytes] = 0;
		}

		// Token: 0x020001CB RID: 459
		public class UTF8StringHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			// Token: 0x06000B9D RID: 2973 RVA: 0x000111E4 File Offset: 0x0000F3E4
			public UTF8StringHandle(string str)
				: base(true)
			{
				if (str == null)
				{
					base.SetHandle(IntPtr.Zero);
					return;
				}
				byte[] array = new byte[Encoding.UTF8.GetByteCount(str) + 1];
				Encoding.UTF8.GetBytes(str, 0, str.Length, array, 0);
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				base.SetHandle(intPtr);
			}

			// Token: 0x06000B9E RID: 2974 RVA: 0x0001124A File Offset: 0x0000F44A
			protected override bool ReleaseHandle()
			{
				if (!this.IsInvalid)
				{
					Marshal.FreeHGlobal(this.handle);
				}
				return true;
			}
		}

		// Token: 0x020001CC RID: 460
		public class SteamParamStringArray
		{
			// Token: 0x06000B9F RID: 2975 RVA: 0x00011260 File Offset: 0x0000F460
			public SteamParamStringArray(IList<string> strings)
			{
				if (strings == null)
				{
					this.m_pSteamParamStringArray = IntPtr.Zero;
					return;
				}
				this.m_Strings = new IntPtr[strings.Count];
				for (int i = 0; i < strings.Count; i++)
				{
					byte[] array = new byte[Encoding.UTF8.GetByteCount(strings[i]) + 1];
					Encoding.UTF8.GetBytes(strings[i], 0, strings[i].Length, array, 0);
					this.m_Strings[i] = Marshal.AllocHGlobal(array.Length);
					Marshal.Copy(array, 0, this.m_Strings[i], array.Length);
				}
				this.m_ptrStrings = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>() * this.m_Strings.Length);
				SteamParamStringArray_t steamParamStringArray_t = new SteamParamStringArray_t
				{
					m_ppStrings = this.m_ptrStrings,
					m_nNumStrings = this.m_Strings.Length
				};
				Marshal.Copy(this.m_Strings, 0, steamParamStringArray_t.m_ppStrings, this.m_Strings.Length);
				this.m_pSteamParamStringArray = Marshal.AllocHGlobal(Marshal.SizeOf<SteamParamStringArray_t>());
				Marshal.StructureToPtr<SteamParamStringArray_t>(steamParamStringArray_t, this.m_pSteamParamStringArray, false);
			}

			// Token: 0x06000BA0 RID: 2976 RVA: 0x00011378 File Offset: 0x0000F578
			protected override void Finalize()
			{
				try
				{
					if (this.m_Strings != null)
					{
						IntPtr[] strings = this.m_Strings;
						for (int i = 0; i < strings.Length; i++)
						{
							Marshal.FreeHGlobal(strings[i]);
						}
					}
					if (this.m_ptrStrings != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_ptrStrings);
					}
					if (this.m_pSteamParamStringArray != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.m_pSteamParamStringArray);
					}
				}
				finally
				{
					base.Finalize();
				}
			}

			// Token: 0x06000BA1 RID: 2977 RVA: 0x00011400 File Offset: 0x0000F600
			public static implicit operator IntPtr(InteropHelp.SteamParamStringArray that)
			{
				return that.m_pSteamParamStringArray;
			}

			// Token: 0x04000C12 RID: 3090
			private IntPtr[] m_Strings;

			// Token: 0x04000C13 RID: 3091
			private IntPtr m_ptrStrings;

			// Token: 0x04000C14 RID: 3092
			private IntPtr m_pSteamParamStringArray;
		}
	}
}
