using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B6B RID: 2923
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshRenderer))]
	public class WantedPoster : MonoBehaviour
	{
		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06005301 RID: 21249 RVA: 0x00203C58 File Offset: 0x00201E58
		public bool GateByWantedLevel
		{
			get
			{
				return this.gateByWantedLevel;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06005302 RID: 21250 RVA: 0x00203C60 File Offset: 0x00201E60
		public int MinimumWantedLevel
		{
			get
			{
				return this.minimumWantedLevel;
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06005303 RID: 21251 RVA: 0x00203C68 File Offset: 0x00201E68
		public bool IsReservedForNpc
		{
			get
			{
				return this._npcReservationOwner != null;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06005304 RID: 21252 RVA: 0x00203C78 File Offset: 0x00201E78
		public bool IsNpcInterestEligible
		{
			get
			{
				WantedPosterPhotoSystem instance = WantedPosterPhotoSystem.Instance;
				MeshRenderer renderer = this.Renderer;
				return !this._consumedByNpc && base.isActiveAndEnabled && base.gameObject.activeInHierarchy && renderer != null && renderer.enabled && renderer.gameObject.activeInHierarchy && instance != null && instance.HasPhoto && instance.CurrentRuntimeMaterial != null && renderer.sharedMaterial == instance.CurrentRuntimeMaterial;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06005305 RID: 21253 RVA: 0x00203D00 File Offset: 0x00201F00
		public Vector3 NpcInterestPoint
		{
			get
			{
				if (!(this.Renderer != null))
				{
					return base.transform.position;
				}
				return this.Renderer.bounds.center;
			}
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x00203D3A File Offset: 0x00201F3A
		public bool TryReserveForNpc(Object owner)
		{
			if (owner == null || !this.IsNpcInterestEligible)
			{
				return false;
			}
			if (this._npcReservationOwner != null && this._npcReservationOwner != owner)
			{
				return false;
			}
			this._npcReservationOwner = owner;
			return true;
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x00203D75 File Offset: 0x00201F75
		public void ReleaseNpcReservation(Object owner)
		{
			if (this._npcReservationOwner == owner)
			{
				this._npcReservationOwner = null;
			}
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x00203D8C File Offset: 0x00201F8C
		public bool ConsumeByNpc(Object owner)
		{
			if (this._consumedByNpc || owner == null || this._npcReservationOwner != owner)
			{
				return false;
			}
			this._consumedByNpc = true;
			this._npcReservationOwner = null;
			if (this.gateByWantedLevel)
			{
				PoiResidencyPlacement.WriteFlag(this, "poster-consumed", true);
				this.HideAuthoredInstanceUntilChunkReuse();
				return true;
			}
			GameObject gameObject = this.ResolveWorldRoot();
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			Collider[] componentsInChildren2 = gameObject.GetComponentsInChildren<Collider>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
			WantedPosterPickup[] componentsInChildren3 = gameObject.GetComponentsInChildren<WantedPosterPickup>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].enabled = false;
			}
			WantedPosterPhotoSystem.UnregisterPoster(this);
			Object.Destroy(gameObject);
			return true;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x00203E58 File Offset: 0x00202058
		public bool ConsumeAuthoredWorldInstance()
		{
			if (!this.gateByWantedLevel || this._consumedByNpc)
			{
				return false;
			}
			PoiResidencyPlacement.WriteFlag(this, "poster-consumed", true);
			this._consumedByNpc = true;
			this._npcReservationOwner = null;
			this.HideAuthoredInstanceUntilChunkReuse();
			return true;
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x00203E8D File Offset: 0x0020208D
		private void HideAuthoredInstanceUntilChunkReuse()
		{
			this._restoreOnChunkReuse = true;
			this.SetWantedVisibility(false);
			WantedPosterPhotoSystem.UnregisterPoster(this);
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x00203EA4 File Offset: 0x002020A4
		private GameObject ResolveWorldRoot()
		{
			WantedPosterPickup componentInParent = base.GetComponentInParent<WantedPosterPickup>();
			if (!(componentInParent != null))
			{
				return base.gameObject;
			}
			return componentInParent.gameObject;
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x00203ECE File Offset: 0x002020CE
		public void SetPlayerPlaced()
		{
			if (!this.gateByWantedLevel)
			{
				return;
			}
			if (this._gatedRenderers == null)
			{
				this.CacheGatedComponents();
			}
			this.RestoreGatedDefaults();
			this._restoreOnChunkReuse = false;
			this._consumedByNpc = false;
			this._npcReservationOwner = null;
			this.gateByWantedLevel = false;
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x0600530D RID: 21261 RVA: 0x00203F09 File Offset: 0x00202109
		public MeshRenderer Renderer
		{
			get
			{
				if (this._renderer == null)
				{
					this._renderer = base.GetComponent<MeshRenderer>();
				}
				return this._renderer;
			}
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x00203F2B File Offset: 0x0020212B
		private void Awake()
		{
			this.CaptureAuthoredMaterial();
			if (!this.gateByWantedLevel)
			{
				return;
			}
			this.CacheGatedComponents();
			this.SetWantedLevel(0);
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x00203F4C File Offset: 0x0020214C
		private void OnEnable()
		{
			this.CaptureAuthoredMaterial();
			if (this.gateByWantedLevel && PoiResidencyPlacement.ReadFlag(this, "poster-consumed"))
			{
				this._consumedByNpc = true;
				this._restoreOnChunkReuse = false;
				this._npcReservationOwner = null;
				this.SetWantedVisibility(false);
				WantedPosterPhotoSystem.UnregisterPoster(this);
				return;
			}
			WantedPosterPhotoSystem instance = WantedPosterPhotoSystem.Instance;
			if (instance == null || !instance.HasPhoto || instance.CurrentRuntimeMaterial == null)
			{
				this.RestoreAuthoredMaterial();
			}
			if (this._restoreOnChunkReuse)
			{
				this._restoreOnChunkReuse = false;
				this._consumedByNpc = false;
				this._npcReservationOwner = null;
				this.RestoreGatedDefaults();
			}
			if (this.gateByWantedLevel)
			{
				this.SetWantedVisibility(false);
			}
			WantedPosterPhotoSystem.RegisterPoster(this);
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x00203FFA File Offset: 0x002021FA
		private void OnDisable()
		{
			WantedPosterPhotoSystem.UnregisterPoster(this);
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x00204004 File Offset: 0x00202204
		internal void CaptureAuthoredMaterial()
		{
			MeshRenderer renderer = this.Renderer;
			if (this._authoredMaterial == null && renderer != null)
			{
				this._authoredMaterial = renderer.sharedMaterial;
			}
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x0020403B File Offset: 0x0020223B
		internal void RestoreAuthoredMaterial()
		{
			if (this.Renderer != null && this._authoredMaterial != null)
			{
				this.Renderer.sharedMaterial = this._authoredMaterial;
			}
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x0020406A File Offset: 0x0020226A
		internal void ApplyRuntimeMaterial(Material material)
		{
			if (material == null || this.Renderer == null)
			{
				return;
			}
			this.CaptureAuthoredMaterial();
			this.Renderer.sharedMaterial = material;
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x00204096 File Offset: 0x00202296
		public void SetWantedLevel(int level)
		{
			if (!this.gateByWantedLevel)
			{
				return;
			}
			this.SetWantedVisibility(level >= Mathf.Max(1, this.minimumWantedLevel));
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x002040BC File Offset: 0x002022BC
		public void SetWantedVisibility(bool visible)
		{
			if (!this.gateByWantedLevel)
			{
				return;
			}
			if (this._gatedRenderers == null)
			{
				this.CacheGatedComponents();
			}
			for (int i = 0; i < this._gatedRenderers.Length; i++)
			{
				if (this._gatedRenderers[i] != null)
				{
					this._gatedRenderers[i].enabled = visible && this._rendererDefaults[i];
				}
			}
			for (int j = 0; j < this._gatedColliders.Length; j++)
			{
				if (this._gatedColliders[j] != null)
				{
					this._gatedColliders[j].enabled = visible && this._colliderDefaults[j];
				}
			}
			for (int k = 0; k < this._gatedBehaviours.Length; k++)
			{
				if (this._gatedBehaviours[k] != null)
				{
					this._gatedBehaviours[k].enabled = visible && this._behaviourDefaults[k];
				}
			}
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x002041A8 File Offset: 0x002023A8
		private void CacheGatedComponents()
		{
			this._gatedRenderers = base.GetComponentsInChildren<Renderer>(true);
			this._gatedColliders = base.GetComponentsInChildren<Collider>(true);
			Behaviour[] componentsInChildren = base.GetComponentsInChildren<Behaviour>(true);
			List<Behaviour> list = new List<Behaviour>(componentsInChildren.Length);
			this._rendererDefaults.Clear();
			foreach (Renderer renderer in this._gatedRenderers)
			{
				this._rendererDefaults.Add(renderer != null && renderer.enabled);
			}
			this._colliderDefaults.Clear();
			foreach (Collider collider in this._gatedColliders)
			{
				this._colliderDefaults.Add(collider != null && collider.enabled);
			}
			this._behaviourDefaults.Clear();
			foreach (Behaviour behaviour in componentsInChildren)
			{
				if (!(behaviour == this))
				{
					list.Add(behaviour);
					this._behaviourDefaults.Add(behaviour != null && behaviour.enabled);
				}
			}
			this._gatedBehaviours = list.ToArray();
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x002042CC File Offset: 0x002024CC
		private void RestoreGatedDefaults()
		{
			if (this._gatedRenderers == null)
			{
				this.CacheGatedComponents();
			}
			for (int i = 0; i < this._gatedRenderers.Length; i++)
			{
				if (this._gatedRenderers[i] != null)
				{
					this._gatedRenderers[i].enabled = this._rendererDefaults[i];
				}
			}
			for (int j = 0; j < this._gatedColliders.Length; j++)
			{
				if (this._gatedColliders[j] != null)
				{
					this._gatedColliders[j].enabled = this._colliderDefaults[j];
				}
			}
			for (int k = 0; k < this._gatedBehaviours.Length; k++)
			{
				if (this._gatedBehaviours[k] != null)
				{
					this._gatedBehaviours[k].enabled = this._behaviourDefaults[k];
				}
			}
		}

		// Token: 0x0400539C RID: 21404
		[Header("Wanted Level Gate")]
		[Tooltip("Nasconde renderer e interazioni finche' il wanted level non raggiunge la soglia. Va abilitato sui poster da muro, non sulle copie in mano.")]
		[SerializeField]
		private bool gateByWantedLevel;

		// Token: 0x0400539D RID: 21405
		[Tooltip("Primo wanted/suspicion level al quale il poster diventa visibile e interagibile.")]
		[Min(1f)]
		[SerializeField]
		private int minimumWantedLevel = 1;

		// Token: 0x0400539E RID: 21406
		private MeshRenderer _renderer;

		// Token: 0x0400539F RID: 21407
		private Material _authoredMaterial;

		// Token: 0x040053A0 RID: 21408
		private Renderer[] _gatedRenderers;

		// Token: 0x040053A1 RID: 21409
		private Collider[] _gatedColliders;

		// Token: 0x040053A2 RID: 21410
		private Behaviour[] _gatedBehaviours;

		// Token: 0x040053A3 RID: 21411
		private readonly List<bool> _rendererDefaults = new List<bool>();

		// Token: 0x040053A4 RID: 21412
		private readonly List<bool> _colliderDefaults = new List<bool>();

		// Token: 0x040053A5 RID: 21413
		private readonly List<bool> _behaviourDefaults = new List<bool>();

		// Token: 0x040053A6 RID: 21414
		private Object _npcReservationOwner;

		// Token: 0x040053A7 RID: 21415
		private bool _consumedByNpc;

		// Token: 0x040053A8 RID: 21416
		private bool _restoreOnChunkReuse;
	}
}
