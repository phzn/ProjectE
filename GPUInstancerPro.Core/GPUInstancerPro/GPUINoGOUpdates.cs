using System;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancerPro
{
	// Token: 0x02000046 RID: 70
	[ExecuteInEditMode]
	public class GPUINoGOUpdates : MonoBehaviour
	{
		// Token: 0x0600027E RID: 638 RVA: 0x0000DAC8 File Offset: 0x0000BCC8
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
					this._matrix4X4s = new NativeArray<Matrix4x4>(this._instanceCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
					if (this.enableColorVariations)
					{
						this._colors = new NativeArray<Color>(this._instanceCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
					}
					this.GenerateInstanceMatrices();
					this._jobHandle.Complete();
					GPUICoreAPI.SetTransformBufferData(this._rendererKey, this._matrix4X4s, 0, 0, 0, true);
					if (this.material != null)
					{
						if (this.enableColorVariations)
						{
							this._colorBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, this._instanceCount, Marshal.SizeOf(typeof(Color)));
							this._colorBuffer.SetData<Color>(this._colors);
							this.material.EnableKeyword("GPUI_COLOR_VARIATION");
							GPUICoreAPI.AddMaterialPropertyOverride(this._rendererKey, "gpuiProFloat4Variation", this._colorBuffer, -1, -1);
						}
						else
						{
							this.material.DisableKeyword("GPUI_COLOR_VARIATION");
						}
					}
				}
			}
			if (this.instanceCountText != null)
			{
				this.instanceCountText.text = this._instanceCount.FormatNumberWithSuffix();
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000DC30 File Offset: 0x0000BE30
		public void OnDisable()
		{
			this.Dispose();
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000DC38 File Offset: 0x0000BE38
		private void Update()
		{
			if (this._rendererKey != 0 && this.runUpdate && this._jobHandle.IsCompleted)
			{
				this._jobHandle.Complete();
				GPUICoreAPI.SetTransformBufferData(this._rendererKey, this._matrix4X4s, 0, 0, 0, false);
				if (this.enableColorVariations)
				{
					this._colorBuffer.SetData<Color>(this._colors);
				}
				this.GenerateInstanceMatrices();
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000DCA4 File Offset: 0x0000BEA4
		public void Dispose()
		{
			this._jobHandle.Complete();
			if (this._rendererKey != 0)
			{
				GPUICoreAPI.DisposeRenderer(this._rendererKey);
				this._rendererKey = 0;
			}
			if (this._matrix4X4s.IsCreated)
			{
				this._matrix4X4s.Dispose();
			}
			if (this._colors.IsCreated)
			{
				this._colors.Dispose();
			}
			if (this._colorBuffer != null)
			{
				this._colorBuffer.Dispose();
			}
			this._colorBuffer = null;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000DD20 File Offset: 0x0000BF20
		private void OnValidate()
		{
			if (GPUIRenderingSystem.IsActive && this._rendererKey != 0)
			{
				this.OnEnable();
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000DD38 File Offset: 0x0000BF38
		private void GenerateInstanceMatrices()
		{
			this._jobHandle = new GPUINoGOUpdates.MatrixDataGeneratorJob
			{
				matrices = this._matrix4X4s,
				time = Time.time,
				radial = this.radial,
				vertical = this.vertical,
				circular = this.circular,
				spinSpeed = this.spinSpeed,
				posOffset = base.transform.position
			}.Schedule(this.radial, 2, default(JobHandle));
			if (this.enableColorVariations)
			{
				this._jobHandle = new GPUINoGOUpdates.ColorDataGeneratorJob
				{
					colors = this._colors,
					time = Time.time,
					radial = this.radial,
					vertical = this.vertical,
					circular = this.circular,
					colorSpeeds = this.colorSpeeds
				}.Schedule(this.radial, 2, this._jobHandle);
			}
		}

		// Token: 0x040001AC RID: 428
		public GameObject prefab;

		// Token: 0x040001AD RID: 429
		public GPUIProfile profile;

		// Token: 0x040001AE RID: 430
		[Range(1f, 100f)]
		public int radial = 32;

		// Token: 0x040001AF RID: 431
		[Range(1f, 100f)]
		public int vertical = 32;

		// Token: 0x040001B0 RID: 432
		[Range(1f, 100f)]
		public int circular = 32;

		// Token: 0x040001B1 RID: 433
		public Material material;

		// Token: 0x040001B2 RID: 434
		public bool enableColorVariations;

		// Token: 0x040001B3 RID: 435
		public bool runUpdate;

		// Token: 0x040001B4 RID: 436
		public Vector2 spinSpeed = Vector2.one;

		// Token: 0x040001B5 RID: 437
		public Vector3 colorSpeeds = Vector3.one;

		// Token: 0x040001B6 RID: 438
		public Text instanceCountText;

		// Token: 0x040001B7 RID: 439
		private int _rendererKey;

		// Token: 0x040001B8 RID: 440
		private NativeArray<Matrix4x4> _matrix4X4s;

		// Token: 0x040001B9 RID: 441
		private NativeArray<Color> _colors;

		// Token: 0x040001BA RID: 442
		private GraphicsBuffer _colorBuffer;

		// Token: 0x040001BB RID: 443
		private int _instanceCount;

		// Token: 0x040001BC RID: 444
		private JobHandle _jobHandle;

		// Token: 0x02000047 RID: 71
		[BurstCompile]
		private struct MatrixDataGeneratorJob : IJobParallelFor
		{
			// Token: 0x06000285 RID: 645 RVA: 0x0000DE74 File Offset: 0x0000C074
			public void Execute(int r)
			{
				Matrix4x4 identity = Matrix4x4.identity;
				Vector3 one = Vector3.one;
				int num = r * this.vertical * this.circular;
				float num2 = (float)r * 0.001f * math.sin(this.spinSpeed.y * this.time);
				for (int i = 0; i < this.vertical; i++)
				{
					float num3 = 5f + (float)r * (Mathf.Pow((float)i * 0.02f, 1.6f) + 1f);
					for (int j = 0; j < this.circular; j++)
					{
						float num4 = (float)i * num2 + 6.2831855f * (float)j / (float)this.circular;
						one.x = num3 * math.cos(num4 - this.time * this.spinSpeed.x) + this.posOffset.x;
						one.y = (float)i + this.posOffset.y;
						one.z = num3 * math.sin(num4 - this.time * this.spinSpeed.x) + this.posOffset.z;
						(ref identity).SetPosition(one);
						this.matrices[num] = identity;
						num++;
					}
				}
			}

			// Token: 0x040001BD RID: 445
			[NativeDisableParallelForRestriction]
			public NativeArray<Matrix4x4> matrices;

			// Token: 0x040001BE RID: 446
			[ReadOnly]
			public float time;

			// Token: 0x040001BF RID: 447
			[ReadOnly]
			public int radial;

			// Token: 0x040001C0 RID: 448
			[ReadOnly]
			public int vertical;

			// Token: 0x040001C1 RID: 449
			[ReadOnly]
			public int circular;

			// Token: 0x040001C2 RID: 450
			[ReadOnly]
			public Vector2 spinSpeed;

			// Token: 0x040001C3 RID: 451
			[ReadOnly]
			public Vector3 posOffset;
		}

		// Token: 0x02000048 RID: 72
		[BurstCompile]
		private struct ColorDataGeneratorJob : IJobParallelFor
		{
			// Token: 0x06000286 RID: 646 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
			public void Execute(int r)
			{
				Color white = Color.white;
				int num = r * this.vertical * this.circular;
				white.r = ((float)(r / this.radial) + this.time * this.colorSpeeds.x) % 1f;
				for (float num2 = 0f; num2 < (float)this.vertical; num2 += 1f)
				{
					white.g = (num2 / (float)this.vertical + this.time * this.colorSpeeds.y) % 1f;
					for (float num3 = 0f; num3 < (float)this.circular; num3 += 1f)
					{
						white.b = (num3 / (float)this.circular - this.time * this.colorSpeeds.z) % 1f;
						this.colors[num] = white;
						num++;
					}
				}
			}

			// Token: 0x040001C4 RID: 452
			[NativeDisableParallelForRestriction]
			public NativeArray<Color> colors;

			// Token: 0x040001C5 RID: 453
			[ReadOnly]
			public float time;

			// Token: 0x040001C6 RID: 454
			[ReadOnly]
			public int radial;

			// Token: 0x040001C7 RID: 455
			[ReadOnly]
			public int vertical;

			// Token: 0x040001C8 RID: 456
			[ReadOnly]
			public int circular;

			// Token: 0x040001C9 RID: 457
			[ReadOnly]
			public Vector3 colorSpeeds;
		}
	}
}
