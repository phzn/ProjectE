using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.Inventory
{
	// Token: 0x020010FE RID: 4350
	public class LootRarityAura : MonoBehaviour
	{
		// Token: 0x06008390 RID: 33680 RVA: 0x0035ACED File Offset: 0x00358EED
		public static void EnsureOn(GameObject host, InventoryItemSO item)
		{
			LootRarityAura.EnsureOn(host, item, (item != null) ? item.rarity : ItemRarity.Common);
		}

		// Token: 0x06008391 RID: 33681 RVA: 0x0035AD08 File Offset: 0x00358F08
		public static void EnsureOn(GameObject host, InventoryItemSO item, ItemRarity instanceRarity)
		{
			if (host == null || item == null)
			{
				return;
			}
			LootRarityAura lootRarityAura = host.GetComponentInChildren<LootRarityAura>(true);
			if (item.hideWorldPickupAura)
			{
				if (lootRarityAura != null)
				{
					lootRarityAura.gameObject.SetActive(false);
					if (Application.isPlaying)
					{
						Object.Destroy(lootRarityAura.gameObject);
						return;
					}
					Object.DestroyImmediate(lootRarityAura.gameObject);
				}
				return;
			}
			RaritySettingsSO instance = RaritySettingsSO.Instance;
			if (instance == null)
			{
				return;
			}
			if (!instance.ShowAura(instanceRarity))
			{
				if (lootRarityAura != null)
				{
					Object.Destroy(lootRarityAura.gameObject);
				}
				return;
			}
			if (lootRarityAura == null)
			{
				GameObject gameObject = new GameObject("RarityAura");
				gameObject.transform.SetParent(host.transform, false);
				lootRarityAura = gameObject.AddComponent<LootRarityAura>();
			}
			lootRarityAura.Apply(instanceRarity, instance);
		}

		// Token: 0x06008392 RID: 33682 RVA: 0x0035ADD0 File Offset: 0x00358FD0
		private void Apply(ItemRarity rarity, RaritySettingsSO settings)
		{
			this._color = settings.GetColor(rarity);
			this._pulseSpeed = settings.AuraPulseSpeed;
			this._pulseAmount = settings.AuraPulseAmount;
			this._haloHeightOffset = settings.HaloHeightOffset;
			this._lightHeight = settings.AuraHeight;
			this._haloBaseScale = settings.HaloRadius * 2f;
			this._haloBaseAlpha = settings.HaloAlpha;
			this._shadowAlpha = settings.HaloShadowAlpha;
			float num;
			float num2;
			float num3;
			Color.RGBToHSV(this._color, out num, out num2, out num3);
			this._ringColor = Color.HSVToRGB(num, Mathf.Min(1f, num2 * 1.35f), 1f);
			if (this._light == null)
			{
				this._light = base.GetComponent<Light>();
			}
			if (this._light == null)
			{
				this._light = base.gameObject.AddComponent<Light>();
			}
			this._light.type = LightType.Point;
			this._light.color = this._color;
			this._light.range = settings.AuraRange;
			this._light.intensity = settings.AuraLightIntensity;
			this._light.shadows = LightShadows.None;
			this.EnsureHalo();
			this._itemRenderer = this.FindItemRenderer();
		}

		// Token: 0x06008393 RID: 33683 RVA: 0x0035AF10 File Offset: 0x00359110
		private void EnsureHalo()
		{
			if (this._haloShadow == null)
			{
				this._haloShadow = this.CreateFlatQuad("HaloShadow", LootRarityAura.GetDiscTexture(), out this._haloShadowMat);
			}
			if (this._halo == null)
			{
				this._halo = this.CreateFlatQuad("Halo", LootRarityAura.GetHaloTexture(), out this._haloMat);
			}
			this.SetHaloWorldScale(this._haloBaseScale);
			this.SetHaloColor(this._haloBaseAlpha);
			if (this._haloShadowMat != null)
			{
				this._haloShadowMat.color = new Color(0f, 0f, 0f, this._shadowAlpha);
				this._haloShadowMat.renderQueue = 2998;
			}
		}

		// Token: 0x06008394 RID: 33684 RVA: 0x0035AFCC File Offset: 0x003591CC
		private Transform CreateFlatQuad(string name, Texture2D tex, out Material mat)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
			gameObject.name = name;
			Object.Destroy(gameObject.GetComponent<Collider>());
			Transform transform = gameObject.transform;
			transform.SetParent(base.transform, false);
			transform.rotation = LootRarityAura.HaloFlatRotation;
			Shader shader = Shader.Find("Sprites/Default");
			if (shader == null)
			{
				shader = Shader.Find("Universal Render Pipeline/Unlit");
			}
			mat = new Material(shader)
			{
				mainTexture = tex
			};
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			component.sharedMaterial = mat;
			component.shadowCastingMode = ShadowCastingMode.Off;
			component.receiveShadows = false;
			return transform;
		}

		// Token: 0x06008395 RID: 33685 RVA: 0x0035B05C File Offset: 0x0035925C
		private void SetHaloColor(float alpha)
		{
			if (this._haloMat == null)
			{
				return;
			}
			Color ringColor = this._ringColor;
			ringColor.a = alpha;
			this._haloMat.color = ringColor;
		}

		// Token: 0x06008396 RID: 33686 RVA: 0x0035B094 File Offset: 0x00359294
		private Renderer FindItemRenderer()
		{
			foreach (Renderer renderer in ((base.transform.parent != null) ? base.transform.parent : base.transform).GetComponentsInChildren<Renderer>(true))
			{
				if (!renderer.transform.IsChildOf(base.transform) && (renderer is MeshRenderer || renderer is SkinnedMeshRenderer))
				{
					return renderer;
				}
			}
			return null;
		}

		// Token: 0x06008397 RID: 33687 RVA: 0x0035B108 File Offset: 0x00359308
		private void Update()
		{
			if (this._itemRenderer != null)
			{
				Bounds bounds = this._itemRenderer.bounds;
				base.transform.position = new Vector3(bounds.center.x, bounds.max.y + this._lightHeight, bounds.center.z);
				if (this._halo != null)
				{
					this._halo.position = new Vector3(bounds.center.x, bounds.min.y + this._haloHeightOffset, bounds.center.z);
				}
				if (this._haloShadow != null)
				{
					this._haloShadow.position = new Vector3(bounds.center.x, bounds.min.y + this._haloHeightOffset * 0.5f, bounds.center.z);
				}
			}
			if (this._haloShadow != null)
			{
				this._haloShadow.rotation = LootRarityAura.HaloFlatRotation;
			}
			if (this._halo != null)
			{
				this._halo.rotation = LootRarityAura.HaloFlatRotation;
				if (this._pulseSpeed > 0f && this._pulseAmount > 0f)
				{
					float num = Mathf.Sin(Time.time * this._pulseSpeed * 3.1415927f * 2f) * 0.5f + 0.5f;
					LootRarityAura.SetQuadWorldScale(this._halo, this._haloBaseScale * (1f + num * this._pulseAmount * 0.25f));
					this.SetHaloColor(Mathf.Lerp(this._haloBaseAlpha * 0.7f, this._haloBaseAlpha, num));
				}
			}
		}

		// Token: 0x06008398 RID: 33688 RVA: 0x0035B2CC File Offset: 0x003594CC
		private void SetHaloWorldScale(float worldSize)
		{
			LootRarityAura.SetQuadWorldScale(this._halo, worldSize);
			LootRarityAura.SetQuadWorldScale(this._haloShadow, worldSize * 1.25f);
		}

		// Token: 0x06008399 RID: 33689 RVA: 0x0035B2EC File Offset: 0x003594EC
		private static void SetQuadWorldScale(Transform quad, float worldSize)
		{
			if (quad == null)
			{
				return;
			}
			Vector3 vector = ((quad.parent != null) ? quad.parent.lossyScale : Vector3.one);
			quad.localScale = new Vector3(worldSize / Mathf.Max(0.0001f, vector.x), worldSize / Mathf.Max(0.0001f, vector.y), worldSize / Mathf.Max(0.0001f, vector.z));
		}

		// Token: 0x0600839A RID: 33690 RVA: 0x0035B365 File Offset: 0x00359565
		private void OnDestroy()
		{
			if (this._haloMat != null)
			{
				Object.Destroy(this._haloMat);
			}
			if (this._haloShadowMat != null)
			{
				Object.Destroy(this._haloShadowMat);
			}
		}

		// Token: 0x0600839B RID: 33691 RVA: 0x0035B39C File Offset: 0x0035959C
		private static Texture2D GetDiscTexture()
		{
			if (LootRarityAura._discTex != null)
			{
				return LootRarityAura._discTex;
			}
			LootRarityAura._discTex = new Texture2D(128, 128, TextureFormat.RGBA32, false)
			{
				name = "RarityHaloDisc(Generated)",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				hideFlags = HideFlags.DontSave
			};
			float num = 63.5f;
			Color32[] array = new Color32[16384];
			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					float num2 = ((float)j - num) / num;
					float num3 = ((float)i - num) / num;
					float num4 = Mathf.Sqrt(num2 * num2 + num3 * num3);
					float num5 = Mathf.Clamp01((0.95f - num4) / 0.4f);
					byte b = (byte)Mathf.RoundToInt(num5 * num5 * 255f);
					array[i * 128 + j] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
				}
			}
			LootRarityAura._discTex.SetPixels32(array);
			LootRarityAura._discTex.Apply(false, true);
			return LootRarityAura._discTex;
		}

		// Token: 0x0600839C RID: 33692 RVA: 0x0035B4A8 File Offset: 0x003596A8
		private static Texture2D GetHaloTexture()
		{
			if (LootRarityAura._haloTex != null)
			{
				return LootRarityAura._haloTex;
			}
			LootRarityAura._haloTex = new Texture2D(128, 128, TextureFormat.RGBA32, false)
			{
				name = "RarityHalo(Generated)",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				hideFlags = HideFlags.DontSave
			};
			float num = 63.5f;
			Color32[] array = new Color32[16384];
			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					float num2 = ((float)j - num) / num;
					float num3 = ((float)i - num) / num;
					float num4 = Mathf.Sqrt(num2 * num2 + num3 * num3);
					float num5 = Mathf.Exp(-Mathf.Pow((num4 - 0.72f) / 0.16f, 2f));
					float num6 = Mathf.Clamp01(1f - num4 / 0.72f) * 0.22f;
					byte b = (byte)Mathf.RoundToInt(Mathf.Clamp01(Mathf.Clamp01(num5 + num6) * Mathf.Clamp01((1f - num4) / 0.12f)) * 255f);
					array[i * 128 + j] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
				}
			}
			LootRarityAura._haloTex.SetPixels32(array);
			LootRarityAura._haloTex.Apply(false, true);
			return LootRarityAura._haloTex;
		}

		// Token: 0x040086A1 RID: 34465
		private Light _light;

		// Token: 0x040086A2 RID: 34466
		private Transform _halo;

		// Token: 0x040086A3 RID: 34467
		private Transform _haloShadow;

		// Token: 0x040086A4 RID: 34468
		private Material _haloMat;

		// Token: 0x040086A5 RID: 34469
		private Material _haloShadowMat;

		// Token: 0x040086A6 RID: 34470
		private Renderer _itemRenderer;

		// Token: 0x040086A7 RID: 34471
		private float _haloBaseScale;

		// Token: 0x040086A8 RID: 34472
		private float _haloBaseAlpha;

		// Token: 0x040086A9 RID: 34473
		private float _shadowAlpha;

		// Token: 0x040086AA RID: 34474
		private float _pulseSpeed;

		// Token: 0x040086AB RID: 34475
		private float _pulseAmount;

		// Token: 0x040086AC RID: 34476
		private float _haloHeightOffset;

		// Token: 0x040086AD RID: 34477
		private float _lightHeight;

		// Token: 0x040086AE RID: 34478
		private Color _color;

		// Token: 0x040086AF RID: 34479
		private Color _ringColor;

		// Token: 0x040086B0 RID: 34480
		private static Texture2D _haloTex;

		// Token: 0x040086B1 RID: 34481
		private static Texture2D _discTex;

		// Token: 0x040086B2 RID: 34482
		private static readonly Quaternion HaloFlatRotation = Quaternion.Euler(90f, 0f, 0f);
	}
}
