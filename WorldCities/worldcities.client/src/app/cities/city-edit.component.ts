import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { environment } from './../../environments/environment';
import { City } from './city';
import { Country } from './../countries/country';

@Component({
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrl: './city-edit.component.scss'
})
export class CityEditComponent implements OnInit {
  city?: City;
  countries?: Country[];
  form!: FormGroup;
  id?: number;
  title?: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private http: HttpClient,
    private router: Router) { }

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl('', Validators.required),
      lat: new FormControl('', Validators.required),
      lon: new FormControl('', Validators.required),
      countryId: new FormControl('', Validators.required)
    });

    this.loadData();
  }

  loadCountries(): void {
    // Fetch all the countries from the server
    var url = environment.baseUrl + 'api/countries';
    var params = new HttpParams()
      .set("pageIndex", "0")
      .set("pageSize", "9999")
      .set("sortColumn", "name")
      .set("sortOrder", "asc");

    this.http.get<any>(url, { params }).subscribe({
      next: (result) => {
        this.countries = result.data;
      },
      error: (error) => console.error(error)
    });
  }

  loadData(): void {
    this.loadCountries();

    // Retrieve the ID from the 'id' parameter
    var idParam = this.activatedRoute.snapshot.paramMap.get('id');
    this.id = idParam ? +idParam : 0;
    if (this.id) {
      // EDIT MODE
      // Fetch the city from the server
      var url = environment.baseUrl + 'api/cities/' + this.id;
      this.http.get<City>(url).subscribe({
        next: (result) => {
          this.city = result;
          this.title = "Edit - " + this.city.name;

          // Update the form with the city value
          this.form.patchValue(this.city);
        },
        error: (error) => console.error(error)
      });
    }
    else {
      // ADD MODE
      this.title = "Create a new City";
    }
  }

  onSubmit(): void {
    var city = (this.id) ? this.city : <City>{};
    if (city) {
      city.name = this.form.controls['name'].value;
      city.lat = +this.form.controls['lat'].value;
      city.lon = +this.form.controls['lon'].value;
      city.countryId = +this.form.controls['countryId'].value;

      if (this.id) {
        // EDIT MODE
        var url = environment.baseUrl + 'api/cities/' + city.id;
        this.http.put<City>(url, city).subscribe({
          next: (result) => {
            console.log("City " + city!.id + " has been updated.");

            // Go back to the cities view
            this.router.navigate(['/cities']);
          },
          error: (error) => console.error(error)
        });
      }
      else {
        // ADD MODE
        var url = environment.baseUrl + 'api/cities';
        this.http.post<City>(url, city).subscribe({
          next: (result) => {
            console.log("City " + result.id + " has been created.");

            // Go back to the cities view
            this.router.navigate(['/cities']);
          },
          error: (error) => console.error(error)
        });
      }
    }
  }
}
