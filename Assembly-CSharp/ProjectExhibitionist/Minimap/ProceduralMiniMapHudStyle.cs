using System;
using UnityEngine;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009AF RID: 2479
	[CreateAssetMenu(fileName = "ProceduralMiniMapHudStyle", menuName = "MapGeneration/Minimap HUD Style")]
	public sealed class ProceduralMiniMapHudStyle : ScriptableObject
	{
		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x060045CA RID: 17866 RVA: 0x001A63A7 File Offset: 0x001A45A7
		public Sprite WantedIcon
		{
			get
			{
				return this.wantedIcon;
			}
		}

		// Token: 0x04004567 RID: 17767
		[SerializeField]
		private Sprite wantedIcon;
	}
}
