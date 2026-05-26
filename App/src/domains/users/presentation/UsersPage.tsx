import { EditOutlined, UserAddOutlined } from '@ant-design/icons';
import { Button, Col, Drawer, Form, Input, Row, Select, Switch, Table, Tag } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import { PageCard } from '../../../shared/ui/PageCard';
import { SectionHeading } from '../../../shared/ui/SectionHeading';
import { roleOptions, userTypeOptions } from '../constants/userConstants';
import { useUsersController } from '../application/useUsersController';
import type { User } from '../models/userModels';

const columns = (onEdit: (user: User) => void): ColumnsType<User> => [
  { title: 'Username', dataIndex: 'username', key: 'username' },
  { title: 'Email', dataIndex: 'email', key: 'email' },
  {
    title: 'Roles',
    dataIndex: 'roles',
    key: 'roles',
    render: (roles: string[]) => roles.map((role) => <Tag key={role}>{role}</Tag>),
  },
  {
    title: 'Status',
    dataIndex: 'isActive',
    key: 'isActive',
    render: (isActive: boolean) => <Tag color={isActive ? 'green' : 'default'}>{isActive ? 'Active' : 'Inactive'}</Tag>,
  },
  {
    title: 'Actions',
    key: 'actions',
    render: (_, user) => (
      <Button icon={<EditOutlined />} onClick={() => onEdit(user)}>
        Edit
      </Button>
    ),
  },
];

export const UsersPage = () => {
  const controller = useUsersController();

  return (
    <div className="space-y-6">
      <SectionHeading
        eyebrow="Administration"
        title="User access management"
        description="Keep identity controls in the same workspace framing as the rest of the system, with bordered cards, compact tables, and drawer-based maintenance."
      />

      <Row gutter={[16, 16]}>
        <Col xs={24} xl={10}>
          <PageCard title="Create user">
            <Form
              form={controller.createForm}
              layout="vertical"
              initialValues={controller.createInitialValues}
              onFinish={controller.handleCreate}
            >
              <Form.Item label="Username" name="username">
                <Input />
              </Form.Item>
              <Form.Item label="Email" name="email">
                <Input />
              </Form.Item>
              <Form.Item label="Password" name="password">
                <Input.Password />
              </Form.Item>
              <Row gutter={12}>
                <Col span={12}>
                  <Form.Item label="User type" name="userTypeId">
                    <Select options={userTypeOptions} />
                  </Form.Item>
                </Col>
                <Col span={12}>
                  <Form.Item label="Active" name="isActive" valuePropName="checked">
                    <Switch />
                  </Form.Item>
                </Col>
              </Row>
              <Form.Item label="Roles" name="roleIds">
                <Select mode="multiple" options={roleOptions} />
              </Form.Item>
              <Button type="primary" htmlType="submit" icon={<UserAddOutlined />}>
                Create user
              </Button>
            </Form>
          </PageCard>
        </Col>
        <Col xs={24} xl={14}>
          <PageCard title="Current users">
            <Table<User>
              rowKey="userId"
              columns={columns(controller.startEditing)}
              dataSource={controller.usersQuery.data?.items ?? []}
              loading={controller.usersQuery.isLoading}
              pagination={false}
              scroll={{ x: 680 }}
            />
          </PageCard>
        </Col>
      </Row>

      <Drawer
        title={controller.editingUser ? `Update ${controller.editingUser.username}` : 'Update user'}
        width={420}
        open={Boolean(controller.editingUser)}
        onClose={() => controller.setEditingUser(null)}
      >
        <Form
          form={controller.updateForm}
          layout="vertical"
          initialValues={controller.updateInitialValues}
          onFinish={controller.handleUpdate}
        >
          <Form.Item label="Email" name="email">
            <Input />
          </Form.Item>
          <Form.Item label="User type" name="userTypeId">
            <Select options={userTypeOptions} />
          </Form.Item>
          <Form.Item label="Roles" name="roleIds">
            <Select mode="multiple" options={roleOptions} />
          </Form.Item>
          <Form.Item label="Active" name="isActive" valuePropName="checked">
            <Switch />
          </Form.Item>
          <Button type="primary" htmlType="submit">
            Save changes
          </Button>
        </Form>
      </Drawer>
    </div>
  );
};
