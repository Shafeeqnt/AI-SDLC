export const validateSettingValue = (value: string): string | null => {
  if (!value.trim()) {
    return 'Setting value is required.';
  }

  return null;
};
