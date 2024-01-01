using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public interface IMainModel : IModel
{
    [Serializable]
    class Moment
    {
        [JsonProperty("UserName")]
        public string UserName;

        [JsonProperty("Content")]
        public string Content;

        [JsonProperty("PhotoCount")]
        public int PhotoCount;

        [JsonProperty("PhotoUrls")]
        public List<string> PhotoUrls;
    }

    [Serializable]
    class MomentsWrapper
    {
        [JsonProperty("moments")]
        public List<Moment> moments;
    }


    public List<Moment> Moments { get; set; }

    public void SetModel(List<Moment> moments);
}