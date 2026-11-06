using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200014B RID: 331
public class CrosshairRaycast : MonoBehaviour
{
	// Token: 0x06000A7D RID: 2685 RVA: 0x000479B4 File Offset: 0x00045BB4
	private void OnGUI()
	{
		GUI.DrawTexture(this.position, this.crosshair);
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x000479C8 File Offset: 0x00045BC8
	private void Update()
	{
		this.position = new Rect((float)(Screen.width - this.crosshair.width) / 2f, (float)(Screen.height - this.crosshair.height) / 2f, (float)this.crosshair.width, (float)this.crosshair.height);
		if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
		{
			Vector2 vector = Mouse.current.position.ReadValue();
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(vector), out raycastHit, float.PositiveInfinity))
			{
				InteractiveObject component = raycastHit.collider.GetComponent<InteractiveObject>();
				if (component != null)
				{
					component.TrigegrInteraction();
				}
			}
		}
	}

	// Token: 0x040009C2 RID: 2498
	public Texture2D crosshair;

	// Token: 0x040009C3 RID: 2499
	private Rect position;
}
