using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Projectiles
{
	public abstract class BALLProj:ModProjectile
	{
		public override string Texture => ChaosBoss.BALLTexturePath;
		public virtual Color Color { get => Color.White; }
		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			Projectile.rotation = Projectile.velocity.ToRotation();
			//Main.NewText($"rotate: {AI2Separator.Get(AI2Int, 1)}");
			for (int i = 0; i < TrailerCount; i++)
			{
				TrailerPos[i] = Projectile.Center;
			}
		}

		public abstract int Radius { get;  }
		public override void SetDefaults()
		{
			Projectile.width = Radius * 2; // The width of projectile hitbox
			Projectile.height = Radius * 2; // The height of projectile hitbox
			Projectile.aiStyle = -1;
			Projectile.friendly = false; // Can the projectile deal damage to enemies?
			Projectile.hostile = true;
			//Projectile.timeLeft = 60 * 4;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}


		public const int TrailerCount = 12;
		public const int TrailerDrawDelay = 2;
		protected int TrailerCurrent = 0;
		public Vector2[] TrailerPos = new Vector2[TrailerCount];
		public float[] TrailerRot = new float[TrailerCount];

		public override void AI()
		{
			base.AI();
			TrailerCurrent++;
			if (TrailerCurrent >= TrailerCount) TrailerCurrent -= TrailerCount;

			TrailerPos[TrailerCurrent] = Projectile.Center;
			TrailerRot[TrailerCurrent] = Projectile.rotation;

		}

		public override bool PreDraw(ref Color lightColor)
		{
			// WackyBagTr.Utilties.Wacky.DrawLine(Main.spriteBatch,Projectile.Center,Projectile.velocity*30,lineWidth:Radius,color:Color.Red);
			//Terraria.Utils.DrawLine(Main.spriteBatch, Projectile.Center, Projectile.Center + Projectile.velocity * 30, Color.Red,Color.Transparent,Radius*2);
			int i;
			float scaleDelta = (float)TrailerDrawDelay / TrailerCount;
			float scale=1f;
			Color color()=> new Color(Color.R* scale / 255, Color.G* scale / 255, Color.B* scale / 255, Color.A* scale/255);
			//
			for (i = TrailerCurrent; i >=0; i -= TrailerDrawDelay) {
				ChaosBoss.DrawBALL(TrailerPos[i], Radius* scale, color(), TrailerRot[i]);
				scale -= scaleDelta;
			}

			for(i += TrailerCount; i>=TrailerCurrent; i -= TrailerDrawDelay)
			{
				ChaosBoss.DrawBALL(TrailerPos[i], Radius * scale, color(), TrailerRot[i]);
				scale -= scaleDelta;
			}
			//ChaosBoss.DrawBALL(Projectile.Center, Radius, Color.White, Projectile.rotation);
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			var closestPos= Terraria.Utils.ClosestPointInRect(targetHitbox, Projectile.Center);
			return (Projectile.Center-closestPos).Length()<Radius*0.85f;
		}
	}
}
