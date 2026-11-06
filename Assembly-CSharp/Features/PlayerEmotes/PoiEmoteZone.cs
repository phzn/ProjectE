using System;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E29 RID: 3625
	[RequireComponent(typeof(Collider))]
	public class PoiEmoteZone : MonoBehaviour
	{
		// Token: 0x06006C26 RID: 27686 RVA: 0x002B4662 File Offset: 0x002B2862
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.PoiEmoteZone_profile)
			{
				return this.profile;
			}
			throw new InvalidOperationException("Unsupported PoiEmoteZone POI binding: " + field.ToString());
		}

		// Token: 0x06006C27 RID: 27687 RVA: 0x002B468C File Offset: 0x002B288C
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.PoiEmoteZone_profile)
			{
				this.profile = (PoiEmoteProfileSO)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported PoiEmoteZone POI binding: " + field.ToString());
		}

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06006C28 RID: 27688 RVA: 0x002B46BC File Offset: 0x002B28BC
		public PoiEmoteProfileSO Profile
		{
			get
			{
				return this.profile;
			}
		}

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06006C29 RID: 27689 RVA: 0x002B46C4 File Offset: 0x002B28C4
		public bool RegistersPoiVisit
		{
			get
			{
				return !this.suppressVisitRegistration;
			}
		}

		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x06006C2A RID: 27690 RVA: 0x002B46CF File Offset: 0x002B28CF
		public bool AffectsEmoteContext
		{
			get
			{
				return !this.collectionOnly;
			}
		}

		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x06006C2B RID: 27691 RVA: 0x002B46DA File Offset: 0x002B28DA
		public PoiEmoteProfileSO CollectionProfile
		{
			get
			{
				if (!(this.collectionProfile != null))
				{
					return this.profile;
				}
				return this.collectionProfile;
			}
		}

		// Token: 0x06006C2C RID: 27692 RVA: 0x002B46F8 File Offset: 0x002B28F8
		private void Awake()
		{
			Collider component = base.GetComponent<Collider>();
			if (!component.isTrigger)
			{
				component.isTrigger = true;
			}
		}

		// Token: 0x06006C2D RID: 27693 RVA: 0x002B471C File Offset: 0x002B291C
		private void OnTriggerEnter(Collider other)
		{
			vThirdPersonController componentInParent = other.GetComponentInParent<vThirdPersonController>();
			if (componentInParent == null)
			{
				return;
			}
			PlayerPoiEmoteContext component = componentInParent.GetComponent<PlayerPoiEmoteContext>();
			if (component == null)
			{
				return;
			}
			PoiEmoteZone.PlayerOverlap playerOverlap;
			if (!this.playerOverlaps.TryGetValue(componentInParent, out playerOverlap))
			{
				playerOverlap = new PoiEmoteZone.PlayerOverlap(component);
				this.playerOverlaps.Add(componentInParent, playerOverlap);
			}
			if (!playerOverlap.Colliders.Add(other))
			{
				return;
			}
			if (playerOverlap.Colliders.Count > 1)
			{
				return;
			}
			playerOverlap.Context.EnterZone(this);
			if (this.debugLogs && this.profile != null)
			{
				Debug.Log("[PoiEmoteZone] Enter '" + this.profile.PoiId + "'", this);
			}
		}

		// Token: 0x06006C2E RID: 27694 RVA: 0x002B47D0 File Offset: 0x002B29D0
		private void OnTriggerExit(Collider other)
		{
			vThirdPersonController componentInParent = other.GetComponentInParent<vThirdPersonController>();
			PoiEmoteZone.PlayerOverlap playerOverlap;
			if ((componentInParent == null || !this.playerOverlaps.TryGetValue(componentInParent, out playerOverlap) || !playerOverlap.Colliders.Contains(other)) && !this.TryFindOverlap(other, out componentInParent, out playerOverlap))
			{
				return;
			}
			if (!playerOverlap.Colliders.Remove(other) || playerOverlap.Colliders.Count > 0)
			{
				return;
			}
			this.playerOverlaps.Remove(componentInParent);
			if (playerOverlap.Context != null)
			{
				playerOverlap.Context.ExitZone(this);
			}
			if (this.debugLogs && this.profile != null)
			{
				Debug.Log("[PoiEmoteZone] Exit '" + this.profile.PoiId + "'", this);
			}
		}

		// Token: 0x06006C2F RID: 27695 RVA: 0x002B4894 File Offset: 0x002B2A94
		private bool TryFindOverlap(Collider collider, out vThirdPersonController player, out PoiEmoteZone.PlayerOverlap overlap)
		{
			foreach (KeyValuePair<vThirdPersonController, PoiEmoteZone.PlayerOverlap> keyValuePair in this.playerOverlaps)
			{
				if (keyValuePair.Value.Colliders.Contains(collider))
				{
					player = keyValuePair.Key;
					overlap = keyValuePair.Value;
					return true;
				}
			}
			player = null;
			overlap = null;
			return false;
		}

		// Token: 0x06006C30 RID: 27696 RVA: 0x002B4914 File Offset: 0x002B2B14
		private void OnDisable()
		{
			foreach (KeyValuePair<vThirdPersonController, PoiEmoteZone.PlayerOverlap> keyValuePair in this.playerOverlaps)
			{
				if (keyValuePair.Value.Context != null)
				{
					keyValuePair.Value.Context.ExitZone(this);
				}
			}
			this.playerOverlaps.Clear();
		}

		// Token: 0x04006EA3 RID: 28323
		[SerializeField]
		private PoiEmoteProfileSO profile;

		// Token: 0x04006EA4 RID: 28324
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04006EA5 RID: 28325
		[Tooltip("Keeps the profile active for local emotes/theme, but does not unlock or count this collider as a POI visit.")]
		[SerializeField]
		private bool suppressVisitRegistration;

		// Token: 0x04006EA6 RID: 28326
		[Tooltip("Records this location without changing the emote wheel or enabling POI-restricted actions.")]
		[SerializeField]
		private bool collectionOnly;

		// Token: 0x04006EA7 RID: 28327
		[Tooltip("Optional collection identity for this zone. The original profile still controls local emotes and actions.")]
		[SerializeField]
		private PoiEmoteProfileSO collectionProfile;

		// Token: 0x04006EA8 RID: 28328
		private readonly Dictionary<vThirdPersonController, PoiEmoteZone.PlayerOverlap> playerOverlaps = new Dictionary<vThirdPersonController, PoiEmoteZone.PlayerOverlap>();

		// Token: 0x02000E2A RID: 3626
		private sealed class PlayerOverlap
		{
			// Token: 0x06006C32 RID: 27698 RVA: 0x002B49A7 File Offset: 0x002B2BA7
			public PlayerOverlap(PlayerPoiEmoteContext context)
			{
				this.Context = context;
			}

			// Token: 0x04006EA9 RID: 28329
			public readonly PlayerPoiEmoteContext Context;

			// Token: 0x04006EAA RID: 28330
			public readonly HashSet<Collider> Colliders = new HashSet<Collider>();
		}
	}
}
