using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;

using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.IO;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos.Projectiles
{
	public class NormalProj : BALLProj
	{


		public override int Radius => 16;

		public float Damping => Projectile.ai[0];
		public float RotateSpeed => Projectile.ai[1];
		public float SpeedChange => Projectile.ai[2];

		public override void AI()
		{
			base.AI();
			Projectile.rotation += RotateSpeed;
			Projectile.velocity += Projectile.rotation.ToRotationVector2() * SpeedChange;
			Projectile.velocity *= Damping;
			
		}
		public override void SetDefaults()
		{
			Projectile.timeLeft = 60 * 4;
			base.SetDefaults();
		}


		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.rotation);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.rotation = reader.ReadSingle();
			base.ReceiveExtraAI(reader);
		}

	}
}
