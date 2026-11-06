using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU
{
	// Token: 0x020002BF RID: 703
	[Serializable]
	public class DuplicateFinder : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060013C7 RID: 5063 RVA: 0x0007F5A4 File Offset: 0x0007D7A4
		public async Task SetDuplicateFlags(List<FObject> fobjects)
		{
			IEnumerable<IGrouping<SyncData, FObject>> enumerable = from x in fobjects
				group x by x.Data.RootFrame;
			Dictionary<SyncData, List<int>> hashedNoDubsInFrames = new Dictionary<SyncData, List<int>>();
			foreach (IGrouping<SyncData, FObject> grouping in enumerable)
			{
				IEnumerable<int> enumerable2 = from x in grouping
					group x by x.Data.Hash into x
					select x.First<FObject>().Data.Hash;
				hashedNoDubsInFrames.Add(grouping.Key, enumerable2.ToList<int>());
			}
			await Task.Run(delegate
			{
				List<int> hashesDuplicates = hashedNoDubsInFrames.GetDuplicates<SyncData, int>().ToList<int>();
				Parallel.ForEach<FObject>(fobjects, delegate(FObject fobject)
				{
					if (hashesDuplicates.Contains(fobject.Data.Hash))
					{
						fobject.Data.IsMutual = true;
					}
				});
				int[] hashDuplicates = (from x in (from x in fobjects
						group x by x.Data.Hash).SelectMany<IGrouping<int, FObject>, FObject>((IGrouping<int, FObject> g) => g.Skip<FObject>(1))
					select x.Data.Hash).ToArray<int>();
				Parallel.ForEach<FObject>(fobjects, delegate(FObject fobject)
				{
					if (hashDuplicates.Contains(fobject.Data.Hash))
					{
						fobject.Data.IsDuplicate = true;
					}
				});
			}, this.monoBeh.GetToken(TokenType.Import));
		}
	}
}
