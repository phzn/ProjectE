using System;
using UnityEngine;

namespace ProjectExhibitionist.FirstPerson
{
	// Token: 0x020009E6 RID: 2534
	[Serializable]
	public sealed class FirstPersonCalibrationValues
	{
		// Token: 0x060047BA RID: 18362 RVA: 0x001B397D File Offset: 0x001B1B7D
		public FirstPersonCalibrationValues Clone()
		{
			return new FirstPersonCalibrationValues
			{
				positionOffset = this.positionOffset,
				rotationOffset = this.rotationOffset,
				fieldOfView = this.fieldOfView,
				nearClipPlane = this.nearClipPlane
			};
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x001B39B4 File Offset: 0x001B1BB4
		public void Sanitize()
		{
			this.positionOffset = FirstPersonCalibrationValues.ClampVector(this.positionOffset, -2f, 2f);
			this.rotationOffset = FirstPersonCalibrationValues.ClampVector(this.rotationOffset, -180f, 180f);
			this.fieldOfView = Mathf.Clamp(FirstPersonCalibrationValues.Finite(this.fieldOfView, 70f), 20f, 120f);
			this.nearClipPlane = Mathf.Clamp(FirstPersonCalibrationValues.Finite(this.nearClipPlane, 0.01f), 0.001f, 0.3f);
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x001B3A41 File Offset: 0x001B1C41
		internal static float Finite(float value, float fallback = 0f)
		{
			if (!float.IsNaN(value) && !float.IsInfinity(value))
			{
				return value;
			}
			return fallback;
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x001B3A56 File Offset: 0x001B1C56
		internal static Vector3 Finite(Vector3 value)
		{
			return new Vector3(FirstPersonCalibrationValues.Finite(value.x, 0f), FirstPersonCalibrationValues.Finite(value.y, 0f), FirstPersonCalibrationValues.Finite(value.z, 0f));
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x001B3A8D File Offset: 0x001B1C8D
		private static Vector3 ClampVector(Vector3 value, float min, float max)
		{
			value = FirstPersonCalibrationValues.Finite(value);
			return new Vector3(Mathf.Clamp(value.x, min, max), Mathf.Clamp(value.y, min, max), Mathf.Clamp(value.z, min, max));
		}

		// Token: 0x0400476C RID: 18284
		[Tooltip("Camera position correction in metres, relative to the player's rotation (scale independent).")]
		public Vector3 positionOffset;

		// Token: 0x0400476D RID: 18285
		[Tooltip("Additional local camera rotation in degrees: pitch, yaw, roll.")]
		public Vector3 rotationOffset;

		// Token: 0x0400476E RID: 18286
		public float fieldOfView = 70f;

		// Token: 0x0400476F RID: 18287
		public float nearClipPlane = 0.01f;
	}
}
