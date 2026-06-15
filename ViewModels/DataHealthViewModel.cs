namespace AspNetWeek4.Mvc.ViewModels;

public class DataHealthViewModel
{
    public List<DataHealthCheck> Checks { get; set; } = new();
}

public class DataHealthCheck
{
    public string Check    { get; set; } = "";
    public string Expected { get; set; } = "";
    public string Actual   { get; set; } = "";
    public string Status   { get; set; } = "";
    public string Note     { get; set; } = "";
}