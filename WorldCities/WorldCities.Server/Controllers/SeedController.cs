using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Security;
using WorldCities.Server.Data;
using WorldCities.Server.Data.Models;

namespace WorldCities.Server.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SeedController(ApplicationDbContext context, IWebHostEnvironment env) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly IWebHostEnvironment _env = env;

    [HttpGet]
    public async Task<ActionResult> Import()
    {
        // Prevent non-development environments from running this method
        if (!_env.IsDevelopment())
            throw new SecurityException("Not allowed");

        string path = Path.Combine(_env.ContentRootPath, "Data/Source/worldcities.xlsx");

        using FileStream stream = System.IO.File.OpenRead(path);
        using ExcelPackage excelPackage = new(stream);

        // Get the first worksheet
        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

        // Define how many rows to process
        int endRowNumber = worksheet.Dimension.End.Row;

        // Initialize the record counters
        int countriesAddedCount = 0;
        int citiesAddedCount = 0;

        // Create a lookup dictionary containing all the countries already existing
        // in the database (it will be empty on first run).
        var countriesByName = _context.Countries.AsNoTracking().ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

        // Iterate through all rows, skipping the first one
        for (int rowNumber = 2; rowNumber <= endRowNumber; rowNumber++)
        {
            ExcelRange row = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];

            string countryName = row[rowNumber, 5].GetValue<string>();
            string iso2 = row[rowNumber, 6].GetValue<string>();
            string iso3 = row[rowNumber, 7].GetValue<string>();

            // Skip this country if it already exists in the database
            if (countriesByName.ContainsKey(countryName))
                continue;

            // Create the country entity
            Country country = new()
            {
                Name = countryName,
                ISO2 = iso2,
                ISO3 = iso3
            };

            // Add the country to the DB context
            await _context.Countries.AddAsync(country);

            // Store the country in the lookup dictionary
            countriesByName.Add(countryName, country);

            // Increment the counter
            countriesAddedCount++;
        }

        // Save all the countries into the database
        if (countriesAddedCount > 0)
            await _context.SaveChangesAsync();

        // Create a lookup dictionary containing all the cities already existing
        // in the database (it will be empty on first run).
        var cities = _context.Cities.AsNoTracking().ToDictionary(x => (x.Name, x.Lat, x.Lon, x.CountryId));

        // Iterate through all rows, skipping the first one
        for (int rowNumber = 2; rowNumber <= endRowNumber; rowNumber++) 
        {
            ExcelRange row = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];

            string cityName = row[rowNumber, 1].GetValue<string>();
            decimal lat = row[rowNumber, 3].GetValue<decimal>();
            decimal lon = row[rowNumber, 4].GetValue<decimal>();
            string countryName = row[rowNumber, 5].GetValue<string>();

            // Retrieve country Id by country name
            var countryId = countriesByName[countryName].Id;

            // Skip this city if it already exists in the database
            if (cities.ContainsKey((Name: cityName, Lat: lat, Lon: lon, CountryId: countryId)))
                continue;

            // Create the city entity
            City city = new() 
            { 
                Name = cityName, 
                Lat = lat, 
                Lon = lon, 
                CountryId = countryId 
            };

            // Add the city to the DB context
            _context.Cities.Add(city);

            // Increment the counter
            citiesAddedCount++;
        }

        // Save all the cities into the database
        if (citiesAddedCount > 0)
            await _context.SaveChangesAsync();

        return new JsonResult(new { Cities = citiesAddedCount, Countries = countriesAddedCount });
    }
}
