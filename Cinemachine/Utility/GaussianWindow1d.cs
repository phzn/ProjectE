using System;
using UnityEngine;

namespace Cinemachine.Utility
{
	// Token: 0x02000060 RID: 96
	internal abstract class GaussianWindow1d<T>
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00017087 File Offset: 0x00015287
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0001708F File Offset: 0x0001528F
		public float Sigma { get; private set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00017098 File Offset: 0x00015298
		public int KernelSize
		{
			get
			{
				return this.mKernel.Length;
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000170A4 File Offset: 0x000152A4
		private void GenerateKernel(float sigma, int maxKernelRadius)
		{
			int num = Math.Min(maxKernelRadius, Mathf.FloorToInt(Mathf.Abs(sigma) * 2.5f));
			this.mKernel = new float[2 * num + 1];
			if (num == 0)
			{
				this.mKernel[0] = 1f;
			}
			else
			{
				float num2 = 0f;
				for (int i = -num; i <= num; i++)
				{
					this.mKernel[i + num] = (float)(Math.Exp((double)((float)(-(float)(i * i)) / (2f * sigma * sigma))) / (6.283185307179586 * (double)sigma * (double)sigma));
					num2 += this.mKernel[i + num];
				}
				for (int j = -num; j <= num; j++)
				{
					this.mKernel[j + num] /= num2;
				}
			}
			this.Sigma = sigma;
		}

		// Token: 0x060003C5 RID: 965
		protected abstract T Compute(int windowPos);

		// Token: 0x060003C6 RID: 966 RVA: 0x00017162 File Offset: 0x00015362
		public GaussianWindow1d(float sigma, int maxKernelRadius = 10)
		{
			this.GenerateKernel(sigma, maxKernelRadius);
			this.mData = new T[this.KernelSize];
			this.mCurrentPos = -1;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00017191 File Offset: 0x00015391
		public void Reset()
		{
			this.mCurrentPos = -1;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001719A File Offset: 0x0001539A
		public bool IsEmpty()
		{
			return this.mCurrentPos < 0;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000171A8 File Offset: 0x000153A8
		public void AddValue(T v)
		{
			if (this.mCurrentPos < 0)
			{
				for (int i = 0; i < this.KernelSize; i++)
				{
					this.mData[i] = v;
				}
				this.mCurrentPos = Mathf.Min(1, this.KernelSize - 1);
			}
			this.mData[this.mCurrentPos] = v;
			int num = this.mCurrentPos + 1;
			this.mCurrentPos = num;
			if (num == this.KernelSize)
			{
				this.mCurrentPos = 0;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00017222 File Offset: 0x00015422
		public T Filter(T v)
		{
			if (this.KernelSize < 3)
			{
				return v;
			}
			this.AddValue(v);
			return this.Value();
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001723C File Offset: 0x0001543C
		public T Value()
		{
			return this.Compute(this.mCurrentPos);
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0001724A File Offset: 0x0001544A
		public int BufferLength
		{
			get
			{
				return this.mData.Length;
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00017254 File Offset: 0x00015454
		public void SetBufferValue(int index, T value)
		{
			this.mData[index] = value;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00017263 File Offset: 0x00015463
		public T GetBufferValue(int index)
		{
			return this.mData[index];
		}

		// Token: 0x0400028E RID: 654
		protected T[] mData;

		// Token: 0x0400028F RID: 655
		protected float[] mKernel;

		// Token: 0x04000290 RID: 656
		protected int mCurrentPos = -1;
	}
}
