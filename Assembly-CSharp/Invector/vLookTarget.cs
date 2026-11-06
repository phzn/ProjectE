using System;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x020004B9 RID: 1209
	public class vLookTarget : MonoBehaviour
	{
		// Token: 0x06001D61 RID: 7521 RVA: 0x000B4643 File Offset: 0x000B2843
		private void OnDrawGizmosSelected()
		{
			this.DrawBox();
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000B464C File Offset: 0x000B284C
		private void Start()
		{
			int num = LayerMask.NameToLayer("HeadTrack");
			base.gameObject.layer = num;
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001D63 RID: 7523 RVA: 0x000B4670 File Offset: 0x000B2870
		public Vector3 lookPoint
		{
			get
			{
				if (this.lookPointTarget)
				{
					return this.lookPointTarget.position;
				}
				return base.transform.TransformPoint(this.centerArea);
			}
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x000B469C File Offset: 0x000B289C
		private void DrawBox()
		{
			Gizmos.color = new Color(1f, 0f, 0f, 1f);
			Gizmos.DrawSphere(this.lookPoint, 0.05f);
			if (this.visibleCheckType == vLookTarget.VisibleCheckType.BoxCast)
			{
				float num = base.transform.lossyScale.x * this.sizeArea.x;
				float num2 = base.transform.lossyScale.y * this.sizeArea.y;
				float num3 = base.transform.lossyScale.z * this.sizeArea.z;
				float num4 = base.transform.lossyScale.x * this.centerArea.x;
				float num5 = base.transform.lossyScale.y * this.centerArea.y;
				float num6 = base.transform.lossyScale.z * this.centerArea.z;
				Gizmos.matrix = Matrix4x4.TRS(base.transform.position + new Vector3(num4, num5, num6), base.transform.rotation, new Vector3(num, num2, num3) * 2f);
				Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
				Gizmos.DrawCube(Vector3.zero, Vector3.one);
				Gizmos.color = new Color(0f, 1f, 0f, 1f);
				Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
				return;
			}
			if (this.visibleCheckType == vLookTarget.VisibleCheckType.SingleCast)
			{
				Vector3 vector = base.transform.TransformPoint(this.centerArea);
				Gizmos.color = new Color(0f, 1f, 0f, 1f);
				Gizmos.DrawSphere(vector, 0.05f);
			}
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x000B4871 File Offset: 0x000B2A71
		internal void EnterLook(vHeadTrack vHeadTrack)
		{
			this.onEnterLook.Invoke(vHeadTrack);
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x000B487F File Offset: 0x000B2A7F
		internal void ExitLook(vHeadTrack vHeadTrack)
		{
			this.onExitLook.Invoke(vHeadTrack);
		}

		// Token: 0x04001E73 RID: 7795
		public bool ignoreHeadTrackAngle;

		// Token: 0x04001E74 RID: 7796
		[Header("Set this to assign a different point to look")]
		public Transform lookPointTarget;

		// Token: 0x04001E75 RID: 7797
		[Header("Area to check if is visible")]
		public Vector3 centerArea = Vector3.zero;

		// Token: 0x04001E76 RID: 7798
		public Vector3 sizeArea = Vector3.one;

		// Token: 0x04001E77 RID: 7799
		public bool useLimitToDetect = true;

		// Token: 0x04001E78 RID: 7800
		public float minDistanceToDetect = 2f;

		// Token: 0x04001E79 RID: 7801
		public vLookTarget.VisibleCheckType visibleCheckType;

		// Token: 0x04001E7A RID: 7802
		[Tooltip("use this to turn the object undetectable")]
		public bool HideObject;

		// Token: 0x04001E7B RID: 7803
		public vLookTarget.OnLookEvent onEnterLook;

		// Token: 0x04001E7C RID: 7804
		public vLookTarget.OnLookEvent onExitLook;

		// Token: 0x020004BA RID: 1210
		[Serializable]
		public class OnLookEvent : UnityEvent<vHeadTrack>
		{
		}

		// Token: 0x020004BB RID: 1211
		public enum VisibleCheckType
		{
			// Token: 0x04001E7E RID: 7806
			None,
			// Token: 0x04001E7F RID: 7807
			SingleCast,
			// Token: 0x04001E80 RID: 7808
			BoxCast
		}
	}
}
