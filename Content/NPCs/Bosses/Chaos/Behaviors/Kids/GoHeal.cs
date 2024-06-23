using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors.Kids
{
	public class GoHeal : NPCBehavior<KidChaosBoss>
	{
		public GoHeal(KidChaosBoss modNPC,INPCBehavior AttackBehavior) : base(modNPC)
		{
			this.AttackBehavior = AttackBehavior;
		}

		public INPCBehavior AttackBehavior { get; }
		public bool Healing;

		public void StartHeal() {
			Healing = true;
			NPC.life = 1;
			NPC.dontTakeDamage = true;
			AttackBehavior.Pause();
		}

		public void StopHeal() {
			Healing = false;
			AttackBehavior.Activate();
			NPC.dontTakeDamage = false;
		}

		public override void AI()
		{
			base.AI();
			if (Healing) {
				ModNPC.OffsetRadius *= 0.95f;
				//ModNPC.TargetVel = ModNPC.Owner.NPC.Center + ModNPC.OffsetPos;
				ModNPC.TargetVel = WackyBagTr.Utilties.Calculates.PredictWithVelDirect(ModNPC.Owner.NPC.Center - NPC.Center + ModNPC.OffsetPos, ModNPC.Owner.NPC.velocity, ModNPC.Speed);
				if (((float)NPC.life / NPC.lifeMax) > MathF.Max(0.2f,(float)ModNPC.Owner.NPC.life/ ModNPC.Owner.NPC.lifeMax)) { 
					StopHeal();
				}
			}
		}
		public override bool CheckDead()
		{
			StartHeal();

			return false;
		}
	}
}
