﻿using HelpSense.API.Features;
using Hints;
using HintServiceMeow.UI.Extension;
using MEC;
using NorthwoodLib.Pools;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpSense.Helper.Misc
{
    public class RespawnHelper
    {
        public static IEnumerator<float> TimerCoroutine()
        {
            do
            {
                yield return Timing.WaitForSeconds(1f);

                List<Player> spectators = ListPool<Player>.Shared.Rent(ReferenceHub.AllHubs.Select(Player.Get).Where(x => !x.IsServer && x.Role == RoleTypeId.Spectator));
                string text = TimerView.Current.GetText(spectators.Count);

                foreach (Player player in spectators)
                {
                    if (player.Role == RoleTypeId.Overwatch && Plugin.Instance.Config.HideTimerForOverwatch || API.API.TimerHidden.Contains(player.UserId))
                        continue;

                    ShowHint(player, text, 1.25f);
                }

                ListPool<Player>.Shared.Return(spectators);
            } while (!RoundSummary.singleton._roundEnded);
        }

        public static void ShowHint(Player player, string message, float duration = 3f)
        {
            player.ReceiveHint(message, duration);//Use compatibility adapter
        }
    }
}
