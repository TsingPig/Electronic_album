using Newtonsoft.Json;
using System;
using System.Collections.Generic;
public interface IBBSModel : IModel
{

    [Serializable]
    class BBS
    {
        [JsonProperty("sectionname")]
        public string sectionname;
    }

    [Serializable]
    class Post
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
    }

    //[Serializable]
    //class Moment
    //{
    //    [JsonProperty("UserName")]
    //    public string UserName;

    //    [JsonProperty("Content")]
    //    public string Content;

    //    [JsonProperty("PhotoCount")]
    //    public int PhotoCount;

    //    [JsonProperty("PhotoUrls")]
    //    public List<string> PhotoUrls;
    //}

    //[Serializable]
    //class MomentsWrapper
    //{
    //    [JsonProperty("moments")]
    //    public List<Moment> moments;
    //}

    //[Serializable]
    //class SectionsWrapper
    //{
    //    [JsonProperty("sections")]
    //    public List<Section> sections;
    //}

    class PostWrapper
    {
        [JsonProperty("posts")]
        public List<Post> posts;
    }

    public IBBSModel.BBS Section { get; set; }
    public List<Post> Posts { get; set; }
    //public List<Moment> Moments { get; set; }
    //public List<Section> Sections { get; set; }

}