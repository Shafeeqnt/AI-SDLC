import { Tag } from 'antd';

const statusMap: Record<number, { label: string; color: string }> = {
  1: { label: 'Open', color: 'green' },
  2: { label: 'Submitted', color: 'gold' },
  3: { label: 'In Progress', color: 'blue' },
  4: { label: 'Resolved', color: 'cyan' },
  5: { label: 'Closed', color: 'default' },
};

export const StatusTag = ({ statusId }: { statusId: number }) => {
  const status = statusMap[statusId] ?? { label: `Status ${statusId}`, color: 'default' };
  return <Tag color={status.color}>{status.label}</Tag>;
};
