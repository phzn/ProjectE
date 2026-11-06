using System;
using System.Collections;
using UnityEngine;

namespace SojaExiles
{
	// Token: 0x0200042B RID: 1067
	public class opencloseWindow1 : MonoBehaviour
	{
		// Token: 0x06001AF7 RID: 6903 RVA: 0x000A88D1 File Offset: 0x000A6AD1
		private void Start()
		{
			this.open = false;
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x000A88DC File Offset: 0x000A6ADC
		private void OnMouseOver()
		{
			if (this.Player && Vector3.Distance(this.Player.position, base.transform.position) < 15f)
			{
				if (!this.open)
				{
					if (Input.GetMouseButtonDown(0))
					{
						base.StartCoroutine(this.opening());
						return;
					}
				}
				else if (this.open && Input.GetMouseButtonDown(0))
				{
					base.StartCoroutine(this.closing());
				}
			}
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x000A8953 File Offset: 0x000A6B53
		private IEnumerator opening()
		{
			MonoBehaviour.print("you are opening the Window");
			this.openandclosewindow1.Play("Openingwindow 1");
			this.open = true;
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000A8962 File Offset: 0x000A6B62
		private IEnumerator closing()
		{
			MonoBehaviour.print("you are closing the Window");
			this.openandclosewindow1.Play("Closingwindow 1");
			this.open = false;
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x04001BDB RID: 7131
		public Animator openandclosewindow1;

		// Token: 0x04001BDC RID: 7132
		public bool open;

		// Token: 0x04001BDD RID: 7133
		public Transform Player;
	}
}
