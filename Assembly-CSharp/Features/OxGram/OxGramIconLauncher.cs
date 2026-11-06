using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.OxGram
{
	// Token: 0x02000EA4 RID: 3748
	[RequireComponent(typeof(Button))]
	public class OxGramIconLauncher : MonoBehaviour
	{
		// Token: 0x06007099 RID: 28825 RVA: 0x002DB750 File Offset: 0x002D9950
		private void Awake()
		{
			base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OpenOxGram));
		}

		// Token: 0x0600709A RID: 28826 RVA: 0x002DB76E File Offset: 0x002D996E
		private void OpenOxGram()
		{
			if (this.app != null)
			{
				this.app.OpenApp();
				return;
			}
			Debug.LogWarning("[OxGram] Icon launcher has no app reference assigned.");
		}

		// Token: 0x0400734B RID: 29515
		[SerializeField]
		private OxGramApp app;
	}
}
