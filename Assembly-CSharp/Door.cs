using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200014A RID: 330
public class Door : MonoBehaviour
{
	// Token: 0x06000A76 RID: 2678 RVA: 0x00047744 File Offset: 0x00045944
	internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
	{
		if (field == UniversityGameplayField.Door_txt)
		{
			return this.txt;
		}
		throw new InvalidOperationException("Unsupported Door POI binding: " + field.ToString());
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0004776E File Offset: 0x0004596E
	internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
	{
		if (field == UniversityGameplayField.Door_txt)
		{
			this.txt = (Text)asset;
			return;
		}
		throw new InvalidOperationException("Unsupported Door POI binding: " + field.ToString());
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x000477A0 File Offset: 0x000459A0
	private void Start()
	{
		this.defaulRot = base.transform.eulerAngles;
		this.openRot = new Vector3(this.defaulRot.x, this.defaulRot.y + this.DoorOpenAngle, this.defaulRot.z);
		if (PoiResidencyPlacement.ReadFlag(this, "door-open"))
		{
			this.open = true;
			base.transform.eulerAngles = this.openRot;
		}
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00047818 File Offset: 0x00045A18
	private void Update()
	{
		if (!this._needsUpdate)
		{
			return;
		}
		Vector3 vector = (this.open ? this.openRot : this.defaulRot);
		Vector3 eulerAngles = base.transform.eulerAngles;
		bool flag = (vector - eulerAngles).sqrMagnitude > 0.01f;
		if (flag)
		{
			base.transform.eulerAngles = Vector3.Slerp(eulerAngles, vector, Time.deltaTime * this.smooth);
		}
		if (this.trig)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				this.open = !this.open;
				PoiResidencyPlacement.WriteFlag(this, "door-open", this.open);
			}
			if (this.txt != null)
			{
				this.txt.text = (this.open ? "Close E" : "Open E");
			}
		}
		if (!this.trig && !flag)
		{
			this._needsUpdate = false;
		}
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x000478FC File Offset: 0x00045AFC
	private void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Player")
		{
			if (this.txt != null)
			{
				this.txt.text = (this.open ? "Open E" : "Close E ");
			}
			this.trig = true;
			this._needsUpdate = true;
		}
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00047956 File Offset: 0x00045B56
	private void OnTriggerExit(Collider coll)
	{
		if (coll.tag == "Player")
		{
			if (this.txt != null)
			{
				this.txt.text = " ";
			}
			this.trig = false;
			this._needsUpdate = true;
		}
	}

	// Token: 0x040009BA RID: 2490
	private bool trig;

	// Token: 0x040009BB RID: 2491
	private bool open;

	// Token: 0x040009BC RID: 2492
	public float smooth = 2f;

	// Token: 0x040009BD RID: 2493
	public float DoorOpenAngle = 90f;

	// Token: 0x040009BE RID: 2494
	private Vector3 defaulRot;

	// Token: 0x040009BF RID: 2495
	private Vector3 openRot;

	// Token: 0x040009C0 RID: 2496
	public Text txt;

	// Token: 0x040009C1 RID: 2497
	private bool _needsUpdate;
}
