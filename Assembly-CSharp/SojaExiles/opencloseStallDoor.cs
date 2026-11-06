using System;
using System.Collections;
using UnityEngine;

namespace SojaExiles
{
	// Token: 0x0200041C RID: 1052
	public class opencloseStallDoor : MonoBehaviour
	{
		// Token: 0x06001AAB RID: 6827 RVA: 0x000A7FF9 File Offset: 0x000A61F9
		private void Start()
		{
			this.open = false;
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x000A8004 File Offset: 0x000A6204
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

		// Token: 0x06001AAD RID: 6829 RVA: 0x000A807B File Offset: 0x000A627B
		private IEnumerator opening()
		{
			MonoBehaviour.print("you are opening the door");
			this.openandclose.Play("OpeningStall");
			this.open = true;
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x000A808A File Offset: 0x000A628A
		private IEnumerator closing()
		{
			MonoBehaviour.print("you are closing the door");
			this.openandclose.Play("ClosingStall");
			this.open = false;
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x04001BAF RID: 7087
		public Animator openandclose;

		// Token: 0x04001BB0 RID: 7088
		public bool open;

		// Token: 0x04001BB1 RID: 7089
		public Transform Player;
	}
}
