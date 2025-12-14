import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

import { environment } from '../../environments/environment';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrl: './fetch-data.component.css'
})
export class FetchDataComponent implements OnInit {
  public forecasts?: WeatherForecast[];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getForecasts();
  }

  private getForecasts(): void {
    this.http.get<WeatherForecast[]>(environment.baseUrl + 'api/weatherforecast')
      .subscribe({
        next: (result) => this.forecasts = result,
        error: (error) => console.error(error)
      });
  }
}

// Interface to store the weather forecast data
interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
