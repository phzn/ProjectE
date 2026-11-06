using System;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x02000012 RID: 18
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineDollyCart.html")]
	public class CinemachineDollyCart : MonoBehaviour
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00006709 File Offset: 0x00004909
		private void FixedUpdate()
		{
			if (this.m_UpdateMethod == CinemachineDollyCart.UpdateMethod.FixedUpdate)
			{
				this.SetCartPosition(this.m_Position + this.m_Speed * Time.deltaTime);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00006730 File Offset: 0x00004930
		private void Update()
		{
			float num = (Application.isPlaying ? this.m_Speed : 0f);
			if (this.m_UpdateMethod == CinemachineDollyCart.UpdateMethod.Update)
			{
				this.SetCartPosition(this.m_Position + num * Time.deltaTime);
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000676E File Offset: 0x0000496E
		private void LateUpdate()
		{
			if (!Application.isPlaying)
			{
				this.SetCartPosition(this.m_Position);
				return;
			}
			if (this.m_UpdateMethod == CinemachineDollyCart.UpdateMethod.LateUpdate)
			{
				this.SetCartPosition(this.m_Position + this.m_Speed * Time.deltaTime);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000067A8 File Offset: 0x000049A8
		private void SetCartPosition(float distanceAlongPath)
		{
			if (this.m_Path != null)
			{
				this.m_Position = this.m_Path.StandardizeUnit(distanceAlongPath, this.m_PositionUnits);
				Vector3 vector = this.m_Path.EvaluatePositionAtUnit(this.m_Position, this.m_PositionUnits);
				Quaternion quaternion = this.m_Path.EvaluateOrientationAtUnit(this.m_Position, this.m_PositionUnits);
				base.transform.ConservativeSetPositionAndRotation(vector, quaternion);
			}
		}

		// Token: 0x04000071 RID: 113
		[Tooltip("The path to follow")]
		public CinemachinePathBase m_Path;

		// Token: 0x04000072 RID: 114
		[Tooltip("When to move the cart, if Velocity is non-zero")]
		public CinemachineDollyCart.UpdateMethod m_UpdateMethod;

		// Token: 0x04000073 RID: 115
		[Tooltip("How to interpret the Path Position.  If set to Path Units, values are as follows: 0 represents the first waypoint on the path, 1 is the second, and so on.  Values in-between are points on the path in between the waypoints.  If set to Distance, then Path Position represents distance along the path.")]
		public CinemachinePathBase.PositionUnits m_PositionUnits = CinemachinePathBase.PositionUnits.Distance;

		// Token: 0x04000074 RID: 116
		[Tooltip("Move the cart with this speed along the path.  The value is interpreted according to the Position Units setting.")]
		[FormerlySerializedAs("m_Velocity")]
		public float m_Speed;

		// Token: 0x04000075 RID: 117
		[Tooltip("The position along the path at which the cart will be placed.  This can be animated directly or, if the velocity is non-zero, will be updated automatically.  The value is interpreted according to the Position Units setting.")]
		[FormerlySerializedAs("m_CurrentDistance")]
		public float m_Position;

		// Token: 0x0200007D RID: 125
		public enum UpdateMethod
		{
			// Token: 0x040002E5 RID: 741
			Update,
			// Token: 0x040002E6 RID: 742
			FixedUpdate,
			// Token: 0x040002E7 RID: 743
			LateUpdate
		}
	}
}
