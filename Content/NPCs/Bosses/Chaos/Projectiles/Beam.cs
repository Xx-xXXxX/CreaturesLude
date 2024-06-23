using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Projectiles
{
	public class Beam:ModProjectile
	{
		public static int Length => 5000;

		public Texture2D Texture2D => TextureAssets.Projectile[Type].Value;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = Length;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			///to draw
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			// The beam itself still stops on tiles, but its invisible "source" Projectile ignores them.
			// This prevents the beams from vanishing if the player shoves the Prism into a wall.
			Projectile.tileCollide = false;

			// Using local NPC immunity allows each beam to strike independently from one another.
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.friendly = false;
			Projectile.hostile= true;
			Projectile.timeLeft = 2;
			
		}
		public virtual int ProjFadeTime => 60*3;

		public virtual float EffectScale {
			get {
				var time = Projectile.timeLeft;
				if (time > ProjFadeTime) return 1;
				return time / (float)ProjFadeTime;
			}
		}
		public virtual bool Continue
		{
			get
			{
				return BaseTimeLeft < 0;
			}
		}
		public virtual float BaseWidth => Projectile.ai[1];
		public virtual int BaseTimeLeft => (int)Projectile.ai[0];
		//public float TarWidth=> BaseWidth* EffectScale;

		public virtual float RealWidth {
			get {
				return Projectile.ai[2];
			}
			set=>Projectile.ai[2] = value;
		}
		//public float Length =>Projectile.velocity.Length();


		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (RealWidth < 2f) return false;
			var beamEndPos = Projectile.Center + Projectile.velocity;
			float _e = default;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos,RealWidth, ref _e); ;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (RealWidth < 2f) return false;
			Rectangle headRect = new(0, 4, 22, 18);
			Rectangle bodyRect = new(24, 6, 30, 14);
			Rectangle tailRect = new(56,0,22,26);
			float heightScale =RealWidth/ 14f;
			float rotation = Projectile.rotation;
			Vector2 rotationVec = rotation.ToRotationVector2();
			Vector2 EndPos = Projectile.Center + Projectile.velocity;
			Main.EntitySpriteDraw(Texture2D,
						 Projectile.Center - Main.screenPosition,
						 headRect,
						 Color.White,
						 rotation,
						 new(0, headRect.Height / 2),
						 new Vector2(heightScale, heightScale),
						 SpriteEffects.None);
			Vector2 CurrentPos = Projectile.Center + rotationVec * headRect.Width* heightScale;
			
			Vector2 MoveVec=rotationVec * bodyRect.Width* heightScale;

			while (Vector2.Dot(Projectile.velocity-(CurrentPos + MoveVec-Projectile.Center), Projectile.velocity) > 0) {
				Main.EntitySpriteDraw(Texture2D,
						 CurrentPos-Main.screenPosition,
						 bodyRect,
						 Color.White,
						 rotation,
						 new(0, bodyRect.Height / 2),
						 new Vector2(heightScale, heightScale),
						 SpriteEffects.None);
				CurrentPos += MoveVec;
			}

			Main.EntitySpriteDraw(Texture2D,
						 EndPos-rotationVec*tailRect.Width * heightScale - Main.screenPosition,
						 tailRect,
						 Color.White,
						 rotation,
						 new(0, tailRect.Height / 2),
						 new Vector2(heightScale, heightScale),
						 SpriteEffects.None);

			Main.EntitySpriteDraw(Texture2D,
						 EndPos - rotationVec * tailRect.Width * heightScale -MoveVec - Main.screenPosition,
						 bodyRect,
						 Color.White,
						 rotation,
						 new(0, bodyRect.Height / 2),
						 new Vector2(heightScale, heightScale),
						 SpriteEffects.None);

			return false;
		}

		public override void AI()
		{
			if (Continue && Projectile.timeLeft <= ProjFadeTime) {
				Projectile.timeLeft += 2;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			RealWidth = WackyBagTr.Utils.ValueMoveTo(RealWidth, BaseWidth*EffectScale, 1);

			base.AI();
		}

		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			Projectile.timeLeft = BaseTimeLeft>=0?BaseTimeLeft:2;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return RealWidth < BaseWidth*0.9f ?false:base.CanHitNPC(target);
		}
		public override bool CanHitPlayer(Player target)
		{
			return RealWidth < BaseWidth * 0.9f ? false : base.CanHitPlayer(target);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return false;
		}
	}
}
