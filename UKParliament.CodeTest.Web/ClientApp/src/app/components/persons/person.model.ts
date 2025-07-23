export interface Person {
  Id: number;
  FirstName: string;
  LastName: string;
  DOB: string | null;
  DepartmentId: number;
  DepartmentName?: string;
  Description: string;
}
