using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004A0 RID: 1184
	public class vLookAtCamera : MonoBehaviour
	{
		// Token: 0x06001CFF RID: 7423 RVA: 0x000B2FAD File Offset: 0x000B11AD
		private void Start()
		{
			if (this.detachOnStart)
			{
				this.parent = base.transform.parent;
				base.transform.SetParent(null);
			}
			this.cameraMain = Camera.main;
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000B2FDF File Offset: 0x000B11DF
		private void OnEnable()
		{
			this.cameraMain = Camera.main;
			this.Align(false);
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000B2FF3 File Offset: 0x000B11F3
		private void FixedUpdate()
		{
			this.Align(this.useSmothRotation);
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000B3004 File Offset: 0x000B1204
		private void Align(bool useSmooth)
		{
			if (this.alignUp && this.parent)
			{
				base.transform.position = this.parent.position + Vector3.up * this.height;
			}
			if (!this.cameraMain)
			{
				return;
			}
			Vector3 vector = this.cameraMain.transform.position - base.transform.position;
			vector.y = 0f;
			Quaternion quaternion = Quaternion.LookRotation(vector);
			if (useSmooth)
			{
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, quaternion, Time.deltaTime * 4f);
				base.transform.eulerAngles = new Vector3(this.justY ? 0f : base.transform.eulerAngles.x, base.transform.eulerAngles.y, 0f);
				return;
			}
			base.transform.eulerAngles = new Vector3(this.justY ? 0f : quaternion.eulerAngles.x, quaternion.eulerAngles.y, 0f);
		}

		// Token: 0x04001E1E RID: 7710
		[Tooltip("Align position to stay always on top of parent")]
		public bool alignUp;

		// Token: 0x04001E1F RID: 7711
		[Tooltip("Height of alignment on top of parent \n!!(Check alignUp to work)!!")]
		public float height = 1f;

		// Token: 0x04001E20 RID: 7712
		[Tooltip("Detach of the parent on start \n!!(if alignUp not is checked, the object not follow the parent)!!")]
		public bool detachOnStart;

		// Token: 0x04001E21 RID: 7713
		[Tooltip("use smoth to look at camera")]
		public bool useSmothRotation = true;

		// Token: 0x04001E22 RID: 7714
		protected Transform parent;

		// Token: 0x04001E23 RID: 7715
		public bool justY;

		// Token: 0x04001E24 RID: 7716
		internal Camera cameraMain;
	}
}
