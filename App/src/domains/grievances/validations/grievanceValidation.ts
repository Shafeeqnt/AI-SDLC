import type { SubmitGrievanceValues } from '../models/grievanceModels';

export const validateGrievanceValues = (values: SubmitGrievanceValues): string | null => {
  if (!values.contactNumber.trim()) {
    return 'Contact number is required.';
  }

  if (!/^[0-9]{7,15}$/.test(values.contactNumber.trim())) {
    return 'Contact number must be 7 to 15 digits.';
  }

  if (!values.emailAddress.trim()) {
    return 'Email address is required.';
  }

  if (!values.grievanceDescription.trim()) {
    return 'Grievance description is required.';
  }

  if (!values.projectName.trim()) {
    return 'Project name is required.';
  }

  if (!values.projectId.trim()) {
    return 'Project ID is required.';
  }

  return null;
};
