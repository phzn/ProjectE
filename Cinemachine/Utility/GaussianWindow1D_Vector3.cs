using System;
using UnityEngine;

namespace Cinemachine.Utility
{
	// Token: 0x02000061 RID: 97
	internal class GaussianWindow1D_Vector3 : GaussianWindow1d<Vector3>
	{
		// Token: 0x060003CF RID: 975 RVA: 0x00017271 File Offset: 0x00015471
		public GaussianWindow1D_Vector3(float sigma, int maxKernelRadius = 10)
			: base(sigma, maxKernelRadius)
		{
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001727C File Offset: 0x0001547C
		protected override Vector3 Compute(int windowPos)
		{
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < base.KernelSize; i++)
			{
				vector += this.mData[windowPos] * this.mKernel[i];
				if (++windowPos == base.KernelSize)
				{
					windowPos = 0;
				}
			}
			return vector;
		}
	}
}
