using System.Collections.Generic;

public class MainModel : IMainModel
{
    private List<IMainModel.Moment> _moments;

    public List<IMainModel.Moment> Moments { get => _moments; set => _moments = value; }

    public void SetModel(List<IMainModel.Moment> moments)
    {
        _moments = moments;
    }
}