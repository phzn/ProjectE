using System;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001B4 RID: 436
	public class PegasusDefaults : ScriptableObject
	{
		// Token: 0x04000D1E RID: 3358
		[Header("POI Selection Ctrl")]
		public KeyCode m_keyPrevPoi = KeyCode.Home;

		// Token: 0x04000D1F RID: 3359
		public KeyCode m_keyNextPoi = KeyCode.End;

		// Token: 0x04000D20 RID: 3360
		[Header("Positioning Ctrl-POI, ShiftCtrl-LookAt")]
		public KeyCode m_keyUp = KeyCode.PageUp;

		// Token: 0x04000D21 RID: 3361
		public KeyCode m_keyDown = KeyCode.PageDown;

		// Token: 0x04000D22 RID: 3362
		public KeyCode m_keyLeft = KeyCode.LeftArrow;

		// Token: 0x04000D23 RID: 3363
		public KeyCode m_keyRight = KeyCode.RightArrow;

		// Token: 0x04000D24 RID: 3364
		public KeyCode m_keyForward = KeyCode.UpArrow;

		// Token: 0x04000D25 RID: 3365
		public KeyCode m_keyBackward = KeyCode.DownArrow;

		// Token: 0x04000D26 RID: 3366
		[Header("Gizmo Sizes")]
		[Range(1f, 6f)]
		public float m_poiGizmoSize = 5f;

		// Token: 0x04000D27 RID: 3367
		[Range(1f, 6f)]
		public float m_lookatGizmoSize = 2f;

		// Token: 0x04000D28 RID: 3368
		[Header("AutoBank Settings")]
		public bool m_autoBankOnByDefault;

		// Token: 0x04000D29 RID: 3369
		[Range(0f, 90f)]
		public float m_maxAutoBankAngle = 45f;

		// Token: 0x04000D2A RID: 3370
		[Range(0f, 90f)]
		public float m_maxAutoBankSpeed = 25f;

		// Token: 0x04000D2B RID: 3371
		[Header("Speed Settings")]
		public PegasusConstants.SpeedType m_flyThroughSpeed = PegasusConstants.SpeedType.Medium;
	}
}
