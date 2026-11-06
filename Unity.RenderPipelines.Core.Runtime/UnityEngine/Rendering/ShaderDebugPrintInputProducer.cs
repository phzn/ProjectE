using System;
using UnityEngine.InputSystem;

namespace UnityEngine.Rendering
{
	// Token: 0x0200007D RID: 125
	public static class ShaderDebugPrintInputProducer
	{
		// Token: 0x060003E3 RID: 995 RVA: 0x00011020 File Offset: 0x0000F220
		public static ShaderDebugPrintInput Get()
		{
			ShaderDebugPrintInput shaderDebugPrintInput = default(ShaderDebugPrintInput);
			shaderDebugPrintInput.pos = Input.mousePosition;
			shaderDebugPrintInput.leftDown = Input.GetMouseButton(0);
			shaderDebugPrintInput.rightDown = Input.GetMouseButton(1);
			shaderDebugPrintInput.middleDown = Input.GetMouseButton(2);
			Mouse current = Mouse.current;
			shaderDebugPrintInput.pos = current.position.ReadValue();
			shaderDebugPrintInput.leftDown = current.leftButton.isPressed;
			shaderDebugPrintInput.rightDown = current.rightButton.isPressed;
			shaderDebugPrintInput.middleDown = current.middleButton.isPressed;
			return shaderDebugPrintInput;
		}
	}
}
