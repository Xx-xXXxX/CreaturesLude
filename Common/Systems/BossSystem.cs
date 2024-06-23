using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.ModLoader;
using CreaturesLude.Content.NPCs.Bosses;
using CreaturesLude.Content.NPCs.Bosses.Chaos;
using Microsoft.Xna.Framework;
using Terraria;
using WackyBagTr.Common.Systems;
using Microsoft.NET.StringTools;
namespace CreaturesLude.Common.Systems
{
	public class BossSystem : WackyBagTr.Common.Systems.BossSystem
	{
		protected override BossData[] GetBossDatas()
		{
			return [
				new BossData(ModContent.NPCType<ChaosBoss>(),nameof(ChaosBoss),20f,[ModContent.NPCType<ChaosBoss>()],new(customPortrait:
				(sb,rect,color)=>{
					ChaosBoss.DrawBALL(rect.Center.ToVector2(),Math.Min(rect.Width/2,rect.Height/2),color);
				})
				)];
		}
	}
}
