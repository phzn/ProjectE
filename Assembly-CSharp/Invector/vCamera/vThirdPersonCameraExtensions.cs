using System;
using UnityEngine;

namespace Invector.vCamera
{
	// Token: 0x0200063C RID: 1596
	public static class vThirdPersonCameraExtensions
	{
		// Token: 0x060024C0 RID: 9408 RVA: 0x000CE9AC File Offset: 0x000CCBAC
		public static void Slerp(this vThirdPersonCameraState to, vThirdPersonCameraState from, float time)
		{
			to.Name = from.Name;
			to.forward = Mathf.Lerp(to.forward, from.forward, time);
			to.right = Mathf.Lerp(to.right, from.right, time);
			to.defaultDistance = Mathf.Lerp(to.defaultDistance, from.defaultDistance, time);
			to.maxDistance = Mathf.Lerp(to.maxDistance, from.maxDistance, time);
			to.minDistance = Mathf.Lerp(to.minDistance, from.minDistance, time);
			to.height = Mathf.Lerp(to.height, from.height, time);
			to.fixedAngle = Vector2.Lerp(to.fixedAngle, from.fixedAngle, time);
			to.smooth = Mathf.Lerp(to.smooth, from.smooth, time);
			to.xMouseSensitivity = Mathf.Lerp(to.xMouseSensitivity, from.xMouseSensitivity, time);
			to.yMouseSensitivity = Mathf.Lerp(to.yMouseSensitivity, from.yMouseSensitivity, time);
			to.yMinLimit = Mathf.Lerp(to.yMinLimit, from.yMinLimit, time);
			to.yMaxLimit = Mathf.Lerp(to.yMaxLimit, from.yMaxLimit, time);
			to.xMinLimit = Mathf.Lerp(to.xMinLimit, from.xMinLimit, time);
			to.xMaxLimit = Mathf.Lerp(to.xMaxLimit, from.xMaxLimit, time);
			to.rotationOffSet = Vector3.Lerp(to.rotationOffSet, from.rotationOffSet, time);
			to.cullingHeight = Mathf.Lerp(to.cullingHeight, from.cullingHeight, time);
			to.cullingMinDist = Mathf.Lerp(to.cullingMinDist, from.cullingMinDist, time);
			to.cameraMode = from.cameraMode;
			to.useZoom = from.useZoom;
			to.lookPoints = from.lookPoints;
			to.fov = Mathf.Lerp(to.fov, from.fov, time);
			if (to.fov <= 0f)
			{
				to.fov = 1f;
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000CEBB4 File Offset: 0x000CCDB4
		public static void CopyState(this vThirdPersonCameraState to, vThirdPersonCameraState from)
		{
			to.Name = from.Name;
			to.forward = from.forward;
			to.right = from.right;
			to.defaultDistance = from.defaultDistance;
			to.maxDistance = from.maxDistance;
			to.minDistance = from.minDistance;
			to.height = from.height;
			to.fixedAngle = from.fixedAngle;
			to.lookPoints = from.lookPoints;
			to.smooth = from.smooth;
			to.xMouseSensitivity = from.xMouseSensitivity;
			to.yMouseSensitivity = from.yMouseSensitivity;
			to.yMinLimit = from.yMinLimit;
			to.yMaxLimit = from.yMaxLimit;
			to.xMinLimit = from.xMinLimit;
			to.xMaxLimit = from.xMaxLimit;
			to.rotationOffSet = from.rotationOffSet;
			to.cullingHeight = from.cullingHeight;
			to.cullingMinDist = from.cullingMinDist;
			to.cameraMode = from.cameraMode;
			to.useZoom = from.useZoom;
			to.fov = from.fov;
			if (to.fov <= 0f)
			{
				to.fov = 1f;
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000CECE4 File Offset: 0x000CCEE4
		public static ClipPlanePoints NearClipPlanePoints(this Camera camera, Vector3 pos, float clipPlaneMargin)
		{
			ClipPlanePoints clipPlanePoints = default(ClipPlanePoints);
			Transform transform = camera.transform;
			float num = camera.fieldOfView / 2f * 0.017453292f;
			float aspect = camera.aspect;
			float nearClipPlane = camera.nearClipPlane;
			float num2 = nearClipPlane * Mathf.Tan(num);
			float num3 = num2 * aspect;
			num2 *= 1f + clipPlaneMargin;
			num3 *= 1f + clipPlaneMargin;
			clipPlanePoints.LowerRight = pos + transform.right * num3;
			clipPlanePoints.LowerRight -= transform.up * num2;
			clipPlanePoints.LowerRight += transform.forward * nearClipPlane;
			clipPlanePoints.LowerLeft = pos - transform.right * num3;
			clipPlanePoints.LowerLeft -= transform.up * num2;
			clipPlanePoints.LowerLeft += transform.forward * nearClipPlane;
			clipPlanePoints.UpperRight = pos + transform.right * num3;
			clipPlanePoints.UpperRight += transform.up * num2;
			clipPlanePoints.UpperRight += transform.forward * nearClipPlane;
			clipPlanePoints.UpperLeft = pos - transform.right * num3;
			clipPlanePoints.UpperLeft += transform.up * num2;
			clipPlanePoints.UpperLeft += transform.forward * nearClipPlane;
			return clipPlanePoints;
		}
	}
}
