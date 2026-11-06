using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x02000187 RID: 391
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 372)]
	public class gameserveritem_t
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x0000EAB9 File Offset: 0x0000CCB9
		public string GetGameDir()
		{
			return Encoding.UTF8.GetString(this.m_szGameDir, 0, Array.IndexOf<byte>(this.m_szGameDir, 0));
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0000EAD8 File Offset: 0x0000CCD8
		public void SetGameDir(string dir)
		{
			this.m_szGameDir = Encoding.UTF8.GetBytes(dir + "\0");
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0000EAF5 File Offset: 0x0000CCF5
		public string GetMap()
		{
			return Encoding.UTF8.GetString(this.m_szMap, 0, Array.IndexOf<byte>(this.m_szMap, 0));
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0000EB14 File Offset: 0x0000CD14
		public void SetMap(string map)
		{
			this.m_szMap = Encoding.UTF8.GetBytes(map + "\0");
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0000EB31 File Offset: 0x0000CD31
		public string GetGameDescription()
		{
			return Encoding.UTF8.GetString(this.m_szGameDescription, 0, Array.IndexOf<byte>(this.m_szGameDescription, 0));
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0000EB50 File Offset: 0x0000CD50
		public void SetGameDescription(string desc)
		{
			this.m_szGameDescription = Encoding.UTF8.GetBytes(desc + "\0");
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0000EB6D File Offset: 0x0000CD6D
		public string GetServerName()
		{
			if (this.m_szServerName[0] == 0)
			{
				return this.m_NetAdr.GetConnectionAddressString();
			}
			return Encoding.UTF8.GetString(this.m_szServerName, 0, Array.IndexOf<byte>(this.m_szServerName, 0));
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0000EBA2 File Offset: 0x0000CDA2
		public void SetServerName(string name)
		{
			this.m_szServerName = Encoding.UTF8.GetBytes(name + "\0");
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0000EBBF File Offset: 0x0000CDBF
		public string GetGameTags()
		{
			return Encoding.UTF8.GetString(this.m_szGameTags, 0, Array.IndexOf<byte>(this.m_szGameTags, 0));
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0000EBDE File Offset: 0x0000CDDE
		public void SetGameTags(string tags)
		{
			this.m_szGameTags = Encoding.UTF8.GetBytes(tags + "\0");
		}

		// Token: 0x04000B57 RID: 2903
		public servernetadr_t m_NetAdr;

		// Token: 0x04000B58 RID: 2904
		public int m_nPing;

		// Token: 0x04000B59 RID: 2905
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHadSuccessfulResponse;

		// Token: 0x04000B5A RID: 2906
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bDoNotRefresh;

		// Token: 0x04000B5B RID: 2907
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szGameDir;

		// Token: 0x04000B5C RID: 2908
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] m_szMap;

		// Token: 0x04000B5D RID: 2909
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szGameDescription;

		// Token: 0x04000B5E RID: 2910
		public uint m_nAppID;

		// Token: 0x04000B5F RID: 2911
		public int m_nPlayers;

		// Token: 0x04000B60 RID: 2912
		public int m_nMaxPlayers;

		// Token: 0x04000B61 RID: 2913
		public int m_nBotPlayers;

		// Token: 0x04000B62 RID: 2914
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bPassword;

		// Token: 0x04000B63 RID: 2915
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSecure;

		// Token: 0x04000B64 RID: 2916
		public uint m_ulTimeLastPlayed;

		// Token: 0x04000B65 RID: 2917
		public int m_nServerVersion;

		// Token: 0x04000B66 RID: 2918
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_szServerName;

		// Token: 0x04000B67 RID: 2919
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_szGameTags;

		// Token: 0x04000B68 RID: 2920
		public CSteamID m_steamID;
	}
}
