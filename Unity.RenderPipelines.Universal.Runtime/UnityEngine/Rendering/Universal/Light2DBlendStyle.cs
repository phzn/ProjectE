using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000026 RID: 38
	[MovedFrom("UnityEngine.Experimental.Rendering.Universal")]
	[Serializable]
	public struct Light2DBlendStyle
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000C378 File Offset: 0x0000A578
		internal Vector2 blendFactors
		{
			get
			{
				Vector2 vector = default(Vector2);
				switch (this.blendMode)
				{
				case Light2DBlendStyle.BlendMode.Additive:
					vector.x = 0f;
					vector.y = 1f;
					break;
				case Light2DBlendStyle.BlendMode.Multiply:
					vector.x = 1f;
					vector.y = 0f;
					break;
				case Light2DBlendStyle.BlendMode.Subtractive:
					vector.x = 0f;
					vector.y = -1f;
					break;
				default:
					vector.x = 1f;
					vector.y = 0f;
					break;
				}
				return vector;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000C410 File Offset: 0x0000A610
		internal Light2DBlendStyle.MaskChannelFilter maskTextureChannelFilter
		{
			get
			{
				switch (this.maskTextureChannel)
				{
				case Light2DBlendStyle.TextureChannel.R:
					return new Light2DBlendStyle.MaskChannelFilter(new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f));
				case Light2DBlendStyle.TextureChannel.G:
					return new Light2DBlendStyle.MaskChannelFilter(new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f));
				case Light2DBlendStyle.TextureChannel.B:
					return new Light2DBlendStyle.MaskChannelFilter(new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 0f));
				case Light2DBlendStyle.TextureChannel.A:
					return new Light2DBlendStyle.MaskChannelFilter(new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 0f));
				case Light2DBlendStyle.TextureChannel.OneMinusR:
					return new Light2DBlendStyle.MaskChannelFilter(new Vector4(1f, 0f, 0f, 0f), new Vector4(1f, 0f, 0f, 0f));
				case Light2DBlendStyle.TextureChannel.OneMinusG:
					return new Light2DBlendStyle.MaskChannelFilter(new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f));
				case Light2DBlendStyle.TextureChannel.OneMinusB:
					return new Light2DBlendStyle.MaskChannelFilter(new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 1f, 0f));
				case Light2DBlendStyle.TextureChannel.OneMinusA:
					return new Light2DBlendStyle.MaskChannelFilter(new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f));
				}
				return new Light2DBlendStyle.MaskChannelFilter(Vector4.zero, Vector4.zero);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0000C622 File Offset: 0x0000A822
		// (set) Token: 0x0600016F RID: 367 RVA: 0x0000C62A File Offset: 0x0000A82A
		internal bool isDirty { readonly get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000C633 File Offset: 0x0000A833
		// (set) Token: 0x06000171 RID: 369 RVA: 0x0000C63B File Offset: 0x0000A83B
		internal bool hasRenderTarget { readonly get; set; }

		// Token: 0x040000D9 RID: 217
		public string name;

		// Token: 0x040000DA RID: 218
		[SerializeField]
		internal Light2DBlendStyle.TextureChannel maskTextureChannel;

		// Token: 0x040000DB RID: 219
		[SerializeField]
		internal Light2DBlendStyle.BlendMode blendMode;

		// Token: 0x040000DE RID: 222
		internal int renderTargetHandleId;

		// Token: 0x040000DF RID: 223
		internal RTHandle renderTargetHandle;

		// Token: 0x02000155 RID: 341
		internal enum TextureChannel
		{
			// Token: 0x0400097D RID: 2429
			None,
			// Token: 0x0400097E RID: 2430
			R,
			// Token: 0x0400097F RID: 2431
			G,
			// Token: 0x04000980 RID: 2432
			B,
			// Token: 0x04000981 RID: 2433
			A,
			// Token: 0x04000982 RID: 2434
			OneMinusR,
			// Token: 0x04000983 RID: 2435
			OneMinusG,
			// Token: 0x04000984 RID: 2436
			OneMinusB,
			// Token: 0x04000985 RID: 2437
			OneMinusA
		}

		// Token: 0x02000156 RID: 342
		internal struct MaskChannelFilter
		{
			// Token: 0x17000244 RID: 580
			// (get) Token: 0x06000A4D RID: 2637 RVA: 0x000470E0 File Offset: 0x000452E0
			// (set) Token: 0x06000A4E RID: 2638 RVA: 0x000470E8 File Offset: 0x000452E8
			public Vector4 mask { readonly get; private set; }

			// Token: 0x17000245 RID: 581
			// (get) Token: 0x06000A4F RID: 2639 RVA: 0x000470F1 File Offset: 0x000452F1
			// (set) Token: 0x06000A50 RID: 2640 RVA: 0x000470F9 File Offset: 0x000452F9
			public Vector4 inverted { readonly get; private set; }

			// Token: 0x06000A51 RID: 2641 RVA: 0x00047102 File Offset: 0x00045302
			public MaskChannelFilter(Vector4 m, Vector4 i)
			{
				this.mask = m;
				this.inverted = i;
			}
		}

		// Token: 0x02000157 RID: 343
		internal enum BlendMode
		{
			// Token: 0x04000989 RID: 2441
			Additive,
			// Token: 0x0400098A RID: 2442
			Multiply,
			// Token: 0x0400098B RID: 2443
			Subtractive
		}

		// Token: 0x02000158 RID: 344
		[Serializable]
		internal struct BlendFactors
		{
			// Token: 0x0400098C RID: 2444
			public float multiplicative;

			// Token: 0x0400098D RID: 2445
			public float additive;
		}
	}
}
