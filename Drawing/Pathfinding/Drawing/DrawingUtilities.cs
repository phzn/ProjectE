using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x0200004C RID: 76
	public static class DrawingUtilities
	{
		// Token: 0x0600027D RID: 637 RVA: 0x0000B860 File Offset: 0x00009A60
		public static Bounds BoundsFrom(GameObject gameObject)
		{
			return DrawingUtilities.BoundsFrom(gameObject.transform);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000B870 File Offset: 0x00009A70
		public static Bounds BoundsFrom(Transform transform)
		{
			transform.gameObject.GetComponents<Component>(DrawingUtilities.componentBuffer);
			Bounds bounds = new Bounds(transform.position, Vector3.zero);
			for (int i = 0; i < DrawingUtilities.componentBuffer.Count; i++)
			{
				Component component = DrawingUtilities.componentBuffer[i];
				Collider collider = component as Collider;
				if (collider != null)
				{
					bounds.Encapsulate(collider.bounds);
				}
				else
				{
					Collider2D collider2D = component as Collider2D;
					if (collider2D != null)
					{
						bounds.Encapsulate(collider2D.bounds);
					}
					else
					{
						MeshRenderer meshRenderer = component as MeshRenderer;
						if (meshRenderer != null)
						{
							bounds.Encapsulate(meshRenderer.bounds);
						}
						else
						{
							SpriteRenderer spriteRenderer = component as SpriteRenderer;
							if (spriteRenderer != null)
							{
								bounds.Encapsulate(spriteRenderer.bounds);
							}
						}
					}
				}
			}
			DrawingUtilities.componentBuffer.Clear();
			int childCount = transform.childCount;
			for (int j = 0; j < childCount; j++)
			{
				bounds.Encapsulate(DrawingUtilities.BoundsFrom(transform.GetChild(j)));
			}
			return bounds;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000B968 File Offset: 0x00009B68
		public static Bounds BoundsFrom(List<Vector3> points)
		{
			if (points.Count == 0)
			{
				throw new ArgumentException("At least 1 point is required");
			}
			Vector3 vector = points[0];
			Vector3 vector2 = points[0];
			for (int i = 0; i < points.Count; i++)
			{
				vector = Vector3.Min(vector, points[i]);
				vector2 = Vector3.Max(vector2, points[i]);
			}
			return new Bounds((vector2 + vector) * 0.5f, (vector2 - vector) * 0.5f);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000B9EC File Offset: 0x00009BEC
		public static Bounds BoundsFrom(Vector3[] points)
		{
			if (points.Length == 0)
			{
				throw new ArgumentException("At least 1 point is required");
			}
			Vector3 vector = points[0];
			Vector3 vector2 = points[0];
			for (int i = 0; i < points.Length; i++)
			{
				vector = Vector3.Min(vector, points[i]);
				vector2 = Vector3.Max(vector2, points[i]);
			}
			return new Bounds((vector2 + vector) * 0.5f, (vector2 - vector) * 0.5f);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000BA6C File Offset: 0x00009C6C
		public static Bounds BoundsFrom(NativeArray<float3> points)
		{
			if (points.Length == 0)
			{
				throw new ArgumentException("At least 1 point is required");
			}
			float3 @float = points[0];
			float3 float2 = points[0];
			for (int i = 0; i < points.Length; i++)
			{
				@float = math.min(@float, points[i]);
				float2 = math.max(float2, points[i]);
			}
			return new Bounds((float2 + @float) * 0.5f, (float2 - @float) * 0.5f);
		}

		// Token: 0x04000134 RID: 308
		private static List<Component> componentBuffer = new List<Component>();
	}
}
