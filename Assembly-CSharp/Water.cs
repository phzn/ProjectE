using System;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class Water : MonoBehaviour
{
	// Token: 0x06000A6D RID: 2669 RVA: 0x0004741E File Offset: 0x0004561E
	private void Start()
	{
		this._renderer = base.GetComponent<Renderer>();
		if (this._renderer == null)
		{
			base.enabled = false;
			return;
		}
		this._material = this._renderer.material;
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00047454 File Offset: 0x00045654
	private void Update()
	{
		if (this._material == null)
		{
			return;
		}
		this.offsetX += Time.deltaTime * this.speed.x;
		this.offsetY += Time.deltaTime * this.speed.y;
		this.secondaryOffsetX += Time.deltaTime * this.secondarySpeed.x;
		this.secondaryOffsetY += Time.deltaTime * this.secondarySpeed.y;
		this._material.mainTextureOffset = new Vector2(this.offsetX, this.offsetY);
		this._material.SetTextureOffset("_DetailAlbedoMap", new Vector2(this.secondaryOffsetX, this.secondaryOffsetY));
	}

	// Token: 0x040009A3 RID: 2467
	public Vector2 speed;

	// Token: 0x040009A4 RID: 2468
	public Vector2 secondarySpeed;

	// Token: 0x040009A5 RID: 2469
	private float offsetX;

	// Token: 0x040009A6 RID: 2470
	private float offsetY;

	// Token: 0x040009A7 RID: 2471
	private float secondaryOffsetX;

	// Token: 0x040009A8 RID: 2472
	private float secondaryOffsetY;

	// Token: 0x040009A9 RID: 2473
	private Renderer _renderer;

	// Token: 0x040009AA RID: 2474
	private Material _material;
}
