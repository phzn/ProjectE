using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000051 RID: 81
	[RequireComponent(typeof(CinemachineTargetGroup))]
	[ExecuteAlways]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/api/Cinemachine.GroupWeightManipulator.html")]
	public class GroupWeightManipulator : MonoBehaviour
	{
		// Token: 0x06000374 RID: 884 RVA: 0x000155F7 File Offset: 0x000137F7
		private void Start()
		{
			this.m_group = base.GetComponent<CinemachineTargetGroup>();
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00015608 File Offset: 0x00013808
		private void OnValidate()
		{
			this.m_Weight0 = Mathf.Max(0f, this.m_Weight0);
			this.m_Weight1 = Mathf.Max(0f, this.m_Weight1);
			this.m_Weight2 = Mathf.Max(0f, this.m_Weight2);
			this.m_Weight3 = Mathf.Max(0f, this.m_Weight3);
			this.m_Weight4 = Mathf.Max(0f, this.m_Weight4);
			this.m_Weight5 = Mathf.Max(0f, this.m_Weight5);
			this.m_Weight6 = Mathf.Max(0f, this.m_Weight6);
			this.m_Weight7 = Mathf.Max(0f, this.m_Weight7);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000156C5 File Offset: 0x000138C5
		private void Update()
		{
			if (this.m_group != null)
			{
				this.UpdateWeights();
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000156DC File Offset: 0x000138DC
		private void UpdateWeights()
		{
			CinemachineTargetGroup.Target[] targets = this.m_group.m_Targets;
			int num = targets.Length - 1;
			if (num < 0)
			{
				return;
			}
			targets[0].weight = this.m_Weight0;
			if (num < 1)
			{
				return;
			}
			targets[1].weight = this.m_Weight1;
			if (num < 2)
			{
				return;
			}
			targets[2].weight = this.m_Weight2;
			if (num < 3)
			{
				return;
			}
			targets[3].weight = this.m_Weight3;
			if (num < 4)
			{
				return;
			}
			targets[4].weight = this.m_Weight4;
			if (num < 5)
			{
				return;
			}
			targets[5].weight = this.m_Weight5;
			if (num < 6)
			{
				return;
			}
			targets[6].weight = this.m_Weight6;
			if (num < 7)
			{
				return;
			}
			targets[7].weight = this.m_Weight7;
		}

		// Token: 0x0400024A RID: 586
		[Tooltip("The weight of the group member at index 0")]
		public float m_Weight0 = 1f;

		// Token: 0x0400024B RID: 587
		[Tooltip("The weight of the group member at index 1")]
		public float m_Weight1 = 1f;

		// Token: 0x0400024C RID: 588
		[Tooltip("The weight of the group member at index 2")]
		public float m_Weight2 = 1f;

		// Token: 0x0400024D RID: 589
		[Tooltip("The weight of the group member at index 3")]
		public float m_Weight3 = 1f;

		// Token: 0x0400024E RID: 590
		[Tooltip("The weight of the group member at index 4")]
		public float m_Weight4 = 1f;

		// Token: 0x0400024F RID: 591
		[Tooltip("The weight of the group member at index 5")]
		public float m_Weight5 = 1f;

		// Token: 0x04000250 RID: 592
		[Tooltip("The weight of the group member at index 6")]
		public float m_Weight6 = 1f;

		// Token: 0x04000251 RID: 593
		[Tooltip("The weight of the group member at index 7")]
		public float m_Weight7 = 1f;

		// Token: 0x04000252 RID: 594
		private CinemachineTargetGroup m_group;
	}
}
