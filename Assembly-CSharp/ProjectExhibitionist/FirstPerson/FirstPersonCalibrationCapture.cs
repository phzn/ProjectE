using System;
using UnityEngine;

namespace ProjectExhibitionist.FirstPerson
{
	// Token: 0x020009E7 RID: 2535
	[Serializable]
	public sealed class FirstPersonCalibrationCapture
	{
		// Token: 0x060047C0 RID: 18368 RVA: 0x001B3AE4 File Offset: 0x001B1CE4
		public void Sanitize()
		{
			if (this.adjustment == null)
			{
				this.adjustment = new FirstPersonCalibrationValues();
			}
			this.adjustment.Sanitize();
			this.playerPosition = FirstPersonCalibrationValues.Finite(this.playerPosition);
			this.playerEuler = FirstPersonCalibrationValues.Finite(this.playerEuler);
			this.cameraPosition = FirstPersonCalibrationValues.Finite(this.cameraPosition);
			this.cameraEuler = FirstPersonCalibrationValues.Finite(this.cameraEuler);
			this.cameraInPlayerSpace = FirstPersonCalibrationValues.Finite(this.cameraInPlayerSpace);
			this.previousTimeScale = Mathf.Clamp(FirstPersonCalibrationValues.Finite(this.previousTimeScale, 1f), 0f, 100f);
			if (this.context == null)
			{
				this.context = new FirstPersonCalibrationContext();
			}
			this.context.Sanitize();
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x001B3BA7 File Offset: 0x001B1DA7
		public static FirstPersonCalibrationContext CaptureContext(FirstPersonController controller)
		{
			return FirstPersonCalibrationContext.CaptureContext(controller);
		}

		// Token: 0x04004770 RID: 18288
		public int schemaVersion = 1;

		// Token: 0x04004771 RID: 18289
		public string capturedUtc;

		// Token: 0x04004772 RID: 18290
		public string scene;

		// Token: 0x04004773 RID: 18291
		public string situation;

		// Token: 0x04004774 RID: 18292
		public string screenshotFile;

		// Token: 0x04004775 RID: 18293
		public string controllerSettingsJson;

		// Token: 0x04004776 RID: 18294
		public FirstPersonCalibrationValues adjustment = new FirstPersonCalibrationValues();

		// Token: 0x04004777 RID: 18295
		public Vector3 playerPosition;

		// Token: 0x04004778 RID: 18296
		public Vector3 playerEuler;

		// Token: 0x04004779 RID: 18297
		public Vector3 cameraPosition;

		// Token: 0x0400477A RID: 18298
		public Vector3 cameraEuler;

		// Token: 0x0400477B RID: 18299
		public Vector3 cameraInPlayerSpace;

		// Token: 0x0400477C RID: 18300
		public float previousTimeScale = 1f;

		// Token: 0x0400477D RID: 18301
		public FirstPersonCalibrationContext context = new FirstPersonCalibrationContext();
	}
}
