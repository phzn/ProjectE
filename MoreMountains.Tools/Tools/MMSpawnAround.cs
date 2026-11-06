using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000060 RID: 96
	public static class MMSpawnAround
	{
		// Token: 0x06000217 RID: 535 RVA: 0x0000A188 File Offset: 0x00008388
		public static void ApplySpawnAroundProperties(GameObject instantiatedObj, MMSpawnAroundProperties props, Vector3 origin)
		{
			instantiatedObj.transform.position = MMSpawnAround.SpawnAroundPosition(props, origin);
			instantiatedObj.transform.rotation = MMSpawnAround.SpawnAroundRotation(props);
			instantiatedObj.transform.localScale = MMSpawnAround.SpawnAroundScale(props);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000A1C0 File Offset: 0x000083C0
		public static Vector3 SpawnAroundPosition(MMSpawnAroundProperties props, Vector3 origin)
		{
			Vector3 vector;
			if (props.Shape == MMSpawnAroundProperties.MMSpawnAroundShapes.Sphere)
			{
				float num = Random.Range(props.MinimumSphereRadius, props.MaximumSphereRadius);
				vector = Random.insideUnitSphere;
				if (props.ForcePlane)
				{
					vector = Vector3.Cross(vector, props.NormalToSpawnPlane);
				}
				vector.Normalize();
				vector *= num;
			}
			else
			{
				vector = MMSpawnAround.PickPositionInsideCube(props);
				if (props.ForcePlane)
				{
					vector = Vector3.Cross(vector, props.NormalToSpawnPlane);
				}
			}
			float num2 = Random.Range(props.MinimumNormalAxisOffset, props.MaximumNormalAxisOffset);
			if (props.UseNormalAxisOffsetCurve)
			{
				float num3 = 0f;
				if (num2 != 0f)
				{
					if (props.InvertNormalOffsetCurve)
					{
						num3 = MMMaths.Remap(num2, props.MinimumNormalAxisOffset, props.MaximumNormalAxisOffset, 1f, 0f);
					}
					else
					{
						num3 = MMMaths.Remap(num2, props.MinimumNormalAxisOffset, props.MaximumNormalAxisOffset, 0f, 1f);
					}
				}
				float num4 = props.NormalOffsetCurve.Evaluate(num3);
				num4 = MMMaths.Remap(num4, 0f, 1f, props.NormalOffsetCurveRemapZero, props.NormalOffsetCurveRemapOne);
				vector *= num4;
			}
			vector += props.NormalToSpawnPlane.normalized * num2;
			return vector + origin;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000A2F8 File Offset: 0x000084F8
		public static Vector3 PickPositionInsideCube(MMSpawnAroundProperties props)
		{
			int i = 0;
			int num = 1000;
			while (i < num)
			{
				float num2 = Random.Range(0f, props.MaximumCubeBaseSize.x);
				float num3 = Random.Range(0f, props.MaximumCubeBaseSize.y);
				float num4 = Random.Range(0f, props.MaximumCubeBaseSize.z);
				if (num2 >= props.MinimumCubeBaseSize.x || num3 >= props.MinimumCubeBaseSize.y || num4 >= props.MinimumCubeBaseSize.z)
				{
					num2 = ((MMMaths.RollADice(2) > 1) ? (-num2) : num2);
					num3 = ((MMMaths.RollADice(2) > 1) ? (-num3) : num3);
					num4 = ((MMMaths.RollADice(2) > 1) ? (-num4) : num4);
					return new Vector3(num2, num3, num4);
				}
				i++;
			}
			return Vector3.zero;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000A3CC File Offset: 0x000085CC
		public static Vector3 SpawnAroundScale(MMSpawnAroundProperties props)
		{
			return MMMaths.RandomVector3(props.MinimumScale, props.MaximumScale);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000A3DF File Offset: 0x000085DF
		public static Quaternion SpawnAroundRotation(MMSpawnAroundProperties props)
		{
			return Quaternion.Euler(MMMaths.RandomVector3(props.MinimumRotation, props.MaximumRotation));
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000A3F8 File Offset: 0x000085F8
		public static void DrawGizmos(MMSpawnAroundProperties props, Vector3 origin, int quantity, float size, Color gizmosColor)
		{
			Gizmos.color = gizmosColor;
			for (int i = 0; i < quantity; i++)
			{
				Gizmos.DrawCube(MMSpawnAround.SpawnAroundPosition(props, origin), MMSpawnAround.SpawnAroundScale(props) * size);
			}
		}
	}
}
