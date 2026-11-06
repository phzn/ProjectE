using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x0200029D RID: 669
	public class MutableGraphTransform : GraphTransform
	{
		// Token: 0x060010AF RID: 4271 RVA: 0x00064BF4 File Offset: 0x00062DF4
		public MutableGraphTransform(Matrix4x4 matrix)
			: base(matrix)
		{
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x00064BFD File Offset: 0x00062DFD
		public void SetMatrix(Matrix4x4 matrix)
		{
			base.Set(matrix);
		}
	}
}
