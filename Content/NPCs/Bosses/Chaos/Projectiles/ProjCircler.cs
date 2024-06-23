
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

using WackyBag;
using WackyBag.Utils;

using WackyBag.Structures;

using WackyBagTr;
using Terraria.GameContent.Events;
using WackyBagTr.Structures;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Projectiles
{
	public class ProjCircler:BALLProj
	{
		public override Color Color => Color.LightGreen;
		

		public static FloatAsIntSeparator AI2Separator = new((0,200),(0,2),(-128,256));

		public int OwnerId {
			get=>AI2Separator.Get(Projectile.ai[2], 0);
			set=> Projectile.ai[2]=AI2Separator.Set(Projectile.ai[2], 0,value);
		}

		public NPC Owner => Main.npc[OwnerId];
		public override int Radius => 48;
		
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = 2;
			Projectile.penetrate = -1;
		}

		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			if (source is EntitySource_Parent es_p) {
				if (es_p.Entity is NPC npc) {
					OwnerId = npc.whoAmI;
				}
			}
			//Main.NewText($"rotate: {AI2Separator.Get(AI2Int, 1)}");
		}
		public float CircleRadius
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public float CircleDirection {
			get => Projectile.ai[1];
			set=>Projectile.ai[1] = value;
		}
		public float CircleRotateSpeedBase=2;
		public float CircleRotateSpeed {
			get => AI2Separator.Get(Projectile.ai[2], 1).ToDirect() * CircleRotateSpeedBase;
		}
		

		public Vector2 Target =>Owner.Center+CircleDirection.ToRotationVector2()*CircleRadius;

		public float Accelerte => 0.15f*(1+0.5f*AI2Separator.Get(Projectile.ai[2], 2)/256f);
		public float Damping => 0.99f;

		public override void AI()
		{
			base.AI();
			if (!Owner.active) return;
			Projectile.timeLeft = 2;
			Vector2 offset = Target - Projectile.position;
			Projectile.velocity += (offset).WithLength(Accelerte);
			Projectile.velocity *= Damping;
			CircleDirection += CircleRotateSpeed * 1 / offset.Length();
			Projectile.rotation = offset.ToRotation();
		}
		public override void OnKill(int timeLeft)
		{
			base.OnKill(timeLeft);
			//for (int i = 0; i < 8; i++)
			//{
			//	Dust.NewDustPerfect(Projectile.Center,ModContent.DustType<SpamBALLDust>())
			//		.fadeIn=10;
			//}
		}

	}
}
