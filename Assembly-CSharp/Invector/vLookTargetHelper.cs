using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004BC RID: 1212
	public static class vLookTargetHelper
	{
		// Token: 0x06001D69 RID: 7529 RVA: 0x000B48C8 File Offset: 0x000B2AC8
		private static vLookTargetHelper.LookPoints GetLookPoints(vLookTarget lookTarget)
		{
			vLookTargetHelper.LookPoints lookPoints = default(vLookTargetHelper.LookPoints);
			Vector3 centerArea = lookTarget.centerArea;
			Vector3 sizeArea = lookTarget.sizeArea;
			Transform transform = lookTarget.transform;
			lookPoints.frontTopLeft = new Vector3(centerArea.x - sizeArea.x, centerArea.y + sizeArea.y, centerArea.z - sizeArea.z);
			lookPoints.frontTopRight = new Vector3(centerArea.x + sizeArea.x, centerArea.y + sizeArea.y, centerArea.z - sizeArea.z);
			lookPoints.frontBottomLeft = new Vector3(centerArea.x - sizeArea.x, centerArea.y - sizeArea.y, centerArea.z - sizeArea.z);
			lookPoints.frontBottomRight = new Vector3(centerArea.x + sizeArea.x, centerArea.y - sizeArea.y, centerArea.z - sizeArea.z);
			lookPoints.backTopLeft = new Vector3(centerArea.x - sizeArea.x, centerArea.y + sizeArea.y, centerArea.z + sizeArea.z);
			lookPoints.backTopRight = new Vector3(centerArea.x + sizeArea.x, centerArea.y + sizeArea.y, centerArea.z + sizeArea.z);
			lookPoints.backBottomLeft = new Vector3(centerArea.x - sizeArea.x, centerArea.y - sizeArea.y, centerArea.z + sizeArea.z);
			lookPoints.backBottomRight = new Vector3(centerArea.x + sizeArea.x, centerArea.y - sizeArea.y, centerArea.z + sizeArea.z);
			lookPoints.frontTopLeft = transform.TransformPoint(lookPoints.frontTopLeft);
			lookPoints.frontTopRight = transform.TransformPoint(lookPoints.frontTopRight);
			lookPoints.frontBottomLeft = transform.TransformPoint(lookPoints.frontBottomLeft);
			lookPoints.frontBottomRight = transform.TransformPoint(lookPoints.frontBottomRight);
			lookPoints.backTopLeft = transform.TransformPoint(lookPoints.backTopLeft);
			lookPoints.backTopRight = transform.TransformPoint(lookPoints.backTopRight);
			lookPoints.backBottomLeft = transform.TransformPoint(lookPoints.backBottomLeft);
			lookPoints.backBottomRight = transform.TransformPoint(lookPoints.backBottomRight);
			return lookPoints;
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x000B4B24 File Offset: 0x000B2D24
		public static bool IsVisible(this vLookTarget lookTarget, Vector3 from, LayerMask layerMask, bool debug = false)
		{
			if (lookTarget.HideObject)
			{
				return false;
			}
			if (lookTarget.visibleCheckType == vLookTarget.VisibleCheckType.None)
			{
				return !lookTarget.useLimitToDetect || Vector3.Distance(from, lookTarget.transform.position) <= lookTarget.minDistanceToDetect;
			}
			if (lookTarget.visibleCheckType == vLookTarget.VisibleCheckType.SingleCast)
			{
				return (!lookTarget.useLimitToDetect || Vector3.Distance(from, lookTarget.centerArea) <= lookTarget.minDistanceToDetect) && vLookTargetHelper.CastPoint(from, lookTarget.transform.TransformPoint(lookTarget.centerArea), lookTarget.transform, layerMask, debug);
			}
			if (lookTarget.visibleCheckType == vLookTarget.VisibleCheckType.BoxCast)
			{
				if (lookTarget.useLimitToDetect && Vector3.Distance(from, lookTarget.transform.position) > lookTarget.minDistanceToDetect)
				{
					return false;
				}
				vLookTargetHelper.LookPoints lookPoints = vLookTargetHelper.GetLookPoints(lookTarget);
				if (vLookTargetHelper.CastPoint(from, lookPoints.frontTopLeft, lookTarget.transform, layerMask, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.frontTopRight, lookTarget.transform, layerMask, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.frontBottomLeft, lookTarget.transform, layerMask, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.frontBottomRight, lookTarget.transform, layerMask, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.backTopLeft, lookTarget.transform, layerMask, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.backTopRight, lookTarget.transform, layerMask, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.backBottomLeft, lookTarget.transform, layerMask, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.backBottomRight, lookTarget.transform, layerMask, debug))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x000B4CAC File Offset: 0x000B2EAC
		public static bool IsVisible(this vLookTarget lookTarget, Vector3 from, bool debug = false)
		{
			if (lookTarget.HideObject)
			{
				return false;
			}
			vLookTargetHelper.LookPoints lookPoints = vLookTargetHelper.GetLookPoints(lookTarget);
			if (lookTarget.visibleCheckType == vLookTarget.VisibleCheckType.None)
			{
				return true;
			}
			if (lookTarget.visibleCheckType == vLookTarget.VisibleCheckType.SingleCast)
			{
				return vLookTargetHelper.CastPoint(from, lookTarget.transform.TransformPoint(lookTarget.centerArea), lookTarget.transform, debug);
			}
			if (lookTarget.visibleCheckType == vLookTarget.VisibleCheckType.BoxCast)
			{
				if (vLookTargetHelper.CastPoint(from, lookPoints.frontTopLeft, lookTarget.transform, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.frontTopRight, lookTarget.transform, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.frontBottomLeft, lookTarget.transform, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.frontBottomRight, lookTarget.transform, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.backTopLeft, lookTarget.transform, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.backTopRight, lookTarget.transform, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.backBottomLeft, lookTarget.transform, debug))
				{
					return true;
				}
				if (vLookTargetHelper.CastPoint(from, lookPoints.backBottomRight, lookTarget.transform, debug))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x000B4DC8 File Offset: 0x000B2FC8
		private static bool CastPoint(Vector3 from, Vector3 point, Transform lookTarget, LayerMask layerMask, bool debug = false)
		{
			RaycastHit raycastHit;
			if (!Physics.Linecast(from, point, out raycastHit, layerMask))
			{
				if (debug)
				{
					Debug.DrawLine(from, point, Color.green);
				}
				return true;
			}
			if (raycastHit.transform != lookTarget.transform)
			{
				if (debug)
				{
					Debug.DrawLine(from, raycastHit.point, Color.red);
				}
				return false;
			}
			if (debug)
			{
				Debug.DrawLine(from, raycastHit.point, Color.green);
			}
			return true;
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x000B4E3C File Offset: 0x000B303C
		private static bool CastPoint(Vector3 from, Vector3 point, Transform lookTarget, bool debug = false)
		{
			RaycastHit raycastHit;
			if (!Physics.Linecast(from, point, out raycastHit))
			{
				if (debug)
				{
					Debug.DrawLine(from, point, Color.green);
				}
				return true;
			}
			if (raycastHit.transform != lookTarget.transform)
			{
				if (debug)
				{
					Debug.DrawLine(from, raycastHit.point, Color.red);
				}
				return false;
			}
			if (debug)
			{
				Debug.DrawLine(from, raycastHit.point, Color.green);
			}
			return true;
		}

		// Token: 0x020004BD RID: 1213
		private struct LookPoints
		{
			// Token: 0x04001E81 RID: 7809
			public Vector3 frontTopLeft;

			// Token: 0x04001E82 RID: 7810
			public Vector3 frontTopRight;

			// Token: 0x04001E83 RID: 7811
			public Vector3 frontBottomLeft;

			// Token: 0x04001E84 RID: 7812
			public Vector3 frontBottomRight;

			// Token: 0x04001E85 RID: 7813
			public Vector3 backTopLeft;

			// Token: 0x04001E86 RID: 7814
			public Vector3 backTopRight;

			// Token: 0x04001E87 RID: 7815
			public Vector3 backBottomLeft;

			// Token: 0x04001E88 RID: 7816
			public Vector3 backBottomRight;
		}
	}
}
