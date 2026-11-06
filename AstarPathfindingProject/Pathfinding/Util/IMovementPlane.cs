using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000296 RID: 662
	public interface IMovementPlane
	{
		// Token: 0x06001083 RID: 4227
		Vector2 ToPlane(Vector3 p);

		// Token: 0x06001084 RID: 4228
		Vector2 ToPlane(Vector3 p, out float elevation);

		// Token: 0x06001085 RID: 4229
		Vector3 ToWorld(Vector2 p, float elevation = 0f);

		// Token: 0x06001086 RID: 4230
		SimpleMovementPlane ToSimpleMovementPlane();
	}
}
