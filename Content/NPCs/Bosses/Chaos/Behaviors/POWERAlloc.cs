using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.ModLoader;

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors
{
	public class POWERAlloc : NPCBehavior<ModNPC>
	{
		public POWERAlloc(ModNPC modNPC) : base(modNPC)
		{
		}

		public float POWER;

		public void AddPowerNeed((Func<float> Weight, Action<float> Alloc) v)=>
			PowerNeeds.Add(v);

		public readonly List<(Func<float> Weight, Action<float> Alloc)> PowerNeeds=[];

		public override void AI()
		{
			float[] weights=PowerNeeds.Select(x=>x.Weight()).ToArray();
			float sum = weights.Sum();
			float n=POWER/ sum;
			for (int i = 0; i < PowerNeeds.Count; i++)
			{
				PowerNeeds[i].Alloc(n*weights[i]);
			}
			base.AI();
		}
	}
}
