using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000032 RID: 50
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineBlending.html")]
	[Serializable]
	public sealed class CinemachineBlenderSettings : ScriptableObject
	{
		// Token: 0x0600025F RID: 607 RVA: 0x0001180C File Offset: 0x0000FA0C
		public CinemachineBlendDefinition GetBlendForVirtualCameras(string fromCameraName, string toCameraName, CinemachineBlendDefinition defaultBlend)
		{
			bool flag = false;
			bool flag2 = false;
			CinemachineBlendDefinition cinemachineBlendDefinition = defaultBlend;
			CinemachineBlendDefinition cinemachineBlendDefinition2 = defaultBlend;
			if (this.m_CustomBlends != null)
			{
				for (int i = 0; i < this.m_CustomBlends.Length; i++)
				{
					CinemachineBlenderSettings.CustomBlend customBlend = this.m_CustomBlends[i];
					if (customBlend.m_From == fromCameraName && customBlend.m_To == toCameraName)
					{
						return customBlend.m_Blend;
					}
					if (customBlend.m_From == "**ANY CAMERA**")
					{
						if (!string.IsNullOrEmpty(toCameraName) && customBlend.m_To == toCameraName)
						{
							if (!flag)
							{
								cinemachineBlendDefinition = customBlend.m_Blend;
							}
							flag = true;
						}
						else if (customBlend.m_To == "**ANY CAMERA**")
						{
							defaultBlend = customBlend.m_Blend;
						}
					}
					else if (customBlend.m_To == "**ANY CAMERA**" && !string.IsNullOrEmpty(fromCameraName) && customBlend.m_From == fromCameraName)
					{
						if (!flag2)
						{
							cinemachineBlendDefinition2 = customBlend.m_Blend;
						}
						flag2 = true;
					}
				}
			}
			if (flag)
			{
				return cinemachineBlendDefinition;
			}
			if (flag2)
			{
				return cinemachineBlendDefinition2;
			}
			return defaultBlend;
		}

		// Token: 0x040001C5 RID: 453
		[Tooltip("The array containing explicitly defined blends between two Virtual Cameras")]
		public CinemachineBlenderSettings.CustomBlend[] m_CustomBlends;

		// Token: 0x040001C6 RID: 454
		public const string kBlendFromAnyCameraLabel = "**ANY CAMERA**";

		// Token: 0x020000A6 RID: 166
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		[Serializable]
		public struct CustomBlend
		{
			// Token: 0x0400036E RID: 878
			[Tooltip("When blending from this camera")]
			public string m_From;

			// Token: 0x0400036F RID: 879
			[Tooltip("When blending to this camera")]
			public string m_To;

			// Token: 0x04000370 RID: 880
			[CinemachineBlendDefinitionProperty]
			[Tooltip("Blend curve definition")]
			public CinemachineBlendDefinition m_Blend;
		}
	}
}
