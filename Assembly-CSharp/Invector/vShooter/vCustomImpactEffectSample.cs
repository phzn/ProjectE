using System;
using System.Collections;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x0200064D RID: 1613
	[CreateAssetMenu(menuName = "Invector/Shooter/Impact Effects/New Custom ImpactEffect", fileName = "CustomImpactEffect@")]
	public class vCustomImpactEffectSample : vImpactEffectBase
	{
		// Token: 0x06002523 RID: 9507 RVA: 0x000D034C File Offset: 0x000CE54C
		public override void DoImpactEffect(Vector3 position, Quaternion rotation, GameObject sender, GameObject receiver)
		{
			Vector3 vector = rotation * Vector3.forward;
			GameObject gameObject = new GameObject();
			gameObject.transform.position = position + vector * this.margin;
			switch (this.alignTransform)
			{
			case vCustomImpactEffectSample.Align.Right:
				gameObject.transform.right = vector;
				break;
			case vCustomImpactEffectSample.Align.Forward:
				gameObject.transform.forward = vector;
				break;
			case vCustomImpactEffectSample.Align.UP:
				gameObject.transform.up = vector;
				break;
			case vCustomImpactEffectSample.Align.Left:
				gameObject.transform.right = -vector;
				break;
			case vCustomImpactEffectSample.Align.Back:
				gameObject.transform.forward = -vector;
				break;
			case vCustomImpactEffectSample.Align.Down:
				gameObject.transform.up = -vector;
				break;
			}
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.material = this.material;
			meshRenderer.material.color = this.color;
			gameObject.AddComponent<MeshFilter>().sharedMesh = this.mesh;
			gameObject.transform.localScale = Vector3.one * this.size;
			gameObject.transform.SetParent(vObjectContainer.root, true);
			gameObject.AddComponent<vCustomImpactEffectSample.Fade>().InitFade(meshRenderer, this.fadeSpeed);
		}

		// Token: 0x040024D8 RID: 9432
		public Mesh mesh;

		// Token: 0x040024D9 RID: 9433
		public float size = 0.02f;

		// Token: 0x040024DA RID: 9434
		public float margin = 0.01f;

		// Token: 0x040024DB RID: 9435
		public float fadeSpeed = 0.1f;

		// Token: 0x040024DC RID: 9436
		public vCustomImpactEffectSample.Align alignTransform;

		// Token: 0x040024DD RID: 9437
		[ColorUsage(true, true)]
		public Color color;

		// Token: 0x040024DE RID: 9438
		public Material material;

		// Token: 0x0200064E RID: 1614
		public enum Align
		{
			// Token: 0x040024E0 RID: 9440
			Right,
			// Token: 0x040024E1 RID: 9441
			Forward,
			// Token: 0x040024E2 RID: 9442
			UP,
			// Token: 0x040024E3 RID: 9443
			Left,
			// Token: 0x040024E4 RID: 9444
			Back,
			// Token: 0x040024E5 RID: 9445
			Down
		}

		// Token: 0x0200064F RID: 1615
		public class Fade : MonoBehaviour
		{
			// Token: 0x06002525 RID: 9509 RVA: 0x000D04AC File Offset: 0x000CE6AC
			public void InitFade(Renderer renderer, float fadeSpeed)
			{
				base.StartCoroutine(this.FadeColor(renderer, fadeSpeed));
			}

			// Token: 0x06002526 RID: 9510 RVA: 0x000D04BD File Offset: 0x000CE6BD
			private IEnumerator FadeColor(Renderer renderer, float fadeSpeed)
			{
				float value = 0f;
				while (value < 1f)
				{
					renderer.material.color = Color.Lerp(renderer.material.color, Color.clear, value);
					value += fadeSpeed * Time.deltaTime;
					value = Mathf.Clamp(value, 0f, 1f);
					yield return null;
				}
				Object.Destroy(base.gameObject);
				yield break;
			}
		}
	}
}
