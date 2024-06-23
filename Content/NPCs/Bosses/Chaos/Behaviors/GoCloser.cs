using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;

using WackyBagTr;
using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors
{
	public class GoCloser : NPCBehavior<ChaosBoss>
	{


		public GoCloser(ChaosBoss modNPC,DamageSpeedEmitPOWER power) : base(modNPC)
		{
			this.power = power;
		}


		public float SpeedBase = 10f;
		private readonly DamageSpeedEmitPOWER power;

		public float Speed => ChaosBoss.SpeedPOWERToSpeedScale(power.SpeedPOWER) * SpeedBase;



		public Vector2 TargetVel
		{
			get
			{
				var tar = NPC.GetTargetData();
				if (tar.Invalid) return NPC.Center;
				return WackyBagTr.Utilties.Calculates.PredictWithVelDirect(tar.Center - NPC.Center, tar.Velocity, Speed);
			}
		}

		public override void AI()
		{
			base.AI();
			var tar= NPC.GetTarget();
			if (tar != null) {
				//Main.NewText(Speed);
				
				NPC.velocity = NPC.velocity * 0.98f + TargetVel * 0.02f;
			}
		}
	}
}
