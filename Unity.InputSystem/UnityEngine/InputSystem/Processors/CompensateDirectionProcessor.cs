using System;
using System.ComponentModel;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x02000101 RID: 257
	[DesignTimeVisible(false)]
	internal class CompensateDirectionProcessor : InputProcessor<Vector3>
	{
		// Token: 0x06000F12 RID: 3858 RVA: 0x00049FC4 File Offset: 0x000481C4
		public override Vector3 Process(Vector3 value, InputControl control)
		{
			if (!InputSystem.settings.compensateForScreenOrientation)
			{
				return value;
			}
			Quaternion quaternion = Quaternion.identity;
			switch (InputRuntime.s_Instance.screenOrientation)
			{
			case ScreenOrientation.PortraitUpsideDown:
				quaternion = Quaternion.Euler(0f, 0f, 180f);
				break;
			case ScreenOrientation.LandscapeLeft:
				quaternion = Quaternion.Euler(0f, 0f, 90f);
				break;
			case ScreenOrientation.LandscapeRight:
				quaternion = Quaternion.Euler(0f, 0f, 270f);
				break;
			}
			return quaternion * value;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0004A050 File Offset: 0x00048250
		public override string ToString()
		{
			return "CompensateDirection()";
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x0004A057 File Offset: 0x00048257
		public override InputProcessor.CachingPolicy cachingPolicy
		{
			get
			{
				return InputProcessor.CachingPolicy.EvaluateOnEveryRead;
			}
		}
	}
}
