using System;
using UnityEngine;

namespace Cinemachine.Utility
{
	// Token: 0x02000064 RID: 100
	public class PositionPredictor
	{
		// Token: 0x060003D5 RID: 981 RVA: 0x00017498 File Offset: 0x00015698
		public bool IsEmpty()
		{
			return !this.m_HavePos;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000174A3 File Offset: 0x000156A3
		public void ApplyTransformDelta(Vector3 positionDelta)
		{
			this.m_Pos += positionDelta;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000174B7 File Offset: 0x000156B7
		public void Reset()
		{
			this.m_HavePos = false;
			this.m_SmoothDampVelocity = Vector3.zero;
			this.m_Velocity = Vector3.zero;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000174D8 File Offset: 0x000156D8
		public void AddPosition(Vector3 pos, float deltaTime, float lookaheadTime)
		{
			if (deltaTime < 0f)
			{
				this.Reset();
			}
			if (this.m_HavePos && deltaTime > 0.0001f)
			{
				Vector3 vector = (pos - this.m_Pos) / deltaTime;
				bool flag = vector.sqrMagnitude < this.m_Velocity.sqrMagnitude;
				this.m_Velocity = Vector3.SmoothDamp(this.m_Velocity, vector, ref this.m_SmoothDampVelocity, this.Smoothing / (float)(flag ? 30 : 10), float.PositiveInfinity, deltaTime);
			}
			this.m_Pos = pos;
			this.m_HavePos = true;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00017568 File Offset: 0x00015768
		public Vector3 PredictPositionDelta(float lookaheadTime)
		{
			return this.m_Velocity * lookaheadTime;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00017576 File Offset: 0x00015776
		public Vector3 PredictPosition(float lookaheadTime)
		{
			return this.m_Pos + this.PredictPositionDelta(lookaheadTime);
		}

		// Token: 0x04000292 RID: 658
		private Vector3 m_Velocity;

		// Token: 0x04000293 RID: 659
		private Vector3 m_SmoothDampVelocity;

		// Token: 0x04000294 RID: 660
		private Vector3 m_Pos;

		// Token: 0x04000295 RID: 661
		private bool m_HavePos;

		// Token: 0x04000296 RID: 662
		public float Smoothing;
	}
}
