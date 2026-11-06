using System;
using Invector;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class vUIIndicatorPosition : MonoBehaviour
{
	// Token: 0x060009F2 RID: 2546 RVA: 0x00045788 File Offset: 0x00043988
	private void Start()
	{
		this._camera = Camera.main;
		if (this.canvas == null)
		{
			this.canvas = base.GetComponentInParent<Canvas>();
		}
		if (this.container == null)
		{
			this.container = base.GetComponentInParent<RectTransform>();
		}
		this.rectTransform = base.GetComponent<RectTransform>();
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x000457E0 File Offset: 0x000439E0
	public void Update()
	{
		if (this.canvas && this.referencePosition)
		{
			this.rectTransform.anchoredPosition = this.ClampToWindow();
		}
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00045810 File Offset: 0x00043A10
	private Vector2 ClampToWindow()
	{
		Vector3 vector = this.referencePosition.position - this._camera.transform.position;
		Vector3 vector2 = this._camera.transform.forward.AngleFormOtherDirection(vector.normalized);
		float num = Mathf.Clamp(Mathf.Abs(vector2.y) - 60f, 0f, 20f) / 20f;
		Vector3 position = this.referencePosition.position;
		Vector3 vector3 = position + Quaternion.AngleAxis(-vector2.y, Vector3.up) * vector;
		return this.canvas.WorldToCanvas(Vector3.Lerp(position, vector3, num), this._camera).ClampInsideRectagle(this.container, this.rectTransform.rect.size);
	}

	// Token: 0x04000936 RID: 2358
	public Transform referencePosition;

	// Token: 0x04000937 RID: 2359
	public RectTransform container;

	// Token: 0x04000938 RID: 2360
	public Canvas canvas;

	// Token: 0x04000939 RID: 2361
	protected RectTransform rectTransform;

	// Token: 0x0400093A RID: 2362
	protected Camera _camera;
}
