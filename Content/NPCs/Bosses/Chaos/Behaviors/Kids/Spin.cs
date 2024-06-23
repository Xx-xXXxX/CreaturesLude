using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.ModLoader;

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors.Kids
{
	public class Spin : NPCBehavior<KidChaosBoss>
	{
		public Spin(KidChaosBoss modNPC) : base(modNPC)
		{
		}

		public float DeltaRadiusAccBase => 4f;
		public float DeltaRadiusAcc=> DeltaRadiusAccBase;
		public float DeltaRadius;


		public float DeltaTanAccBase => 3f;
		public float DeltaTanAcc=> DeltaTanAccBase;
		public float DeltaTan;

		public float DefaultRadius => 16 * 8f;

		public override void AI()
		{
			base.AI();
			DeltaRadius+= DeltaRadiusAcc * WackyBag.Utils.Calculate.StdGaussian();
			DeltaTan+= DeltaTanAcc * WackyBag.Utils.Calculate.StdGaussian();
			DeltaRadius *= 0.99f;
			DeltaTan *= 0.99f;

			ModNPC.OffsetRadius += DeltaRadius;//
			ModNPC.OffsetDirection += DeltaTan / ModNPC.OffsetRadius;//DeltaDirection * WackyBag.Utils.Calculate.StdGaussian();
			ModNPC.OffsetRadius = (ModNPC.OffsetRadius- DefaultRadius)*0.99f+ DefaultRadius;
		}
	}
}
