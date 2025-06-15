export interface IHomeOfficeEntry {
  id         :  number;
  startTime  :  string;
  endTime    ?: string;
  description?: string;
  isEmailSent:  boolean;
  userId     :  string;
}