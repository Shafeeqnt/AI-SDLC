import { BellOutlined } from '@ant-design/icons';
import { Empty, List, Tag, Typography } from 'antd';
import { PageCard } from '../../../shared/ui/PageCard';
import { SectionHeading } from '../../../shared/ui/SectionHeading';
import { useNotificationsController } from '../application/useNotificationsController';

export const NotificationsPage = () => {
  const notificationsQuery = useNotificationsController();

  return (
    <div className="space-y-6">
      <SectionHeading
        eyebrow="Operational alerts"
        title="Notification center"
        description="Relevant staff alerts and user-facing updates stay in a single, scrollable card with the same workspace styling as the Figma shell."
      />
      <PageCard title="Latest notifications">
        <List
          loading={notificationsQuery.isLoading}
          locale={{ emptyText: <Empty description="No notifications available." /> }}
          dataSource={notificationsQuery.data?.items ?? []}
          renderItem={(item) => (
            <List.Item>
              <List.Item.Meta
                avatar={<BellOutlined className="rounded-md border border-[var(--panel-border)] p-2 text-[var(--accent-strong)]" />}
                title={
                  <div className="flex flex-wrap items-center gap-2">
                    <Typography.Text className="!text-sm !font-semibold !text-[var(--text-strong)]">
                      {item.message}
                    </Typography.Text>
                    <Tag color={item.isRead ? 'default' : 'green'}>{item.isRead ? 'Read' : 'Unread'}</Tag>
                  </div>
                }
                description={
                  <div className="text-xs text-[var(--text-subtle)]">
                    {new Date(item.createdDateUtc).toLocaleString()}
                    {item.grievanceId ? ` • Grievance #${item.grievanceId}` : ''}
                  </div>
                }
              />
            </List.Item>
          )}
        />
      </PageCard>
    </div>
  );
};
