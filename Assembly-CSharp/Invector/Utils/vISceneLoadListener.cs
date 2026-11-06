using System;

namespace Invector.Utils
{
	// Token: 0x02000601 RID: 1537
	public interface vISceneLoadListener
	{
		// Token: 0x0600237C RID: 9084
		void OnStartLoadScene(string sceneName);

		// Token: 0x0600237D RID: 9085
		void OnFinishLoadScene(string sceneName);
	}
}
