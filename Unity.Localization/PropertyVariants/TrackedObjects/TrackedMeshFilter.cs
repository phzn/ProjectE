using System;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.PropertyVariants.TrackedObjects
{
	// Token: 0x02000102 RID: 258
	[DisplayName("Mesh Filter", null)]
	[CustomTrackedObject(typeof(MeshFilter), false)]
	[Serializable]
	public class TrackedMeshFilter : TrackedObject
	{
		// Token: 0x06000691 RID: 1681 RVA: 0x000168AA File Offset: 0x00014AAA
		public override bool CanTrackProperty(string propertyPath)
		{
			return propertyPath == "m_Mesh";
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000168B8 File Offset: 0x00014AB8
		public override AsyncOperationHandle ApplyLocale(Locale variantLocale, Locale defaultLocale)
		{
			if (base.TrackedProperties.Count == 0)
			{
				return default(AsyncOperationHandle);
			}
			if (this.m_CurrentOperation.IsValid())
			{
				if (!this.m_CurrentOperation.IsDone)
				{
					this.m_CurrentOperation.Completed -= this.MeshOperationCompleted;
				}
				AddressablesInterface.SafeRelease(this.m_CurrentOperation);
				this.m_CurrentOperation = default(AsyncOperationHandle<Mesh>);
			}
			ITrackedProperty trackedProperty = base.TrackedProperties[0];
			UnityObjectProperty unityObjectProperty = trackedProperty as UnityObjectProperty;
			if (unityObjectProperty != null)
			{
				LocaleIdentifier localeIdentifier = ((defaultLocale != null) ? defaultLocale.Identifier : default(LocaleIdentifier));
				Object @object;
				if (unityObjectProperty.GetValue(variantLocale.Identifier, localeIdentifier, out @object))
				{
					this.SetMesh(@object as Mesh);
				}
			}
			else
			{
				LocalizedAssetProperty localizedAssetProperty = trackedProperty as LocalizedAssetProperty;
				if (localizedAssetProperty != null && !localizedAssetProperty.LocalizedObject.IsEmpty)
				{
					this.m_CurrentOperation = localizedAssetProperty.LocalizedObject.LoadAssetAsync<Mesh>();
					if (this.m_CurrentOperation.IsDone)
					{
						this.MeshOperationCompleted(this.m_CurrentOperation);
					}
					else
					{
						if (!localizedAssetProperty.LocalizedObject.ForceSynchronous)
						{
							this.m_CurrentOperation.Completed += this.MeshOperationCompleted;
							return this.m_CurrentOperation;
						}
						AsyncOperationUtility.SynchronousLoad<Mesh>(this.m_CurrentOperation);
						this.MeshOperationCompleted(this.m_CurrentOperation);
					}
				}
			}
			return default(AsyncOperationHandle);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00016A20 File Offset: 0x00014C20
		private void MeshOperationCompleted(AsyncOperationHandle<Mesh> assetOp)
		{
			this.SetMesh(assetOp.Result);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00016A2F File Offset: 0x00014C2F
		private void SetMesh(Mesh mesh)
		{
			((MeshFilter)base.Target).sharedMesh = mesh;
		}

		// Token: 0x04000234 RID: 564
		private const string k_MeshProperty = "m_Mesh";

		// Token: 0x04000235 RID: 565
		private AsyncOperationHandle<Mesh> m_CurrentOperation;
	}
}
