using Newtonsoft.Json;

namespace RPM_JobScheduler.DAL.Models;

public class Datum
{
    public string period { get; set; }
    public string duoarea { get; set; }

    [JsonProperty("area-name")]
    public string areaname { get; set; }
    public string product { get; set; }

    [JsonProperty("product-name")]
    public string productname { get; set; }
    public string process { get; set; }

    [JsonProperty("process-name")]
    public string processname { get; set; }
    public string series { get; set; }

    [JsonProperty("series-description")]
    public string seriesdescription { get; set; }
    public double value { get; set; }
    public string units { get; set; }
}
