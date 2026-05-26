import type { PagedResult } from '../../../shared/models/api';

export type Grievance = {
  grievanceId: number;
  referenceNumber: string;
  complainerName?: string | null;
  organizationName?: string | null;
  contactNumber: string;
  emailAddress: string;
  grievanceDescription: string;
  projectName: string;
  projectId: string;
  statusId: number;
  createdDateUtc: string;
  updatedDateUtc?: string | null;
  closedDateUtc?: string | null;
};

export type SubmitGrievanceValues = {
  complainerName?: string;
  organizationName?: string;
  contactNumber: string;
  emailAddress: string;
  grievanceDescription: string;
  projectName: string;
  projectId: string;
};

export type GrievanceList = PagedResult<Grievance>;
