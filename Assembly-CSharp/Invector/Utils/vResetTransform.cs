using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x02000611 RID: 1553
	[vClassHeader("Reset Transform", true, "icon_v2", false, "", useHelpBox = true, helpBoxText = "Use this to Reset transformation values<b><color=red>\nPosition Zero\nRotation Zero\nScale One</color> </b>", openClose = false)]
	public class vResetTransform : vMonoBehaviour
	{
		// Token: 0x060023C7 RID: 9159 RVA: 0x000CA717 File Offset: 0x000C8917
		private void Start()
		{
			if (this.resetPositionOnStart)
			{
				this.ResetPosition();
			}
			if (this.resetRotationOnStart)
			{
				this.ResetRotation();
			}
			if (this.resetScaleOnStart)
			{
				this.ResetScale();
			}
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x000CA743 File Offset: 0x000C8943
		public void ResetRotation()
		{
			if (base.transform.parent)
			{
				base.transform.localEulerAngles = Vector3.zero;
				return;
			}
			base.transform.eulerAngles = Vector3.zero;
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x000CA778 File Offset: 0x000C8978
		public void ResetPosition()
		{
			if (base.transform.parent)
			{
				base.transform.localPosition = Vector3.zero;
				return;
			}
			base.transform.position = Vector3.zero;
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x000CA7AD File Offset: 0x000C89AD
		public void ResetScale()
		{
			base.transform.localScale = Vector3.one;
		}

		// Token: 0x040023CB RID: 9163
		public bool resetPositionOnStart;

		// Token: 0x040023CC RID: 9164
		public bool resetRotationOnStart;

		// Token: 0x040023CD RID: 9165
		public bool resetScaleOnStart;
	}
}
