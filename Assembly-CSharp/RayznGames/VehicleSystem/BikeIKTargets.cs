using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200076F RID: 1903
	public class BikeIKTargets : MonoBehaviour
	{
		// Token: 0x06003196 RID: 12694 RVA: 0x0011724C File Offset: 0x0011544C
		private void Update()
		{
			this.leftHandTarget.position = this.handleLeft.position;
			this.leftHandTarget.localEulerAngles = this.handleLeft.localRotation.eulerAngles + this.leftHandRotation;
			this.rightHandTarget.position = this.handleRight.position;
			this.rightHandTarget.localEulerAngles = this.handleRight.localRotation.eulerAngles + this.rightHandRotation;
			this.leftFootTarget.position = this.pedalLeft.position;
			this.rightFootTarget.position = this.pedalRight.position;
		}

		// Token: 0x0400320D RID: 12813
		[Header("BikePositions")]
		[SerializeField]
		private Transform handleLeft;

		// Token: 0x0400320E RID: 12814
		[SerializeField]
		private Transform handleRight;

		// Token: 0x0400320F RID: 12815
		[SerializeField]
		private Transform pedalLeft;

		// Token: 0x04003210 RID: 12816
		[SerializeField]
		private Transform pedalRight;

		// Token: 0x04003211 RID: 12817
		[Header("IK Rig Targets")]
		[SerializeField]
		private Transform leftHandTarget;

		// Token: 0x04003212 RID: 12818
		[SerializeField]
		private Vector3 leftHandRotation = new Vector3(0f, 0f, 0f);

		// Token: 0x04003213 RID: 12819
		[SerializeField]
		private Transform rightHandTarget;

		// Token: 0x04003214 RID: 12820
		[SerializeField]
		private Vector3 rightHandRotation = new Vector3(0f, 0f, 0f);

		// Token: 0x04003215 RID: 12821
		[SerializeField]
		private Transform leftFootTarget;

		// Token: 0x04003216 RID: 12822
		[SerializeField]
		private Transform rightFootTarget;
	}
}
