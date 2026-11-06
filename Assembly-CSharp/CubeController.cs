using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200016D RID: 365
public class CubeController : MonoBehaviour
{
	// Token: 0x06000BB4 RID: 2996 RVA: 0x00050E68 File Offset: 0x0004F068
	private void Start()
	{
		for (int i = 0; i < this.cubeColors.Length; i++)
		{
			this.cubeColors[i].buttonInfo.id = i;
			UltimateRadialMenu.RegisterButton("ObjectExample", new Action<int>(this.UpdateCubeColor), this.cubeColors[i].buttonInfo, -1);
		}
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00050EC0 File Offset: 0x0004F0C0
	private void Update()
	{
		Vector2 vector = Vector2.zero;
		Mouse device = InputSystem.GetDevice<Mouse>();
		if (device == null)
		{
			return;
		}
		vector = device.position.ReadValue();
		bool wasPressedThisFrame = device.leftButton.wasPressedThisFrame;
		bool wasReleasedThisFrame = device.leftButton.wasReleasedThisFrame;
		if (wasPressedThisFrame)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(vector), out raycastHit))
			{
				this.onMouseDownTransform = raycastHit.transform;
			}
			else if (UltimateRadialMenu.ReturnComponent("ObjectExample").IsEnabled && UltimateRadialMenu.ReturnComponent("ObjectExample").CurrentButtonIndex < 0)
			{
				UltimateRadialMenu.ReturnComponent("ObjectExample").Disable();
			}
		}
		RaycastHit raycastHit2;
		if (wasReleasedThisFrame && Physics.Raycast(Camera.main.ScreenPointToRay(vector), out raycastHit2) && raycastHit2.transform == this.onMouseDownTransform)
		{
			this.selectedRenderer = raycastHit2.transform.GetComponent<Renderer>();
			Vector3 vector2 = Camera.main.WorldToScreenPoint(raycastHit2.transform.position);
			UltimateRadialMenu.ReturnComponent("ObjectExample").SetPosition(vector2, false);
			if (!UltimateRadialMenu.ReturnComponent("ObjectExample").IsEnabled)
			{
				UltimateRadialMenu.ReturnComponent("ObjectExample").Enable();
			}
		}
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00050FED File Offset: 0x0004F1ED
	public void UpdateCubeColor(int id)
	{
		if (this.selectedRenderer == null)
		{
			return;
		}
		this.selectedRenderer.material.color = this.cubeColors[id].color;
	}

	// Token: 0x04000B7C RID: 2940
	public CubeController.CubeColor[] cubeColors;

	// Token: 0x04000B7D RID: 2941
	private Transform onMouseDownTransform;

	// Token: 0x04000B7E RID: 2942
	private Renderer selectedRenderer;

	// Token: 0x0200016E RID: 366
	[Serializable]
	public class CubeColor
	{
		// Token: 0x04000B7F RID: 2943
		public Color color = Color.white;

		// Token: 0x04000B80 RID: 2944
		public UltimateRadialButtonInfo buttonInfo;
	}
}
