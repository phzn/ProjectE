using System;
using Features.MainMenu;
using Features.Police;
using Features.Suspicion;
using UnityEngine;

namespace Features.ClothingLoot
{
	// Token: 0x02001350 RID: 4944
	[DisallowMultipleComponent]
	public sealed class ClothingStoreAlarmController : MonoBehaviour
	{
		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x060098E0 RID: 39136 RVA: 0x003EAD11 File Offset: 0x003E8F11
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x060098E1 RID: 39137 RVA: 0x003EAD19 File Offset: 0x003E8F19
		public Transform RuntimeRig
		{
			get
			{
				return this._runtimeRig;
			}
		}

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x060098E2 RID: 39138 RVA: 0x003EAD21 File Offset: 0x003E8F21
		public AudioSource SirenSource
		{
			get
			{
				return this._sirenSource;
			}
		}

		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x060098E3 RID: 39139 RVA: 0x003EAD29 File Offset: 0x003E8F29
		public Light BeaconLight
		{
			get
			{
				return this._beaconLight;
			}
		}

		// Token: 0x060098E4 RID: 39140 RVA: 0x003EAD31 File Offset: 0x003E8F31
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			ClothingStoreAlarmController._synthesizedSirenClip = null;
		}

		// Token: 0x060098E5 RID: 39141 RVA: 0x003EAD3C File Offset: 0x003E8F3C
		public static bool ShouldTrigger(float chance, float roll)
		{
			if (float.IsNaN(chance) || float.IsNaN(roll))
			{
				return false;
			}
			float num = Mathf.Clamp01(chance);
			return num > 0f && (num >= 1f || Mathf.Clamp01(roll) < num);
		}

		// Token: 0x060098E6 RID: 39142 RVA: 0x003EAD80 File Offset: 0x003E8F80
		public static bool TryTrigger(ClothingLootTableInteractable source, float durationSeconds)
		{
			if (source == null)
			{
				return false;
			}
			Transform storeRoot = source.StoreRoot;
			if (storeRoot == null)
			{
				return false;
			}
			ClothingStoreAlarmController clothingStoreAlarmController = storeRoot.GetComponent<ClothingStoreAlarmController>();
			if (clothingStoreAlarmController == null)
			{
				clothingStoreAlarmController = storeRoot.gameObject.AddComponent<ClothingStoreAlarmController>();
			}
			clothingStoreAlarmController.PlaceRigAtCeiling(source);
			clothingStoreAlarmController.TriggerAlarm(durationSeconds);
			return true;
		}

		// Token: 0x060098E7 RID: 39143 RVA: 0x003EADD5 File Offset: 0x003E8FD5
		private void Awake()
		{
			this.EnsureRuntimeRig();
		}

		// Token: 0x060098E8 RID: 39144 RVA: 0x003EADE0 File Offset: 0x003E8FE0
		private void Update()
		{
			if (!this._isActive)
			{
				return;
			}
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime >= this._endsAtUnscaledTime)
			{
				this.StopAlarm();
				return;
			}
			this.UpdateBeacon(unscaledTime - this._startedAtUnscaledTime);
		}

		// Token: 0x060098E9 RID: 39145 RVA: 0x003EAE1A File Offset: 0x003E901A
		private void OnDisable()
		{
			this.StopAlarm();
		}

		// Token: 0x060098EA RID: 39146 RVA: 0x003EAE24 File Offset: 0x003E9024
		public void TriggerAlarm(float durationSeconds = -1f)
		{
			this.EnsureRuntimeRig();
			float num = ((durationSeconds > 0f) ? durationSeconds : this.defaultDurationSeconds);
			float unscaledTime = Time.unscaledTime;
			this._startedAtUnscaledTime = unscaledTime;
			this._endsAtUnscaledTime = unscaledTime + Mathf.Max(0.1f, num);
			this._isActive = true;
			if (this._beaconLight != null)
			{
				this._beaconLight.enabled = true;
			}
			this.UpdateBeacon(0f);
			if (Application.isPlaying && this._sirenSource != null && !this._sirenSource.isPlaying)
			{
				this._sirenSource.Play();
			}
			ClothingStoreAlarmController.PromoteSuspicionToLevel(3);
			PoliceManager instance = PoliceManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.TryRequestPatrolResponse(2, 3);
		}

		// Token: 0x060098EB RID: 39147 RVA: 0x003EAEDC File Offset: 0x003E90DC
		public void StopAlarm()
		{
			this._isActive = false;
			if (this._sirenSource != null && this._sirenSource.isPlaying)
			{
				this._sirenSource.Stop();
			}
			if (this._beaconLight != null)
			{
				this._beaconLight.intensity = 0f;
				this._beaconLight.enabled = false;
			}
		}

		// Token: 0x060098EC RID: 39148 RVA: 0x003EAF40 File Offset: 0x003E9140
		private void PlaceRigAtCeiling(ClothingLootTableInteractable source)
		{
			this.EnsureRuntimeRig();
			if (this._runtimeRig == null || source == null)
			{
				return;
			}
			Bounds bounds;
			Vector3 vector;
			Bounds bounds2;
			if (this.TryGetCeilingBounds(out bounds))
			{
				vector = new Vector3(bounds.center.x, bounds.min.y - Mathf.Max(0f, this.ceilingInset), bounds.center.z);
			}
			else if (source.TryGetVisualBounds(out bounds2))
			{
				vector = new Vector3(bounds2.center.x, bounds2.max.y + Mathf.Max(0.1f, this.fallbackHeightAboveTable), bounds2.center.z);
			}
			else
			{
				vector = source.transform.position + Vector3.up * Mathf.Max(0.1f, this.fallbackHeightAboveTable);
			}
			this._runtimeRig.SetPositionAndRotation(vector, Quaternion.identity);
		}

		// Token: 0x060098ED RID: 39149 RVA: 0x003EB038 File Offset: 0x003E9238
		private bool TryGetCeilingBounds(out Bounds ceilingBounds)
		{
			Transform transform = base.transform.Find("Ceiling");
			if (transform == null)
			{
				foreach (Transform transform2 in base.GetComponentsInChildren<Transform>(true))
				{
					if (transform2 != null && string.Equals(transform2.name, "Ceiling", StringComparison.OrdinalIgnoreCase))
					{
						transform = transform2;
						break;
					}
				}
			}
			if (transform != null)
			{
				Collider component = transform.GetComponent<Collider>();
				if (component != null)
				{
					ceilingBounds = component.bounds;
					return true;
				}
				Renderer component2 = transform.GetComponent<Renderer>();
				if (component2 != null)
				{
					ceilingBounds = component2.bounds;
					return true;
				}
			}
			ceilingBounds = default(Bounds);
			return false;
		}

		// Token: 0x060098EE RID: 39150 RVA: 0x003EB0EC File Offset: 0x003E92EC
		private void EnsureRuntimeRig()
		{
			if (this._runtimeRig == null)
			{
				Transform transform = base.transform.Find("Clothing Store Alarm (Runtime)");
				if (transform != null)
				{
					this._runtimeRig = transform;
				}
				else
				{
					GameObject gameObject = new GameObject("Clothing Store Alarm (Runtime)")
					{
						hideFlags = HideFlags.DontSave
					};
					this._runtimeRig = gameObject.transform;
					this._runtimeRig.SetParent(base.transform, false);
				}
			}
			this.RemoveLegacyBeaconVisual();
			if (this._sirenSource == null)
			{
				this._sirenSource = this._runtimeRig.GetComponent<AudioSource>();
				if (this._sirenSource == null)
				{
					this._sirenSource = this._runtimeRig.gameObject.AddComponent<AudioSource>();
				}
			}
			this.ConfigureSirenSource();
			if (this._beaconLight == null)
			{
				this._beaconLight = this._runtimeRig.GetComponent<Light>();
				if (this._beaconLight == null)
				{
					this._beaconLight = this._runtimeRig.gameObject.AddComponent<Light>();
				}
			}
			this.ConfigureBeaconLight();
		}

		// Token: 0x060098EF RID: 39151 RVA: 0x003EB1F4 File Offset: 0x003E93F4
		private void RemoveLegacyBeaconVisual()
		{
			if (this._runtimeRig == null)
			{
				return;
			}
			Transform transform = this._runtimeRig.Find("Police Beacon Visual");
			if (transform == null)
			{
				return;
			}
			transform.gameObject.SetActive(false);
			if (Application.isPlaying)
			{
				Object.Destroy(transform.gameObject);
				return;
			}
			Object.DestroyImmediate(transform.gameObject);
		}

		// Token: 0x060098F0 RID: 39152 RVA: 0x003EB258 File Offset: 0x003E9458
		private void ConfigureSirenSource()
		{
			if (this._sirenSource == null)
			{
				return;
			}
			this._sirenSource.playOnAwake = false;
			this._sirenSource.loop = true;
			this._sirenSource.spatialBlend = 1f;
			this._sirenSource.rolloffMode = AudioRolloffMode.Logarithmic;
			this._sirenSource.minDistance = Mathf.Max(0.1f, this.sirenMinDistance);
			this._sirenSource.maxDistance = Mathf.Max(this._sirenSource.minDistance, this.sirenMaxDistance);
			this._sirenSource.dopplerLevel = 0f;
			this._sirenSource.spread = 120f;
			this._sirenSource.volume = Mathf.Clamp01(this.sirenVolume);
			this._sirenSource.clip = ClothingStoreAlarmController.GetOrCreateSirenClip();
			GameAudioRouter.Route(this._sirenSource, GameAudioRouter.Channel.Sfx);
		}

		// Token: 0x060098F1 RID: 39153 RVA: 0x003EB338 File Offset: 0x003E9538
		private void ConfigureBeaconLight()
		{
			if (this._beaconLight == null)
			{
				return;
			}
			this._beaconLight.type = LightType.Point;
			this._beaconLight.shadows = LightShadows.None;
			this._beaconLight.renderMode = LightRenderMode.ForcePixel;
			this._beaconLight.range = Mathf.Max(0.1f, this.beaconRange);
			this._beaconLight.intensity = 0f;
			this._beaconLight.color = this.redColor;
			this._beaconLight.enabled = false;
		}

		// Token: 0x060098F2 RID: 39154 RVA: 0x003EB3C0 File Offset: 0x003E95C0
		private void UpdateBeacon(float elapsedSeconds)
		{
			if (this._beaconLight == null)
			{
				return;
			}
			float num = elapsedSeconds * Mathf.Max(0.1f, this.colorChangesPerSecond);
			int num2 = Mathf.FloorToInt(num);
			float num3 = Mathf.Sin((num - (float)num2) * 3.1415927f);
			this._beaconLight.color = (((num2 & 1) == 0) ? this.redColor : this.blueColor);
			this._beaconLight.intensity = Mathf.Max(0f, this.beaconIntensity) * Mathf.Lerp(0.18f, 1f, num3);
		}

		// Token: 0x060098F3 RID: 39155 RVA: 0x003EB450 File Offset: 0x003E9650
		private static AudioClip GetOrCreateSirenClip()
		{
			if (ClothingStoreAlarmController._synthesizedSirenClip != null)
			{
				return ClothingStoreAlarmController._synthesizedSirenClip;
			}
			int num = Mathf.RoundToInt(88200f);
			float[] array = new float[num];
			double num2 = 0.0;
			for (int i = 0; i < num; i++)
			{
				double num3 = (double)i / 44100.0;
				double num4 = Math.Sin(3.141592653589793 * num3);
				double num5 = 720.0 + 190.0 * num4;
				double num6 = 0.84 + 0.16 * Math.Sin(25.132741228718345 * num3);
				double num7 = Math.Sin(num2);
				double num8 = Math.Sin(num2 * 2.0);
				array[i] = (float)((num7 * 0.48 + num8 * 0.09) * num6);
				num2 += 6.283185307179586 * num5 / 44100.0;
			}
			ClothingStoreAlarmController._synthesizedSirenClip = AudioClip.Create("ClothingStoreAlarm_SynthSiren", num, 1, 44100, false);
			ClothingStoreAlarmController._synthesizedSirenClip.hideFlags = HideFlags.HideAndDontSave;
			ClothingStoreAlarmController._synthesizedSirenClip.SetData(array, 0);
			return ClothingStoreAlarmController._synthesizedSirenClip;
		}

		// Token: 0x060098F4 RID: 39156 RVA: 0x003EB588 File Offset: 0x003E9788
		private static void PromoteSuspicionToLevel(int requestedLevel)
		{
			SuspicionManager instance = SuspicionManager.Instance;
			if (instance == null || instance.levels == null || instance.levels.Length == 0)
			{
				return;
			}
			int num = Mathf.Clamp(requestedLevel, 0, instance.MaxLevel);
			if (instance.CurrentLevel >= num)
			{
				return;
			}
			float scoreToEnter = instance.levels[num].scoreToEnter;
			instance.SetScore(Mathf.Max(instance.CurrentScore, scoreToEnter), false);
		}

		// Token: 0x04009DEC RID: 40428
		public const float DefaultTriggerChance = 0.5f;

		// Token: 0x04009DED RID: 40429
		private const string RuntimeRigName = "Clothing Store Alarm (Runtime)";

		// Token: 0x04009DEE RID: 40430
		private const string LegacyBeaconVisualName = "Police Beacon Visual";

		// Token: 0x04009DEF RID: 40431
		private const string CeilingName = "Ceiling";

		// Token: 0x04009DF0 RID: 40432
		private const int TargetSuspicionLevel = 3;

		// Token: 0x04009DF1 RID: 40433
		private const int RequestedPatrolMinimum = 2;

		// Token: 0x04009DF2 RID: 40434
		private const int RequestedPatrolMaximum = 3;

		// Token: 0x04009DF3 RID: 40435
		private const int SirenSampleRate = 44100;

		// Token: 0x04009DF4 RID: 40436
		private const float SirenClipDuration = 2f;

		// Token: 0x04009DF5 RID: 40437
		[Header("Timing")]
		[SerializeField]
		[Min(1f)]
		private float defaultDurationSeconds = 30f;

		// Token: 0x04009DF6 RID: 40438
		[Header("Siren")]
		[SerializeField]
		[Range(0f, 1f)]
		private float sirenVolume = 0.18f;

		// Token: 0x04009DF7 RID: 40439
		[SerializeField]
		[Min(0.1f)]
		private float sirenMinDistance = 1f;

		// Token: 0x04009DF8 RID: 40440
		[SerializeField]
		[Min(1f)]
		private float sirenMaxDistance = 12f;

		// Token: 0x04009DF9 RID: 40441
		[Header("Ceiling light")]
		[SerializeField]
		private Color redColor = new Color(1f, 0.015f, 0.01f, 1f);

		// Token: 0x04009DFA RID: 40442
		[SerializeField]
		private Color blueColor = new Color(0.02f, 0.12f, 1f, 1f);

		// Token: 0x04009DFB RID: 40443
		[SerializeField]
		[Min(0f)]
		private float beaconIntensity = 9f;

		// Token: 0x04009DFC RID: 40444
		[SerializeField]
		[Min(0.1f)]
		private float beaconRange = 11f;

		// Token: 0x04009DFD RID: 40445
		[SerializeField]
		[Min(0.1f)]
		private float colorChangesPerSecond = 4f;

		// Token: 0x04009DFE RID: 40446
		[SerializeField]
		[Min(0f)]
		private float ceilingInset = 0.08f;

		// Token: 0x04009DFF RID: 40447
		[SerializeField]
		[Min(0.1f)]
		private float fallbackHeightAboveTable = 3.3f;

		// Token: 0x04009E00 RID: 40448
		private static AudioClip _synthesizedSirenClip;

		// Token: 0x04009E01 RID: 40449
		private Transform _runtimeRig;

		// Token: 0x04009E02 RID: 40450
		private AudioSource _sirenSource;

		// Token: 0x04009E03 RID: 40451
		private Light _beaconLight;

		// Token: 0x04009E04 RID: 40452
		private bool _isActive;

		// Token: 0x04009E05 RID: 40453
		private float _startedAtUnscaledTime;

		// Token: 0x04009E06 RID: 40454
		private float _endsAtUnscaledTime;
	}
}
