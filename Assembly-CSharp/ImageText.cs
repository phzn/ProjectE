using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200014C RID: 332
public class ImageText : MonoBehaviour
{
	// Token: 0x06000A80 RID: 2688 RVA: 0x00047A8A File Offset: 0x00045C8A
	private void Awake()
	{
		this.mainCamera = Camera.main;
		this.colliderObj = base.GetComponent<Collider>();
		if (this.colliderObj == null)
		{
			Debug.LogError("Este objeto necesita un Collider para detectar el ratón.");
		}
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x00047ABC File Offset: 0x00045CBC
	private void Update()
	{
		if (this.mainCamera == null || this.colliderObj == null)
		{
			return;
		}
		Vector2 vector = ((Mouse.current != null) ? Mouse.current.position.ReadValue() : Vector2.zero);
		RaycastHit raycastHit;
		if (Physics.Raycast(this.mainCamera.ScreenPointToRay(vector), out raycastHit))
		{
			this.triggered = raycastHit.collider == this.colliderObj;
			return;
		}
		this.triggered = false;
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x00047B40 File Offset: 0x00045D40
	private void OnGUI()
	{
		if (!this.triggered || this.crosshair == null)
		{
			return;
		}
		Vector2 vector = new Vector2((float)(Screen.width / 2) - this.labelSize.x / 2f, (float)(Screen.height / 2 + 50));
		GUI.Label(new Rect(vector.x, vector.y, this.labelSize.x, this.labelSize.y), this.promptText);
		GUI.DrawTexture(new Rect((float)(Screen.width - this.crosshair.width) / 2f, (float)(Screen.height - this.crosshair.height) / 2f, (float)this.crosshair.width, (float)this.crosshair.height), this.crosshair);
	}

	// Token: 0x040009C4 RID: 2500
	[SerializeField]
	private Texture2D crosshair;

	// Token: 0x040009C5 RID: 2501
	[SerializeField]
	private string promptText = "Click on mouse";

	// Token: 0x040009C6 RID: 2502
	[SerializeField]
	private Vector2 labelSize = new Vector2(200f, 50f);

	// Token: 0x040009C7 RID: 2503
	private bool triggered;

	// Token: 0x040009C8 RID: 2504
	private Camera mainCamera;

	// Token: 0x040009C9 RID: 2505
	private Collider colliderObj;
}
