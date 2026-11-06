using System;
using System.Collections;
using UnityEngine;

namespace SojaExiles
{
	// Token: 0x02000428 RID: 1064
	public class opencloseWindow : MonoBehaviour
	{
		// Token: 0x06001AE6 RID: 6886 RVA: 0x000A8728 File Offset: 0x000A6928
		private void Start()
		{
			this.open = false;
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x000A8734 File Offset: 0x000A6934
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

		// Token: 0x06001AE8 RID: 6888 RVA: 0x000A87AB File Offset: 0x000A69AB
		private IEnumerator opening()
		{
			MonoBehaviour.print("you are opening the Window");
			this.openandclosewindow.Play("Openingwindow");
			this.open = true;
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x000A87BA File Offset: 0x000A69BA
		private IEnumerator closing()
		{
			MonoBehaviour.print("you are closing the Window");
			this.openandclosewindow.Play("Closingwindow");
			this.open = false;
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x04001BD2 RID: 7122
		public Animator openandclosewindow;

		// Token: 0x04001BD3 RID: 7123
		public bool open;

		// Token: 0x04001BD4 RID: 7124
		public Transform Player;
	}
}
