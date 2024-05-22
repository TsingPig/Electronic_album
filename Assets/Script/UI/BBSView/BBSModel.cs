using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class BBSModel : IBBSModel
{

    private IBBSModel.BBS _section;
    private List<IBBSModel.Post> _posts;

    public IBBSModel.BBS Section { get => _section; set => _section = value; }
    public List<IBBSModel.Post> Posts { get => _posts; set => _posts = value; }

    public void SetModel(IBBSModel.BBS section)
    {
        _section = section;
    }

}