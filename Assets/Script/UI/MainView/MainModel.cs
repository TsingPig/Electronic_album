using System.Collections.Generic;

public class MainModel : IMainModel
{

    private List<IMainModel.Moment> _moments;

    private List<IMainModel.Section> _sections;

    public List<IMainModel.Moment> Moments { get => _moments; set => _moments = value; }

    public List<IMainModel.Section> Sections { get => _sections; set => _sections = value; }

    public void SetModel(List<IMainModel.Moment> moments, List<IMainModel.Section> sections)
    {
        _moments = moments;
        _sections = sections;
    }
}