using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x02000612 RID: 1554
	public class vScaleControl : MonoBehaviour
	{
		// Token: 0x060023CC RID: 9164 RVA: 0x000CA7BF File Offset: 0x000C89BF
		private void Awake()
		{
			this.defaultScale = base.transform.localScale;
			this.targetScale = this.defaultScale;
		}

		// Token: 0x1700064E RID: 1614
		// (set) Token: 0x060023CD RID: 9165 RVA: 0x000CA7DE File Offset: 0x000C89DE
		public float scaleX
		{
			set
			{
				this.targetScale.x = this.defaultScale.x * value;
				base.transform.localScale = this.targetScale;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (set) Token: 0x060023CE RID: 9166 RVA: 0x000CA809 File Offset: 0x000C8A09
		public float scaleY
		{
			set
			{
				this.targetScale.y = this.defaultScale.y * value;
				base.transform.localScale = this.targetScale;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (set) Token: 0x060023CF RID: 9167 RVA: 0x000CA834 File Offset: 0x000C8A34
		public float scaleZ
		{
			set
			{
				this.targetScale.z = this.defaultScale.z * value;
				base.transform.localScale = this.targetScale;
			}
		}

		// Token: 0x040023CE RID: 9166
		private Vector3 targetScale;

		// Token: 0x040023CF RID: 9167
		private Vector3 defaultScale;
	}
}
