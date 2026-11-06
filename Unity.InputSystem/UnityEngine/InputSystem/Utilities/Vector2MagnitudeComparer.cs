using System;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200012D RID: 301
	public struct Vector2MagnitudeComparer : IComparer<Vector2>
	{
		// Token: 0x060010FD RID: 4349 RVA: 0x000514D8 File Offset: 0x0004F6D8
		public int Compare(Vector2 x, Vector2 y)
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
