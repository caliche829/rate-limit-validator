import { Component, OnInit } from '@angular/core';
import { NgbPagination, NgbTimepickerModule, NgbDateStruct, NgbTimeStruct, NgbModule, NgbAlertModule, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { RequestReportService } from '../../services/request-report.service';
import { RequestReportQuery } from '../../interfaces/request-report-query';
import { FormsModule } from '@angular/forms';
import { RequestData } from '../../interfaces/request-data';
import { NgFor, CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { DATE_PIPE_DEFAULT_OPTIONS } from "@angular/common";



@Component({
  selector: 'app-request-report',
  standalone: true,
	imports: [NgbPagination, NgFor, FormsModule, HttpClientModule, CommonModule, NgbTimepickerModule, NgbModule, NgbAlertModule, NgbDatepickerModule],
  templateUrl: './request-report.component.html',
  styleUrl: './request-report.component.css',
  providers: [
    {
      provide: DATE_PIPE_DEFAULT_OPTIONS,
      useValue: { dateFormat: "longDate" }
    }
  ],
})
export class RequestReportComponent implements OnInit {
  records: Array<RequestData> = [];
  totalRecords: number = 0;
  pageSize: number = 10;
  page: number = 1;
  phoneNumber: string = '';
  date: NgbDateStruct;
  time: NgbTimeStruct = { hour: 0, minute: 0, second: 0 };

  constructor(private requestReportService: RequestReportService) { }

  ngOnInit(): void {
    this.loadRecords();
  }

  loadRecords(): void {
    let dateFilter = ''

    if (this.date !== undefined && this.date !== null) dateFilter = `${this.date.year}-${this.date.month}-${this.date.day}`
    
    let body: RequestReportQuery = {
      page: this.page,
      pageSize: this.pageSize,
      phoneNumber: this.phoneNumber,
      date: dateFilter,
      time: `${this.time.hour}:${this.time.minute}:${this.time.second}`,
    };
    this.requestReportService.getRequestReport(body).subscribe(data => {
      this.records = data.data;
      this.totalRecords = data.totalRecords;
    });
  }

  onPageChange(page: number): void {
    this.page = page;
    this.loadRecords();
  }
}