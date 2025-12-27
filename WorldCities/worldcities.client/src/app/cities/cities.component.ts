import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

import { environment } from '../../environments/environment';
import { City } from './city';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.scss'
})
export class CitiesComponent implements OnInit {
  public cities!: City[];
  public displayedColumns: string[] = ['id', 'name', 'lat', 'lon'];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get<City[]>(environment.baseUrl + 'api/cities')
      .subscribe({
        next: (result) => this.cities = result,
        error: (error) => console.error(error)
      });
  }
}
