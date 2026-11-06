using System;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A60 RID: 2656
	public sealed class PlayerActivityLease : IDisposable
	{
		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06004B8B RID: 19339 RVA: 0x001D5ABC File Offset: 0x001D3CBC
		public string Reason { get; }

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06004B8C RID: 19340 RVA: 0x001D5AC4 File Offset: 0x001D3CC4
		public Object Owner { get; }

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06004B8D RID: 19341 RVA: 0x001D5ACC File Offset: 0x001D3CCC
		// (set) Token: 0x06004B8E RID: 19342 RVA: 0x001D5AD4 File Offset: 0x001D3CD4
		public PlayerActivity RequiredActivities { get; private set; }

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06004B8F RID: 19343 RVA: 0x001D5ADD File Offset: 0x001D3CDD
		// (set) Token: 0x06004B90 RID: 19344 RVA: 0x001D5AE5 File Offset: 0x001D3CE5
		public PlayerContextResource Resources { get; private set; }

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06004B91 RID: 19345 RVA: 0x001D5AEE File Offset: 0x001D3CEE
		// (set) Token: 0x06004B92 RID: 19346 RVA: 0x001D5AF6 File Offset: 0x001D3CF6
		public PlayerActivity BlockedActivities { get; private set; }

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06004B93 RID: 19347 RVA: 0x001D5AFF File Offset: 0x001D3CFF
		public bool AllowsUtilityModals { get; }

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06004B94 RID: 19348 RVA: 0x001D5B07 File Offset: 0x001D3D07
		public bool IsActive
		{
			get
			{
				return PlayerActivityGate.IsLeaseActive(this);
			}
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x001D5B0F File Offset: 0x001D3D0F
		internal PlayerActivityLease(string reason, Object owner, PlayerActivity requiredActivities, PlayerContextResource resources, PlayerActivity blockedActivities, bool allowsUtilityModals = false)
		{
			this.Reason = reason;
			this.Owner = owner;
			this.RequiredActivities = requiredActivities;
			this.Resources = resources;
			this.BlockedActivities = blockedActivities;
			this.AllowsUtilityModals = allowsUtilityModals;
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x001D5B44 File Offset: 0x001D3D44
		internal void ApplyProfile(PlayerActivity requiredActivities, PlayerContextResource resources, PlayerActivity blockedActivities)
		{
			this.RequiredActivities = requiredActivities;
			this.Resources = resources;
			this.BlockedActivities = blockedActivities;
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x001D5B5B File Offset: 0x001D3D5B
		public void Dispose()
		{
			PlayerActivityGate.Release(this);
		}
	}
}
