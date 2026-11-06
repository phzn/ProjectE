using System;
using System.Collections.Generic;
using Features.MainMenu;
using UnityEngine;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E27 RID: 3623
	public class PlayerPoiEmoteContext : MonoBehaviour
	{
		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x06006C17 RID: 27671 RVA: 0x002B426D File Offset: 0x002B246D
		// (set) Token: 0x06006C18 RID: 27672 RVA: 0x002B4275 File Offset: 0x002B2475
		public PoiEmoteProfileSO CurrentProfile { get; private set; }

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06006C19 RID: 27673 RVA: 0x002B4280 File Offset: 0x002B2480
		// (remove) Token: 0x06006C1A RID: 27674 RVA: 0x002B42B8 File Offset: 0x002B24B8
		public event Action<PoiEmoteProfileSO> OnZoneEntered;

		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06006C1B RID: 27675 RVA: 0x002B42F0 File Offset: 0x002B24F0
		// (remove) Token: 0x06006C1C RID: 27676 RVA: 0x002B4328 File Offset: 0x002B2528
		public event Action<PoiEmoteProfileSO> OnPoiVisited;

		// Token: 0x140000BF RID: 191
		// (add) Token: 0x06006C1D RID: 27677 RVA: 0x002B4360 File Offset: 0x002B2560
		// (remove) Token: 0x06006C1E RID: 27678 RVA: 0x002B4398 File Offset: 0x002B2598
		public event Action<PoiEmoteProfileSO, PoiEmoteProfileSO> OnProfileChanged;

		// Token: 0x06006C1F RID: 27679 RVA: 0x002B43D0 File Offset: 0x002B25D0
		public void EnterZone(PoiEmoteZone zone)
		{
			if (zone == null || zone.Profile == null)
			{
				return;
			}
			if (!this.activeZones.Add(zone))
			{
				return;
			}
			if (zone.AffectsEmoteContext)
			{
				this.RecomputeProfile();
			}
			if (zone.RegistersPoiVisit)
			{
				CollectionUnlockRegistry.RegisterPoiVisit(zone.CollectionProfile, zone.gameObject.scene);
				Action<PoiEmoteProfileSO> onPoiVisited = this.OnPoiVisited;
				if (onPoiVisited != null)
				{
					onPoiVisited(zone.CollectionProfile);
				}
				Action<PoiEmoteProfileSO> onZoneEntered = this.OnZoneEntered;
				if (onZoneEntered == null)
				{
					return;
				}
				onZoneEntered(zone.Profile);
			}
		}

		// Token: 0x06006C20 RID: 27680 RVA: 0x002B445D File Offset: 0x002B265D
		public void ExitZone(PoiEmoteZone zone)
		{
			if (zone == null)
			{
				return;
			}
			if (!this.activeZones.Remove(zone))
			{
				return;
			}
			if (zone.AffectsEmoteContext)
			{
				this.RecomputeProfile();
			}
		}

		// Token: 0x06006C21 RID: 27681 RVA: 0x002B4488 File Offset: 0x002B2688
		public bool IsInPoi(string poiId)
		{
			if (string.IsNullOrWhiteSpace(poiId))
			{
				return true;
			}
			foreach (PoiEmoteZone poiEmoteZone in this.activeZones)
			{
				if (!(poiEmoteZone == null) && !(poiEmoteZone.Profile == null) && poiEmoteZone.AffectsEmoteContext && string.Equals(poiEmoteZone.Profile.PoiId, poiId, StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006C22 RID: 27682 RVA: 0x002B4518 File Offset: 0x002B2718
		private void RecomputeProfile()
		{
			PoiEmoteProfileSO currentProfile = this.CurrentProfile;
			PoiEmoteProfileSO poiEmoteProfileSO = null;
			int num = int.MinValue;
			foreach (PoiEmoteZone poiEmoteZone in this.activeZones)
			{
				if (!(poiEmoteZone == null) && !(poiEmoteZone.Profile == null) && poiEmoteZone.AffectsEmoteContext)
				{
					int priority = poiEmoteZone.Profile.Priority;
					if (priority > num)
					{
						num = priority;
						poiEmoteProfileSO = poiEmoteZone.Profile;
					}
				}
			}
			this.CurrentProfile = poiEmoteProfileSO;
			Action<PoiEmoteProfileSO, PoiEmoteProfileSO> onProfileChanged = this.OnProfileChanged;
			if (onProfileChanged == null)
			{
				return;
			}
			onProfileChanged(currentProfile, this.CurrentProfile);
		}

		// Token: 0x06006C23 RID: 27683 RVA: 0x002B45D4 File Offset: 0x002B27D4
		private void OnDisable()
		{
			if (this.activeZones.Count == 0 && this.CurrentProfile == null)
			{
				return;
			}
			PoiEmoteProfileSO currentProfile = this.CurrentProfile;
			this.activeZones.Clear();
			this.CurrentProfile = null;
			if (currentProfile != null)
			{
				Action<PoiEmoteProfileSO, PoiEmoteProfileSO> onProfileChanged = this.OnProfileChanged;
				if (onProfileChanged == null)
				{
					return;
				}
				onProfileChanged(currentProfile, null);
			}
		}

		// Token: 0x04006E96 RID: 28310
		private readonly HashSet<PoiEmoteZone> activeZones = new HashSet<PoiEmoteZone>();
	}
}
