using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x0200044A RID: 1098
	[Serializable]
	public class vThirdPersonCameraState
	{
		// Token: 0x06001B84 RID: 7044 RVA: 0x000AD710 File Offset: 0x000AB910
		public vThirdPersonCameraState(string name)
		{
			this.Name = name;
			this.forward = -1f;
			this.right = 0f;
			this.defaultDistance = 1.5f;
			this.maxDistance = 3f;
			this.minDistance = 0.5f;
			this.height = 0f;
			this.smooth = 10f;
			this.smoothDamp = 0f;
			this.xMouseSensitivity = 3f;
			this.yMouseSensitivity = 3f;
			this.yMinLimit = -40f;
			this.yMaxLimit = 80f;
			this.xMinLimit = -360f;
			this.xMaxLimit = 360f;
			this.cullingHeight = 0.2f;
			this.cullingMinDist = 0.1f;
			this.fov = 60f;
			this.useZoom = false;
			this.forward = 60f;
			this.fixedAngle = Vector2.zero;
			this.cameraMode = TPCameraMode.FreeDirectional;
		}

		// Token: 0x04001CB4 RID: 7348
		public string Name;

		// Token: 0x04001CB5 RID: 7349
		public float forward;

		// Token: 0x04001CB6 RID: 7350
		public float right;

		// Token: 0x04001CB7 RID: 7351
		public float defaultDistance;

		// Token: 0x04001CB8 RID: 7352
		public float maxDistance;

		// Token: 0x04001CB9 RID: 7353
		public float minDistance;

		// Token: 0x04001CBA RID: 7354
		public float height;

		// Token: 0x04001CBB RID: 7355
		public float smooth = 10f;

		// Token: 0x04001CBC RID: 7356
		public float smoothDamp;

		// Token: 0x04001CBD RID: 7357
		public float xMouseSensitivity;

		// Token: 0x04001CBE RID: 7358
		public float yMouseSensitivity;

		// Token: 0x04001CBF RID: 7359
		public float yMinLimit;

		// Token: 0x04001CC0 RID: 7360
		public float yMaxLimit;

		// Token: 0x04001CC1 RID: 7361
		public float xMinLimit;

		// Token: 0x04001CC2 RID: 7362
		public float xMaxLimit;

		// Token: 0x04001CC3 RID: 7363
		public Vector3 rotationOffSet;

		// Token: 0x04001CC4 RID: 7364
		public float cullingHeight;

		// Token: 0x04001CC5 RID: 7365
		public float cullingMinDist;

		// Token: 0x04001CC6 RID: 7366
		public float fov;

		// Token: 0x04001CC7 RID: 7367
		public bool useZoom;

		// Token: 0x04001CC8 RID: 7368
		public Vector2 fixedAngle;

		// Token: 0x04001CC9 RID: 7369
		public List<LookPoint> lookPoints;

		// Token: 0x04001CCA RID: 7370
		public TPCameraMode cameraMode;
	}
}
