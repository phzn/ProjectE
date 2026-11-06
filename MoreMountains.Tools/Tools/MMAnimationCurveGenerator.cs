using System;
using System.Reflection;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000158 RID: 344
	public class MMAnimationCurveGenerator : MonoBehaviour
	{
		// Token: 0x0600079E RID: 1950 RVA: 0x00024454 File Offset: 0x00022654
		public virtual void GenerateAnimationCurvesAsset()
		{
			this._scriptableObjectType = Type.GetType("UnityEditor.CurvePresetLibrary, UnityEditor");
			this._addMethodInfo = this._scriptableObjectType.GetMethod("Add");
			ScriptableObject scriptableObject = ScriptableObject.CreateInstance(this._scriptableObjectType);
			foreach (object obj in Enum.GetValues(typeof(MMTween.MMTweenCurve)))
			{
				MMTween.MMTweenCurve mmtweenCurve = (MMTween.MMTweenCurve)obj;
				this.CreateAnimationCurve(scriptableObject, mmtweenCurve, this.Resolution, this.GenerateAntiCurves);
			}
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x000244F8 File Offset: 0x000226F8
		protected virtual void CreateAnimationCurve(ScriptableObject asset, MMTween.MMTweenCurve curveType, int curveResolution, bool anti)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			for (int i = 0; i < curveResolution; i++)
			{
				this._keyframe.time = (float)i / ((float)curveResolution - 1f);
				if (anti)
				{
					this._keyframe.value = MMTween.Tween(this._keyframe.time, 0f, 1f, 1f, 0f, curveType);
				}
				else
				{
					this._keyframe.value = MMTween.Tween(this._keyframe.time, 0f, 1f, 0f, 1f, curveType);
				}
				animationCurve.AddKey(this._keyframe);
			}
			for (int j = 0; j < curveResolution; j++)
			{
				animationCurve.SmoothTangents(j, 0f);
			}
			this._parameters = new object[]
			{
				animationCurve,
				curveType.ToString()
			};
			this._addMethodInfo.Invoke(asset, this._parameters);
		}

		// Token: 0x04000935 RID: 2357
		[Header("Save settings")]
		public string AnimationCurveFilePath = "Assets/MMTools/MMTween/Editor/";

		// Token: 0x04000936 RID: 2358
		public string AnimationCurveFileName = "MMCurves.curves";

		// Token: 0x04000937 RID: 2359
		[Header("Animation Curves")]
		public int Resolution = 50;

		// Token: 0x04000938 RID: 2360
		public bool GenerateAntiCurves;

		// Token: 0x04000939 RID: 2361
		[MMInspectorButton("GenerateAnimationCurvesAsset")]
		public bool GenerateAnimationCurvesButton;

		// Token: 0x0400093A RID: 2362
		protected Type _scriptableObjectType;

		// Token: 0x0400093B RID: 2363
		protected Keyframe _keyframe;

		// Token: 0x0400093C RID: 2364
		protected MethodInfo _addMethodInfo;

		// Token: 0x0400093D RID: 2365
		protected object[] _parameters;
	}
}
