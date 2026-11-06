using System;
using Features.Achievements;
using Features.Inventory;
using Features.Suspicion;
using Features.WardrobeFeedback;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B6C RID: 2924
	[DisallowMultipleComponent]
	public class WantedPosterBreakController : MonoBehaviour
	{
		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06005319 RID: 21273 RVA: 0x002043CB File Offset: 0x002025CB
		// (set) Token: 0x0600531A RID: 21274 RVA: 0x002043D2 File Offset: 0x002025D2
		public static WantedPosterBreakController Instance { get; private set; }

		// Token: 0x0600531B RID: 21275 RVA: 0x002043DA File Offset: 0x002025DA
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			WantedPosterBreakController.Instance = null;
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x002043E4 File Offset: 0x002025E4
		private void Awake()
		{
			if (!(WantedPosterBreakController.Instance != null) || !(WantedPosterBreakController.Instance != this))
			{
				WantedPosterBreakController.Instance = this;
				return;
			}
			if (this.IsCoreOwned && !WantedPosterBreakController.Instance.IsCoreOwned)
			{
				WantedPosterBreakController instance = WantedPosterBreakController.Instance;
				WantedPosterBreakController.Instance = this;
				instance.enabled = false;
				Object.Destroy(instance);
				return;
			}
			base.enabled = false;
			Object.Destroy(this);
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x0020444B File Offset: 0x0020264B
		private void OnDestroy()
		{
			if (WantedPosterBreakController.Instance == this)
			{
				WantedPosterBreakController.Instance = null;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x0600531E RID: 21278 RVA: 0x00204460 File Offset: 0x00202660
		private bool IsCoreOwned
		{
			get
			{
				return base.gameObject.scene.IsValid() && base.gameObject.scene.name == "_Core";
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x0600531F RID: 21279 RVA: 0x002044A1 File Offset: 0x002026A1
		internal bool IsActiveInstance
		{
			get
			{
				return WantedPosterBreakController.Instance == this && base.isActiveAndEnabled;
			}
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x002044B8 File Offset: 0x002026B8
		private bool CanDiscard()
		{
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (instance == null || instance.GetActiveItem() == null)
			{
				return false;
			}
			if (this.posterItem != null)
			{
				return instance.GetActiveItem() == this.posterItem;
			}
			GameObject spawnedHandObject = instance.GetSpawnedHandObject();
			return spawnedHandObject != null && (spawnedHandObject.GetComponentInChildren<WantedPoster>(true) != null || spawnedHandObject.GetComponentInChildren<WantedPosterBreakObject>(true) != null);
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x00204534 File Offset: 0x00202734
		public bool TriggerDiscard()
		{
			if (!this.CanDiscard())
			{
				return false;
			}
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			WardrobeFeedback.PlayItemDiscardAt(WantedPosterBreakController.ResolveVisualCenter(instance.GetSpawnedHandObject(), instance.transform.position + Vector3.up * 1.2f), this.discardColor);
			if (!instance.ConsumeActiveItem())
			{
				return false;
			}
			AchievementTrackingService.RecordPosterDestroyed();
			SuspicionManager instance2 = SuspicionManager.Instance;
			if (instance2 != null && instance2.CurrentLevel > 0 && Random.value < this.wantedReductionChance)
			{
				instance2.TryDemoteOneLevel();
			}
			return true;
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x002045C3 File Offset: 0x002027C3
		public bool TriggerBreak()
		{
			return this.TriggerDiscard();
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x002045CC File Offset: 0x002027CC
		private static Vector3 ResolveVisualCenter(GameObject target, Vector3 fallback)
		{
			if (target == null)
			{
				return fallback;
			}
			Renderer[] componentsInChildren = target.GetComponentsInChildren<Renderer>(true);
			bool flag = false;
			Bounds bounds = default(Bounds);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (!(renderer == null))
				{
					if (!flag)
					{
						bounds = renderer.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			if (!flag)
			{
				return target.transform.position;
			}
			return bounds.center;
		}

		// Token: 0x040053AA RID: 21418
		[Header("Item")]
		[Tooltip("The Wanted Poster item. Discard is accepted only while this item is active.")]
		[SerializeField]
		private InventoryItemSO posterItem;

		// Token: 0x040053AB RID: 21419
		[Header("Discard Feedback")]
		[SerializeField]
		private Color discardColor = new Color(1f, 0.16f, 0.42f, 1f);

		// Token: 0x040053AC RID: 21420
		[Header("Wanted Reduction")]
		[Tooltip("Probabilita' che distruggere un Wanted Poster riduca il wanted level di uno.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float wantedReductionChance = 0.5f;
	}
}
