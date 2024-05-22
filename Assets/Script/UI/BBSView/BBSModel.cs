using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class BBSModel : IBBSModel
{


    //private List<IBBSModel.Moment> _moments;

    private IBBSModel.BBS _section;

    public IBBSModel.BBS Section { get => _section; set => _section = value; }

    //public List<IBBSModel.Moment> Moments { get => _moments; set => _moments = value; }

    //public List<IBBSModel.Section> Sections { get => _sections; set => _sections = value; }

    public void SetModel(IBBSModel.BBS section)
    {
        _section = section;
    }

}