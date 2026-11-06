using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

namespace DA_Assets.Tools
{
	// Token: 0x02000013 RID: 19
	public class ParallelGaussianBlur
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00002A73 File Offset: 0x00000C73
		public ParallelGaussianBlur(Texture2D tex, float step)
		{
			this._tex2D = tex;
			this._radial = step;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002A9C File Offset: 0x00000C9C
		public void GaussianBlur()
		{
			this._RawTextureData = this._tex2D.GetRawTextureData<Color32>();
			this._width = this._tex2D.width;
			this._height = this._tex2D.height;
			this._width = this._tex2D.width;
			this._height = this._tex2D.height;
			this.m_red = new int[this._width * this._height];
			this.m_green = new int[this._width * this._height];
			this.m_blue = new int[this._width * this._height];
			this.m_alpha = new int[this._width * this._height];
			Parallel.For(0, this._width * this._height, this._pOptions, delegate(int i)
			{
				this.m_red[i] = (int)this._RawTextureData[i].r;
				this.m_green[i] = (int)this._RawTextureData[i].g;
				this.m_blue[i] = (int)this._RawTextureData[i].b;
				this.m_alpha[i] = (int)this._RawTextureData[i].a;
			});
			int[] newAlpha = new int[this._width * this._height];
			int[] newRed = new int[this._width * this._height];
			int[] newGreen = new int[this._width * this._height];
			int[] newBlue = new int[this._width * this._height];
			Parallel.Invoke(new Action[]
			{
				delegate
				{
					this.gaussBlur_4(this.m_alpha, newAlpha, this._radial);
				},
				delegate
				{
					this.gaussBlur_4(this.m_red, newRed, this._radial);
				},
				delegate
				{
					this.gaussBlur_4(this.m_green, newGreen, this._radial);
				},
				delegate
				{
					this.gaussBlur_4(this.m_blue, newBlue, this._radial);
				}
			});
			Parallel.For(0, this._width * this._height, this._pOptions, delegate(int i)
			{
				if (newAlpha[i] > 255)
				{
					newAlpha[i] = 255;
				}
				if (newRed[i] > 255)
				{
					newRed[i] = 255;
				}
				if (newGreen[i] > 255)
				{
					newGreen[i] = 255;
				}
				if (newBlue[i] > 255)
				{
					newBlue[i] = 255;
				}
				if (newAlpha[i] < 0)
				{
					newAlpha[i] = 0;
				}
				if (newRed[i] < 0)
				{
					newRed[i] = 0;
				}
				if (newGreen[i] < 0)
				{
					newGreen[i] = 0;
				}
				if (newBlue[i] < 0)
				{
					newBlue[i] = 0;
				}
				this._RawTextureData[i] = new Color32((byte)newRed[i], (byte)newGreen[i], (byte)newBlue[i], (byte)newAlpha[i]);
			});
			this._tex2D.Apply();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002C6C File Offset: 0x00000E6C
		private int[] boxesForGauss(float sigma, int n)
		{
			int num = (int)Mathf.Floor(Mathf.Sqrt(12f * sigma * sigma / (float)n + 1f));
			if (num % 2 == 0)
			{
				num--;
			}
			int num2 = num + 2;
			float num3 = Mathf.Round((12f * sigma * sigma - (float)(n * num * num) - (float)(4 * n * num) - (float)(3 * n)) / (float)(-4 * num - 4));
			List<int> list = new List<int>();
			for (int i = 0; i < n; i++)
			{
				list.Add(((float)i < num3) ? num : num2);
			}
			return list.ToArray();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002CFC File Offset: 0x00000EFC
		private void gaussBlur_4(int[] colorChannel, int[] destChannel, float r)
		{
			int[] array = this.boxesForGauss(r, 3);
			this.boxBlur_4(colorChannel, destChannel, this._width, this._height, (array[0] - 1) / 2);
			this.boxBlur_4(destChannel, colorChannel, this._width, this._height, (array[1] - 1) / 2);
			this.boxBlur_4(colorChannel, destChannel, this._width, this._height, (array[2] - 1) / 2);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002D64 File Offset: 0x00000F64
		private void boxBlur_4(int[] colorChannel, int[] destChannel, int w, int h, int r)
		{
			for (int i = 0; i < colorChannel.Length; i++)
			{
				destChannel[i] = colorChannel[i];
			}
			this.boxBlurH_4(destChannel, colorChannel, w, h, r);
			this.boxBlurT_4(colorChannel, destChannel, w, h, r);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002DA0 File Offset: 0x00000FA0
		private void boxBlurH_4(int[] colorChannel, int[] dest, int w, int h, int radial)
		{
			float iar = 1f / (float)(radial + radial + 1);
			Parallel.For(0, h, this._pOptions, delegate(int i)
			{
				int num = i * w;
				int num2 = num;
				int num3 = num + radial;
				int num4 = colorChannel[num];
				int num5 = colorChannel[num + w - 1];
				int num6 = (radial + 1) * num4;
				for (int j = 0; j < radial; j++)
				{
					num6 += colorChannel[num + j];
				}
				for (int k = 0; k <= radial; k++)
				{
					num6 += colorChannel[num3++] - num4;
					dest[num++] = (int)Mathf.Round((float)num6 * iar);
				}
				for (int l = radial + 1; l < w - radial; l++)
				{
					num6 += colorChannel[num3++] - dest[num2++];
					dest[num++] = (int)Mathf.Round((float)num6 * iar);
				}
				for (int m = w - radial; m < w; m++)
				{
					num6 += num5 - colorChannel[num2++];
					dest[num++] = (int)Mathf.Round((float)num6 * iar);
				}
			});
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002E08 File Offset: 0x00001008
		private void boxBlurT_4(int[] colorChannel, int[] dest, int w, int h, int r)
		{
			float iar = 1f / (float)(r + r + 1);
			Parallel.For(0, w, this._pOptions, delegate(int i)
			{
				int num = i;
				int num2 = num;
				int num3 = num + r * w;
				int num4 = colorChannel[num];
				int num5 = colorChannel[num + w * (h - 1)];
				int num6 = (r + 1) * num4;
				for (int j = 0; j < r; j++)
				{
					num6 += colorChannel[num + j * w];
				}
				for (int k = 0; k <= r; k++)
				{
					num6 += colorChannel[num3] - num4;
					dest[num] = (int)Mathf.Round((float)num6 * iar);
					num3 += w;
					num += w;
				}
				for (int l = r + 1; l < h - r; l++)
				{
					num6 += colorChannel[num3] - colorChannel[num2];
					dest[num] = (int)Mathf.Round((float)num6 * iar);
					num2 += w;
					num3 += w;
					num += w;
				}
				for (int m = h - r; m < h; m++)
				{
					num6 += num5 - colorChannel[num2];
					dest[num] = (int)Mathf.Round((float)num6 * iar);
					num2 += w;
					num += w;
				}
			});
		}

		// Token: 0x04000024 RID: 36
		private readonly ParallelOptions _pOptions = new ParallelOptions
		{
			MaxDegreeOfParallelism = 8
		};

		// Token: 0x04000025 RID: 37
		private NativeArray<Color32> _RawTextureData;

		// Token: 0x04000026 RID: 38
		private Texture2D _tex2D;

		// Token: 0x04000027 RID: 39
		private float _radial;

		// Token: 0x04000028 RID: 40
		private int _width;

		// Token: 0x04000029 RID: 41
		private int _height;

		// Token: 0x0400002A RID: 42
		private int[] m_red;

		// Token: 0x0400002B RID: 43
		private int[] m_green;

		// Token: 0x0400002C RID: 44
		private int[] m_blue;

		// Token: 0x0400002D RID: 45
		private int[] m_alpha;
	}
}
