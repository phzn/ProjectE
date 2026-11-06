using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200077A RID: 1914
	public class CurveSizeAttribute : PropertyAttribute
	{
		// Token: 0x06003222 RID: 12834 RVA: 0x0011C7D5 File Offset: 0x0011A9D5
		public CurveSizeAttribute(float height = 60f)
		{
			this.height = height;
		}

		// Token: 0x040032EA RID: 13034
		public readonly float height;
	}
}
