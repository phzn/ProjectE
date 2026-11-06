using System;
using System.ComponentModel;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Processors
{
	// Token: 0x02000102 RID: 258
	[DesignTimeVisible(false)]
	internal class CompensateRotationProcessor : InputProcessor<Quaternion>
	{
		// Token: 0x06000F16 RID: 3862 RVA: 0x0004A064 File Offset: 0x00048264
		public override Quaternion Process(Quaternion value, InputControl control)
		{
			if (!InputSystem.settings.compensateForScreenOrientation)
			{
				return value;
			}
			Quaternion identity = Quaternion.identity;
			switch (InputRuntime.s_Instance.screenOrientation)
			{
			case ScreenOrientation.PortraitUpsideDown:
				identity = new Quaternion(0f, 0f, 1f, 0f);
				break;
			case ScreenOrientation.LandscapeLeft:
				identity = new Quaternion(0f, 0f, 0.70710677f, -0.70710677f);
				break;
			case ScreenOrientation.LandscapeRight:
				identity = new Quaternion(0f, 0f, -0.70710677f, -0.70710677f);
				break;
			}
			return value * identity;
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0004A102 File Offset: 0x00048302
		public override string ToString()
		{
			return "CompensateRotation()";
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x0004A109 File Offset: 0x00048309
		public override InputProcessor.CachingPolicy cachingPolicy
		{
			get
			{
				return InputProcessor.CachingPolicy.EvaluateOnEveryRead;
			}
		}
	}
}
