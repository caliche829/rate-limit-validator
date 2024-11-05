import { TestBed } from '@angular/core/testing';

import { RequestReportService } from './request-report.service';

describe('RequestReportService', () => {
  let service: RequestReportService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RequestReportService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
