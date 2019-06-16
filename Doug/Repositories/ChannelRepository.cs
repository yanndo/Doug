﻿using Doug.Models;
using System.Linq;

namespace Doug.Repositories
{
    public interface IChannelRepository
    {
        string GetAccessToken();
        string GetRemindJob();
        void SetRemindJob(string jobId);
        void SendGambleChallenge(GambleChallenge challenge);
        GambleChallenge GetGambleChallenge(string target);
        void RemoveGambleChallenge(string target);
    }

    public class ChannelRepository : IChannelRepository
    {
        private readonly DougContext _db;

        public ChannelRepository(DougContext dougContext)
        {
            _db = dougContext;
        }

        public string GetAccessToken()
        {
            return _db.Channel.Single().Token;
        }

        public GambleChallenge GetGambleChallenge(string target)
        {
            return _db.GambleChallenges.SingleOrDefault(cha => cha.TargetId == target);
        }

        public string GetRemindJob()
        {
            return _db.Channel.Single().CoffeeRemindJobId;
        }

        public void RemoveGambleChallenge(string target)
        {
            var challenge = _db.GambleChallenges.SingleOrDefault(cha => cha.TargetId == target);
            if (challenge != null)
            {
                _db.GambleChallenges.Remove(challenge);
                _db.SaveChanges();
            }
        }

        public void SendGambleChallenge(GambleChallenge challenge)
        {
            _db.GambleChallenges.Add(challenge);
            _db.SaveChanges();
        }

        public void SetRemindJob(string jobId)
        {
            _db.Channel.Single().CoffeeRemindJobId = jobId;
            _db.SaveChanges();
        }
    }
}
