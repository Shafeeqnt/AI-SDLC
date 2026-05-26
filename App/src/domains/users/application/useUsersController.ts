import { App, Form } from 'antd';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useState } from 'react';
import { getErrorMessage } from '../../../shared/utils/errors';
import { userService } from '../services/userService';
import type { UpdateUserPayload, User, UserPayload } from '../models/userModels';
import { validateCreateUser, validateUpdateUser } from '../validations/userValidation';

const createInitialValues: UserPayload = {
  username: '',
  email: '',
  userTypeId: 1,
  isActive: true,
  password: '',
  roleIds: [3],
};

const updateInitialValues: UpdateUserPayload = {
  email: '',
  userTypeId: 1,
  isActive: true,
  roleIds: [3],
};

export const useUsersController = () => {
  const queryClient = useQueryClient();
  const { message } = App.useApp();
  const [createForm] = Form.useForm<UserPayload>();
  const [updateForm] = Form.useForm<UpdateUserPayload>();
  const [editingUser, setEditingUser] = useState<User | null>(null);

  const usersQuery = useQuery({
    queryKey: ['users'],
    queryFn: userService.list,
  });

  const refreshUsers = async () => {
    await queryClient.invalidateQueries({ queryKey: ['users'] });
  };

  const createMutation = useMutation({
    mutationFn: userService.create,
    onSuccess: async () => {
      message.success('User created successfully.');
      createForm.resetFields();
      await refreshUsers();
    },
    onError: (error) => {
      message.error(getErrorMessage(error));
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ userId, values }: { userId: number; values: UpdateUserPayload }) => userService.update(userId, values),
    onSuccess: async () => {
      message.success('User updated successfully.');
      setEditingUser(null);
      updateForm.resetFields();
      await refreshUsers();
    },
    onError: (error) => {
      message.error(getErrorMessage(error));
    },
  });

  const handleCreate = (values: UserPayload) => {
    const validationMessage = validateCreateUser(values);
    if (validationMessage) {
      message.error(validationMessage);
      return;
    }

    createMutation.mutate(values);
  };

  const startEditing = (user: User) => {
    setEditingUser(user);
    updateForm.setFieldsValue({
      email: user.email,
      userTypeId: user.userTypeId,
      isActive: user.isActive,
      roleIds: user.roleIds,
    });
  };

  const handleUpdate = (values: UpdateUserPayload) => {
    if (!editingUser) {
      message.error('No user selected.');
      return;
    }

    const validationMessage = validateUpdateUser(values);
    if (validationMessage) {
      message.error(validationMessage);
      return;
    }

    updateMutation.mutate({ userId: editingUser.userId, values });
  };

  return {
    usersQuery,
    createForm,
    updateForm,
    editingUser,
    setEditingUser,
    handleCreate,
    handleUpdate,
    startEditing,
    createInitialValues,
    updateInitialValues,
  };
};
