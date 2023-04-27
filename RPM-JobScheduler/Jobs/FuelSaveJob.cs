using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Quartz;
using RPM_JobScheduler.Configuration;
using RPM_JobScheduler.DAL.DB;
using RPM_JobScheduler.DAL.Models;
using RPM_JobScheduler.Extensions;
using Serilog;
using System.Net.Http.Json;

namespace RPM_JobScheduler.Jobs;

public class FuelSaveJob : IJob
{
    private AppDbContext _appDbContext;
    private FuelJobSettingsConfiguration _jobSettings;
    private FuelAPISettingsConfiguration _apiSettings;

    public FuelSaveJob()
    {
        ConfigureServices();
    }
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"[{DateTime.Now.ToString("YYYYmmddHHss")}] Starting fuel price job.");
        await PerformJob();
    }

    private async Task PerformJob()
    {
        var fuelApiResponse = await GetFuelPrices();

        var periodData = fuelApiResponse.response.data.Select(x => x.ToFuelPriceRecord());

        var cutOffDate = DateTime.Now.AddDays(_jobSettings.CutoffPeriod * -1);

        try
        {
            using (_appDbContext)
            {
                _appDbContext.AddRange(periodData.Where(x => x.Period >= cutOffDate));
                await _appDbContext.SaveChangesAsync();

                Console.WriteLine($"[{DateTime.Now.ToString("YYYYmmddHHss")}] Successfully saved fuel data");
            }
        }
        catch(Exception ex)
        {
            Log.Error("Encountered error writing to database.", ex);
        }
    }

    private async Task<RootAPIResponse> GetFuelPrices()
    {
        using (var client = new HttpClient())
        {
            try
            {
                var queryString = new Dictionary<string, string>()
                {
                    ["frequency"] = "weekly",
                    ["data[0]"] = "value",
                    ["facets[series][]"] = "EMD_EPD2D_PTE_NUS_DPG",
                    ["sort[0][column]"] = "period",
                    ["sort[0][direction]"] = "desc",
                    ["offset"] = "0",
                    ["length"] = "5000",
                    ["api_key"] = _apiSettings.ApiKey
                };

                var uri = QueryHelpers.AddQueryString(_apiSettings.EndPoint, queryString);
                var response = await client.GetFromJsonAsync<RootAPIResponse>(uri);

                Console.WriteLine("Querying the fuel api.");

                return response != null ? response : throw new HttpRequestException();
            }
            catch (HttpRequestException ex)
            {
                Log.Error("Called api and received an error.", ex);
                throw ex;
            }
        }
    }

    private void ConfigureServices()
    {
        var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", optional: false)
                    .Build();

        var fuelApiSettings = configuration.GetSection("FuelAPISettings").Get<FuelAPISettingsConfiguration>();
        var fuelJobSettings = configuration.GetSection("FuelJobSettings").Get<FuelJobSettingsConfiguration>();
        var connectionString = configuration.GetSection("ConnectionString").Value;

        if (fuelApiSettings != null)
        {
            _apiSettings = fuelApiSettings;
        }

        if (fuelJobSettings != null)
        {
            _jobSettings = fuelJobSettings;
        }

        if (!string.IsNullOrEmpty(connectionString))
        {
            _appDbContext = new AppDbContext(connectionString);
        }
    }
}
