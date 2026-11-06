using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001A3 RID: 419
	[AddComponentMenu("")]
	public class DemoPackageTester : MonoBehaviour
	{
		// Token: 0x060009DD RID: 2525 RVA: 0x0002DF4E File Offset: 0x0002C14E
		protected virtual void Awake()
		{
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0002DF50 File Offset: 0x0002C150
		protected virtual void TestForDependencies()
		{
			string text = "";
			bool flag = false;
			bool flag2 = true;
			bool flag3 = true;
			if (this.RequiresCinemachine && !flag2)
			{
				text += "Cinemachine";
			}
			if (this.RequiresTMP && !flag3)
			{
				if (text != "")
				{
					text += ", ";
				}
				text += "TextMeshPro";
			}
			if (this.RequiresPostProcessing && !flag)
			{
				if (text != "")
				{
					text += ", ";
				}
				text += "PostProcessing";
			}
		}

		// Token: 0x04000C1C RID: 3100
		[MMFInformation("This component is only used to display an error in the console in case dependencies for this demo haven't been installed. You can safely remove it if you want, and typically you wouldn't want to keep that in your own game.", MMFInformationAttribute.InformationType.Warning, false)]
		public bool RequiresPostProcessing;

		// Token: 0x04000C1D RID: 3101
		public bool RequiresTMP;

		// Token: 0x04000C1E RID: 3102
		public bool RequiresCinemachine;
	}
}
