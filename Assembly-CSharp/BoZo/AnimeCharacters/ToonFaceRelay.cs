using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008CC RID: 2252
	public class ToonFaceRelay : MonoBehaviour
	{
		// Token: 0x06003C9D RID: 15517 RVA: 0x001689C1 File Offset: 0x00166BC1
		private void Start()
		{
			this.animator = base.GetComponentInParent<ToonFacialAnimator>();
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x001689CF File Offset: 0x00166BCF
		public void SetFace(int preset)
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.SetFace(preset);
		}

		// Token: 0x04003C9B RID: 15515
		private ToonFacialAnimator animator;
	}
}
