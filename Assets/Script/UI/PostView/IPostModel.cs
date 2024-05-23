using Newtonsoft.Json;
using System.Collections.Generic;
using System;

public interface IPostModel : IModel
{
    [Serializable]
    public class Comment
    {
        [JsonProperty("UserName")]
        public string UserName;

        [JsonProperty("Content")]
        public string Content;

        [JsonProperty("CreateTime")]
        public string CreateTime;

        [JsonProperty("CommentId")]
        public int CommentId;
    }

    class CommentWrapper
    {
        [JsonProperty("comments")]
        public List<Comment> comments;
    }

    IBBSModel.Post Post { get; set; }

    public List<Comment> Comments { get; set; }
}