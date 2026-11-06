using System;
using UnityEngine;
using UnityEngine.UI;

namespace Invector
{
	// Token: 0x02000492 RID: 1170
	public class vTutorialTextTrigger : MonoBehaviour
	{
		// Token: 0x06001CB7 RID: 7351 RVA: 0x000B1C3B File Offset: 0x000AFE3B
		protected virtual void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				this.EnableTutorialPanel();
			}
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x000B1C55 File Offset: 0x000AFE55
		public virtual void EnableTutorialPanel()
		{
			this.painel.SetActive(true);
			this._textUI.gameObject.SetActive(true);
			this._textUI.text = this.text;
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000B1C85 File Offset: 0x000AFE85
		protected virtual void OnTriggerExit(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				this.DisableTutorialPanel();
			}
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000B1C9F File Offset: 0x000AFE9F
		public virtual void DisableTutorialPanel()
		{
			this.painel.SetActive(false);
			this._textUI.gameObject.SetActive(false);
			this._textUI.text = " ";
		}

		// Token: 0x04001DFE RID: 7678
		[TextArea(5, 3000)]
		[Multiline]
		public string text;

		// Token: 0x04001DFF RID: 7679
		public Text _textUI;

		// Token: 0x04001E00 RID: 7680
		public GameObject painel;
	}
}
