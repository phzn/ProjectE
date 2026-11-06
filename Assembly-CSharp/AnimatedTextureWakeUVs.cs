using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class AnimatedTextureWakeUVs : MonoBehaviour
{
	// Token: 0x06000A5D RID: 2653 RVA: 0x00047004 File Offset: 0x00045204
	private void Start()
	{
		this._size = new Vector2(1f / (float)this._uvTieX, 1f / (float)this._uvTieY);
		this._myRenderer = base.GetComponent<Renderer>();
		if (this._myRenderer == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00047058 File Offset: 0x00045258
	private void Update()
	{
		int num = (int)(Time.timeSinceLevelLoad * (float)this._fps) % (this._uvTieX * this._uvTieY);
		if (num != this._lastIndex)
		{
			int num2 = num % this._uvTieX;
			int num3 = num / this._uvTieY;
			Vector2 vector = new Vector2((float)num2 * this._size.x, 1f - this._size.y - (float)num3 * this._size.y);
			this._myRenderer.material.SetTextureOffset("_MainTex", vector);
			this._myRenderer.material.SetTextureScale("_MainTex", this._size);
			this._lastIndex = num;
		}
	}

	// Token: 0x04000993 RID: 2451
	public int _uvTieX = 1;

	// Token: 0x04000994 RID: 2452
	public int _uvTieY = 1;

	// Token: 0x04000995 RID: 2453
	public int _fps = 10;

	// Token: 0x04000996 RID: 2454
	private Vector2 _size;

	// Token: 0x04000997 RID: 2455
	private Renderer _myRenderer;

	// Token: 0x04000998 RID: 2456
	private int _lastIndex = -1;
}
