using System;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000CF RID: 207
	public interface ITextInputReceiver
	{
		// Token: 0x06000D50 RID: 3408
		void OnTextInput(char character);

		// Token: 0x06000D51 RID: 3409
		void OnIMECompositionChanged(IMECompositionString compositionString);
	}
}
