export interface Person {
  id: number;
  firstName: string;
  lastName: string;
  dob: string | null;
  departmentId: number;
  departmentName?: string;
  description: string;
}
