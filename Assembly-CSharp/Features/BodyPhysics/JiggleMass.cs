using System;
using UnityEngine;

namespace Features.BodyPhysics
{
	// Token: 0x02001504 RID: 5380
	public sealed class JiggleMass
	{
		// Token: 0x170019A1 RID: 6561
		// (get) Token: 0x0600A626 RID: 42534 RVA: 0x00456EF0 File Offset: 0x004550F0
		public Vector3 Offset
		{
			get
			{
				return this.Mass - this.Anchor;
			}
		}

		// Token: 0x0600A627 RID: 42535 RVA: 0x00456F03 File Offset: 0x00455103
		public void Reset()
		{
			this.Settled = false;
			this.Moving = false;
			this.Velocity = Vector3.zero;
		}

		// Token: 0x0600A628 RID: 42536 RVA: 0x00456F1E File Offset: 0x0045511E
		public void Kick(Vector3 direction, float speed)
		{
			if (this.Moving)
			{
				this.Velocity += direction * speed;
			}
		}

		// Token: 0x0600A629 RID: 42537 RVA: 0x00456F40 File Offset: 0x00455140
		public void Transport(Vector3 previousPosition, Quaternion previousRotation, Vector3 position, Quaternion rotation)
		{
			if (!this.Settled)
			{
				return;
			}
			Quaternion quaternion = rotation * Quaternion.Inverse(previousRotation);
			this.Mass = position + quaternion * (this.Mass - previousPosition);
			this.Anchor = position + quaternion * (this.Anchor - previousPosition);
			this.Velocity = quaternion * this.Velocity;
		}

		// Token: 0x0600A62A RID: 42538 RVA: 0x00456FB4 File Offset: 0x004551B4
		public void Follow(Vector3 anchor, float deltaTime, float damping, float maxSwing, float maxSpeed = 6f)
		{
			if (deltaTime <= 0f)
			{
				return;
			}
			if (!this.Settled)
			{
				this.Anchor = anchor;
				this.Mass = anchor;
				this.Velocity = Vector3.zero;
				this.Settled = true;
				this.Moving = false;
				return;
			}
			Vector3 vector = (anchor - this.Anchor) / deltaTime;
			if (!this.Moving)
			{
				this.Velocity = vector;
				this.Mass += anchor - this.Anchor;
				this.Anchor = anchor;
				this.Moving = true;
				return;
			}
			Vector3 mass = this.Mass;
			Vector3 velocity = this.Velocity;
			JiggleMass.SpringStep(ref mass, ref velocity, this.Anchor, anchor, deltaTime, 6.2831855f * this.Frequency, damping);
			Vector3 vector2 = mass - anchor;
			Vector3 vector3 = velocity - vector;
			if (vector2.sqrMagnitude > maxSwing * maxSwing)
			{
				Vector3 normalized = vector2.normalized;
				vector2 = normalized * maxSwing;
				float num = Vector3.Dot(vector3, normalized);
				if (num > 0f)
				{
					vector3 -= normalized * num;
				}
			}
			this.Mass = anchor + vector2;
			this.Velocity = vector + Vector3.ClampMagnitude(vector3, maxSpeed);
			this.Anchor = anchor;
		}

		// Token: 0x0600A62B RID: 42539 RVA: 0x004570FC File Offset: 0x004552FC
		public static void SpringStep(ref Vector3 mass, ref Vector3 velocity, Vector3 from, Vector3 to, float deltaTime, float omega, float damping)
		{
			if (deltaTime <= 0f)
			{
				return;
			}
			int num = Mathf.Clamp(Mathf.CeilToInt(deltaTime / 0.004166667f), 1, 64);
			float num2 = deltaTime / (float)num;
			Vector3 vector = (to - from) / deltaTime;
			float num3 = omega * omega;
			float num4 = 2f * Mathf.Clamp(damping, 0.01f, 1f) * omega;
			for (int i = 0; i < num; i++)
			{
				Vector3 vector2 = Vector3.Lerp(from, to, (float)i / (float)num);
				Vector3 vector3 = num3 * (vector2 - mass) - num4 * (velocity - vector);
				velocity += vector3 * num2;
				mass += velocity * num2;
			}
		}

		// Token: 0x0400AAF1 RID: 43761
		private const float Step = 0.004166667f;

		// Token: 0x0400AAF2 RID: 43762
		public Vector3 Mass;

		// Token: 0x0400AAF3 RID: 43763
		public Vector3 Velocity;

		// Token: 0x0400AAF4 RID: 43764
		public Vector3 Anchor;

		// Token: 0x0400AAF5 RID: 43765
		public float Frequency = 3.7f;

		// Token: 0x0400AAF6 RID: 43766
		public bool Settled;

		// Token: 0x0400AAF7 RID: 43767
		public bool Moving;
	}
}
