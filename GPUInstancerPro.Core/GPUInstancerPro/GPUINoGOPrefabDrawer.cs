using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancerPro
{
	// Token: 0x02000045 RID: 69
	[ExecuteInEditMode]
	public class GPUINoGOPrefabDrawer : MonoBehaviour
	{
		// Token: 0x06000275 RID: 629 RVA: 0x0000D7F2 File Offset: 0x0000B9F2
		public void OnEnable()
		{
			this.RegisterRenderers();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000D7FA File Offset: 0x0000B9FA
		public void OnDisable()
		{
			this.DisposeRenderers();
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000D802 File Offset: 0x0000BA02
		private void OnValidate()
		{
			if (GPUIRenderingSystem.IsActive && this._isInitialized)
			{
				this.RegisterRenderers();
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000D81C File Offset: 0x0000BA1C
		private void RegisterRenderers()
		{
			this.DisposeRenderers();
			this._isInitialized = true;
			if (this.instanceCount <= 0 || this.prefabObject == null)
			{
				return;
			}
			GPUICoreAPI.RegisterRenderer(this, this.prefabObject, (this.profile == null) ? GPUIProfile.DefaultProfile : this.profile, out this._rendererKey);
			GPUICoreAPI.SetTransformBufferData(this._rendererKey, this.GenerateMatrixArray(), 0, 0, 0, true);
			Renderer[] componentsInChildren = this.prefabObject.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				foreach (Material material in componentsInChildren[i].sharedMaterials)
				{
					if (!(material == null))
					{
						if (this.enableColorVariations)
						{
							material.EnableKeyword(this.variationKeyword);
							GPUICoreAPI.AddMaterialPropertyOverride(this._rendererKey, this.variationBufferName, this.GenerateColorBuffer(), -1, -1);
						}
						else
						{
							material.DisableKeyword(this.variationKeyword);
						}
					}
				}
			}
			if (this.instanceCountText != null)
			{
				this.instanceCountText.text = this.instanceCount.FormatNumberWithSuffix();
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000D933 File Offset: 0x0000BB33
		private void DisposeRenderers()
		{
			this._isInitialized = false;
			if (this._rendererKey != 0)
			{
				GPUICoreAPI.DisposeRenderer(this._rendererKey);
				this._rendererKey = 0;
			}
			if (this._colorBuffer != null)
			{
				this._colorBuffer.Dispose();
				this._colorBuffer = null;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000D970 File Offset: 0x0000BB70
		private Matrix4x4[] GenerateMatrixArray()
		{
			Matrix4x4[] array = new Matrix4x4[this.instanceCount];
			Matrix4x4 identity = Matrix4x4.identity;
			int num = Mathf.FloorToInt(Mathf.Pow((float)this.instanceCount, 0.33333334f));
			int num2 = num * num;
			Vector3 position = base.transform.position;
			for (int i = 0; i < this.instanceCount; i++)
			{
				(ref identity).SetPosition(new Vector3((float)(i % num), (float)(i / num2), (float)(i / num % num)) * this.spacing + position);
				array[i] = identity;
			}
			return array;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000DA08 File Offset: 0x0000BC08
		private GraphicsBuffer GenerateColorBuffer()
		{
			if (this._colorBuffer != null)
			{
				this._colorBuffer.Dispose();
			}
			Color[] array = new Color[this.instanceCount];
			Random.InitState(42);
			for (int i = 0; i < this.instanceCount; i++)
			{
				array[i] = Random.ColorHSV();
			}
			this._colorBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, this.instanceCount, Marshal.SizeOf(typeof(Color)));
			this._colorBuffer.SetData(array);
			return this._colorBuffer;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000DA8C File Offset: 0x0000BC8C
		public int GetRendererKey()
		{
			return this._rendererKey;
		}

		// Token: 0x040001A1 RID: 417
		public GameObject prefabObject;

		// Token: 0x040001A2 RID: 418
		public GPUIProfile profile;

		// Token: 0x040001A3 RID: 419
		public int instanceCount = 1000;

		// Token: 0x040001A4 RID: 420
		public float spacing = 1.5f;

		// Token: 0x040001A5 RID: 421
		public bool enableColorVariations;

		// Token: 0x040001A6 RID: 422
		public string variationKeyword = "GPUI_COLOR_VARIATION";

		// Token: 0x040001A7 RID: 423
		public string variationBufferName = "gpuiProFloat4Variation";

		// Token: 0x040001A8 RID: 424
		public Text instanceCountText;

		// Token: 0x040001A9 RID: 425
		private int _rendererKey;

		// Token: 0x040001AA RID: 426
		private GraphicsBuffer _colorBuffer;

		// Token: 0x040001AB RID: 427
		private bool _isInitialized;
	}
}
