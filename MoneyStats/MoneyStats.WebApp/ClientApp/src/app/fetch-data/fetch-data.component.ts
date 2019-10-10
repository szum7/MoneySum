import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-fetch-data',
    templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
    public forecasts: WeatherForecast[];

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
        this.http.get<WeatherForecast[]>(this.baseUrl + 'weatherforecast').subscribe(result => {
            this.forecasts = result;
        }, error => console.error(error));
    }

    public getTestData() {
        this.http.get<any>(this.baseUrl + 'api/tag/testcall').subscribe(result => {
            console.log(result);
        }, error => console.error(error));
    }

    public getTags() {
        this.http.get<any>(this.baseUrl + 'api/tag/GetTags').subscribe(result => {
            console.log(result);
        }, error => console.error(error));
    }

}

interface WeatherForecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
