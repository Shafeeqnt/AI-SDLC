import { CommentOutlined, FileTextOutlined, SendOutlined } from '@ant-design/icons';
import { Button, Col, Empty, Form, Input, Row, Statistic, Table, Typography } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import { useMemo } from 'react';
import { PageCard } from '../../../shared/ui/PageCard';
import { SectionHeading } from '../../../shared/ui/SectionHeading';
import { StatusTag } from '../../../shared/ui/StatusTag';
import { useGrievancesController } from '../application/useGrievancesController';
import type { Grievance } from '../models/grievanceModels';

const columns: ColumnsType<Grievance> = [
  {
    title: 'Reference',
    dataIndex: 'referenceNumber',
    key: 'referenceNumber',
  },
  {
    title: 'Project',
    dataIndex: 'projectName',
    key: 'projectName',
  },
  {
    title: 'Contact',
    dataIndex: 'contactNumber',
    key: 'contactNumber',
  },
  {
    title: 'Status',
    dataIndex: 'statusId',
    key: 'statusId',
    render: (statusId: number) => <StatusTag statusId={statusId} />,
  },
];

export const GrievancesPage = () => {
  const controller = useGrievancesController();

  const summary = useMemo(() => {
    const items = controller.mineQuery.data?.items ?? [];
    return {
      total: items.length,
      open: items.filter((item) => item.statusId === 1).length,
      inProgress: items.filter((item) => item.statusId === 3).length,
    };
  }, [controller.mineQuery.data]);

  return (
    <div className="space-y-6">
      <SectionHeading
        eyebrow="Submission workspace"
        title="Grievance intake and review"
        description="This page follows the Figma workspace rhythm: breadcrumb-led header, border-defined cards, two-column form layout, and compact records for progress visibility."
      />

      <Row gutter={[16, 16]}>
        <Col xs={24} md={8}>
          <PageCard>
            <Statistic title="My submissions" value={summary.total} prefix={<CommentOutlined />} />
          </PageCard>
        </Col>
        <Col xs={24} md={8}>
          <PageCard>
            <Statistic title="Open items" value={summary.open} prefix={<FileTextOutlined />} />
          </PageCard>
        </Col>
        <Col xs={24} md={8}>
          <PageCard>
            <Statistic title="In progress" value={summary.inProgress} prefix={<SendOutlined />} />
          </PageCard>
        </Col>
      </Row>

      <Row gutter={[16, 16]}>
        <Col xs={24} xl={11}>
          <PageCard title="Create grievance">
            <Form form={controller.form} layout="vertical" onFinish={controller.handleSubmit}>
              <Row gutter={12}>
                <Col xs={24} md={12}>
                  <Form.Item label="Complainer name" name="complainerName">
                    <Input />
                  </Form.Item>
                </Col>
                <Col xs={24} md={12}>
                  <Form.Item label="Organization name" name="organizationName">
                    <Input />
                  </Form.Item>
                </Col>
                <Col xs={24} md={12}>
                  <Form.Item label="Contact number" name="contactNumber">
                    <Input />
                  </Form.Item>
                </Col>
                <Col xs={24} md={12}>
                  <Form.Item label="Email address" name="emailAddress">
                    <Input />
                  </Form.Item>
                </Col>
                <Col xs={24} md={12}>
                  <Form.Item label="Project name" name="projectName">
                    <Input />
                  </Form.Item>
                </Col>
                <Col xs={24} md={12}>
                  <Form.Item label="Project ID" name="projectId">
                    <Input />
                  </Form.Item>
                </Col>
                <Col span={24}>
                  <Form.Item label="Grievance description" name="grievanceDescription">
                    <Input.TextArea rows={6} />
                  </Form.Item>
                </Col>
              </Row>
              <Button type="primary" htmlType="submit" loading={controller.mineQuery.isFetching}>
                Submit grievance
              </Button>
              <Typography.Paragraph className="!mb-0 !mt-3 !text-xs !text-[var(--text-subtle)]">
                Required fields match the backend contract and stay in the application layer before mutation dispatch.
              </Typography.Paragraph>
            </Form>
          </PageCard>
        </Col>
        <Col xs={24} xl={13}>
          <PageCard title="My grievance queue">
            <Table<Grievance>
              rowKey="grievanceId"
              columns={columns}
              dataSource={controller.mineQuery.data?.items ?? []}
              loading={controller.mineQuery.isLoading}
              pagination={false}
              locale={{ emptyText: <Empty description="No grievances raised yet." /> }}
              scroll={{ x: 640 }}
            />
          </PageCard>
        </Col>
      </Row>

      {controller.canSeeQueue ? (
        <PageCard title="Staff review queue">
          <Table<Grievance>
            rowKey="grievanceId"
            columns={columns}
            dataSource={controller.queueQuery.data?.items ?? []}
            loading={controller.queueQuery.isLoading}
            pagination={false}
            locale={{ emptyText: <Empty description="No staff-visible grievances available." /> }}
            scroll={{ x: 640 }}
          />
        </PageCard>
      ) : null}
    </div>
  );
};
