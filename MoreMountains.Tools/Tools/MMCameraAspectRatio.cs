using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200001C RID: 28
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("More Mountains/Tools/Camera/MMCameraAspectRatio")]
	public class MMCameraAspectRatio : MonoBehaviour
	{
		// Token: 0x0600009C RID: 156 RVA: 0x0000350E File Offset: 0x0000170E
		protected virtual void OnEnable()
		{
			if (this.ApplyAspectRatioOnEnable)
			{
				this.ApplyAspectRatio();
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000351E File Offset: 0x0000171E
		protected virtual void Start()
		{
			if (this.ApplyAspectRatioOnStart)
			{
				this.ApplyAspectRatio();
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003530 File Offset: 0x00001730
		public virtual void ApplyAspectRatio()
		{
			if (this.TargetCamera == null)
			{
				return;
			}
			float num = this._defaultAspect;
			float num2 = 1f;
			float num3 = 1f;
			MMCameraAspectRatio.Modes mode = this.Mode;
			if (mode != MMCameraAspectRatio.Modes.Fixed)
			{
				if (mode == MMCameraAspectRatio.Modes.ScreenRatio)
				{
					num2 = (float)Screen.width;
					num3 = (float)Screen.height;
				}
			}
			else
			{
				num2 = this.FixedAspectRatio.x;
				num3 = this.FixedAspectRatio.y;
			}
			num = ((num3 != 0f) ? (num2 / num3) : this._defaultAspect);
			this.TargetCamera.aspect = num;
		}

		// Token: 0x04000098 RID: 152
		[Header("Camera")]
		[Tooltip("the camera to change the aspect ratio on")]
		public Camera TargetCamera;

		// Token: 0x04000099 RID: 153
		[Tooltip("the mode of choice, fixed will force a specified ratio, while ScreenRatio will adapt the camera's aspect to the current screen ratio")]
		public MMCameraAspectRatio.Modes Mode;

		// Token: 0x0400009A RID: 154
		[Tooltip("in fixed mode, the ratio to apply to the camera")]
		[MMEnumCondition("Mode", new int[] { 0 })]
		public Vector2 FixedAspectRatio = Vector2.zero;

		// Token: 0x0400009B RID: 155
		[Header("Automation")]
		[Tooltip("whether or not to apply the ratio automatically on Start")]
		public bool ApplyAspectRatioOnStart = true;

		// Token: 0x0400009C RID: 156
		[Tooltip("whether or not to apply the ratio automatically on enable")]
		public bool ApplyAspectRatioOnEnable;

		// Token: 0x0400009D RID: 157
		[Header("Debug")]
		[MMInspectorButton("ApplyAspectRatio")]
		public bool ApplyAspectRatioButton;

		// Token: 0x0400009E RID: 158
		protected float _defaultAspect = 1.7777778f;

		// Token: 0x020003A4 RID: 932
		public enum Modes
		{
			// Token: 0x04001D64 RID: 7524
			Fixed,
			// Token: 0x04001D65 RID: 7525
			ScreenRatio
		}
	}
}
