using System;
using UnityEngine;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E65 RID: 3685
	[DisallowMultipleComponent]
	public class SelfieCameraOverlayUI : MonoBehaviour
	{
		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x06006E15 RID: 28181 RVA: 0x002C4349 File Offset: 0x002C2549
		public GameObject ActiveRoot
		{
			get
			{
				if (!(this.root != null))
				{
					return base.gameObject;
				}
				return this.root;
			}
		}

		// Token: 0x06006E16 RID: 28182 RVA: 0x002C4368 File Offset: 0x002C2568
		private CanvasGroup ResolveGroup()
		{
			if (this.canvasGroup != null)
			{
				return this.canvasGroup;
			}
			GameObject activeRoot = this.ActiveRoot;
			if (activeRoot != null)
			{
				this.canvasGroup = activeRoot.GetComponent<CanvasGroup>();
			}
			return this.canvasGroup;
		}

		// Token: 0x06006E17 RID: 28183 RVA: 0x002C43AC File Offset: 0x002C25AC
		public void SetVisible(bool visible)
		{
			CanvasGroup canvasGroup = this.ResolveGroup();
			if (canvasGroup != null)
			{
				canvasGroup.alpha = (visible ? 1f : 0f);
				canvasGroup.blocksRaycasts = visible;
				canvasGroup.interactable = visible;
				return;
			}
			GameObject activeRoot = this.ActiveRoot;
			if (activeRoot != null)
			{
				activeRoot.SetActive(visible);
			}
		}

		// Token: 0x040070B0 RID: 28848
		[Tooltip("Root con il CanvasGroup la cui ALPHA controlla la visibilita' (0=nascosto, 1=visibile). Il GO resta attivo. Se null, usa questo GameObject.")]
		public GameObject root;

		// Token: 0x040070B1 RID: 28849
		[Tooltip("CanvasGroup la cui alpha viene messa a 0/1 per nascondere/mostrare l'overlay. Se null, viene cercato su 'root' (o su questo GO).")]
		public CanvasGroup canvasGroup;

		// Token: 0x040070B2 RID: 28850
		[Tooltip("Testo che mostra lo zoom corrente (es. '1.5x'). Aggiornato da PhoneCameraCapture.")]
		public Text zoomLabel;

		// Token: 0x040070B3 RID: 28851
		[Tooltip("Image full-screen per il flash dello scatto (parte trasparente; il codice la fa lampeggiare).")]
		public Image flashImage;

		// Token: 0x040070B4 RID: 28852
		[Tooltip("RectTransform in basso a sinistra verso cui 'vola' la miniatura della foto scattata.")]
		public RectTransform galleryAnchor;

		// Token: 0x040070B5 RID: 28853
		[Tooltip("Bottone shutter (opzionale). Se assegnato e clickToShoot e' attivo su PhoneCameraCapture, cliccarlo scatta la foto. Lo scatto resta comunque su tasto (SPACE).")]
		public Button shutterButton;
	}
}
