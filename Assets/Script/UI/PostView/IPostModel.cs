using Newtonsoft.Json;
using System.Collections.Generic;
using System;

public interface IPostModel : IModel
{
    IBBSModel.Post Post { get; set; }
}