namespace RPM_JobScheduler.DAL.Models;

public class Response
{
    public int total { get; set; }
    public string dateFormat { get; set; }
    public string frequency { get; set; }
    public List<Datum> data { get; set; }
}
