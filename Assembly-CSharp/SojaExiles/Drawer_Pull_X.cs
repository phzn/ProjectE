using System;
using System.Collections;
using UnityEngine;

namespace SojaExiles
{
	// Token: 0x0200041F RID: 1055
	public class Drawer_Pull_X : MonoBehaviour
	{
		// Token: 0x06001ABC RID: 6844 RVA: 0x000A81A1 File Offset: 0x000A63A1
		private void Start()
		{
			this.open = false;
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x000A81AC File Offset: 0x000A63AC
		private void OnMouseOver()
		{
			if (this.Player && Vector3.Distance(this.Player.position, base.transform.position) < 10f)
			{
				MonoBehaviour.print("object name");
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

		// Token: 0x06001ABE RID: 6846 RVA: 0x000A822D File Offset: 0x000A642D
		private IEnumerator opening()
		{
			MonoBehaviour.print("you are opening the door");
			this.pull_01.Play("openpull_01");
			this.open = true;
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x000A823C File Offset: 0x000A643C
		private IEnumerator closing()
		{
			MonoBehaviour.print("you are closing the door");
			this.pull_01.Play("closepush_01");
			this.open = false;
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x04001BB8 RID: 7096
		public Animator pull_01;

		// Token: 0x04001BB9 RID: 7097
		public bool open;

		// Token: 0x04001BBA RID: 7098
		public Transform Player;
	}
}
