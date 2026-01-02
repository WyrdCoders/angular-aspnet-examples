import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { environment } from './../../environments/environment';
import { City } from './city';

@Component({
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrl: './city-edit.component.scss'
})
export class CityEditComponent implements OnInit {
  form!: FormGroup;
  city?: City;
  title?: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private http: HttpClient,
    private router: Router) { }

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl(''),
      lat: new FormControl(''),
      lon: new FormControl('')
    });

    this.loadData();
  }

  loadData(): void {
    // Retrieve the ID from the 'id' parameter
    var idParam = this.activatedRoute.snapshot.paramMap.get('id');
    var id = idParam ? +idParam : 0;

    // Fetch the city from the server
    var url = environment.baseUrl + 'api/cities/' + id;
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

  onSubmit(): void {
    var city = this.city;
    if (city) {
      city.name = this.form.controls['name'].value;
      city.lat = +this.form.controls['lat'].value;
      city.lon = +this.form.controls['lon'].value;

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
  }
}
