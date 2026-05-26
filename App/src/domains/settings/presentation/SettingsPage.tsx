import { SaveOutlined, SettingOutlined } from '@ant-design/icons';
import { Button, Empty, Input, List, Space, Typography } from 'antd';
import { useMemo, useState } from 'react';
import { PageCard } from '../../../shared/ui/PageCard';
import { SectionHeading } from '../../../shared/ui/SectionHeading';
import { useSettingsController } from '../application/useSettingsController';

export const SettingsPage = () => {
  const controller = useSettingsController();
  const [drafts, setDrafts] = useState<Record<string, string>>({});

  const items = useMemo(
    () =>
      (controller.settingsQuery.data ?? []).map((item) => ({
        ...item,
        draftValue: drafts[item.settingKey] ?? item.settingValue,
      })),
    [controller.settingsQuery.data, drafts],
  );

  return (
    <div className="space-y-6">
      <SectionHeading
        eyebrow="System controls"
        title="Configurable settings"
        description="Settings stay intentionally compact: key/value rows, low-friction inline editing, and restrained enterprise styling rather than a generic admin table."
      />
      <PageCard title="Operational settings">
        <List
          dataSource={items}
          locale={{ emptyText: <Empty description="No configurable settings returned by the API." /> }}
          renderItem={(item) => (
            <List.Item>
              <div className="grid w-full gap-3 md:grid-cols-[1.25fr_1fr_auto] md:items-center">
                <div>
                  <Space>
                    <SettingOutlined className="text-[var(--accent-strong)]" />
                    <Typography.Text className="!font-semibold !text-[var(--text-strong)]">
                      {item.settingKey}
                    </Typography.Text>
                  </Space>
                  <Typography.Paragraph className="!mb-0 !mt-1 !text-xs !text-[var(--text-subtle)]">
                    Last updated: {item.updatedDateUtc ? new Date(item.updatedDateUtc).toLocaleString() : 'Not available'}
                  </Typography.Paragraph>
                </div>
                <Input
                  value={item.draftValue}
                  onChange={(event) =>
                    setDrafts((current) => ({
                      ...current,
                      [item.settingKey]: event.target.value,
                    }))
                  }
                />
                <Button
                  type="primary"
                  icon={<SaveOutlined />}
                  loading={controller.isSaving}
                  onClick={() => controller.handleSave(item.settingKey, item.draftValue)}
                >
                  Save
                </Button>
              </div>
            </List.Item>
          )}
        />
      </PageCard>
    </div>
  );
};
