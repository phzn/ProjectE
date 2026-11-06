using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x020000E1 RID: 225
	[Serializable]
	public class TextureCurve : IDisposable
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x00024C1E File Offset: 0x00022E1E
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x00024C26 File Offset: 0x00022E26
		public int length { get; private set; }

		// Token: 0x17000120 RID: 288
		public Keyframe this[int index]
		{
			get
			{
				return this.m_Curve[index];
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00024C3D File Offset: 0x00022E3D
		public TextureCurve(AnimationCurve baseCurve, float zeroValue, bool loop, in Vector2 bounds)
			: this(baseCurve.keys, zeroValue, loop, in bounds)
		{
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00024C50 File Offset: 0x00022E50
		public TextureCurve(Keyframe[] keys, float zeroValue, bool loop, in Vector2 bounds)
		{
			this.m_Curve = new AnimationCurve(keys);
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			Vector2 vector = bounds;
			this.m_Range = vector.magnitude;
			this.length = keys.Length;
			this.SetDirty();
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00024CA4 File Offset: 0x00022EA4
		~TextureCurve()
		{
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00024CCC File Offset: 0x00022ECC
		[Obsolete("Please use Release() instead.")]
		public void Dispose()
		{
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00024CCE File Offset: 0x00022ECE
		public void Release()
		{
			CoreUtils.Destroy(this.m_Texture);
			this.m_Texture = null;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00024CE2 File Offset: 0x00022EE2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDirty()
		{
			this.m_IsCurveDirty = true;
			this.m_IsTextureDirty = true;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00024CF2 File Offset: 0x00022EF2
		private static GraphicsFormat GetTextureFormat()
		{
			if (SystemInfo.IsFormatSupported(GraphicsFormat.R16_SFloat, FormatUsage.SetPixels))
			{
				return GraphicsFormat.R16_SFloat;
			}
			if (SystemInfo.IsFormatSupported(GraphicsFormat.R8_UNorm, FormatUsage.SetPixels))
			{
				return GraphicsFormat.R8_UNorm;
			}
			return GraphicsFormat.R8G8B8A8_UNorm;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00024D10 File Offset: 0x00022F10
		public Texture2D GetTexture()
		{
			if (this.m_Texture == null)
			{
				this.m_Texture = new Texture2D(128, 1, TextureCurve.GetTextureFormat(), TextureCreationFlags.None);
				this.m_Texture.name = "CurveTexture";
				this.m_Texture.hideFlags = HideFlags.HideAndDontSave;
				this.m_Texture.filterMode = FilterMode.Bilinear;
				this.m_Texture.wrapMode = TextureWrapMode.Clamp;
				this.m_Texture.anisoLevel = 0;
				this.m_IsTextureDirty = true;
			}
			if (this.m_IsTextureDirty)
			{
				Color[] array = new Color[128];
				for (int i = 0; i < array.Length; i++)
				{
					array[i].r = this.Evaluate((float)i * 0.0078125f);
				}
				this.m_Texture.SetPixels(array);
				this.m_Texture.Apply(false, false);
				this.m_IsTextureDirty = false;
			}
			return this.m_Texture;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00024DEC File Offset: 0x00022FEC
		public float Evaluate(float time)
		{
			if (this.m_IsCurveDirty)
			{
				this.length = this.m_Curve.length;
			}
			if (this.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.length == 1)
			{
				return this.m_Curve.Evaluate(time);
			}
			if (this.m_IsCurveDirty)
			{
				if (this.m_LoopingCurve == null)
				{
					this.m_LoopingCurve = new AnimationCurve();
				}
				Keyframe keyframe = this.m_Curve[this.length - 1];
				keyframe.time -= this.m_Range;
				Keyframe keyframe2 = this.m_Curve[0];
				keyframe2.time += this.m_Range;
				this.m_LoopingCurve.keys = this.m_Curve.keys;
				this.m_LoopingCurve.AddKey(keyframe);
				this.m_LoopingCurve.AddKey(keyframe2);
				this.m_IsCurveDirty = false;
			}
			return this.m_LoopingCurve.Evaluate(time);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00024EE9 File Offset: 0x000230E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int AddKey(float time, float value)
		{
			int num = this.m_Curve.AddKey(time, value);
			if (num > -1)
			{
				this.SetDirty();
			}
			return num;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00024F02 File Offset: 0x00023102
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int MoveKey(int index, in Keyframe key)
		{
			int num = this.m_Curve.MoveKey(index, key);
			this.SetDirty();
			return num;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00024F1C File Offset: 0x0002311C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RemoveKey(int index)
		{
			this.m_Curve.RemoveKey(index);
			this.SetDirty();
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00024F30 File Offset: 0x00023130
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SmoothTangents(int index, float weight)
		{
			this.m_Curve.SmoothTangents(index, weight);
			this.SetDirty();
		}

		// Token: 0x040004AD RID: 1197
		private const int k_Precision = 128;

		// Token: 0x040004AE RID: 1198
		private const float k_Step = 0.0078125f;

		// Token: 0x040004B0 RID: 1200
		[SerializeField]
		private bool m_Loop;

		// Token: 0x040004B1 RID: 1201
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x040004B2 RID: 1202
		[SerializeField]
		private float m_Range;

		// Token: 0x040004B3 RID: 1203
		[SerializeField]
		private AnimationCurve m_Curve;

		// Token: 0x040004B4 RID: 1204
		private AnimationCurve m_LoopingCurve;

		// Token: 0x040004B5 RID: 1205
		private Texture2D m_Texture;

		// Token: 0x040004B6 RID: 1206
		private bool m_IsCurveDirty;

		// Token: 0x040004B7 RID: 1207
		private bool m_IsTextureDirty;
	}
}
