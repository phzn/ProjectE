using System;
using Features.DebugTools;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.PubicHairMask;
using Features.SpicyHatch;
using Invector.vCharacterController;
using MagicaCloth2;
using UnityEngine;

namespace Features.UniversityUniform
{
	// Token: 0x02000BC2 RID: 3010
	public class UniversityUniformWindController : MonoBehaviour
	{
		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x0600565C RID: 22108 RVA: 0x00218F24 File Offset: 0x00217124
		public bool IsWindActive
		{
			get
			{
				return this._windOn;
			}
		}

		// Token: 0x0600565D RID: 22109 RVA: 0x00218F2C File Offset: 0x0021712C
		private void Awake()
		{
			Animator componentInParent = base.GetComponentInParent<Animator>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			this._inventory = transform.GetComponentInChildren<PlayerInventoryManager>(true) ?? PlayerInventoryManager.Instance;
			this._wearController = transform.GetComponentInChildren<WearableUseController>(true) ?? Object.FindObjectOfType<WearableUseController>(true);
			GameObject gameObject = new GameObject("UniversityWindZone");
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.localPosition = new Vector3(0f, 0.1f, 0f);
			this._zone = gameObject.AddComponent<MagicaWindZone>();
			this._zone.mode = MagicaWindZone.Mode.SphereDirection;
			this._zone.radius = this.windRadius;
			this._zone.main = this.windMain;
			this._zone.turbulence = this.windTurbulence;
			this._zone.directionAngleX = -90f;
			this._zone.isAddition = true;
			gameObject.SetActive(false);
			if (this.windVfxPrefab != null)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.windVfxPrefab, base.transform);
				gameObject2.name = "UniversityWindVFX";
				gameObject2.transform.localPosition = new Vector3(0f, 0.05f, 0f);
				this._vfx = gameObject2.GetComponent<ParticleSystem>();
			}
			this._sfx = base.gameObject.AddComponent<AudioSource>();
			this._sfx.clip = this.windClip;
			this._sfx.loop = true;
			this._sfx.playOnAwake = false;
			this._sfx.spatialBlend = 1f;
			this._sfx.minDistance = 1.5f;
			this._sfx.maxDistance = 14f;
			this._sfx.volume = 0f;
		}

		// Token: 0x0600565E RID: 22110 RVA: 0x00219104 File Offset: 0x00217304
		private void OnDisable()
		{
			this.SetWind(false);
			if (this._sfx != null)
			{
				this._sfx.volume = 0f;
				this._sfx.Stop();
			}
		}

		// Token: 0x0600565F RID: 22111 RVA: 0x00219136 File Offset: 0x00217336
		private void OnDestroy()
		{
			if (this._zone != null)
			{
				Object.Destroy(this._zone.gameObject);
			}
			if (this._vfx != null)
			{
				Object.Destroy(this._vfx.gameObject);
			}
		}

		// Token: 0x06005660 RID: 22112 RVA: 0x00219174 File Offset: 0x00217374
		private bool IsUniformItem(WearableItemSO item)
		{
			return !(item == null) && ((this.fullSetItem != null && item == this.fullSetItem) || (this.skirtItem != null && item == this.skirtItem) || (!string.IsNullOrEmpty(this.fullSetItemId) && item.itemId == this.fullSetItemId) || (!string.IsNullOrEmpty(this.skirtItemId) && item.itemId == this.skirtItemId));
		}

		// Token: 0x06005661 RID: 22113 RVA: 0x0021920C File Offset: 0x0021740C
		private bool UniformSelectedAndWorn()
		{
			WearableItemSO wearableItemSO = ((this._inventory != null) ? (this._inventory.GetActiveCloth() as WearableItemSO) : null);
			return this.IsUniformItem(wearableItemSO) && this._wearController != null && this._wearController.IsWorn(wearableItemSO);
		}

		// Token: 0x06005662 RID: 22114 RVA: 0x00219260 File Offset: 0x00217460
		private void Update()
		{
			bool flag = !FlyCamera.IsActive && this.windInput != null && this.windInput.GetButton() && this.UniformSelectedAndWorn();
			if (flag != this._windOn)
			{
				this.SetWind(flag);
			}
			if (this._windOn)
			{
				if (this.embFaceDuration > 0f)
				{
					SpicyHatchEmbarrassment.NotifySkirtLifted(this.embFaceDuration);
				}
				PubicHairStencilMask.NotifyWindLift();
			}
			this.UpdateSfx();
		}

		// Token: 0x06005663 RID: 22115 RVA: 0x002192D0 File Offset: 0x002174D0
		private void SetWind(bool on)
		{
			this._windOn = on;
			if (this._zone != null && this._zone.gameObject.activeSelf != on)
			{
				this._zone.gameObject.SetActive(on);
			}
			if (this._vfx != null)
			{
				if (on)
				{
					this._vfx.Play();
					return;
				}
				this._vfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x00219340 File Offset: 0x00217540
		private void UpdateSfx()
		{
			if (this._sfx == null)
			{
				return;
			}
			float num = (this._windOn ? this.sfxVolume : 0f);
			if (this._windOn && !this._sfx.isPlaying && this._sfx.clip != null)
			{
				this._sfx.Play();
			}
			float num2 = (this._windOn ? this.sfxFadeIn : this.sfxFadeOut);
			float num3 = ((num2 > 0.01f) ? (this.sfxVolume / num2 * Time.deltaTime) : 1f);
			this._sfx.volume = Mathf.MoveTowards(this._sfx.volume, num, num3);
			if (!this._windOn && this._sfx.isPlaying && this._sfx.volume <= 0.001f)
			{
				this._sfx.Stop();
			}
		}

		// Token: 0x040056CF RID: 22223
		[Header("Cloth (attiva se selezionato in inventario E indossato — full set O gonna)")]
		[SerializeField]
		private WearableItemSO fullSetItem;

		// Token: 0x040056D0 RID: 22224
		[SerializeField]
		private string fullSetItemId = "cloth_university_uniform_full";

		// Token: 0x040056D1 RID: 22225
		[SerializeField]
		private WearableItemSO skirtItem;

		// Token: 0x040056D2 RID: 22226
		[SerializeField]
		private string skirtItemId = "cloth_university_uniform_skirt";

		// Token: 0x040056D3 RID: 22227
		[Header("Input (tieni premuto)")]
		[SerializeField]
		private GenericInput windInput = new GenericInput("N", "X", "X");

		// Token: 0x040056D4 RID: 22228
		[Header("Wind (stessi valori del tombino)")]
		[SerializeField]
		private float windMain = 20f;

		// Token: 0x040056D5 RID: 22229
		[SerializeField]
		private float windRadius = 1.5f;

		// Token: 0x040056D6 RID: 22230
		[Range(0f, 1f)]
		[SerializeField]
		private float windTurbulence = 1f;

		// Token: 0x040056D7 RID: 22231
		[Header("Reaction")]
		[Tooltip("Secondi di Embarrassed Supreme dopo il vento (timer esteso finché tieni premuto). 0 = niente reazione.")]
		[SerializeField]
		private float embFaceDuration = 25f;

		// Token: 0x040056D8 RID: 22232
		[Header("VFX")]
		[Tooltip("Prefab WindPuffVFX (particelle vento condivise col tombino): istanziato in Awake sotto il player, Play/Stop col vento.")]
		[SerializeField]
		private GameObject windVfxPrefab;

		// Token: 0x040056D9 RID: 22233
		[Header("Audio")]
		[Tooltip("Clip del vento (stessa del tombino: HeavyWind di Azure[Sky]).")]
		[SerializeField]
		private AudioClip windClip;

		// Token: 0x040056DA RID: 22234
		[Range(0f, 1f)]
		[SerializeField]
		private float sfxVolume = 0.2f;

		// Token: 0x040056DB RID: 22235
		[SerializeField]
		private float sfxFadeIn = 1.2f;

		// Token: 0x040056DC RID: 22236
		[SerializeField]
		private float sfxFadeOut = 1.8f;

		// Token: 0x040056DD RID: 22237
		private PlayerInventoryManager _inventory;

		// Token: 0x040056DE RID: 22238
		private WearableUseController _wearController;

		// Token: 0x040056DF RID: 22239
		private MagicaWindZone _zone;

		// Token: 0x040056E0 RID: 22240
		private ParticleSystem _vfx;

		// Token: 0x040056E1 RID: 22241
		private AudioSource _sfx;

		// Token: 0x040056E2 RID: 22242
		private bool _windOn;
	}
}
