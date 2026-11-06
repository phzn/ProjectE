using System;
using UnityEngine;

namespace Cinemachine.Utility
{
	// Token: 0x02000062 RID: 98
	internal class GaussianWindow1D_Quaternion : GaussianWindow1d<Quaternion>
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x000172D2 File Offset: 0x000154D2
		public GaussianWindow1D_Quaternion(float sigma, int maxKernelRadius = 10)
			: base(sigma, maxKernelRadius)
		{
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000172DC File Offset: 0x000154DC
		protected override Quaternion Compute(int windowPos)
		{
			Quaternion quaternion = new Quaternion(0f, 0f, 0f, 0f);
			Quaternion quaternion2 = this.mData[this.mCurrentPos];
			Quaternion quaternion3 = Quaternion.Inverse(quaternion2);
			for (int i = 0; i < base.KernelSize; i++)
			{
				float num = this.mKernel[i];
				Quaternion quaternion4 = quaternion3 * this.mData[windowPos];
				if (Quaternion.Dot(Quaternion.identity, quaternion4) < 0f)
				{
					num = -num;
				}
				quaternion.x += quaternion4.x * num;
				quaternion.y += quaternion4.y * num;
				quaternion.z += quaternion4.z * num;
				quaternion.w += quaternion4.w * num;
				if (++windowPos == base.KernelSize)
				{
					windowPos = 0;
				}
			}
			return quaternion2 * Quaternion.Normalize(quaternion);
		}
	}
}
