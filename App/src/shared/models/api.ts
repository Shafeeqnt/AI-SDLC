export type ApiEnvelope<T> = {
  success: boolean;
  statusCode: number;
  data: T;
  message: string;
  errors: string[];
};

export type PagedResult<T> = {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
};
