using System;
using System.Collections.Generic;
using System.Linq;
using DA_Assets.DAI;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000327 RID: 807
	[Serializable]
	public class ProjectCacher : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060015E6 RID: 5606 RVA: 0x0008E11C File Offset: 0x0008C31C
		internal void Cache<T>(T @object)
		{
			try
			{
				FigmaProject figmaProject = (FigmaProject)Convert.ChangeType(@object, typeof(FigmaProject));
				string projectUrl = this.monoBeh.Settings.MainSettings.ProjectUrl;
				RecentProject recentProject = new RecentProject
				{
					Url = projectUrl,
					Name = figmaProject.Name,
					DateTime = DateTime.Now
				};
				List<RecentProject> list = this.GetRecentProjects();
				list.RemoveAll((RecentProject pc) => pc.Url == projectUrl);
				list.Insert(0, recentProject);
				if (list.Count > FcuConfig.RecentProjectsLimit)
				{
					list = list.Take<RecentProject>(FcuConfig.RecentProjectsLimit).ToList<RecentProject>();
				}
				this.SaveAll(list);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x0008E1F8 File Offset: 0x0008C3F8
		public List<RecentProject> GetRecentProjects()
		{
			return new List<RecentProject>();
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0008E1FF File Offset: 0x0008C3FF
		private void SaveAll(List<RecentProject> cachedProjects)
		{
			if (cachedProjects == null)
			{
				cachedProjects = new List<RecentProject>();
			}
			DAJson.ToJson(cachedProjects);
		}
	}
}
