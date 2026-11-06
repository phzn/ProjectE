using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200018C RID: 396
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CSteamID : IEquatable<CSteamID>, IComparable<CSteamID>
	{
		// Token: 0x06000972 RID: 2418 RVA: 0x0000F056 File Offset: 0x0000D256
		public CSteamID(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.Set(unAccountID, eUniverse, eAccountType);
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0000F069 File Offset: 0x0000D269
		public CSteamID(AccountID_t unAccountID, uint unAccountInstance, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.m_SteamID = 0UL;
			this.InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0000F07E File Offset: 0x0000D27E
		public CSteamID(ulong ulSteamID)
		{
			this.m_SteamID = ulSteamID;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0000F087 File Offset: 0x0000D287
		public void Set(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			if (eAccountType == EAccountType.k_EAccountTypeClan || eAccountType == EAccountType.k_EAccountTypeGameServer)
			{
				this.SetAccountInstance(0U);
				return;
			}
			this.SetAccountInstance(1U);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0000F0B5 File Offset: 0x0000D2B5
		public void InstancedSet(AccountID_t unAccountID, uint unInstance, EUniverse eUniverse, EAccountType eAccountType)
		{
			this.SetAccountID(unAccountID);
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(eAccountType);
			this.SetAccountInstance(unInstance);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0000F0D4 File Offset: 0x0000D2D4
		public void Clear()
		{
			this.m_SteamID = 0UL;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0000F0DE File Offset: 0x0000D2DE
		public void CreateBlankAnonLogon(EUniverse eUniverse)
		{
			this.SetAccountID(new AccountID_t(0U));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(EAccountType.k_EAccountTypeAnonGameServer);
			this.SetAccountInstance(0U);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0000F101 File Offset: 0x0000D301
		public void CreateBlankAnonUserLogon(EUniverse eUniverse)
		{
			this.SetAccountID(new AccountID_t(0U));
			this.SetEUniverse(eUniverse);
			this.SetEAccountType(EAccountType.k_EAccountTypeAnonUser);
			this.SetAccountInstance(0U);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0000F125 File Offset: 0x0000D325
		public bool BBlankAnonAccount()
		{
			return this.GetAccountID() == new AccountID_t(0U) && this.BAnonAccount() && this.GetUnAccountInstance() == 0U;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0000F14D File Offset: 0x0000D34D
		public bool BGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0000F163 File Offset: 0x0000D363
		public bool BPersistentGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0000F16E File Offset: 0x0000D36E
		public bool BAnonGameServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0000F179 File Offset: 0x0000D379
		public bool BContentServerAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeContentServer;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0000F184 File Offset: 0x0000D384
		public bool BClanAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeClan;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0000F18F File Offset: 0x0000D38F
		public bool BChatAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeChat;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0000F19A File Offset: 0x0000D39A
		public bool IsLobby()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeChat && (this.GetUnAccountInstance() & 262144U) > 0U;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0000F1B6 File Offset: 0x0000D3B6
		public bool BIndividualAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeIndividual || this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0000F1CD File Offset: 0x0000D3CD
		public bool BAnonAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0000F1E4 File Offset: 0x0000D3E4
		public bool BAnonUserAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0000F1F0 File Offset: 0x0000D3F0
		public bool BConsoleUserAccount()
		{
			return this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0000F1FC File Offset: 0x0000D3FC
		public void SetAccountID(AccountID_t other)
		{
			this.m_SteamID = (this.m_SteamID & 18446744069414584320UL) | ((ulong)(uint)other & (ulong)(-1));
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0000F21F File Offset: 0x0000D41F
		public void SetAccountInstance(uint other)
		{
			this.m_SteamID = (this.m_SteamID & 18442240478377148415UL) | (((ulong)other & 1048575UL) << 32);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0000F244 File Offset: 0x0000D444
		public void SetEAccountType(EAccountType other)
		{
			this.m_SteamID = (this.m_SteamID & 18379190079298994175UL) | (ulong)((ulong)((long)other & 15L) << 52);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0000F266 File Offset: 0x0000D466
		public void SetEUniverse(EUniverse other)
		{
			this.m_SteamID = (this.m_SteamID & 72057594037927935UL) | (ulong)((ulong)((long)other & 255L) << 56);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0000F28B File Offset: 0x0000D48B
		public AccountID_t GetAccountID()
		{
			return new AccountID_t((uint)(this.m_SteamID & (ulong)(-1)));
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0000F29C File Offset: 0x0000D49C
		public uint GetUnAccountInstance()
		{
			return (uint)((this.m_SteamID >> 32) & 1048575UL);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0000F2AF File Offset: 0x0000D4AF
		public EAccountType GetEAccountType()
		{
			return (EAccountType)((this.m_SteamID >> 52) & 15UL);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0000F2BF File Offset: 0x0000D4BF
		public EUniverse GetEUniverse()
		{
			return (EUniverse)((this.m_SteamID >> 56) & 255UL);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0000F2D4 File Offset: 0x0000D4D4
		public bool IsValid()
		{
			return this.GetEAccountType() > EAccountType.k_EAccountTypeInvalid && this.GetEAccountType() < EAccountType.k_EAccountTypeMax && this.GetEUniverse() > EUniverse.k_EUniverseInvalid && this.GetEUniverse() < EUniverse.k_EUniverseMax && (this.GetEAccountType() != EAccountType.k_EAccountTypeIndividual || (!(this.GetAccountID() == new AccountID_t(0U)) && this.GetUnAccountInstance() <= 1U)) && (this.GetEAccountType() != EAccountType.k_EAccountTypeClan || (!(this.GetAccountID() == new AccountID_t(0U)) && this.GetUnAccountInstance() == 0U)) && (this.GetEAccountType() != EAccountType.k_EAccountTypeGameServer || !(this.GetAccountID() == new AccountID_t(0U)));
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0000F376 File Offset: 0x0000D576
		public override string ToString()
		{
			return this.m_SteamID.ToString();
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0000F383 File Offset: 0x0000D583
		public override bool Equals(object other)
		{
			return other is CSteamID && this == (CSteamID)other;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0000F3A0 File Offset: 0x0000D5A0
		public override int GetHashCode()
		{
			return this.m_SteamID.GetHashCode();
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0000F3AD File Offset: 0x0000D5AD
		public static bool operator ==(CSteamID x, CSteamID y)
		{
			return x.m_SteamID == y.m_SteamID;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0000F3BD File Offset: 0x0000D5BD
		public static bool operator !=(CSteamID x, CSteamID y)
		{
			return !(x == y);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0000F3C9 File Offset: 0x0000D5C9
		public static explicit operator CSteamID(ulong value)
		{
			return new CSteamID(value);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0000F3D1 File Offset: 0x0000D5D1
		public static explicit operator ulong(CSteamID that)
		{
			return that.m_SteamID;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0000F3D9 File Offset: 0x0000D5D9
		public bool Equals(CSteamID other)
		{
			return this.m_SteamID == other.m_SteamID;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0000F3E9 File Offset: 0x0000D5E9
		public int CompareTo(CSteamID other)
		{
			return this.m_SteamID.CompareTo(other.m_SteamID);
		}

		// Token: 0x04000B6D RID: 2925
		public static readonly CSteamID Nil = default(CSteamID);

		// Token: 0x04000B6E RID: 2926
		public static readonly CSteamID OutofDateGS = new CSteamID(new AccountID_t(0U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000B6F RID: 2927
		public static readonly CSteamID LanModeGS = new CSteamID(new AccountID_t(0U), 0U, EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000B70 RID: 2928
		public static readonly CSteamID NotInitYetGS = new CSteamID(new AccountID_t(1U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000B71 RID: 2929
		public static readonly CSteamID NonSteamGS = new CSteamID(new AccountID_t(2U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);

		// Token: 0x04000B72 RID: 2930
		public ulong m_SteamID;
	}
}
