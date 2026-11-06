using System;
using Pathfinding.Drawing;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000071 RID: 113
	[Serializable]
	public class AutoRepathPolicy
	{
		// Token: 0x06000433 RID: 1075 RVA: 0x000144C8 File Offset: 0x000126C8
		public virtual bool ShouldRecalculatePath(Vector3 position, float radius, Vector3 destination, float time)
		{
			if (this.mode == AutoRepathPolicy.Mode.Never || float.IsPositiveInfinity(destination.x))
			{
				return false;
			}
			float num = time - this.lastRepathTime;
			if (this.mode == AutoRepathPolicy.Mode.EveryNSeconds)
			{
				return num >= this.period;
			}
			float num2 = (destination - this.lastDestination).sqrMagnitude / Mathf.Max((position - this.lastDestination).sqrMagnitude, radius * radius) * (this.sensitivity * this.sensitivity);
			if (float.IsNaN(num2))
			{
				num2 = 0f;
			}
			return num >= this.maximumPeriod * (1f - Mathf.Sqrt(num2));
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00014573 File Offset: 0x00012773
		public virtual void Reset()
		{
			this.lastRepathTime = float.NegativeInfinity;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00014580 File Offset: 0x00012780
		public virtual void DidRecalculatePath(Vector3 destination, float time)
		{
			this.lastRepathTime = time;
			this.lastDestination = destination;
			this.lastRepathTime -= (global::UnityEngine.Random.value - 0.5f) * 0.3f * ((this.mode == AutoRepathPolicy.Mode.Dynamic) ? this.maximumPeriod : this.period);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000145D4 File Offset: 0x000127D4
		public void DrawGizmos(CommandBuilder draw, Vector3 position, float radius, NativeMovementPlane movementPlane)
		{
			if (this.visualizeSensitivity && !float.IsPositiveInfinity(this.lastDestination.x))
			{
				float num = Mathf.Sqrt(Mathf.Max((position - this.lastDestination).sqrMagnitude, radius * radius) / (this.sensitivity * this.sensitivity));
				draw.Circle(this.lastDestination, movementPlane.ToWorld(float2.zero, 1f), num, Color.magenta);
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00014654 File Offset: 0x00012854
		public AutoRepathPolicy Clone()
		{
			return base.MemberwiseClone() as AutoRepathPolicy;
		}

		// Token: 0x04000294 RID: 660
		public AutoRepathPolicy.Mode mode = AutoRepathPolicy.Mode.Dynamic;

		// Token: 0x04000295 RID: 661
		[FormerlySerializedAs("interval")]
		public float period = 0.5f;

		// Token: 0x04000296 RID: 662
		public float sensitivity = 10f;

		// Token: 0x04000297 RID: 663
		[FormerlySerializedAs("maximumInterval")]
		public float maximumPeriod = 2f;

		// Token: 0x04000298 RID: 664
		public bool visualizeSensitivity;

		// Token: 0x04000299 RID: 665
		private Vector3 lastDestination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x0400029A RID: 666
		private float lastRepathTime = float.NegativeInfinity;

		// Token: 0x02000072 RID: 114
		public enum Mode : byte
		{
			// Token: 0x0400029C RID: 668
			Never,
			// Token: 0x0400029D RID: 669
			EveryNSeconds,
			// Token: 0x0400029E RID: 670
			Dynamic
		}
	}
}
