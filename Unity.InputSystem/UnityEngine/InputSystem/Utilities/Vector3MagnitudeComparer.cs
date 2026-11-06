using System;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200012E RID: 302
	public struct Vector3MagnitudeComparer : IComparer<Vector3>
	{
		// Token: 0x060010FE RID: 4350 RVA: 0x00051504 File Offset: 0x0004F704
		public int Compare(Vector3 x, Vector3 y)
		{
			float sqrMagnitude = x.sqrMagnitude;
			float sqrMagnitude2 = y.sqrMagnitude;
			if (sqrMagnitude < sqrMagnitude2)
			{
				return -1;
			}
			if (sqrMagnitude > sqrMagnitude2)
			{
				return 1;
			}
			return 0;
		}
	}
}
