using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
[DisallowMultipleComponent]
[RequireComponent(typeof(Renderer))]
public sealed class LEDBannerScroller : MonoBehaviour
{
	// Token: 0x060000FC RID: 252 RVA: 0x0000872C File Offset: 0x0000692C
	private void Awake()
	{
		Renderer component = base.GetComponent<Renderer>();
		this.material = component.sharedMaterial;
		this.hasEmission = this.scrollEmission && this.material != null && this.material.HasProperty(LEDBannerScroller.EmissionMapID);
	}

	// Token: 0x060000FD RID: 253 RVA: 0x0000877C File Offset: 0x0000697C
	private void Update()
	{
		if (this.material == null)
		{
			return;
		}
		this.offset.x = Mathf.Repeat(this.offset.x + this.scrollSpeed * Time.deltaTime, 1f);
		this.material.SetTextureOffset(LEDBannerScroller.BaseMapID, this.offset);
		if (this.hasEmission)
		{
			this.material.SetTextureOffset(LEDBannerScroller.EmissionMapID, this.offset);
		}
	}

	// Token: 0x060000FE RID: 254 RVA: 0x000087FC File Offset: 0x000069FC
	private void OnDisable()
	{
		if (this.material == null)
		{
			return;
		}
		Vector2 zero = Vector2.zero;
		this.material.SetTextureOffset(LEDBannerScroller.BaseMapID, zero);
		if (this.hasEmission)
		{
			this.material.SetTextureOffset(LEDBannerScroller.EmissionMapID, zero);
		}
		this.offset = zero;
	}

	// Token: 0x040000C7 RID: 199
	private static readonly int BaseMapID = Shader.PropertyToID("_BaseMap");

	// Token: 0x040000C8 RID: 200
	private static readonly int EmissionMapID = Shader.PropertyToID("_EmissionMap");

	// Token: 0x040000C9 RID: 201
	[Tooltip("Horizontal scroll speed in UV units per second. Negative reverses direction.")]
	[SerializeField]
	private float scrollSpeed = 0.2f;

	// Token: 0x040000CA RID: 202
	[Tooltip("Also scroll the emission map so the LEDs visually match.")]
	[SerializeField]
	private bool scrollEmission = true;

	// Token: 0x040000CB RID: 203
	private Material material;

	// Token: 0x040000CC RID: 204
	private Vector2 offset;

	// Token: 0x040000CD RID: 205
	private bool hasEmission;
}
