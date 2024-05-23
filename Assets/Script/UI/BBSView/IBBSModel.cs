using Newtonsoft.Json;
using System;
using System.Collections.Generic;




public interface IBBSModel : IModel
{
    [Serializable]
    public class Post
    {
        [JsonProperty("UserName")]
        public string UserName;

        [JsonProperty("Title")]
        public string Title;

        [JsonProperty("Content")]
        public string Content;

        [JsonProperty("PhotoCount")]
        public int PhotoCount;

        [JsonProperty("PhotoUrls")]
        public List<string> PhotoUrls;

        [JsonProperty("CreateTime")]
        public string CreateTime;

        [JsonProperty("PostId")]
        public int PostId;
    }

    [Serializable]
    class BBS
    {
        [JsonProperty("sectionname")]
        public string sectionname;
    }

    class PostWrapper
    {
        [JsonProperty("posts")]
        public List<Post> posts;
    }

    public BBS Section { get; set; }
    public List<Post> Posts { get; set; }


}