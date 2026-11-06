using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class vDisplayValueFade : MonoBehaviour
{
	// Token: 0x06000A34 RID: 2612 RVA: 0x00046523 File Offset: 0x00044723
	private void Awake()
	{
		this.group.alpha = 0f;
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x00046538 File Offset: 0x00044738
	public void Update()
	{
		if (!(this.rotateTransform == null))
		{
			base.transform.Translate(Vector3.up * this.upSpeed * Time.deltaTime);
			base.transform.forward = this.rotateTransform.position - base.transform.position;
			this.currentTime += Time.deltaTime;
			float num = this.currentTime / this.timeToDestroy;
			if (this.group)
			{
				this.group.alpha = this.groupAlphaCurve.Evaluate(1f - num);
			}
			if (this.currentTime >= this.timeToDestroy)
			{
				Object.Destroy(base.gameObject);
			}
			return;
		}
		if (Camera.current)
		{
			this.rotateTransform = Camera.current.transform;
			base.transform.forward = this.rotateTransform.position - base.transform.position;
			this.group.alpha = 1f;
			return;
		}
		this.group.alpha = 0f;
	}

	// Token: 0x04000965 RID: 2405
	public CanvasGroup group;

	// Token: 0x04000966 RID: 2406
	public AnimationCurve groupAlphaCurve;

	// Token: 0x04000967 RID: 2407
	public float upSpeed;

	// Token: 0x04000968 RID: 2408
	public float timeToDestroy = 4f;

	// Token: 0x04000969 RID: 2409
	private float currentTime;

	// Token: 0x0400096A RID: 2410
	private Transform rotateTransform;
}
