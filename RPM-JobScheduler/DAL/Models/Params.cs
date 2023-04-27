namespace RPM_JobScheduler.DAL.Models;

public class Params
{
    public string frequency { get; set; }
    public List<string> data { get; set; }
    public Facets facets { get; set; }
    public List<Sort> sort { get; set; }
    public int offset { get; set; }
    public int length { get; set; }
    public string api_key { get; set; }
}
