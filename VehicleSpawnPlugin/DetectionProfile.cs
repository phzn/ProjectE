using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000009 RID: 9
	[CreateAssetMenu(menuName = "VehicleSpawnPlugin/Detection Profile", fileName = "NewDetectionProfile")]
	public class DetectionProfile : ScriptableObject
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00006A80 File Offset: 0x00004C80
		public bool IsBlockingTag(string tag)
		{
			using (List<string>.Enumerator enumerator = this.blockingTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == tag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000083 RID: 131
		[Tooltip("How far ahead the vehicle looks for obstacles.")]
		[Range(2f, 40f)]
		public float detectionRange = 12f;

		// Token: 0x04000084 RID: 132
		[Tooltip("Radius of the forward SphereCast.")]
		[Range(0.2f, 2f)]
		public float detectionRadius = 0.7f;

		// Token: 0x04000085 RID: 133
		[Tooltip("Which layers are considered obstacles. Include vehicles, NPCs, large props.")]
		public LayerMask detectionLayers = -1;

		// Token: 0x04000086 RID: 134
		[Tooltip("Objects with these tags are treated as blocking obstacles (player, NPCs, pedestrians, etc). Vehicles brake to avoid them.")]
		public List<string> blockingTags = new List<string> { "Player", "Pedestrian" };
	}
}
