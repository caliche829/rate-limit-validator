
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReportResult } from '../interfaces/report-result';
import { RequestReportQuery } from '../interfaces/request-report-query';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RequestReportService {
  private apiUrl = `${environment.apiUrl}/Report`;

  constructor(private http: HttpClient) { }

  getRequestReport(body: RequestReportQuery): Observable<ReportResult> {
    return this.http.post<ReportResult>(this.apiUrl, body);
  }
}
