using CreaturesLude.Content.NPCs.Bosses.Chaos.Projectiles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

using WackyBagTr.NPCs.Behaviors;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Behaviors
{
	public class BeamHandler : NPCBehavior<ChaosBoss>
	{

		public BeamHandler(ChaosBoss modNPC,DamageSpeedEmitPOWER DSEPOWER) : base(modNPC)
		{
			this.DSEPOWER = DSEPOWER;
		}

		public float Power;
		public int Length => Beam.Length;

		public float BaseBeamWidth=>64;

		public float BaseRotateSpeed => MathF.PI / (8 * 60);

		public float BaseDamage => 400;

		public int Damage =>(int) (BaseDamage * Power*DSEPOWER.DamagePOWER);
		public float RotateSpeed => BaseRotateSpeed * ChaosBoss.SpeedPOWERToSpeedScale(Power * DSEPOWER.SpeedPOWER);
		public float Width => BaseBeamWidth * Power * DSEPOWER.EmitPOWER;

		public DamageSpeedEmitPOWER DSEPOWER { get; }

		public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			writer.Write(ProjIdentity);
		}

		public override void ReciveExtraAI(BinaryReader reader)
		{
			base.ReciveExtraAI(reader);
			ProjIdentity=reader.ReadInt32();
		}

		public int ProjIdentity = -1;
		public Projectile? Proj;
		public override void AI()
		{

			if (Proj==null || !Proj.active || Proj.identity!=ProjIdentity|| Proj.type != ModContent.ProjectileType<Beam>()) {
				ProjIdentity = -1;
				if (Main.netMode == NetmodeID.MultiplayerClient) {
					//ProjId = -1;
					return;
				}

				var ProjId_ = Projectile.NewProjectile(NPC.GetSource_FromAI(),
									  NPC.Center,
									  Main.rand.NextVector2CircularEdge(1, 1) * Length,
									  ModContent.ProjectileType<Beam>(),
									  Damage,
									  0,
									  -1,
									  Main.rand.Next(60 * 10, 60 * 50),
									  Width, 0
									  );
				ProjIdentity = Main.projectile[ProjId_].identity;
				Proj = Main.projectile[ProjId_];
			}
			Proj ??= Main.projectile.FirstOrDefault(p=>p.identity== ProjIdentity);
			if (Proj != null)
			{
				//Projectile Proj = Main.projectile[ProjId];

				float rotation = Proj.velocity.ToRotation();

				rotation = Terraria.Utils.AngleTowards(rotation, (NPC.GetTargetData().Center - NPC.Center).ToRotation(), RotateSpeed);

				float radius = ModNPC.Radius;
				var rotvec = rotation.ToRotationVector2();
				Proj.velocity = rotvec * Length;
				Proj.Center = NPC.Center + rotvec * radius;

				Proj.damage = Damage;
				Proj.ai[1] = Width;

				
			}
			base.AI();
		}
		public override void Pause()
		{
			base.Pause();
			if (Proj != null) {
				Proj.timeLeft = (Proj.ModProjectile as Beam)?.ProjFadeTime??60;
			}
		}
		public override void BossHeadRotation(ref float rotation)
		
		{
			if (Proj == null) return;
			float heightScale = Proj.ai[2] / 14f;
			Texture2D texture= TextureAssets.Projectile[ModContent.ProjectileType<Beam>()].Value;

			Rectangle bodyRect = new(24, 6, 30, 14);

			WackyBagTr.Utilties.Wacky.DrawInMap(texture, Proj.Center,bodyRect,Color.White,Proj.velocity.ToRotation(),new Vector2(0,bodyRect.Height/2),new Vector2(Proj.velocity.Length()/bodyRect.Width, heightScale),SpriteEffects.None);
		}
	}
}
