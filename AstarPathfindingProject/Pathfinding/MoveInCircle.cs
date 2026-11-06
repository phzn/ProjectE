using System;
using Pathfinding.Drawing;
using Unity.Entities;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000012 RID: 18
	[UniqueComponent(tag = "ai.destination")]
	[AddComponentMenu("Pathfinding/AI/Behaviors/MoveInCircle")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/moveincircle.html")]
	public class MoveInCircle : VersionedMonoBehaviour
	{
		// Token: 0x06000089 RID: 137 RVA: 0x000047E3 File Offset: 0x000029E3
		private void OnEnable()
		{
			this.ai = base.GetComponent<IAstarAI>();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000047F4 File Offset: 0x000029F4
		public static Vector3 CalculateDestination(Vector3 position, Vector3 target, Vector3 targetUp, float radius, float offset)
		{
			Vector3 normalized = (position - target).normalized;
			Vector3 vector = Vector3.Cross(normalized, targetUp);
			return target + normalized * radius + vector * offset;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004833 File Offset: 0x00002A33
		private void Update()
		{
			this.ai.destination = MoveInCircle.CalculateDestination(this.ai.position, this.target.position, this.target.up, this.radius, this.offset);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004872 File Offset: 0x00002A72
		public override void DrawGizmos()
		{
			if (this.target)
			{
				Draw.Circle(this.target.position, this.target.up, this.radius, Color.white);
			}
		}

		// Token: 0x04000073 RID: 115
		public Transform target;

		// Token: 0x04000074 RID: 116
		public float radius = 5f;

		// Token: 0x04000075 RID: 117
		public float offset = 2f;

		// Token: 0x04000076 RID: 118
		private IAstarAI ai;

		// Token: 0x02000013 RID: 19
		public struct DestinationMoveInCircle : IComponentData, IQueryTypeParameter, IEnableableComponent
		{
			// Token: 0x04000077 RID: 119
			public Entity target;

			// Token: 0x04000078 RID: 120
			public float radius;

			// Token: 0x04000079 RID: 121
			public float offset;
		}
	}
}
