using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000D4E RID: 3406
	[DefaultExecutionOrder(10000)]
	public sealed class RainShelterRuntime : MonoBehaviour
	{
		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06006406 RID: 25606 RVA: 0x00281928 File Offset: 0x0027FB28
		// (remove) Token: 0x06006407 RID: 25607 RVA: 0x0028195C File Offset: 0x0027FB5C
		public static event Action VisualMaskChanged;

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06006408 RID: 25608 RVA: 0x0028198F File Offset: 0x0027FB8F
		public static bool UsesVolumeMask
		{
			get
			{
				return RainShelterRuntime._instance != null && RainShelterRuntime._instance._maskCount > 0;
			}
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06006409 RID: 25609 RVA: 0x002819AD File Offset: 0x0027FBAD
		public static float CurrentShelterBlend
		{
			get
			{
				if (!(RainShelterRuntime._instance != null))
				{
					return 0f;
				}
				return RainShelterRuntime._instance._playerShelterBlend;
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x0600640A RID: 25610 RVA: 0x002819CC File Offset: 0x0027FBCC
		public static bool IsSheltered
		{
			get
			{
				if (!(PlayerRegistry.Player != null))
				{
					return RainShelterRuntime._instance != null && RainShelterRuntime._instance._closedScope;
				}
				return RainShelterRuntime.IsShelteredAt(PlayerRegistry.Player.position);
			}
		}

		// Token: 0x0600640B RID: 25611 RVA: 0x00281A04 File Offset: 0x0027FC04
		public static bool IsShelteredAt(Vector3 position)
		{
			return (RainShelterRuntime._instance != null && RainShelterRuntime._instance._closedScope) || RainShelterVolume.ContainsAny(position);
		}

		// Token: 0x0600640C RID: 25612 RVA: 0x00281A27 File Offset: 0x0027FC27
		public static bool IsClosedPoiShelterScene(string sceneName)
		{
			return !string.IsNullOrEmpty(sceneName) && RainShelterRuntime.ClosedPoiShelterScenes.Contains(sceneName);
		}

		// Token: 0x0600640D RID: 25613 RVA: 0x00281A3E File Offset: 0x0027FC3E
		public static bool IsLocalPoiShelterScene(string sceneName)
		{
			return sceneName == "POI - University" || sceneName == "POI - Stadium";
		}

		// Token: 0x0600640E RID: 25614 RVA: 0x00281A5A File Offset: 0x0027FC5A
		public static bool IsInlinePoiShelterScene(string sceneName)
		{
			return sceneName == "Game - Current";
		}

		// Token: 0x0600640F RID: 25615 RVA: 0x00281A67 File Offset: 0x0027FC67
		public static bool IsShelteredInScene(string sceneName, Vector3 position)
		{
			return RainShelterRuntime.IsClosedPoiShelterScene(sceneName) || (RainShelterRuntime.IsLocalPoiShelterScene(sceneName) && RainShelterVolume.ContainsAny(position)) || (RainShelterRuntime.IsInlinePoiShelterScene(sceneName) && RainShelterVolume.ContainsClosedPoi(position));
		}

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06006410 RID: 25616 RVA: 0x00281A94 File Offset: 0x0027FC94
		public static int EnvironmentalCollisionMask
		{
			get
			{
				if (RainShelterRuntime._cachedEnvironmentalCollisionMask == null)
				{
					RainShelterRuntime._cachedEnvironmentalCollisionMask = new int?(LayerMask.GetMask(new string[] { "Default", "Floor", "Interactable", "InteractableTerrain", "Glass", "VisionBlocker" }));
				}
				return RainShelterRuntime._cachedEnvironmentalCollisionMask.Value;
			}
		}

		// Token: 0x06006411 RID: 25617 RVA: 0x00281AFC File Offset: 0x0027FCFC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetCachedLayerMask()
		{
			RainShelterRuntime._cachedEnvironmentalCollisionMask = null;
		}

		// Token: 0x06006412 RID: 25618 RVA: 0x00281B09 File Offset: 0x0027FD09
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void EnsureRuntime()
		{
			if (RainShelterRuntime._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[RainShelterRuntime]");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<RainShelterRuntime>();
		}

		// Token: 0x06006413 RID: 25619 RVA: 0x00281B30 File Offset: 0x0027FD30
		public static float EvaluateShelteredRainVolume(float authoredVolume, float shelterBlend)
		{
			float num = Mathf.Clamp01(shelterBlend);
			num = num * num * (3f - 2f * num);
			return Mathf.Max(0f, authoredVolume) * Mathf.Lerp(1f, 0.22f, num);
		}

		// Token: 0x06006414 RID: 25620 RVA: 0x00281B74 File Offset: 0x0027FD74
		public static float AdvanceShelterBlend(float currentBlend, float targetBlend, ref float velocity, float deltaTime)
		{
			currentBlend = Mathf.Clamp01(currentBlend);
			targetBlend = Mathf.Clamp01(targetBlend);
			if (deltaTime <= 0f)
			{
				return currentBlend;
			}
			float num = ((targetBlend > currentBlend) ? 0.28f : 0.65f);
			float num2 = Mathf.SmoothDamp(currentBlend, targetBlend, ref velocity, num, float.PositiveInfinity, deltaTime);
			num2 = Mathf.Clamp01(num2);
			if (Mathf.Abs(num2 - targetBlend) <= 0.0001f && Mathf.Abs(velocity) <= 0.0001f)
			{
				velocity = 0f;
				return targetBlend;
			}
			return num2;
		}

		// Token: 0x06006415 RID: 25621 RVA: 0x00281BEC File Offset: 0x0027FDEC
		public static float CalculateShelterMaskRadius(Vector3 observerPosition, Bounds roofBounds)
		{
			float num = 1.25f;
			if (observerPosition.x >= roofBounds.min.x && observerPosition.x <= roofBounds.max.x && observerPosition.z >= roofBounds.min.z && observerPosition.z <= roofBounds.max.z)
			{
				num = Mathf.Min(new float[]
				{
					observerPosition.x - roofBounds.min.x,
					roofBounds.max.x - observerPosition.x,
					observerPosition.z - roofBounds.min.z,
					roofBounds.max.z - observerPosition.z
				});
			}
			return Mathf.Clamp(num + 0.25f, 1.25f, 12f);
		}

		// Token: 0x06006416 RID: 25622 RVA: 0x00004F31 File Offset: 0x00003131
		public static void ConfigureRainCollision(ParticleSystem particleSystem)
		{
		}

		// Token: 0x06006417 RID: 25623 RVA: 0x00281CCF File Offset: 0x0027FECF
		private void Awake()
		{
			RainShelterRuntime._instance = this;
		}

		// Token: 0x06006418 RID: 25624 RVA: 0x00281CD8 File Offset: 0x0027FED8
		private void OnEnable()
		{
			RainShelterRuntime._instance = this;
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			RainShelterVolume.Changed -= this.OnVolumesChanged;
			RainShelterVolume.Changed += this.OnVolumesChanged;
			AzureParticleFX.InstanceEnabled -= this.OnParticleEnabled;
			AzureParticleFX.InstanceEnabled += this.OnParticleEnabled;
			this.RefreshScope();
		}

		// Token: 0x06006419 RID: 25625 RVA: 0x00281D79 File Offset: 0x0027FF79
		private void OnActiveSceneChanged(Scene previous, Scene current)
		{
			this.RefreshScope();
		}

		// Token: 0x0600641A RID: 25626 RVA: 0x00281D79 File Offset: 0x0027FF79
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.RefreshScope();
		}

		// Token: 0x0600641B RID: 25627 RVA: 0x00281D81 File Offset: 0x0027FF81
		private void OnVolumesChanged()
		{
			this._nextTick = 0f;
			if (this._localScope || this._inlineScope)
			{
				this.TickShelter();
			}
		}

		// Token: 0x0600641C RID: 25628 RVA: 0x00281DA4 File Offset: 0x0027FFA4
		private void RefreshScope()
		{
			string name = SceneManager.GetActiveScene().name;
			this._closedScope = RainShelterRuntime.IsClosedPoiShelterScene(name);
			this._localScope = RainShelterRuntime.IsLocalPoiShelterScene(name);
			this._inlineScope = RainShelterRuntime.IsInlinePoiShelterScene(name);
			this._playerShelterBlend = (this._closedScope ? 1f : 0f);
			this._nextTick = 0f;
			this.TickShelter();
		}

		// Token: 0x0600641D RID: 25629 RVA: 0x00281E0E File Offset: 0x0028000E
		private void Update()
		{
			if (!this._localScope && (!this._inlineScope || !RainShelterVolume.HasClosedPoiVolumes))
			{
				return;
			}
			if (Time.unscaledTime < this._nextTick)
			{
				return;
			}
			this._nextTick = Time.unscaledTime + 0.2f;
			this.TickShelter();
		}

		// Token: 0x0600641E RID: 25630 RVA: 0x00281E4D File Offset: 0x0028004D
		private void OnParticleEnabled(AzureParticleFX fx)
		{
			if (fx != null)
			{
				fx.SetEmissionSuppressed(this._precipitationSuppressed);
			}
		}

		// Token: 0x0600641F RID: 25631 RVA: 0x00281E64 File Offset: 0x00280064
		private void RestorePrecipitation()
		{
			IReadOnlyList<AzureParticleFX> activeInstances = AzureParticleFX.activeInstances;
			for (int i = 0; i < activeInstances.Count; i++)
			{
				if (activeInstances[i] != null)
				{
					activeInstances[i].SetEmissionSuppressed(false);
				}
			}
		}

		// Token: 0x06006420 RID: 25632 RVA: 0x00281EA4 File Offset: 0x002800A4
		private void TickShelter()
		{
			Transform player = PlayerRegistry.Player;
			bool flag = this._inlineScope && player != null && RainShelterVolume.ContainsClosedPoi(player.position);
			bool flag2 = this._closedScope || flag;
			AzureWeatherPropertyLinker.SuppressPrecipitation = flag2;
			this._precipitationSuppressed = flag2;
			IReadOnlyList<AzureParticleFX> activeInstances = AzureParticleFX.activeInstances;
			for (int i = 0; i < activeInstances.Count; i++)
			{
				if (activeInstances[i] != null)
				{
					activeInstances[i].SetEmissionSuppressed(flag2);
				}
			}
			this._playerShelterBlend = ((flag2 || (this._localScope && RainShelterRuntime.IsSheltered)) ? 1f : 0f);
			if (!this._localScope)
			{
				this.SetMaskCount(0, null);
				return;
			}
			Camera main = Camera.main;
			if (main == null)
			{
				this.SetMaskCount(0, null);
				return;
			}
			Vector3 position = main.transform.position;
			float num = ((this._maskCount > 0) ? 9025f : 8100f);
			for (int j = 0; j < 8; j++)
			{
				this._nearest[j] = null;
				this._distances[j] = num;
			}
			IReadOnlyList<RainShelterVolume> activeVolumes = RainShelterVolume.ActiveVolumes;
			for (int k = 0; k < activeVolumes.Count; k++)
			{
				RainShelterVolume rainShelterVolume = activeVolumes[k];
				if (!(rainShelterVolume == null))
				{
					float num2 = ((player != null && rainShelterVolume.Contains(player.position)) ? (-1f) : rainShelterVolume.SqrDistance(position));
					for (int l = 0; l < 8; l++)
					{
						if (num2 < this._distances[l])
						{
							for (int m = 7; m > l; m--)
							{
								this._nearest[m] = this._nearest[m - 1];
								this._distances[m] = this._distances[m - 1];
							}
							this._nearest[l] = rainShelterVolume;
							this._distances[l] = num2;
							break;
						}
					}
				}
			}
			RainShelterVolume rainShelterVolume2 = null;
			for (int n = 0; n < 8; n++)
			{
				this._matrices[n] = Matrix4x4.identity;
				if (rainShelterVolume2 == null && this._nearest[n] != null && this._nearest[n].RoofHeightMap != null)
				{
					rainShelterVolume2 = this._nearest[n];
				}
			}
			int num3 = 0;
			if (rainShelterVolume2 != null)
			{
				this._matrices[num3++] = rainShelterVolume2.WorldToUnitBox;
			}
			int num4 = 0;
			while (num4 < 8 && num3 < 8)
			{
				if (this._nearest[num4] != null && this._nearest[num4].RoofHeightMap == null)
				{
					this._matrices[num3++] = this._nearest[num4].WorldToUnitBox;
				}
				num4++;
			}
			if (num3 > 0)
			{
				Shader.SetGlobalMatrixArray(RainShelterRuntime.VolumeMatricesId, this._matrices);
			}
			this.SetMaskCount(num3, (rainShelterVolume2 != null) ? rainShelterVolume2.RoofHeightMap : null);
		}

		// Token: 0x06006421 RID: 25633 RVA: 0x002821A8 File Offset: 0x002803A8
		private void SetMaskCount(int count, Texture2D roofHeightMap = null)
		{
			Shader.SetGlobalFloat(RainShelterRuntime.VolumeCountId, (float)count);
			Shader.SetGlobalFloat(RainShelterRuntime.RoofHeightMapEnabledId, (roofHeightMap != null) ? 1f : 0f);
			if (this._roofHeightMap != roofHeightMap)
			{
				Shader.SetGlobalTexture(RainShelterRuntime.RoofHeightMapId, (roofHeightMap != null) ? roofHeightMap : Texture2D.blackTexture);
			}
			if (this._maskCount == count && this._roofHeightMap == roofHeightMap)
			{
				return;
			}
			this._maskCount = count;
			this._roofHeightMap = roofHeightMap;
			Action visualMaskChanged = RainShelterRuntime.VisualMaskChanged;
			if (visualMaskChanged == null)
			{
				return;
			}
			visualMaskChanged();
		}

		// Token: 0x06006422 RID: 25634 RVA: 0x00282240 File Offset: 0x00280440
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			RainShelterVolume.Changed -= this.OnVolumesChanged;
			AzureParticleFX.InstanceEnabled -= this.OnParticleEnabled;
			AzureWeatherPropertyLinker.SuppressPrecipitation = false;
			this.RestorePrecipitation();
			this.SetMaskCount(0, null);
		}

		// Token: 0x06006423 RID: 25635 RVA: 0x002822A5 File Offset: 0x002804A5
		private void OnDestroy()
		{
			this.OnDisable();
			if (RainShelterRuntime._instance == this)
			{
				RainShelterRuntime._instance = null;
			}
		}

		// Token: 0x04006709 RID: 26377
		public const string ShelterBlendName = "_Project_RainShelterBlend";

		// Token: 0x0400670A RID: 26378
		public const string ShelterDataName = "_Project_RainShelterData";

		// Token: 0x0400670B RID: 26379
		public const string ShelterAffectedName = "_Project_RainShelterAffected";

		// Token: 0x0400670C RID: 26380
		public const string ShelterParticleShaderName = "Project/Weather/Rain Shelter Particles";

		// Token: 0x0400670D RID: 26381
		public const float IndoorRainAudioMultiplier = 0.22f;

		// Token: 0x0400670E RID: 26382
		public const float ShelterEnterSmoothTimeSeconds = 0.28f;

		// Token: 0x0400670F RID: 26383
		public const float ShelterExitSmoothTimeSeconds = 0.65f;

		// Token: 0x04006710 RID: 26384
		public const float MinimumShelterMaskRadiusMeters = 1.25f;

		// Token: 0x04006711 RID: 26385
		public const float MaximumShelterMaskRadiusMeters = 12f;

		// Token: 0x04006712 RID: 26386
		private const int MaxMaskVolumes = 8;

		// Token: 0x04006713 RID: 26387
		private static readonly int VolumeCountId = Shader.PropertyToID("_Project_RainShelterVolumeCount");

		// Token: 0x04006714 RID: 26388
		private static readonly int VolumeMatricesId = Shader.PropertyToID("_Project_RainShelterMatrices8");

		// Token: 0x04006715 RID: 26389
		private static readonly int RoofHeightMapId = Shader.PropertyToID("_Project_RainShelterRoofHeightMap");

		// Token: 0x04006716 RID: 26390
		private static readonly int RoofHeightMapEnabledId = Shader.PropertyToID("_Project_RainShelterRoofHeightMapEnabled");

		// Token: 0x04006717 RID: 26391
		private static readonly HashSet<string> ClosedPoiShelterScenes = new HashSet<string> { "POI - Art Gallery", "POI - Mall", "POI - Pool", "CielHome" };

		// Token: 0x04006718 RID: 26392
		private static RainShelterRuntime _instance;

		// Token: 0x04006719 RID: 26393
		private static int? _cachedEnvironmentalCollisionMask;

		// Token: 0x0400671A RID: 26394
		private readonly Matrix4x4[] _matrices = new Matrix4x4[8];

		// Token: 0x0400671B RID: 26395
		private readonly float[] _distances = new float[8];

		// Token: 0x0400671C RID: 26396
		private readonly RainShelterVolume[] _nearest = new RainShelterVolume[8];

		// Token: 0x0400671D RID: 26397
		private bool _closedScope;

		// Token: 0x0400671E RID: 26398
		private bool _localScope;

		// Token: 0x0400671F RID: 26399
		private bool _inlineScope;

		// Token: 0x04006720 RID: 26400
		private bool _precipitationSuppressed;

		// Token: 0x04006721 RID: 26401
		private float _nextTick;

		// Token: 0x04006722 RID: 26402
		private float _playerShelterBlend;

		// Token: 0x04006723 RID: 26403
		private int _maskCount;

		// Token: 0x04006724 RID: 26404
		private Texture2D _roofHeightMap;
	}
}
