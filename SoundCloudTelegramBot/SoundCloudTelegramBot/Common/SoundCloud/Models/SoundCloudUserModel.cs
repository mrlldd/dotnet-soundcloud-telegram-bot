using System;

namespace SoundCloudTelegramBot.Common.SoundCloud.Models
{
    public class SoundCloudUserModel
    {
        public string AvatarUrl { get; set; }
        public string City { get; set; }
        public int CommentsCount { get; set; }
        public string CountryCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public CreatorSubscriptionModel CreatorSubscription { get; set; }
        public CreatorSubscriptionModel[] CreatorSubscriptions { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingsCount { get; set; }
        public string FullName { get; set; }
        public int GroupsCount { get; set; }
        public long Id { get; set; }
        public string Kind { get; set; }
        public DateTime LastModified { get; set; }
        public string LastName { get; set; }
        public int LikesCount { get; set; }
        public string Permalink { get; set; }
        public string PermalinkUrl { get; set; }
        public int PlaylistCount { get; set; }
        public int PlaylistLikesCount { get; set; }
        public int? RepostsCount { get; set; }
        public int TrackCount { get; set; }
        public string Uri { get; set; }
        public string Urn { get; set; }
        public string Username { get; set; }
        public bool Verified { get; set; }
        public VisualsModel Visuals { get; set; } // actually idk what is that
    }
}