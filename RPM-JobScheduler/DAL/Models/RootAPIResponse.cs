namespace RPM_JobScheduler.DAL.Models;

public class RootAPIResponse
{
    public Response response { get; set; }
    public Request request { get; set; }
    public string apiVersion { get; set; }
}
