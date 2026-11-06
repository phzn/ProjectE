using System;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x02000927 RID: 2343
	public static class SubwayPassengerCollision
	{
		// Token: 0x06003F8F RID: 16271 RVA: 0x00179434 File Offset: 0x00177634
		public static void Configure(GameObject passengerRoot, Animator animator, bool seated)
		{
			if (passengerRoot == null)
			{
				return;
			}
			Transform transform = null;
			Transform transform2 = null;
			if (seated && animator != null && animator.avatar != null && animator.avatar.isValid && animator.isHuman && animator.transform.IsChildOf(passengerRoot.transform))
			{
				transform = animator.GetBoneTransform(HumanBodyBones.Hips);
				transform2 = animator.GetBoneTransform(HumanBodyBones.Head);
			}
			foreach (CapsuleCollider capsuleCollider in passengerRoot.GetComponents<CapsuleCollider>())
			{
				if (!capsuleCollider.isTrigger && capsuleCollider.direction == 1)
				{
					Vector3 lossyScale = capsuleCollider.transform.lossyScale;
					float num = Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.z));
					if (num > 0.0001f)
					{
						capsuleCollider.radius = Mathf.Min(capsuleCollider.radius, 0.2f / num);
						if (transform != null && transform2 != null)
						{
							SubwayPassengerCollision.ConfigureSeatedCapsule(capsuleCollider, transform.position, transform2.position);
						}
					}
				}
			}
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x00179550 File Offset: 0x00177750
		private static void ConfigureSeatedCapsule(CapsuleCollider capsule, Vector3 hipsWorldPosition, Vector3 headWorldPosition)
		{
			Transform transform = capsule.transform;
			Vector3 lossyScale = transform.lossyScale;
			float num = Mathf.Abs(lossyScale.y);
			if (num <= 0.0001f)
			{
				return;
			}
			Vector3 vector = transform.InverseTransformPoint(hipsWorldPosition);
			Vector3 vector2 = transform.InverseTransformPoint(headWorldPosition);
			if (vector2.y <= vector.y)
			{
				return;
			}
			float num2 = Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.z));
			float num3 = capsule.radius * num2 / num;
			capsule.center = (vector + vector2) * 0.5f;
			capsule.height = vector2.y - vector.y + 2f * num3;
		}

		// Token: 0x04003F65 RID: 16229
		private const float MaximumWorldRadius = 0.2f;

		// Token: 0x04003F66 RID: 16230
		private const float MinimumScale = 0.0001f;
	}
}
