using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancerPro
{
	// Token: 0x02000049 RID: 73
	[ExecuteInEditMode]
	public class GPUINoGOUpdatesCompute : MonoBehaviour
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000E0AC File Offset: 0x0000C2AC
		public void OnEnable()
		{
			this.Dispose();
			if (this.prefab == null)
			{
				return;
			}
			this._instanceCount = this.radial * this.vertical * this.circular;
			if (this._instanceCount > 0)
			{
				GPUICoreAPI.RegisterRenderer(this, this.prefab, this.profile, out this._rendererKey);
				if (this._rendererKey != 0)
				{
					GPUICoreAPI.SetBufferSize(this._rendererKey, this._instanceCount);
					GPUICoreAPI.SetInstanceCount(this._rendererKey, this._instanceCount);
					this._colorBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, this._instanceCount, Marshal.SizeOf(typeof(Color)));
					this.material.EnableKeyword("GPUI_COLOR_VARIATION");
					GPUICoreAPI.AddMaterialPropertyOverride(this._rendererKey, GPUINoGOUpdatesCompute.PROP_gpuiProFloat4Variation, this._colorBuffer, -1, -1);
				}
			}
			if (this.instanceCountText != null)
			{
				this.instanceCountText.text = this._instanceCount.FormatNumberWithSuffix();
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000E1A7 File Offset: 0x0000C3A7
		public void OnDisable()
		{
			this.Dispose();
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000E1B0 File Offset: 0x0000C3B0
		private void Update()
		{
			if (this._rendererKey == 0 || this._instanceCount == 0 || this.computeShader == null)
			{
				return;
			}
			GPUITransformBufferData gpuitransformBufferData;
			int num;
			int num2;
			if (GPUICoreAPI.TryGetTransformBufferData(this._rendererKey, out gpuitransformBufferData, out num, out num2, false))
			{
				this.computeShader.SetBuffer(0, GPUIConstants.PROP_gpuiTransformBuffer, gpuitransformBufferData.GetTransformBuffer().Buffer);
				this.computeShader.SetBuffer(0, GPUINoGOUpdatesCompute.PROP_gpuiProFloat4Variation, this._colorBuffer);
				this.computeShader.SetInt(GPUINoGOUpdatesCompute.PROP_startIndex, num);
				this.computeShader.SetInt(GPUINoGOUpdatesCompute.PROP_radial, this.radial);
				this.computeShader.SetInt(GPUINoGOUpdatesCompute.PROP_vertical, this.vertical);
				this.computeShader.SetInt(GPUINoGOUpdatesCompute.PROP_circular, this.circular);
				this.computeShader.SetFloat(GPUINoGOUpdatesCompute.PROP_time, Time.time);
				this.computeShader.SetVector(GPUINoGOUpdatesCompute.PROP_spinSpeed, this.spinSpeed);
				this.computeShader.SetVector(GPUINoGOUpdatesCompute.PROP_colorSpeeds, this.colorSpeeds);
				this.computeShader.DispatchXYZ(0, this.radial, this.vertical, this.circular);
				gpuitransformBufferData.OnTransformDataModified();
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000E2E8 File Offset: 0x0000C4E8
		public void Dispose()
		{
			if (this._rendererKey != 0)
			{
				GPUICoreAPI.DisposeRenderer(this._rendererKey);
				this._rendererKey = 0;
			}
			if (this._colorBuffer != null)
			{
				this._colorBuffer.Dispose();
			}
			this._colorBuffer = null;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000E31E File Offset: 0x0000C51E
		private void OnValidate()
		{
			if (GPUIRenderingSystem.IsActive && this._rendererKey != 0)
			{
				this.OnEnable();
			}
		}

		// Token: 0x040001CA RID: 458
		public GameObject prefab;

		// Token: 0x040001CB RID: 459
		public GPUIProfile profile;

		// Token: 0x040001CC RID: 460
		[Range(1f, 100f)]
		public int radial = 32;

		// Token: 0x040001CD RID: 461
		[Range(1f, 100f)]
		public int vertical = 32;

		// Token: 0x040001CE RID: 462
		[Range(1f, 100f)]
		public int circular = 32;

		// Token: 0x040001CF RID: 463
		public Material material;

		// Token: 0x040001D0 RID: 464
		public Vector2 spinSpeed = Vector2.one;

		// Token: 0x040001D1 RID: 465
		public Vector3 colorSpeeds = Vector3.one;

		// Token: 0x040001D2 RID: 466
		public Text instanceCountText;

		// Token: 0x040001D3 RID: 467
		public ComputeShader computeShader;

		// Token: 0x040001D4 RID: 468
		private int _rendererKey;

		// Token: 0x040001D5 RID: 469
		private GraphicsBuffer _colorBuffer;

		// Token: 0x040001D6 RID: 470
		private int _instanceCount;

		// Token: 0x040001D7 RID: 471
		private static readonly int PROP_gpuiProFloat4Variation = Shader.PropertyToID("gpuiProFloat4Variation");

		// Token: 0x040001D8 RID: 472
		private static readonly int PROP_startIndex = Shader.PropertyToID("startIndex");

		// Token: 0x040001D9 RID: 473
		private static readonly int PROP_radial = Shader.PropertyToID("radial");

		// Token: 0x040001DA RID: 474
		private static readonly int PROP_vertical = Shader.PropertyToID("vertical");

		// Token: 0x040001DB RID: 475
		private static readonly int PROP_circular = Shader.PropertyToID("circular");

		// Token: 0x040001DC RID: 476
		private static readonly int PROP_time = Shader.PropertyToID("time");

		// Token: 0x040001DD RID: 477
		private static readonly int PROP_spinSpeed = Shader.PropertyToID("spinSpeed");

		// Token: 0x040001DE RID: 478
		private static readonly int PROP_colorSpeeds = Shader.PropertyToID("colorSpeeds");
	}
}
