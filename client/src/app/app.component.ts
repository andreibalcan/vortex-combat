import { JsonPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, JsonPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'client';

  weatherData: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.http.get('http://localhost:5299/weatherforecast')
      .subscribe(data => {
        this.weatherData = data;
      });
  }
}
