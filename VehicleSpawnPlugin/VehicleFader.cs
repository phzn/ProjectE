using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000004 RID: 4
	[DisallowMultipleComponent]
	public class VehicleFader : MonoBehaviour
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000045EE File Offset: 0x000027EE
		public bool IsFading
		{
			get
			{
				return this._fading;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000045F8 File Offset: 0x000027F8
		public static VehicleFader GetOrAdd(GameObject go)
		{
			VehicleFader vehicleFader = go.GetComponent<VehicleFader>();
			if (vehicleFader == null)
			{
				vehicleFader = go.AddComponent<VehicleFader>();
			}
			return vehicleFader;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004620 File Offset: 0x00002820
		public void BeginFade(float duration, Action onComplete)
		{
			if (VspDiag.Carry)
			{
				VehicleAI component = base.GetComponent<VehicleAI>();
				Debug.Log(string.Format("[CarryDiag] VehicleFader.BeginFade '{0}' duration={1:F2} alreadyFading={2} HasPassenger={3} frame={4}", new object[]
				{
					base.name,
					duration,
					this._fading,
					component != null && component.HasPassenger,
					Time.frameCount
				}), this);
			}
			if (this._fading)
			{
				return;
			}
			this._onComplete = onComplete;
			this._fadeDuration = Mathf.Max(0.05f, duration);
			this._fadeTimer = 0f;
			this.BuildTransparentClones();
			if (this._slots.Count == 0)
			{
				Action onComplete2 = this._onComplete;
				if (onComplete2 != null)
				{
					onComplete2();
				}
				this._onComplete = null;
				return;
			}
			this._fading = true;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000046F8 File Offset: 0x000028F8
		private void Update()
		{
			if (!this._fading || VehicleSimulationPause.IsPaused)
			{
				return;
			}
			this._fadeTimer += Time.deltaTime;
			float num = 1f - Mathf.Clamp01(this._fadeTimer / this._fadeDuration);
			this.ApplyAlpha(num);
			if (this._fadeTimer >= this._fadeDuration)
			{
				this._fading = false;
				if (VspDiag.Carry)
				{
					VehicleAI component = base.GetComponent<VehicleAI>();
					Debug.Log(string.Format("[CarryDiag] VehicleFader complete '{0}' HasPassenger={1} frame={2}", base.name, component != null && component.HasPassenger, Time.frameCount), this);
				}
				Action onComplete = this._onComplete;
				this._onComplete = null;
				if (onComplete == null)
				{
					return;
				}
				onComplete();
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000047B8 File Offset: 0x000029B8
		private void ApplyAlpha(float a)
		{
			for (int i = 0; i < this._slots.Count; i++)
			{
				VehicleFader.FadeSlot fadeSlot = this._slots[i];
				if (fadeSlot.clones != null && fadeSlot.colorProp != 0)
				{
					for (int j = 0; j < fadeSlot.clones.Length; j++)
					{
						Material material = fadeSlot.clones[j];
						if (!(material == null))
						{
							Color color = fadeSlot.baseColors[j];
							color.a = a;
							material.SetColor(fadeSlot.colorProp, color);
						}
					}
				}
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004844 File Offset: 0x00002A44
		private void BuildTransparentClones()
		{
			this._slots.Clear();
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(false))
			{
				if (!(renderer == null))
				{
					Material[] sharedMaterials = renderer.sharedMaterials;
					if (sharedMaterials != null && sharedMaterials.Length != 0)
					{
						Material[] array = new Material[sharedMaterials.Length];
						Color[] array2 = new Color[sharedMaterials.Length];
						int num = 0;
						for (int j = 0; j < sharedMaterials.Length; j++)
						{
							Material material = sharedMaterials[j];
							if (material == null)
							{
								array[j] = null;
							}
							else
							{
								Material material2 = new Material(material);
								VehicleFader.MakeTransparent(material2);
								array[j] = material2;
								if (num == 0)
								{
									if (material2.HasProperty(VehicleFader.BaseColorId))
									{
										num = VehicleFader.BaseColorId;
									}
									else if (material2.HasProperty(VehicleFader.LegacyColor))
									{
										num = VehicleFader.LegacyColor;
									}
								}
								array2[j] = ((num != 0 && material2.HasProperty(num)) ? material2.GetColor(num) : Color.white);
							}
						}
						renderer.materials = array;
						this._slots.Add(new VehicleFader.FadeSlot
						{
							renderer = renderer,
							originalShared = sharedMaterials,
							clones = array,
							colorProp = num,
							baseColors = array2
						});
					}
				}
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000499C File Offset: 0x00002B9C
		private static void MakeTransparent(Material mat)
		{
			if (mat.HasProperty(VehicleFader.SurfaceId))
			{
				mat.SetFloat(VehicleFader.SurfaceId, 1f);
			}
			if (mat.HasProperty(VehicleFader.SrcBlendId))
			{
				mat.SetFloat(VehicleFader.SrcBlendId, 5f);
			}
			if (mat.HasProperty(VehicleFader.DstBlendId))
			{
				mat.SetFloat(VehicleFader.DstBlendId, 10f);
			}
			if (mat.HasProperty(VehicleFader.ZWriteId))
			{
				mat.SetFloat(VehicleFader.ZWriteId, 0f);
			}
			mat.SetOverrideTag("RenderType", "Transparent");
			mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
			mat.DisableKeyword("_ALPHATEST_ON");
			mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			mat.renderQueue = 3000;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004A5C File Offset: 0x00002C5C
		public void RestoreImmediate()
		{
			this._fading = false;
			this._fadeTimer = 0f;
			this._onComplete = null;
			for (int i = 0; i < this._slots.Count; i++)
			{
				VehicleFader.FadeSlot fadeSlot = this._slots[i];
				if (fadeSlot.renderer != null && fadeSlot.originalShared != null)
				{
					fadeSlot.renderer.sharedMaterials = fadeSlot.originalShared;
				}
				if (fadeSlot.clones != null)
				{
					for (int j = 0; j < fadeSlot.clones.Length; j++)
					{
						if (fadeSlot.clones[j] != null)
						{
							Object.Destroy(fadeSlot.clones[j]);
						}
					}
				}
			}
			this._slots.Clear();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004B0F File Offset: 0x00002D0F
		private void OnDisable()
		{
			if (this._slots.Count > 0)
			{
				this.RestoreImmediate();
			}
		}

		// Token: 0x04000053 RID: 83
		private static readonly int SurfaceId = Shader.PropertyToID("_Surface");

		// Token: 0x04000054 RID: 84
		private static readonly int SrcBlendId = Shader.PropertyToID("_SrcBlend");

		// Token: 0x04000055 RID: 85
		private static readonly int DstBlendId = Shader.PropertyToID("_DstBlend");

		// Token: 0x04000056 RID: 86
		private static readonly int ZWriteId = Shader.PropertyToID("_ZWrite");

		// Token: 0x04000057 RID: 87
		private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

		// Token: 0x04000058 RID: 88
		private static readonly int LegacyColor = Shader.PropertyToID("_Color");

		// Token: 0x04000059 RID: 89
		private const int RenderQueueTransparent = 3000;

		// Token: 0x0400005A RID: 90
		private readonly List<VehicleFader.FadeSlot> _slots = new List<VehicleFader.FadeSlot>(4);

		// Token: 0x0400005B RID: 91
		private bool _fading;

		// Token: 0x0400005C RID: 92
		private float _fadeTimer;

		// Token: 0x0400005D RID: 93
		private float _fadeDuration = 0.7f;

		// Token: 0x0400005E RID: 94
		private Action _onComplete;

		// Token: 0x0200001B RID: 27
		private struct FadeSlot
		{
			// Token: 0x040000DC RID: 220
			public Renderer renderer;

			// Token: 0x040000DD RID: 221
			public Material[] originalShared;

			// Token: 0x040000DE RID: 222
			public Material[] clones;

			// Token: 0x040000DF RID: 223
			public int colorProp;

			// Token: 0x040000E0 RID: 224
			public Color[] baseColors;
		}
	}
}
