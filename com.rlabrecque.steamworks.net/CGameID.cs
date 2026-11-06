using System;

namespace Steamworks
{
	// Token: 0x0200018B RID: 395
	[Serializable]
	public struct CGameID : IEquatable<CGameID>, IComparable<CGameID>
	{
		// Token: 0x06000959 RID: 2393 RVA: 0x0000EE27 File Offset: 0x0000D027
		public CGameID(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0000EE30 File Offset: 0x0000D030
		public CGameID(AppId_t nAppID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0000EE41 File Offset: 0x0000D041
		public CGameID(AppId_t nAppID, uint nModID)
		{
			this.m_GameID = 0UL;
			this.SetAppID(nAppID);
			this.SetType(CGameID.EGameIDType.k_EGameIDTypeGameMod);
			this.SetModID(nModID);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0000EE60 File Offset: 0x0000D060
		public bool IsSteamApp()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeApp;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0000EE6B File Offset: 0x0000D06B
		public bool IsMod()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeGameMod;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0000EE76 File Offset: 0x0000D076
		public bool IsShortcut()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeShortcut;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0000EE81 File Offset: 0x0000D081
		public bool IsP2PFile()
		{
			return this.Type() == CGameID.EGameIDType.k_EGameIDTypeP2P;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0000EE8C File Offset: 0x0000D08C
		public AppId_t AppID()
		{
			return new AppId_t((uint)(this.m_GameID & 16777215UL));
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0000EEA1 File Offset: 0x0000D0A1
		public CGameID.EGameIDType Type()
		{
			return (CGameID.EGameIDType)((this.m_GameID >> 24) & 255UL);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0000EEB4 File Offset: 0x0000D0B4
		public uint ModID()
		{
			return (uint)((this.m_GameID >> 32) & (ulong)(-1));
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0000EEC4 File Offset: 0x0000D0C4
		public bool IsValid()
		{
			switch (this.Type())
			{
			case CGameID.EGameIDType.k_EGameIDTypeApp:
				return this.AppID() != AppId_t.Invalid;
			case CGameID.EGameIDType.k_EGameIDTypeGameMod:
				return this.AppID() != AppId_t.Invalid && (this.ModID() & 2147483648U) > 0U;
			case CGameID.EGameIDType.k_EGameIDTypeShortcut:
				return (this.ModID() & 2147483648U) > 0U;
			case CGameID.EGameIDType.k_EGameIDTypeP2P:
				return this.AppID() == AppId_t.Invalid && (this.ModID() & 2147483648U) > 0U;
			default:
				return false;
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0000EF5A File Offset: 0x0000D15A
		public void Reset()
		{
			this.m_GameID = 0UL;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0000EF64 File Offset: 0x0000D164
		public void Set(ulong GameID)
		{
			this.m_GameID = GameID;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0000EF6D File Offset: 0x0000D16D
		private void SetAppID(AppId_t other)
		{
			this.m_GameID = (this.m_GameID & 18446744073692774400UL) | ((ulong)(uint)other & 16777215UL);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0000EF91 File Offset: 0x0000D191
		private void SetType(CGameID.EGameIDType other)
		{
			this.m_GameID = (this.m_GameID & 18446744069431361535UL) | (ulong)((ulong)((long)other & 255L) << 24);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0000EFB6 File Offset: 0x0000D1B6
		private void SetModID(uint other)
		{
			this.m_GameID = (this.m_GameID & (ulong)(-1)) | (((ulong)other & (ulong)(-1)) << 32);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0000EFD0 File Offset: 0x0000D1D0
		public override string ToString()
		{
			return this.m_GameID.ToString();
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0000EFDD File Offset: 0x0000D1DD
		public override bool Equals(object other)
		{
			return other is CGameID && this == (CGameID)other;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0000EFFA File Offset: 0x0000D1FA
		public override int GetHashCode()
		{
			return this.m_GameID.GetHashCode();
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0000F007 File Offset: 0x0000D207
		public static bool operator ==(CGameID x, CGameID y)
		{
			return x.m_GameID == y.m_GameID;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0000F017 File Offset: 0x0000D217
		public static bool operator !=(CGameID x, CGameID y)
		{
			return !(x == y);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0000F023 File Offset: 0x0000D223
		public static explicit operator CGameID(ulong value)
		{
			return new CGameID(value);
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0000F02B File Offset: 0x0000D22B
		public static explicit operator ulong(CGameID that)
		{
			return that.m_GameID;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0000F033 File Offset: 0x0000D233
		public bool Equals(CGameID other)
		{
			return this.m_GameID == other.m_GameID;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0000F043 File Offset: 0x0000D243
		public int CompareTo(CGameID other)
		{
			return this.m_GameID.CompareTo(other.m_GameID);
		}

		// Token: 0x04000B6C RID: 2924
		public ulong m_GameID;

		// Token: 0x020001E8 RID: 488
		public enum EGameIDType
		{
			// Token: 0x04000C25 RID: 3109
			k_EGameIDTypeApp,
			// Token: 0x04000C26 RID: 3110
			k_EGameIDTypeGameMod,
			// Token: 0x04000C27 RID: 3111
			k_EGameIDTypeShortcut,
			// Token: 0x04000C28 RID: 3112
			k_EGameIDTypeP2P
		}
	}
}
