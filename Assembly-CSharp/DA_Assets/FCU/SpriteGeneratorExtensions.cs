using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200034F RID: 847
	public static class SpriteGeneratorExtensions
	{
		// Token: 0x06001669 RID: 5737 RVA: 0x00093228 File Offset: 0x00091428
		public static void SetToObject(this Camera camera, GameObject target)
		{
			target.transform.position = new Vector3((float)((int)target.transform.position.x), (float)((int)target.transform.position.y), (float)((int)target.transform.position.z));
			Renderer component = target.GetComponent<Renderer>();
			Vector3 size = component.bounds.size;
			Vector3 center = component.bounds.center;
			camera.transform.position = new Vector3(center.x, center.y, -1f);
			camera.orthographicSize = Mathf.Max(size.x / (2f * camera.aspect), size.y / 2f);
		}
	}
}
