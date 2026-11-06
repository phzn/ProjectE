using System;
using System.Collections;
using UnityEngine;

namespace SojaExiles
{
	// Token: 0x02000422 RID: 1058
	public class Drawer_Pull_Z : MonoBehaviour
	{
		// Token: 0x06001ACD RID: 6861 RVA: 0x000A8355 File Offset: 0x000A6555
		private void Start()
		{
			this.open = false;
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x000A8360 File Offset: 0x000A6560
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

		// Token: 0x06001ACF RID: 6863 RVA: 0x000A83E1 File Offset: 0x000A65E1
		private IEnumerator opening()
		{
			MonoBehaviour.print("you are opening the door");
			this.pull.Play("openpull");
			this.open = true;
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000A83F0 File Offset: 0x000A65F0
		private IEnumerator closing()
		{
			MonoBehaviour.print("you are closing the door");
			this.pull.Play("closepush");
			this.open = false;
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x04001BC1 RID: 7105
		public Animator pull;

		// Token: 0x04001BC2 RID: 7106
		public bool open;

		// Token: 0x04001BC3 RID: 7107
		public Transform Player;
	}
}
