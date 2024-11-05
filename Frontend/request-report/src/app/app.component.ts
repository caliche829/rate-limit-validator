import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';
import { RequestReportComponent } from "./components/request-report/request-report.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RequestReportComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'request-report';
  apiUrl = environment.apiUrl;
}
