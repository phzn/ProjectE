using System;
using Unity.Entities.Serialization;
using Unity.Loading;
using UnityEngine.SceneManagement;

namespace Unity.Entities.Content
{
	// Token: 0x020004BF RID: 1215
	[Serializable]
	public struct WeakObjectSceneReference : IEquatable<WeakObjectSceneReference>
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06001D41 RID: 7489 RVA: 0x000662F6 File Offset: 0x000644F6
		public bool IsReferenceValid
		{
			get
			{
				return this.Id.IsValid;
			}
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x00066308 File Offset: 0x00064508
		public Scene LoadAsync(ContentSceneParameters loadParams)
		{
			return RuntimeContentManager.LoadSceneAsync(this.Id, loadParams);
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x00066316 File Offset: 0x00064516
		public void Unload(ref Scene scene)
		{
			RuntimeContentManager.UnloadScene(ref scene);
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0006631E File Offset: 0x0006451E
		public override string ToString()
		{
			return string.Format("WeakSceneReference -> {0}", this.Id);
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x00066335 File Offset: 0x00064535
		public bool Equals(WeakObjectSceneReference other)
		{
			return this.Id.Equals(other.Id);
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x00066348 File Offset: 0x00064548
		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06001D47 RID: 7495 RVA: 0x0006635B File Offset: 0x0006455B
		[Obsolete("This property is no longer valid.  Check the loading status of the scene returned from LoadAsync.")]
		public SceneLoadingStatus LoadingStatus
		{
			get
			{
				return SceneLoadingStatus.Failed;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06001D48 RID: 7496 RVA: 0x00066360 File Offset: 0x00064560
		[Obsolete("This property is no longer valid.  Use the scene returned from LoadAsync.")]
		public Scene SceneResult
		{
			get
			{
				return default(Scene);
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x00066378 File Offset: 0x00064578
		[Obsolete("This property is no longer valid.  The scene file does not exist in all cases (e.g. play mode).")]
		public ContentSceneFile SceneFileResult
		{
			get
			{
				return default(ContentSceneFile);
			}
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x00002287 File Offset: 0x00000487
		[Obsolete("Release has been replaced with Unload(ref Scene scene).  You will need to use the scene returned from LoadAsync to unload the scene and its resources.")]
		public void Release()
		{
		}

		// Token: 0x04000A84 RID: 2692
		public UntypedWeakReferenceId Id;
	}
}
