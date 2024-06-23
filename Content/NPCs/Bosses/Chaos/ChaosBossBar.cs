﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.BigProgressBar;

using Terraria.GameContent;

using Terraria.ModLoader;

using Terraria;
using Microsoft.Xna.Framework;

namespace CreaturesLude.Content.NPCs.Bosses.Chaos
{
	// Showcases a custom boss bar with basic logic for displaying the icon, life, and shields properly.
	// Has no custom texture, meaning it will use the default vanilla boss bar texture
	public class ChaosBossBar : ModBossBar
	{
		private int bossHeadIndex = -1;

		public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
		{
			// Display the previously assigned head index
			if (bossHeadIndex != -1)
			{
				return TextureAssets.NpcHeadBoss[bossHeadIndex];
			}
			return null!;
		}

		public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax)
		{
			// Here the game wants to know if to draw the boss bar or not. Return false whenever the conditions don't apply.
			// If there is no possibility of returning false (or null) the bar will get drawn at times when it shouldn't, so write defensive code!

			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active)
				return false;

			// We assign bossHeadIndex here because we need to use it in GetIconTexture
			bossHeadIndex = npc.GetBossHeadTextureIndex();

			life = npc.life;
			lifeMax = npc.lifeMax;

			

			return true;
		}
	}
}
