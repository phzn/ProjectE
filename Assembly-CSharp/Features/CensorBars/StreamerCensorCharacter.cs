using System;
using System.Collections.Generic;
using ProjectExhibitionist;
using UnityEngine;

namespace Features.CensorBars
{
	// Token: 0x020014BC RID: 5308
	[DisallowMultipleComponent]
	public sealed class StreamerCensorCharacter : MonoBehaviour
	{
		// Token: 0x0600A33D RID: 41789 RVA: 0x0043CDB6 File Offset: 0x0043AFB6
		public static void Ensure(ClothingToggleController clothing)
		{
			if (clothing != null && clothing.GetComponent<StreamerCensorCharacter>() == null)
			{
				clothing.gameObject.AddComponent<StreamerCensorCharacter>();
			}
		}

		// Token: 0x0600A33E RID: 41790 RVA: 0x0043CDDC File Offset: 0x0043AFDC
		internal static void Ensure(Animator animator)
		{
			if (animator == null || !animator.isHuman || animator.GetComponentInParent<StreamerCensorCharacter>() != null)
			{
				return;
			}
			StreamerCensorRigData streamerCensorRigData = Resources.Load<StreamerCensorRigData>("StreamerCensorRigData");
			if (streamerCensorRigData == null)
			{
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in animator.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (skinnedMeshRenderer.name == "Body_sotai_NSFW" && skinnedMeshRenderer.sharedMesh == streamerCensorRigData.sourceBody)
				{
					animator.gameObject.AddComponent<StreamerCensorCharacter>();
					return;
				}
			}
		}

		// Token: 0x0600A33F RID: 41791 RVA: 0x0043CE6A File Offset: 0x0043B06A
		private void OnEnable()
		{
			StreamerCensorCharacter.Active.Add(this);
		}

		// Token: 0x0600A340 RID: 41792 RVA: 0x0043CE78 File Offset: 0x0043B078
		private void OnDisable()
		{
			StreamerCensorCharacter.Active.Remove(this);
		}

		// Token: 0x0600A341 RID: 41793 RVA: 0x0043CE88 File Offset: 0x0043B088
		private void OnDestroy()
		{
			StreamerCensorCharacter.Active.Remove(this);
			if (this._probe != null)
			{
				StreamerCensorCharacter.Release(this._probe.gameObject);
			}
			StreamerCensorCharacter.Release(this._root);
			StreamerCensorCharacter.Release(this._bakedBody);
			Mesh[] bakedFuta = this._bakedFuta;
			for (int i = 0; i < bakedFuta.Length; i++)
			{
				StreamerCensorCharacter.Release(bakedFuta[i]);
			}
		}

		// Token: 0x0600A342 RID: 41794 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void Release(Object value)
		{
			if (value == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x0600A343 RID: 41795 RVA: 0x0043CEF4 File Offset: 0x0043B0F4
		private bool Resolve()
		{
			if (this._root != null)
			{
				return this._body != null && this._probe != null;
			}
			this._clothing = base.GetComponent<ClothingToggleController>() ?? base.GetComponentInParent<ClothingToggleController>();
			this._animator = base.GetComponent<Animator>() ?? base.GetComponentInChildren<Animator>(true);
			this._data = Resources.Load<StreamerCensorRigData>("StreamerCensorRigData");
			if (this._animator == null || !this._animator.isHuman || this._data == null || this._data.bodyProbe == null)
			{
				return false;
			}
			this._hips = this._animator.GetBoneTransform(HumanBodyBones.Hips);
			this._chest = this._animator.GetBoneTransform(HumanBodyBones.Chest);
			foreach (Transform transform in this._animator.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == "c_spine_02_bend.x")
				{
					this._chest = transform;
					break;
				}
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._animator.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (skinnedMeshRenderer.name == "Body_sotai_NSFW")
				{
					this._body = skinnedMeshRenderer;
				}
				else if (skinnedMeshRenderer.name == "sao")
				{
					this._futa[0] = skinnedMeshRenderer;
				}
				else if (skinnedMeshRenderer.name == "tama")
				{
					this._futa[1] = skinnedMeshRenderer;
				}
			}
			if (this._body == null || this._hips == null || this._chest == null)
			{
				return false;
			}
			this._root = new GameObject("Censor (Streamer)")
			{
				hideFlags = HideFlags.DontSave
			};
			this._root.transform.SetParent(this._animator.transform, false);
			string[] array = new string[] { "Censor_Top", "Censor_Bottom_Front", "Censor_Bottom_Rear", "Censor_Futa_Penis", "Censor_Futa_Testicles" };
			for (int j = 0; j < this._boxes.Length; j++)
			{
				GameObject gameObject = new GameObject(array[j], new Type[] { typeof(MeshFilter) });
				gameObject.transform.SetParent(this._root.transform, false);
				gameObject.GetComponent<MeshFilter>().sharedMesh = this._data.boxMesh;
				this._boxes[j] = gameObject.transform;
			}
			GameObject gameObject2 = new GameObject("Censor body shape probe")
			{
				hideFlags = HideFlags.DontSave
			};
			gameObject2.transform.SetParent(this._body.transform, false);
			this._probe = gameObject2.AddComponent<SkinnedMeshRenderer>();
			this._probe.enabled = false;
			this._probe.sharedMesh = this._data.bodyProbe;
			this._probe.bones = this._body.bones;
			this._probe.rootBone = this._body.rootBone;
			this._probe.quality = SkinQuality.Bone4;
			this._shapeIndices = new int[this._data.bodyProbe.blendShapeCount];
			for (int k = 0; k < this._shapeIndices.Length; k++)
			{
				this._shapeIndices[k] = this._body.sharedMesh.GetBlendShapeIndex(this._data.bodyProbe.GetBlendShapeName(k));
			}
			this._bakedBody = new Mesh
			{
				name = "Streamer body measurement",
				hideFlags = HideFlags.HideAndDontSave
			};
			for (int l = 0; l < 2; l++)
			{
				this._bakedFuta[l] = new Mesh
				{
					name = "Streamer Futa measurement",
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			return true;
		}

		// Token: 0x0600A344 RID: 41796 RVA: 0x0043D2C4 File Offset: 0x0043B4C4
		internal void Collect(Camera camera, List<StreamerCensorMesh> meshes)
		{
			if (camera == null || !this.Resolve() || !this._animator.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this._clothing != null && this._clothing.HasVisibleFullCoverageOutfit)
			{
				return;
			}
			if (this._body.enabled && this._body.gameObject.activeInHierarchy && (camera.cullingMask & (1 << this._body.gameObject.layer)) != 0)
			{
				this.MeasureBody();
				for (int i = 0; i < 3; i++)
				{
					this.AddBox(i, (i == 0) ? this._chest : this._hips, meshes);
				}
			}
			for (int j = 0; j < 2; j++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = this._futa[j];
				if (!(skinnedMeshRenderer == null) && skinnedMeshRenderer.enabled && skinnedMeshRenderer.gameObject.activeInHierarchy && (camera.cullingMask & (1 << skinnedMeshRenderer.gameObject.layer)) != 0)
				{
					if (!Application.isPlaying || StreamerCensorRenderFeature.IsCapturing || this._futaFrame[j] != Time.frameCount)
					{
						skinnedMeshRenderer.BakeMesh(this._bakedFuta[j], true);
						this._bakedFuta[j].GetVertices(this._points);
						this._valid[j + 3] = false;
						Matrix4x4 matrix4x = this._hips.worldToLocalMatrix * skinnedMeshRenderer.transform.localToWorldMatrix;
						foreach (Vector3 vector in this._points)
						{
							this.Include(j + 3, matrix4x.MultiplyPoint3x4(vector));
						}
						this._futaFrame[j] = Time.frameCount;
					}
					this.AddBox(j + 3, this._hips, meshes);
				}
			}
		}

		// Token: 0x0600A345 RID: 41797 RVA: 0x0043D4B0 File Offset: 0x0043B6B0
		private void MeasureBody()
		{
			if (Application.isPlaying && !StreamerCensorRenderFeature.IsCapturing && this._bodyFrame == Time.frameCount)
			{
				return;
			}
			for (int i = 0; i < this._shapeIndices.Length; i++)
			{
				if (this._shapeIndices[i] >= 0)
				{
					this._probe.SetBlendShapeWeight(i, this._body.GetBlendShapeWeight(this._shapeIndices[i]));
				}
			}
			this._probe.BakeMesh(this._bakedBody, true);
			this._bakedBody.GetVertices(this._points);
			for (int j = 0; j < 3; j++)
			{
				this._valid[j] = false;
			}
			Matrix4x4 matrix4x = this._chest.worldToLocalMatrix * this._probe.transform.localToWorldMatrix;
			Matrix4x4 matrix4x2 = this._hips.worldToLocalMatrix * this._probe.transform.localToWorldMatrix;
			for (int k = 0; k < this._points.Count; k++)
			{
				int num = this._data.regions[k];
				this.Include(num, ((num == 0) ? matrix4x : matrix4x2).MultiplyPoint3x4(this._points[k]));
			}
			this._bodyFrame = Time.frameCount;
		}

		// Token: 0x0600A346 RID: 41798 RVA: 0x0043D5EB File Offset: 0x0043B7EB
		private void Include(int region, Vector3 point)
		{
			if (!this._valid[region])
			{
				this._bounds[region] = new Bounds(point, Vector3.zero);
				this._valid[region] = true;
				return;
			}
			this._bounds[region].Encapsulate(point);
		}

		// Token: 0x0600A347 RID: 41799 RVA: 0x0043D62C File Offset: 0x0043B82C
		private void AddBox(int index, Transform anchor, List<StreamerCensorMesh> meshes)
		{
			if (!this._valid[index])
			{
				return;
			}
			Bounds bounds = this._bounds[index];
			bounds.Expand((index < 3) ? 0.02f : 0.012f);
			Transform transform = this._boxes[index];
			transform.SetPositionAndRotation(anchor.TransformPoint(bounds.center), anchor.rotation);
			transform.localScale = Vector3.Scale(bounds.size, anchor.lossyScale) / Mathf.Max(0.0001f, this._root.transform.lossyScale.x);
			Matrix4x4 matrix4x = anchor.localToWorldMatrix * Matrix4x4.TRS(bounds.center, Quaternion.identity, bounds.size);
			meshes.Add(new StreamerCensorMesh(this._data.boxMesh, matrix4x));
		}

		// Token: 0x0400A702 RID: 42754
		internal static readonly HashSet<StreamerCensorCharacter> Active = new HashSet<StreamerCensorCharacter>();

		// Token: 0x0400A703 RID: 42755
		private ClothingToggleController _clothing;

		// Token: 0x0400A704 RID: 42756
		private Animator _animator;

		// Token: 0x0400A705 RID: 42757
		private StreamerCensorRigData _data;

		// Token: 0x0400A706 RID: 42758
		private SkinnedMeshRenderer _body;

		// Token: 0x0400A707 RID: 42759
		private SkinnedMeshRenderer _probe;

		// Token: 0x0400A708 RID: 42760
		private Transform _chest;

		// Token: 0x0400A709 RID: 42761
		private Transform _hips;

		// Token: 0x0400A70A RID: 42762
		private readonly Transform[] _boxes = new Transform[5];

		// Token: 0x0400A70B RID: 42763
		private readonly Bounds[] _bounds = new Bounds[5];

		// Token: 0x0400A70C RID: 42764
		private readonly bool[] _valid = new bool[5];

		// Token: 0x0400A70D RID: 42765
		private readonly List<Vector3> _points = new List<Vector3>(4096);

		// Token: 0x0400A70E RID: 42766
		private readonly SkinnedMeshRenderer[] _futa = new SkinnedMeshRenderer[2];

		// Token: 0x0400A70F RID: 42767
		private readonly Mesh[] _bakedFuta = new Mesh[2];

		// Token: 0x0400A710 RID: 42768
		private readonly int[] _futaFrame = new int[] { -1, -1 };

		// Token: 0x0400A711 RID: 42769
		private Mesh _bakedBody;

		// Token: 0x0400A712 RID: 42770
		private int[] _shapeIndices;

		// Token: 0x0400A713 RID: 42771
		private GameObject _root;

		// Token: 0x0400A714 RID: 42772
		private int _bodyFrame = -1;
	}
}
