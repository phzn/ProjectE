using System;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Detection;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.Missions;
using Features.PlayerEmotes;
using Features.PubicHairMask;
using Features.SpicyHatch;
using Invector.vCharacterController;
using MagicaCloth2;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.CheerleaderWind
{
	// Token: 0x020014B6 RID: 5302
	public class CheerleaderWindController : MonoBehaviour
	{
		// Token: 0x17001917 RID: 6423
		// (get) Token: 0x0600A320 RID: 41760 RVA: 0x0043C029 File Offset: 0x0043A229
		public bool IsWindActive
		{
			get
			{
				return this._windOn;
			}
		}

		// Token: 0x0600A321 RID: 41761 RVA: 0x0043C034 File Offset: 0x0043A234
		private void Awake()
		{
			Animator componentInParent = base.GetComponentInParent<Animator>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			this._inventory = transform.GetComponentInChildren<PlayerInventoryManager>(true) ?? PlayerInventoryManager.Instance;
			this._wearController = transform.GetComponentInChildren<WearableUseController>(true) ?? Object.FindObjectOfType<WearableUseController>(true);
			this._emoteController = transform.GetComponentInChildren<GameplayEmoteController>(true) ?? Object.FindObjectOfType<GameplayEmoteController>(true);
			this._runtimeConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			this._runtimeConfig.objectName = this.objectName;
			this._runtimeConfig.hideFlags = HideFlags.HideAndDontSave;
			this._runtimeConfig.usableActions = this._actions.Actions;
			GameObject gameObject = new GameObject("CheerleaderWindZone");
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
				gameObject2.name = "CheerleaderWindVFX";
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

		// Token: 0x0600A322 RID: 41762 RVA: 0x0043C264 File Offset: 0x0043A464
		private void OnDisable()
		{
			this.SetWind(false);
			if (this._sfx != null)
			{
				this._sfx.volume = 0f;
				this._sfx.Stop();
			}
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._runtimeConfig);
			}
		}

		// Token: 0x0600A323 RID: 41763 RVA: 0x0043C2C0 File Offset: 0x0043A4C0
		private void OnDestroy()
		{
			if (this._runtimeConfig != null)
			{
				Object.Destroy(this._runtimeConfig);
			}
			if (this._zone != null)
			{
				Object.Destroy(this._zone.gameObject);
			}
			if (this._vfx != null)
			{
				Object.Destroy(this._vfx.gameObject);
			}
		}

		// Token: 0x0600A324 RID: 41764 RVA: 0x0043C324 File Offset: 0x0043A524
		private bool SuitSelectedAndWorn(out WearableItemSO selected)
		{
			selected = ((this._inventory != null) ? (this._inventory.GetActiveCloth() as WearableItemSO) : null);
			return !(selected == null) && (((this.suitClothItem != null) ? (selected == this.suitClothItem) : (!string.IsNullOrEmpty(this.suitItemId) && selected.itemId == this.suitItemId)) && this._wearController != null) && this._wearController.IsWorn(selected);
		}

		// Token: 0x17001918 RID: 6424
		// (get) Token: 0x0600A325 RID: 41765 RVA: 0x0043C3BE File Offset: 0x0043A5BE
		private bool DancePlaying
		{
			get
			{
				return this._emoteController != null && this._emoteController.IsPlaying;
			}
		}

		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x0600A326 RID: 41766 RVA: 0x0043C3DB File Offset: 0x0043A5DB
		private bool CheerleaderDancePlaying
		{
			get
			{
				return this.DancePlaying && this.danceEmote != null && this._emoteController.PlayingEmote == this.danceEmote;
			}
		}

		// Token: 0x0600A327 RID: 41767 RVA: 0x0043C40C File Offset: 0x0043A60C
		private void Update()
		{
			WearableItemSO wearableItemSO;
			bool flag = !FlyCamera.IsActive && WallCoverActivityLock.ActiveController == null && this.windInput != null && this.windInput.GetButton() && (!this.DancePlaying || this.CheerleaderDancePlaying) && this.SuitSelectedAndWorn(out wearableItemSO);
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
			WearableItemSO wearableItemSO2;
			if (!FlyCamera.IsActive && this.danceEmote != null && this._emoteController != null && !this._emoteController.IsPlaying && this.danceInput != null && this.danceInput.GetButtonDown() && this.SuitSelectedAndWorn(out wearableItemSO) && this._emoteController.TryPlayContextualEmote(this.danceEmote, false) && this.SuitSelectedAndWorn(out wearableItemSO2))
			{
				MissionGameplaySignals.NotifyWearableAction(wearableItemSO2, "cheerleader_dance", this.danceActionLabel);
			}
		}

		// Token: 0x0600A328 RID: 41768 RVA: 0x0043C518 File Offset: 0x0043A718
		private void SetWind(bool on)
		{
			this._windOn = on;
			WearableItemSO wearableItemSO;
			if (on && this.SuitSelectedAndWorn(out wearableItemSO))
			{
				MissionGameplaySignals.NotifyWearableAction(wearableItemSO, "cheerleader_skirt_breeze", this.actionLabel);
			}
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

		// Token: 0x0600A329 RID: 41769 RVA: 0x0043C5A8 File Offset: 0x0043A7A8
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

		// Token: 0x0600A32A RID: 41770 RVA: 0x0043C694 File Offset: 0x0043A894
		private void LateUpdate()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			this._actions.Begin();
			this.BuildActions();
			bool flag = this._actions.End();
			if (this._actions.Actions.Count == 0)
			{
				instance.HideIfCurrent(this._runtimeConfig);
				return;
			}
			if (flag || instance.CurrentConfig == null || (this.CheerleaderDancePlaying && instance.CurrentConfig == this._emoteController.EndEmoteHUDConfig))
			{
				this._runtimeConfig.objectName = this.objectName;
				instance.Show(this._runtimeConfig);
			}
		}

		// Token: 0x0600A32B RID: 41771 RVA: 0x0043C73C File Offset: 0x0043A93C
		private void BuildActions()
		{
			if (WallCoverActivityLock.ActiveController != null)
			{
				return;
			}
			WearableItemSO wearableItemSO = ((this._inventory != null) ? (this._inventory.GetActiveCloth() as WearableItemSO) : null);
			if (!(wearableItemSO != null) || !((this.suitClothItem != null) ? (wearableItemSO == this.suitClothItem) : (!string.IsNullOrEmpty(this.suitItemId) && wearableItemSO.itemId == this.suitItemId)))
			{
				return;
			}
			if (!this.DancePlaying)
			{
				if (this._wearController != null)
				{
					this._wearController.AppendVisibleWearActions(wearableItemSO, this._actions);
				}
				if (this._wearController != null && this._wearController.IsWorn(wearableItemSO) && !this.DancePlaying)
				{
					this._actions.Add(this.windKeyLabel, this.actionLabel, "");
					if (this.danceEmote != null)
					{
						this._actions.Add(this.danceKeyLabel, this.danceActionLabel, "");
					}
				}
				return;
			}
			if (!this.CheerleaderDancePlaying || this._wearController == null || !this._wearController.IsWorn(wearableItemSO))
			{
				return;
			}
			this._actions.Add(this.windKeyLabel, this.actionLabel, "");
			InteractionConfigSO endEmoteHUDConfig = this._emoteController.EndEmoteHUDConfig;
			if (endEmoteHUDConfig != null && endEmoteHUDConfig.usableActions != null && endEmoteHUDConfig.usableActions.Count > 0)
			{
				using (List<UsableAction>.Enumerator enumerator = endEmoteHUDConfig.usableActions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UsableAction usableAction = enumerator.Current;
						if (usableAction != null)
						{
							this._actions.Add(usableAction.keyDisplayLabel, usableAction.actionName, usableAction.description);
						}
					}
					return;
				}
			}
			this._actions.Add((this._emoteController.EndEmoteInput != null) ? this._emoteController.EndEmoteInput.keyboard : "Space", "End emote", "");
		}

		// Token: 0x0400A6DB RID: 42715
		[Header("Cloth (attiva se selezionata in inventario E indossata)")]
		[SerializeField]
		private WearableItemSO suitClothItem;

		// Token: 0x0400A6DC RID: 42716
		[SerializeField]
		private string suitItemId = "cloth_cheerleader_suite";

		// Token: 0x0400A6DD RID: 42717
		[Header("Input (tieni premuto)")]
		[SerializeField]
		private GenericInput windInput = new GenericInput("B", "Y", "B");

		// Token: 0x0400A6DE RID: 42718
		[Header("Dance (emote one-press)")]
		[Tooltip("EmoteDefinitionSO della Cheerleader Dance (loop, manual end) suonata via GameplayEmoteController.")]
		[SerializeField]
		private EmoteDefinitionSO danceEmote;

		// Token: 0x0400A6DF RID: 42719
		[SerializeField]
		private GenericInput danceInput = new GenericInput("N", "X", "X");

		// Token: 0x0400A6E0 RID: 42720
		[SerializeField]
		private string danceKeyLabel = "N";

		// Token: 0x0400A6E1 RID: 42721
		[SerializeField]
		private string danceActionLabel = "Dance";

		// Token: 0x0400A6E2 RID: 42722
		[Header("Wind (stessi valori del tombino)")]
		[SerializeField]
		private float windMain = 20f;

		// Token: 0x0400A6E3 RID: 42723
		[SerializeField]
		private float windRadius = 1.5f;

		// Token: 0x0400A6E4 RID: 42724
		[Range(0f, 1f)]
		[SerializeField]
		private float windTurbulence = 1f;

		// Token: 0x0400A6E5 RID: 42725
		[Header("Reaction")]
		[Tooltip("Secondi di Embarrassed Supreme dopo il vento (timer esteso finché tieni premuto). 0 = niente reazione.")]
		[SerializeField]
		private float embFaceDuration = 25f;

		// Token: 0x0400A6E6 RID: 42726
		[Header("VFX")]
		[Tooltip("Prefab WindPuffVFX (particelle vento condivise col tombino): istanziato in Awake sotto il player, Play/Stop col vento.")]
		[SerializeField]
		private GameObject windVfxPrefab;

		// Token: 0x0400A6E7 RID: 42727
		[Header("Audio")]
		[Tooltip("Clip del vento (stessa del tombino: HeavyWind di Azure[Sky]).")]
		[SerializeField]
		private AudioClip windClip;

		// Token: 0x0400A6E8 RID: 42728
		[Range(0f, 1f)]
		[SerializeField]
		private float sfxVolume = 0.2f;

		// Token: 0x0400A6E9 RID: 42729
		[SerializeField]
		private float sfxFadeIn = 1.2f;

		// Token: 0x0400A6EA RID: 42730
		[SerializeField]
		private float sfxFadeOut = 1.8f;

		// Token: 0x0400A6EB RID: 42731
		[Header("Display")]
		[SerializeField]
		private string objectName = "Cheerleader Suit";

		// Token: 0x0400A6EC RID: 42732
		[SerializeField]
		private string windKeyLabel = "B";

		// Token: 0x0400A6ED RID: 42733
		[SerializeField]
		private string actionLabel = "Skirt Breeze (hold)";

		// Token: 0x0400A6EE RID: 42734
		private PlayerInventoryManager _inventory;

		// Token: 0x0400A6EF RID: 42735
		private WearableUseController _wearController;

		// Token: 0x0400A6F0 RID: 42736
		private GameplayEmoteController _emoteController;

		// Token: 0x0400A6F1 RID: 42737
		private MagicaWindZone _zone;

		// Token: 0x0400A6F2 RID: 42738
		private ParticleSystem _vfx;

		// Token: 0x0400A6F3 RID: 42739
		private AudioSource _sfx;

		// Token: 0x0400A6F4 RID: 42740
		private InteractionConfigSO _runtimeConfig;

		// Token: 0x0400A6F5 RID: 42741
		private readonly InteractionActionBuffer _actions = new InteractionActionBuffer();

		// Token: 0x0400A6F6 RID: 42742
		private bool _windOn;
	}
}
