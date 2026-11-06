using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
[RequireComponent(typeof(Light))]
public class StreetLightController : MonoBehaviour
{
	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000101 RID: 257 RVA: 0x00008889 File Offset: 0x00006A89
	public float ActivationDistance
	{
		get
		{
			return this.activationDistance;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000102 RID: 258 RVA: 0x00008891 File Offset: 0x00006A91
	public Vector3 Position
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000103 RID: 259 RVA: 0x0000889E File Offset: 0x00006A9E
	public bool CanCastShadows
	{
		get
		{
			return this.canCastShadows;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000104 RID: 260 RVA: 0x000088A6 File Offset: 0x00006AA6
	public bool IsRuntimeEligible
	{
		get
		{
			return this._isRuntimeEligible;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000105 RID: 261 RVA: 0x000088AE File Offset: 0x00006AAE
	public bool IsPresentationVisible
	{
		get
		{
			return this._presentationVisible;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000106 RID: 262 RVA: 0x000088AE File Offset: 0x00006AAE
	public bool IsChunkLightingAvailable
	{
		get
		{
			return this._presentationVisible;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000107 RID: 263 RVA: 0x000088B6 File Offset: 0x00006AB6
	public bool WasAuthoredRealtime
	{
		get
		{
			return this._wasAuthoredRealtime;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000108 RID: 264 RVA: 0x000088BE File Offset: 0x00006ABE
	public bool IsAuthoredNightEnabled
	{
		get
		{
			return this.nightLightingEnabled;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000109 RID: 265 RVA: 0x000088C6 File Offset: 0x00006AC6
	public bool IsAlwaysOn
	{
		get
		{
			return this.alwaysOn;
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x000088D0 File Offset: 0x00006AD0
	private void Awake()
	{
		if (this.streetLight == null)
		{
			this.streetLight = base.GetComponent<Light>();
		}
		if (this.streetLight != null)
		{
			this._baseIntensity = this.streetLight.intensity;
			LightmapBakeType lightmapBakeType = this.streetLight.bakingOutput.lightmapBakeType;
			this._wasAuthoredRealtime = lightmapBakeType == LightmapBakeType.Realtime;
			this._isRuntimeEligible = !this.excludeFromRuntimeBudget && this.nightLightingEnabled;
			if (!this.nightLightingEnabled)
			{
				this.streetLight.enabled = false;
			}
			if (this._isRuntimeEligible)
			{
				if (lightmapBakeType == LightmapBakeType.Baked)
				{
					LightBakingOutput bakingOutput = this.streetLight.bakingOutput;
					bakingOutput.isBaked = false;
					bakingOutput.lightmapBakeType = LightmapBakeType.Realtime;
					bakingOutput.occlusionMaskChannel = -1;
					bakingOutput.probeOcclusionLightIndex = -1;
					this.streetLight.bakingOutput = bakingOutput;
				}
				this.streetLight.enabled = false;
				this.streetLight.shadows = LightShadows.None;
			}
		}
		this._propertyBlock = new MaterialPropertyBlock();
		if (this.emissiveRenderer != null)
		{
			this._emissionColor = ((this.streetLight != null) ? (this.streetLight.color * this.nightIntensity) : (Color.white * this.nightIntensity));
			this.UpdateEmissive(false);
		}
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00008A17 File Offset: 0x00006C17
	private void OnEnable()
	{
		if (this._isRuntimeEligible && !this._isRegistered)
		{
			StreetLightManager.Register(this);
			this._isRegistered = true;
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00008A36 File Offset: 0x00006C36
	private void OnDisable()
	{
		this._managerRequestedEnabled = false;
		this.ApplyEffectiveEnabledState();
		if (this._isRegistered)
		{
			StreetLightManager.Unregister(this);
			this._isRegistered = false;
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00008A5A File Offset: 0x00006C5A
	public void SetLightEnabled(bool enabled)
	{
		if (!this._isRuntimeEligible)
		{
			return;
		}
		this._managerRequestedEnabled = enabled;
		this.ApplyEffectiveEnabledState();
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00008A72 File Offset: 0x00006C72
	public void SetChunkLightingAvailable(bool visible)
	{
		if (this._presentationVisible == visible)
		{
			return;
		}
		this._presentationVisible = visible;
		this.ApplyEffectiveEnabledState();
		StreetLightManager.NotifyPresentationVisibilityChanged(this, visible);
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00008A92 File Offset: 0x00006C92
	public void SetPresentationVisible(bool visible)
	{
		this.SetChunkLightingAvailable(visible);
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00008A9B File Offset: 0x00006C9B
	public void SetShadowsEnabled(bool enabled)
	{
		this._shadowsEnabled = enabled;
		if (this._isRuntimeEligible && this.streetLight != null)
		{
			this.streetLight.shadows = (enabled ? this.shadowType : LightShadows.None);
		}
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00008AD4 File Offset: 0x00006CD4
	public void SetNightMode(bool isNight)
	{
		bool flag = isNight || this.alwaysOn;
		if (this._isRuntimeEligible && this.streetLight != null)
		{
			this.streetLight.intensity = (flag ? Mathf.Max(this._baseIntensity, this.nightIntensity) : 0f);
		}
		this.UpdateEmissive(flag && this.streetLight != null && this.streetLight.enabled);
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00008B50 File Offset: 0x00006D50
	private void UpdateEmissive(bool enabled)
	{
		if (this.emissiveRenderer == null || this._propertyBlock == null)
		{
			return;
		}
		this.emissiveRenderer.GetPropertyBlock(this._propertyBlock, this.materialIndex);
		this._propertyBlock.SetColor(StreetLightController.EmissionColor, enabled ? this._emissionColor : Color.black);
		this.emissiveRenderer.SetPropertyBlock(this._propertyBlock, this.materialIndex);
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00008BC4 File Offset: 0x00006DC4
	private void ApplyEffectiveEnabledState()
	{
		if (this.streetLight == null || !this._isRuntimeEligible)
		{
			return;
		}
		bool flag = this._managerRequestedEnabled && this._presentationVisible;
		if (this.streetLight.enabled != flag)
		{
			this.streetLight.enabled = flag;
		}
		this.UpdateEmissive(flag);
	}

	// Token: 0x040000CE RID: 206
	[Header("References")]
	[SerializeField]
	private Light streetLight;

	// Token: 0x040000CF RID: 207
	[Header("Emissive (Optional)")]
	[SerializeField]
	private Renderer emissiveRenderer;

	// Token: 0x040000D0 RID: 208
	[SerializeField]
	private int materialIndex;

	// Token: 0x040000D1 RID: 209
	[Header("Settings")]
	[HideInInspector]
	[SerializeField]
	private float activationDistance = 50f;

	// Token: 0x040000D2 RID: 210
	[Tooltip("Minimum night intensity. Stronger authored lights keep their original value.")]
	[SerializeField]
	private float nightIntensity = 3f;

	// Token: 0x040000D3 RID: 211
	[Tooltip("Se false, questa luce non avrà MAI ombre (ignora impostazioni manager)")]
	[SerializeField]
	private bool canCastShadows = true;

	// Token: 0x040000D4 RID: 212
	[Tooltip("Esclude esplicitamente questa fixture dal budget runtime. Lo stato Enabled della Light è solo lo stato di boot.")]
	[SerializeField]
	private bool excludeFromRuntimeBudget;

	// Token: 0x040000D5 RID: 213
	[Tooltip("Authored household occupancy. Kept stable across nights, camera movement and chunk pooling.")]
	[SerializeField]
	private bool nightLightingEnabled = true;

	// Token: 0x040000D6 RID: 214
	[Tooltip("Indoor fixture: remains lit during daytime while its chunk is available. Uses the shared manager and no per-light Update.")]
	[SerializeField]
	private bool alwaysOn;

	// Token: 0x040000D7 RID: 215
	[Tooltip("Tipo di ombre quando abilitato dal manager")]
	[SerializeField]
	private LightShadows shadowType = LightShadows.Soft;

	// Token: 0x040000D8 RID: 216
	private float _baseIntensity;

	// Token: 0x040000D9 RID: 217
	private bool _wasAuthoredRealtime;

	// Token: 0x040000DA RID: 218
	private bool _isRuntimeEligible;

	// Token: 0x040000DB RID: 219
	private bool _isRegistered;

	// Token: 0x040000DC RID: 220
	private bool _managerRequestedEnabled;

	// Token: 0x040000DD RID: 221
	private bool _presentationVisible = true;

	// Token: 0x040000DE RID: 222
	private bool _shadowsEnabled = true;

	// Token: 0x040000DF RID: 223
	private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

	// Token: 0x040000E0 RID: 224
	private MaterialPropertyBlock _propertyBlock;

	// Token: 0x040000E1 RID: 225
	private Color _emissionColor;
}
