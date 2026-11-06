using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200005F RID: 95
	[Serializable]
	public class MMSpawnAroundProperties
	{
		// Token: 0x040002DD RID: 733
		[Header("Shape")]
		[Tooltip("the shape within which objects should spawn")]
		public MMSpawnAroundProperties.MMSpawnAroundShapes Shape;

		// Token: 0x040002DE RID: 734
		[Header("Position")]
		[Tooltip("the minimum distance to the origin of the spawn at which objects can be spawned")]
		[MMEnumCondition("Shape", new int[] { 0 })]
		public float MinimumSphereRadius = 1f;

		// Token: 0x040002DF RID: 735
		[Tooltip("the maximum distance to the origin of the spawn at which objects can be spawned")]
		[MMEnumCondition("Shape", new int[] { 0 })]
		public float MaximumSphereRadius = 2f;

		// Token: 0x040002E0 RID: 736
		[Tooltip("the minimum size of the cube's base")]
		[MMEnumCondition("Shape", new int[] { 1 })]
		public Vector3 MinimumCubeBaseSize = Vector3.one;

		// Token: 0x040002E1 RID: 737
		[Tooltip("the maximum size of the cube's base")]
		[MMEnumCondition("Shape", new int[] { 1 })]
		public Vector3 MaximumCubeBaseSize = new Vector3(2f, 2f, 2f);

		// Token: 0x040002E2 RID: 738
		[Header("Plane")]
		[Tooltip("if this is true, spawn will be constrained to the plane defined by the NormalToSpawnPlane property")]
		public bool ForcePlane = true;

		// Token: 0x040002E3 RID: 739
		[Tooltip("a Vector3 that specifies the normal to the plane you want to spawn objects on (if you want to spawn objects on the x/z plane, the normal to that plane would be the y axis (0,1,0)")]
		public Vector3 NormalToSpawnPlane = Vector3.up;

		// Token: 0x040002E4 RID: 740
		[Header("NormalAxisOffset")]
		[Tooltip("the minimum offset to apply on the normal axis")]
		public float MinimumNormalAxisOffset;

		// Token: 0x040002E5 RID: 741
		[Tooltip("the maximum offset to apply on the normal axis")]
		public float MaximumNormalAxisOffset;

		// Token: 0x040002E6 RID: 742
		[Header("NormalAxisOffsetCurve")]
		[Tooltip("whether or not to use a curve to offset the object's spawn position along the spawn plane")]
		public bool UseNormalAxisOffsetCurve;

		// Token: 0x040002E7 RID: 743
		[Tooltip("a curve used to define how distance to the origin should be altered (potentially above min/max distance)")]
		[MMCondition("UseNormalAxisOffsetCurve", true)]
		public AnimationCurve NormalOffsetCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040002E8 RID: 744
		[Tooltip("the value to which the curve's zero should be remapped to")]
		[MMCondition("UseNormalAxisOffsetCurve", true)]
		public float NormalOffsetCurveRemapZero;

		// Token: 0x040002E9 RID: 745
		[Tooltip("the value to which the curve's one should be remapped to")]
		[MMCondition("UseNormalAxisOffsetCurve", true)]
		public float NormalOffsetCurveRemapOne = 1f;

		// Token: 0x040002EA RID: 746
		[Tooltip("whether or not to invert the curve (horizontally)")]
		[MMCondition("UseNormalAxisOffsetCurve", true)]
		public bool InvertNormalOffsetCurve;

		// Token: 0x040002EB RID: 747
		[Header("Rotation")]
		[Tooltip("the minimum random rotation to apply (in degrees)")]
		public Vector3 MinimumRotation = Vector3.zero;

		// Token: 0x040002EC RID: 748
		[Tooltip("the maximum random rotation to apply (in degrees)")]
		public Vector3 MaximumRotation = Vector3.zero;

		// Token: 0x040002ED RID: 749
		[Header("Scale")]
		[Tooltip("the minimum random scale to apply")]
		public Vector3 MinimumScale = Vector3.one;

		// Token: 0x040002EE RID: 750
		[Tooltip("the maximum random scale to apply")]
		public Vector3 MaximumScale = Vector3.one;

		// Token: 0x020003CE RID: 974
		public enum MMSpawnAroundShapes
		{
			// Token: 0x04001DFA RID: 7674
			Sphere,
			// Token: 0x04001DFB RID: 7675
			Cube
		}
	}
}
