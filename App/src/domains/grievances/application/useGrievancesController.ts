import { App, Form } from 'antd';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { grievanceService } from '../services/grievanceService';
import { useAuth } from '../../authentication/application/useAuth';
import type { SubmitGrievanceValues } from '../models/grievanceModels';
import { validateGrievanceValues } from '../validations/grievanceValidation';
import { getErrorMessage } from '../../../shared/utils/errors';

export const useGrievancesController = () => {
  const queryClient = useQueryClient();
  const { message } = App.useApp();
  const { session } = useAuth();
  const [form] = Form.useForm<SubmitGrievanceValues>();

  const mineQuery = useQuery({
    queryKey: ['grievances', 'mine'],
    queryFn: () => grievanceService.listMine(),
  });

  const queueQuery = useQuery({
    queryKey: ['grievances', 'queue'],
    queryFn: () => grievanceService.listQueue(),
    enabled: Boolean(session?.roles.some((role) => role === 'SystemAdministrator' || role === 'StaffMember')),
  });

  const submitMutation = useMutation({
    mutationFn: grievanceService.submit,
    onSuccess: async () => {
      message.success('Grievance submitted.');
      form.resetFields();
      await queryClient.invalidateQueries({ queryKey: ['grievances'] });
      await queryClient.invalidateQueries({ queryKey: ['notifications'] });
    },
    onError: (error) => {
      message.error(getErrorMessage(error));
    },
  });

  const handleSubmit = (values: SubmitGrievanceValues) => {
    const validationMessage = validateGrievanceValues(values);
    if (validationMessage) {
      message.error(validationMessage);
      return;
    }

    submitMutation.mutate(values);
  };

  return {
    form,
    mineQuery,
    queueQuery,
    handleSubmit,
    canSeeQueue: Boolean(session?.roles.some((role) => role === 'SystemAdministrator' || role === 'StaffMember')),
  };
};
