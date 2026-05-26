import {
  ArrowRightOutlined,
  CommentOutlined,
  KeyOutlined,
  MailOutlined,
  SafetyCertificateOutlined,
  UserOutlined,
} from '@ant-design/icons';
import { Button, Card, Col, Form, Input, Row, Tabs, Typography } from 'antd';
import { useLoginController } from '../application/useLoginController';

export const LoginPage = () => {
  const controller = useLoginController();

  return (
    <div className="min-h-screen bg-[var(--page-background)] px-4 py-6 md:px-8 md:py-10">
      <div className="mx-auto grid max-w-7xl gap-6 lg:grid-cols-[1.15fr_0.85fr]">
        <Card className="rounded-lg border border-[var(--panel-border)] shadow-none">
          <div className="flex flex-col gap-8">
            <div className="space-y-4">
              <div className="flex h-12 w-12 items-center justify-center rounded-md border border-[var(--accent-strong)] bg-[var(--accent-soft)] text-lg text-[var(--accent-strong)]">
                <CommentOutlined />
              </div>
              <div className="space-y-2">
                <Typography.Text className="!text-xs !font-semibold uppercase tracking-[0.18em] !text-[var(--accent-strong)]">
                  Grievance Workspace
                </Typography.Text>
                <Typography.Title level={1} className="!mb-0 !text-4xl !font-semibold !text-[var(--text-strong)]">
                  Keep submissions structured, visible, and accountable.
                </Typography.Title>
                <Typography.Paragraph className="!mb-0 !max-w-2xl !text-base !text-[var(--text-subtle)]">
                  The interface follows the Pan enterprise shell from Figma: breadcrumb-led workspace framing,
                  compact utility typography, light-gray surfaces, and action controls anchored by green approval
                  states.
                </Typography.Paragraph>
              </div>
            </div>

            <Row gutter={[16, 16]}>
              {[
                {
                  icon: <SafetyCertificateOutlined />,
                  title: 'Role-secured access',
                  copy: 'System administrators, staff, and end users operate from the same shell with controlled navigation.',
                },
                {
                  icon: <MailOutlined />,
                  title: 'Submission alerts',
                  copy: 'Fresh grievances create a staff-ready queue with notifications and compact review panels.',
                },
                {
                  icon: <KeyOutlined />,
                  title: 'Guided recovery',
                  copy: 'Reset flows support email links and recovery prompts without breaking the enterprise layout.',
                },
              ].map((feature) => (
                <Col key={feature.title} xs={24} md={8}>
                  <div className="rounded-md border border-[var(--panel-border)] bg-white p-4">
                    <div className="mb-3 flex h-10 w-10 items-center justify-center rounded-md border border-[var(--panel-border)] text-[var(--accent-strong)]">
                      {feature.icon}
                    </div>
                    <Typography.Title level={5} className="!mb-2 !text-[var(--text-strong)]">
                      {feature.title}
                    </Typography.Title>
                    <Typography.Paragraph className="!mb-0 !text-sm !text-[var(--text-subtle)]">
                      {feature.copy}
                    </Typography.Paragraph>
                  </div>
                </Col>
              ))}
            </Row>
          </div>
        </Card>

        <Card className="rounded-lg border border-[var(--panel-border)] shadow-none">
          <Tabs
            defaultActiveKey="login"
            items={[
              {
                key: 'login',
                label: 'Sign In',
                children: (
                  <Form form={controller.loginForm} layout="vertical" onFinish={controller.handleLogin}>
                    <Form.Item label="Username" name="username">
                      <Input prefix={<UserOutlined />} size="large" />
                    </Form.Item>
                    <Form.Item label="Password" name="password">
                      <Input.Password prefix={<KeyOutlined />} size="large" />
                    </Form.Item>
                    <Button type="primary" htmlType="submit" size="large" className="w-full">
                      Sign in
                    </Button>
                  </Form>
                ),
              },
              {
                key: 'forgot',
                label: 'Forgot Password',
                children: (
                  <Form form={controller.forgotForm} layout="vertical" onFinish={controller.handleForgotPassword}>
                    <Form.Item label="Username or email" name="usernameOrEmail">
                      <Input prefix={<MailOutlined />} size="large" />
                    </Form.Item>
                    <Button type="primary" htmlType="submit" size="large" className="w-full">
                      Request reset link
                    </Button>
                  </Form>
                ),
              },
              {
                key: 'tokenReset',
                label: 'Token Reset',
                children: (
                  <Form form={controller.tokenResetForm} layout="vertical" onFinish={controller.handleTokenReset}>
                    <Form.Item label="Reset token" name="resetToken">
                      <Input size="large" />
                    </Form.Item>
                    <Form.Item label="New password" name="newPassword">
                      <Input.Password size="large" />
                    </Form.Item>
                    <Button type="primary" htmlType="submit" size="large" className="w-full">
                      Apply new password
                    </Button>
                  </Form>
                ),
              },
              {
                key: 'recovery',
                label: 'Recovery Questions',
                children: (
                  <Form
                    form={controller.recoveryForm}
                    layout="vertical"
                    onFinish={controller.handleRecoveryReset}
                    initialValues={{ recoveryAnswers: ['', ''] }}
                  >
                    <Form.Item label="Username" name="username">
                      <Input size="large" />
                    </Form.Item>
                    <Form.List name="recoveryAnswers">
                      {(fields) => (
                        <>
                          {fields.map((field, index) => (
                            <Form.Item
                              key={field.key}
                              label={`Recovery answer ${index + 1}`}
                              name={field.name}
                            >
                              <Input size="large" />
                            </Form.Item>
                          ))}
                        </>
                      )}
                    </Form.List>
                    <Form.Item label="New password" name="newPassword">
                      <Input.Password size="large" />
                    </Form.Item>
                    <Button type="primary" htmlType="submit" size="large" className="w-full">
                      Reset using recovery answers
                    </Button>
                  </Form>
                ),
              },
            ]}
          />
          <div className="mt-6 rounded-md border border-[var(--panel-border)] bg-[var(--workspace-background)] p-4">
            <Typography.Text className="!text-xs !font-semibold uppercase tracking-[0.16em] !text-[var(--accent-strong)]">
              Default admin seed
            </Typography.Text>
            <Typography.Paragraph className="!my-2 !text-sm !text-[var(--text-subtle)]">
              Use the seeded administrator from the backend migration if your local database is up.
            </Typography.Paragraph>
            <Typography.Text code>sysadmin / Assyst@123</Typography.Text>
            <div className="mt-4 flex items-center gap-2 text-sm text-[var(--text-subtle)]">
              <ArrowRightOutlined className="text-[var(--accent-strong)]" />
              Frontend assumes the API is available on <code>http://localhost:5000</code>.
            </div>
          </div>
        </Card>
      </div>
    </div>
  );
};
