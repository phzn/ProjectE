using System;
using Invector.vCharacterController.vActions;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class vTriggerLadderMiddle : MonoBehaviour
{
	// Token: 0x06000963 RID: 2403 RVA: 0x00043A91 File Offset: 0x00041C91
	private void Start()
	{
		this._collider = base.GetComponent<BoxCollider>();
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00043AA0 File Offset: 0x00041CA0
	private void OnDrawGizmos()
	{
		if (!this.debugMode)
		{
			return;
		}
		if (!this._collider)
		{
			this._collider = base.GetComponent<BoxCollider>();
			return;
		}
		int num = (int)(this._collider.size.y / this.stepHeight);
		for (int i = 0; i < num; i++)
		{
			Gizmos.DrawSphere(this.refTarget.position + this.refTarget.up * (float)i * this.stepHeight, 0.1f);
			Gizmos.DrawLine(this.refTarget.position + this.refTarget.up * (float)i * this.stepHeight, this.refTarget.position + this.refTarget.up * (float)i * this.stepHeight + this.refTarget.forward * 0.5f);
		}
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00043BAC File Offset: 0x00041DAC
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (Vector3.Angle(other.transform.forward, this.refTarget.transform.forward) > this.entryAngleFwd)
			{
				return;
			}
			this.ladderAction = other.GetComponent<vLadderAction>();
			if (this.ladderAction.isUsingLadder)
			{
				return;
			}
			this.ladderAction.TriggerMidAirEnterLadder(this);
		}
	}

	// Token: 0x0400089A RID: 2202
	public Transform refTarget;

	// Token: 0x0400089B RID: 2203
	public float stepHeight = 0.5f;

	// Token: 0x0400089C RID: 2204
	public float entryAngleFwd = 45f;

	// Token: 0x0400089D RID: 2205
	public bool debugMode = true;

	// Token: 0x0400089E RID: 2206
	private vLadderAction ladderAction;

	// Token: 0x0400089F RID: 2207
	internal BoxCollider _collider;
}
