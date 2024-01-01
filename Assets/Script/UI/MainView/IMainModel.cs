using System;
using System.Collections.Generic;

public interface IMainModel : IModel
{
    [Serializable]
    class Moment
    {
        public string UserName;
        public string Content;
        public int PhotoCount;
        public List<string> PhotoUrls;
    }

    public List<Moment> Moments { get; set; }

    public void SetModel(List<Moment> moments);
}