using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000E3 RID: 227
	public class MMBoundsExtensions : MonoBehaviour
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x00016994 File Offset: 0x00014B94
		public static Vector3 MMRandomPointInBounds(Bounds bounds)
		{
			return new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00016A00 File Offset: 0x00014C00
		public static Bounds GetColliderBounds(GameObject theObject)
		{
			if (theObject.GetComponent<Collider>() != null)
			{
				return theObject.GetComponent<Collider>().bounds;
			}
			if (theObject.GetComponent<Collider2D>() != null)
			{
				return theObject.GetComponent<Collider2D>().bounds;
			}
			if (theObject.GetComponentInChildren<Collider>() != null)
			{
				Bounds bounds = theObject.GetComponentInChildren<Collider>().bounds;
				foreach (Collider collider in theObject.GetComponentsInChildren<Collider>())
				{
					bounds.Encapsulate(collider.bounds);
				}
				return bounds;
			}
			if (theObject.GetComponentInChildren<Collider2D>() != null)
			{
				Bounds bounds2 = theObject.GetComponentInChildren<Collider2D>().bounds;
				foreach (Collider2D collider2D in theObject.GetComponentsInChildren<Collider2D>())
				{
					bounds2.Encapsulate(collider2D.bounds);
				}
				return bounds2;
			}
			return new Bounds(Vector3.zero, Vector3.zero);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00016ADC File Offset: 0x00014CDC
		public static Bounds GetRendererBounds(GameObject theObject)
		{
			if (theObject.GetComponent<Renderer>() != null)
			{
				return theObject.GetComponent<Renderer>().bounds;
			}
			if (theObject.GetComponentInChildren<Renderer>() != null)
			{
				Bounds bounds = theObject.GetComponentInChildren<Renderer>().bounds;
				foreach (Renderer renderer in theObject.GetComponentsInChildren<Renderer>())
				{
					bounds.Encapsulate(renderer.bounds);
				}
				return bounds;
			}
			return new Bounds(Vector3.zero, Vector3.zero);
		}
	}
}
