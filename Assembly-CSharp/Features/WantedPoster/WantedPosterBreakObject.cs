using System;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B6D RID: 2925
	[DisallowMultipleComponent]
	public class WantedPosterBreakObject : MonoBehaviour
	{
		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06005325 RID: 21285 RVA: 0x00204678 File Offset: 0x00202878
		public Animator Animator
		{
			get
			{
				if (this.animator == null)
				{
					this.animator = base.GetComponentInChildren<Animator>(true);
				}
				return this.animator;
			}
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x0020469B File Offset: 0x0020289B
		private void Start()
		{
			if (WantedPosterPhotoSystem.Instance != null)
			{
				WantedPosterPhotoSystem.Instance.ApplyCurrentMaterialSnapshotTo(base.gameObject);
			}
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x002046BA File Offset: 0x002028BA
		public void PlayBreak()
		{
			if (this.Animator != null)
			{
				this.Animator.SetFloat(this.playParameter, 1f);
			}
		}

		// Token: 0x040053AD RID: 21421
		[Tooltip("Animator del poster-oggetto (con Poster_Main.controller). Se null, cercato nei figli.")]
		[SerializeField]
		private Animator animator;

		// Token: 0x040053AE RID: 21422
		[Tooltip("Parametro float che controlla la velocità della clip di rottura: 0 = fermo sul frame iniziale (poster intero in mano), 1 = scorre (si spezza). PlayBreak lo mette a 1.")]
		[SerializeField]
		private string playParameter = "Play";
	}
}
