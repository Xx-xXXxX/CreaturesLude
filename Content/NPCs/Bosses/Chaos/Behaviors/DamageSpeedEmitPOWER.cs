using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors
{
	public class DamageSpeedEmitPOWER:NPCBehavior<ChaosBoss>
	{
		public DamageSpeedEmitPOWER(ChaosBoss modNPC) : base(modNPC)
		{
		}

		public float DamagePOWERWeight = 1;
		public float SpeedPOWERWeight = 1;
		public float EmitPOWERWeight = 1;

		public float POWERWeight => 3 / (DamagePOWERWeight + SpeedPOWERWeight + EmitPOWERWeight);

		public float DamagePOWER => DamagePOWERWeight * POWERWeight;
		public float SpeedPOWER => SpeedPOWERWeight * POWERWeight;
		public float EmitPOWER => EmitPOWERWeight * POWERWeight;

	}
}
