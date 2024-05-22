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

    public IBBSModel.BBS Section { get; set; }
    //public List<Moment> Moments { get; set; }
    //public List<Section> Sections { get; set; }

    //public void SetModel(List<Moment> moments, List<Section> bBsTypes);
}