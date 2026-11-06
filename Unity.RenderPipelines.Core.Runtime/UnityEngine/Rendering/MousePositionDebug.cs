using System;
using UnityEngine.InputSystem;

namespace UnityEngine.Rendering
{
	// Token: 0x02000076 RID: 118
	public class MousePositionDebug
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00010455 File Offset: 0x0000E655
		public static MousePositionDebug instance
		{
			get
			{
				if (MousePositionDebug.s_Instance == null)
				{
					MousePositionDebug.s_Instance = new MousePositionDebug();
				}
				return MousePositionDebug.s_Instance;
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001046D File Offset: 0x0000E66D
		public void Build()
		{
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001046F File Offset: 0x0000E66F
		public void Cleanup()
		{
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00010471 File Offset: 0x0000E671
		public Vector2 GetMousePosition(float ScreenHeight, bool sceneView)
		{
			return this.GetInputMousePosition();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00010479 File Offset: 0x0000E679
		private Vector2 GetInputMousePosition()
		{
			if (Pointer.current == null)
			{
				return new Vector2(-1f, -1f);
			}
			return Pointer.current.position.ReadValue();
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x000104A1 File Offset: 0x0000E6A1
		public Vector2 GetMouseClickPosition(float ScreenHeight)
		{
			return Vector2.zero;
		}

		// Token: 0x0400020E RID: 526
		private static MousePositionDebug s_Instance;
	}
}
