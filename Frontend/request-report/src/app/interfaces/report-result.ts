import { RequestData } from "./request-data";

export interface ReportResult {
  data: Array<RequestData>;
  totalRecords: number;
}