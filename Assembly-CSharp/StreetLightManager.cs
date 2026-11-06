using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;

// Token: 0x02000023 RID: 35
public class StreetLightManager : MonoBehaviour
{
	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000116 RID: 278 RVA: 0x00008C79 File Offset: 0x00006E79
	// (set) Token: 0x06000117 RID: 279 RVA: 0x00008C80 File Offset: 0x00006E80
	public static StreetLightManager Instance { get; private set; }

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000118 RID: 280 RVA: 0x00008C88 File Offset: 0x00006E88
	public bool IsNight
	{
		get
		{
			return this._isNight;
		}
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00008C90 File Offset: 0x00006E90
	private void Awake()
	{
		if (StreetLightManager.Instance != null && StreetLightManager.Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		StreetLightManager.Instance = this;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00008CC0 File Offset: 0x00006EC0
	private void Start()
	{
		if (this.playerTransform == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				this.playerTransform = gameObject.transform;
			}
		}
		if (this.azureSystem == null)
		{
			this.azureSystem = Object.FindObjectOfType<AzureCoreSystem>();
		}
		this.UpdateNightState();
		this.StartLightProcessing();
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00008D20 File Offset: 0x00006F20
	public void Initialize(Transform player, AzureCoreSystem azure, Camera camera = null)
	{
		this.playerTransform = player;
		this.azureSystem = azure;
		this.UpdateNightState();
		this.StartLightProcessing();
		if (this.showDebugInfo)
		{
			Debug.Log("[StreetLightManager] Initialized via bootstrapper - Player: " + ((player != null) ? player.name : null) + ", Azure: " + ((azure != null) ? azure.name : null));
		}
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00008D7B File Offset: 0x00006F7B
	private void Update()
	{
		if (Time.time - this._lastUpdateTime < this.updateInterval)
		{
			return;
		}
		this._lastUpdateTime = Time.time;
		this.UpdateNightState();
		this.StartLightProcessing();
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00008DA9 File Offset: 0x00006FA9
	private void LateUpdate()
	{
		if (!this._presentationRefreshPending)
		{
			return;
		}
		this._presentationRefreshPending = false;
		this.UpdateNightState();
		this.StartLightProcessing();
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00008DC8 File Offset: 0x00006FC8
	private void UpdateNightState()
	{
		float num = ((this.azureSystem != null) ? this.azureSystem.timeSystem.timeOfDay : ((float)DateTime.Now.Hour));
		bool isNight = this._isNight;
		if (this.nightStartHour > this.nightEndHour)
		{
			this._isNight = num >= this.nightStartHour || num < this.nightEndHour;
		}
		else
		{
			this._isNight = num >= this.nightStartHour && num < this.nightEndHour;
		}
		if (isNight != this._isNight)
		{
			foreach (StreetLightController streetLightController in this._activeLights)
			{
				if (streetLightController != null)
				{
					streetLightController.SetNightMode(this._isNight);
				}
			}
			if (this.showDebugInfo)
			{
				Debug.Log(string.Format("[StreetLightManager] Night mode: {0} (hour: {1:F1})", this._isNight, num));
			}
		}
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00008ED4 File Offset: 0x000070D4
	private void StartLightProcessing()
	{
		this._presentationRefreshPending = false;
		this.PruneUnavailableActiveLights();
		this._totalLights = 0;
		foreach (StreetLightController streetLightController in StreetLightManager._allLights)
		{
			if (!(streetLightController == null) && streetLightController.isActiveAndEnabled && streetLightController.IsRuntimeEligible && streetLightController.IsChunkLightingAvailable)
			{
				this._totalLights++;
				if (!this._isNight && !streetLightController.IsAlwaysOn)
				{
					this.EvictActiveLight(streetLightController);
				}
				else
				{
					if (this._activeLights.Add(streetLightController))
					{
						streetLightController.SetNightMode(this._isNight);
						streetLightController.SetLightEnabled(true);
					}
					float num = ((this.playerTransform != null) ? Vector3.SqrMagnitude(streetLightController.Position - this.playerTransform.position) : float.PositiveInfinity);
					this.ProcessShadowsForLight(streetLightController, num);
				}
			}
		}
		this._activeLightCount = this._activeLights.Count;
		if (this.showDebugInfo && Time.frameCount % 60 == 0)
		{
			Debug.Log(string.Format("[StreetLightManager] Active: {0}/{1} | ", this._activeLightCount, this._totalLights) + string.Format("Shadows: {0}/{1} | Night: {2}", this._shadowLights.Count, this.maxShadowLights, this._isNight));
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00009068 File Offset: 0x00007268
	private void PruneUnavailableActiveLights()
	{
		this._unavailableActiveLights.Clear();
		foreach (StreetLightController streetLightController in this._activeLights)
		{
			if (streetLightController == null || !streetLightController.isActiveAndEnabled || !streetLightController.IsRuntimeEligible || !streetLightController.IsChunkLightingAvailable)
			{
				this._unavailableActiveLights.Add(streetLightController);
			}
		}
		for (int i = 0; i < this._unavailableActiveLights.Count; i++)
		{
			this.EvictActiveLight(this._unavailableActiveLights[i]);
		}
		this._unavailableActiveLights.Clear();
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00009120 File Offset: 0x00007320
	private bool EvictActiveLight(StreetLightController light)
	{
		bool flag = this._activeLights.Remove(light);
		bool flag2 = this._shadowLights.Remove(light);
		if (light != null)
		{
			if (flag)
			{
				light.SetLightEnabled(false);
			}
			if (flag2)
			{
				light.SetShadowsEnabled(false);
			}
		}
		return flag || flag2;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00009168 File Offset: 0x00007368
	private void ProcessShadowsForLight(StreetLightController light, float distSqr)
	{
		if (!this.enableShadows || !light.CanCastShadows)
		{
			if (this._shadowLights.Contains(light))
			{
				this._shadowLights.Remove(light);
				light.SetShadowsEnabled(false);
			}
			return;
		}
		bool flag = this._shadowLights.Contains(light);
		float num = this.shadowDistance * this.shadowDistance;
		float num2 = (this.shadowDistance + this.shadowHysteresis) * (this.shadowDistance + this.shadowHysteresis);
		if (flag)
		{
			if (distSqr > num2)
			{
				this._shadowLights.Remove(light);
				light.SetShadowsEnabled(false);
				return;
			}
		}
		else if (this._shadowLights.Count < this.maxShadowLights && distSqr <= num)
		{
			this._shadowLights.Add(light);
			light.SetShadowsEnabled(true);
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00009224 File Offset: 0x00007424
	public static void Register(StreetLightController light)
	{
		if (light != null && light.IsRuntimeEligible && StreetLightManager._allLights.Add(light) && StreetLightManager.Instance != null)
		{
			StreetLightManager.Instance._presentationRefreshPending = true;
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0000925C File Offset: 0x0000745C
	public static void Unregister(StreetLightController light)
	{
		bool flag = StreetLightManager._allLights.Remove(light);
		if (StreetLightManager.Instance == null)
		{
			return;
		}
		bool flag2 = StreetLightManager.Instance.EvictActiveLight(light);
		if (flag || flag2)
		{
			StreetLightManager.Instance._presentationRefreshPending = true;
		}
		StreetLightManager.Instance._activeLightCount = StreetLightManager.Instance._activeLights.Count;
	}

	// Token: 0x06000125 RID: 293 RVA: 0x000092B8 File Offset: 0x000074B8
	public static void NotifyPresentationVisibilityChanged(StreetLightController light, bool visible)
	{
		if (StreetLightManager.Instance == null || light == null)
		{
			return;
		}
		if (!visible)
		{
			StreetLightManager.Instance.EvictActiveLight(light);
			light.SetLightEnabled(false);
			StreetLightManager.Instance._activeLightCount = StreetLightManager.Instance._activeLights.Count;
		}
		StreetLightManager.Instance._presentationRefreshPending = true;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00009318 File Offset: 0x00007518
	public void SetShadowsEnabled(bool enabled)
	{
		this.enableShadows = enabled;
		if (!enabled)
		{
			foreach (StreetLightController streetLightController in this._shadowLights)
			{
				if (streetLightController != null)
				{
					streetLightController.SetShadowsEnabled(false);
				}
			}
			this._shadowLights.Clear();
		}
		if (this.showDebugInfo)
		{
			Debug.Log("[StreetLightManager] Shadows " + (enabled ? "ENABLED" : "DISABLED"));
		}
	}

	// Token: 0x06000127 RID: 295 RVA: 0x000093B0 File Offset: 0x000075B0
	public void SetShadowDistance(float distance)
	{
		this.shadowDistance = Mathf.Max(0f, distance);
	}

	// Token: 0x06000128 RID: 296 RVA: 0x000093C3 File Offset: 0x000075C3
	public void SetMaxShadowLights(int count)
	{
		this.maxShadowLights = Mathf.Max(0, count);
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000129 RID: 297 RVA: 0x000093D2 File Offset: 0x000075D2
	public bool ShadowsEnabled
	{
		get
		{
			return this.enableShadows;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x0600012A RID: 298 RVA: 0x000093DA File Offset: 0x000075DA
	public float ShadowDistance
	{
		get
		{
			return this.shadowDistance;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x0600012B RID: 299 RVA: 0x000093E2 File Offset: 0x000075E2
	public int MaxShadowLights
	{
		get
		{
			return this.maxShadowLights;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x0600012C RID: 300 RVA: 0x000093EA File Offset: 0x000075EA
	public int CurrentShadowLightCount
	{
		get
		{
			return this._shadowLights.Count;
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x000093F8 File Offset: 0x000075F8
	private void OnDisable()
	{
		this._unavailableActiveLights.Clear();
		this._unavailableActiveLights.AddRange(this._activeLights);
		foreach (StreetLightController streetLightController in this._unavailableActiveLights)
		{
			this.EvictActiveLight(streetLightController);
		}
		this._unavailableActiveLights.Clear();
		this._activeLightCount = 0;
		this._presentationRefreshPending = true;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00009484 File Offset: 0x00007684
	private void OnEnable()
	{
		this._presentationRefreshPending = true;
	}

	// Token: 0x0600012F RID: 303 RVA: 0x0000948D File Offset: 0x0000768D
	private void OnDestroy()
	{
		if (StreetLightManager.Instance == this)
		{
			StreetLightManager.Instance = null;
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x000094A4 File Offset: 0x000076A4
	private void OnDrawGizmosSelected()
	{
		if (this.playerTransform == null)
		{
			return;
		}
		if (this.enableShadows)
		{
			Gizmos.color = new Color(1f, 0.3f, 0f, 0.5f);
			Gizmos.DrawWireSphere(this.playerTransform.position, this.shadowDistance);
		}
	}

	// Token: 0x040000E3 RID: 227
	[Header("References")]
	[SerializeField]
	private Transform playerTransform;

	// Token: 0x040000E4 RID: 228
	[SerializeField]
	private AzureCoreSystem azureSystem;

	// Token: 0x040000E5 RID: 229
	[Header("Time Settings")]
	[Tooltip("Ora in cui le luci si accendono")]
	[SerializeField]
	private float nightStartHour = 17f;

	// Token: 0x040000E6 RID: 230
	[Tooltip("Ora in cui le luci si spengono")]
	[SerializeField]
	private float nightEndHour = 6f;

	// Token: 0x040000E7 RID: 231
	[Header("Performance")]
	[Tooltip("Intervallo di aggiornamento in secondi")]
	[SerializeField]
	private float updateInterval = 0.3f;

	// Token: 0x040000E8 RID: 232
	[Header("Shadow Settings")]
	[Tooltip("Abilita ombre sulle luci stradali (impatto performance significativo)")]
	[SerializeField]
	private bool enableShadows;

	// Token: 0x040000E9 RID: 233
	[Tooltip("Distanza massima per ombre (solo luci molto vicine)")]
	[SerializeField]
	private float shadowDistance = 15f;

	// Token: 0x040000EA RID: 234
	[Tooltip("Distanza extra prima di rimuovere ombre (evita flickering)")]
	[SerializeField]
	private float shadowHysteresis = 5f;

	// Token: 0x040000EB RID: 235
	[Tooltip("Max luci con ombre attive contemporaneamente")]
	[SerializeField]
	private int maxShadowLights = 3;

	// Token: 0x040000EC RID: 236
	[Header("Debug")]
	[SerializeField]
	private bool showDebugInfo;

	// Token: 0x040000ED RID: 237
	private static HashSet<StreetLightController> _allLights = new HashSet<StreetLightController>();

	// Token: 0x040000EE RID: 238
	private HashSet<StreetLightController> _activeLights = new HashSet<StreetLightController>();

	// Token: 0x040000EF RID: 239
	private readonly List<StreetLightController> _unavailableActiveLights = new List<StreetLightController>();

	// Token: 0x040000F0 RID: 240
	private float _lastUpdateTime;

	// Token: 0x040000F1 RID: 241
	private bool _isNight;

	// Token: 0x040000F2 RID: 242
	private bool _presentationRefreshPending;

	// Token: 0x040000F3 RID: 243
	private HashSet<StreetLightController> _shadowLights = new HashSet<StreetLightController>();

	// Token: 0x040000F4 RID: 244
	private int _totalLights;

	// Token: 0x040000F5 RID: 245
	private int _activeLightCount;
}
