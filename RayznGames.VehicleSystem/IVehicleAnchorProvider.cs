using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200000F RID: 15
	public interface IVehicleAnchorProvider
	{
		// Token: 0x0600009C RID: 156
		Transform GetAnchor(VehicleAnchorType type);

		// Token: 0x0600009D RID: 157
		bool HasAnchor(VehicleAnchorType type);

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600009E RID: 158
		float CurrentSpeed { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600009F RID: 159
		Transform VehicleTransform { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A0 RID: 160
		bool UsesInvertedHierarchy { get; }
	}
}
