using System;
using UnityEngine.InputSystem;

// Token: 0x02000002 RID: 2
internal static class UISupport
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void Initialize()
	{
		InputSystem.RegisterLayout("\n            {\n                \"name\" : \"VirtualMouse\",\n                \"extend\" : \"Mouse\"\n            }\n        ", null, null);
	}
}
