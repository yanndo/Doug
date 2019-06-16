﻿using Doug.Models;
using Doug.Slack;

namespace Doug.Items
{
    public class AwakeningOrb : Item
    {
        public AwakeningOrb()
        {
            Name = "Orb of Awakening";
            Description = "When equipped, this strange orb will notify you privately who flamed you. You must be active to receive the notification.";
            Rarity = Rarity.Rare;
            Icon = ":crystal_ball:";
        }

        public override string OnGettingFlamed(Command command, string slur, ISlackWebApi slack)
        {
            var message = string.Format(DougMessages.UserFlamedYou, Utils.UserMention(command.UserId));

            slack.SendEphemeralMessage(message, command.GetTargetUserId(), command.ChannelId);

            return base.OnGettingFlamed(command, slur, slack);
        }
    }
}