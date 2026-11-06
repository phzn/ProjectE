using System;
using System.Collections;
using UnityEngine;

namespace SojaExiles
{
	// Token: 0x02000416 RID: 1046
	public class opencloseDoor : MonoBehaviour
	{
		// Token: 0x06001A89 RID: 6793 RVA: 0x000A7C11 File Offset: 0x000A5E11
		private void Start()
		{
			this.open = PoiResidencyPlacement.ReadFlag(this, "door-open");
			if (this.open && this.openandclose != null)
			{
				this.openandclose.Play("Opening", 0, 1f);
			}
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x000A7C50 File Offset: 0x000A5E50
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

		// Token: 0x06001A8B RID: 6795 RVA: 0x000A7CC7 File Offset: 0x000A5EC7
		private IEnumerator opening()
		{
			MonoBehaviour.print("you are opening the door");
			this.openandclose.Play("Opening");
			this.open = true;
			PoiResidencyPlacement.WriteFlag(this, "door-open", true);
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000A7CD6 File Offset: 0x000A5ED6
		private IEnumerator closing()
		{
			MonoBehaviour.print("you are closing the door");
			this.openandclose.Play("Closing");
			this.open = false;
			PoiResidencyPlacement.WriteFlag(this, "door-open", false);
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x04001B9D RID: 7069
		public Animator openandclose;

		// Token: 0x04001B9E RID: 7070
		public bool open;

		// Token: 0x04001B9F RID: 7071
		public Transform Player;
	}
}
