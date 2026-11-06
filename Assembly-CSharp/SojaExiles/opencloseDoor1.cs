using System;
using System.Collections;
using UnityEngine;

namespace SojaExiles
{
	// Token: 0x02000419 RID: 1049
	public class opencloseDoor1 : MonoBehaviour
	{
		// Token: 0x06001A9A RID: 6810 RVA: 0x000A7E05 File Offset: 0x000A6005
		private void Start()
		{
			this.open = PoiResidencyPlacement.ReadFlag(this, "door-open");
			if (this.open && this.openandclose1 != null)
			{
				this.openandclose1.Play("Opening 1", 0, 1f);
			}
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x000A7E44 File Offset: 0x000A6044
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

		// Token: 0x06001A9C RID: 6812 RVA: 0x000A7EBB File Offset: 0x000A60BB
		private IEnumerator opening()
		{
			MonoBehaviour.print("you are opening the door");
			this.openandclose1.Play("Opening 1");
			this.open = true;
			PoiResidencyPlacement.WriteFlag(this, "door-open", true);
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000A7ECA File Offset: 0x000A60CA
		private IEnumerator closing()
		{
			MonoBehaviour.print("you are closing the door");
			this.openandclose1.Play("Closing 1");
			this.open = false;
			PoiResidencyPlacement.WriteFlag(this, "door-open", false);
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x04001BA6 RID: 7078
		public Animator openandclose1;

		// Token: 0x04001BA7 RID: 7079
		public bool open;

		// Token: 0x04001BA8 RID: 7080
		public Transform Player;
	}
}
