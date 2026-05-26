import {
  BellOutlined,
  CommentOutlined,
  LogoutOutlined,
  SettingOutlined,
  TeamOutlined,
} from '@ant-design/icons';
import { Avatar, Breadcrumb, Button, Layout, Menu, Space, Tag, Typography } from 'antd';
import type { MenuProps } from 'antd';
import { useMemo } from 'react';
import { Link, Outlet, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../domains/authentication/application/useAuth';

const { Header, Sider, Content } = Layout;

type NavigationItem = Required<MenuProps>['items'][number];

const navigationItems: NavigationItem[] = [
  {
    key: '/grievances',
    icon: <CommentOutlined />,
    label: <Link to="/grievances">Grievances</Link>,
  },
  {
    key: '/users',
    icon: <TeamOutlined />,
    label: <Link to="/users">Users</Link>,
  },
  {
    key: '/notifications',
    icon: <BellOutlined />,
    label: <Link to="/notifications">Notifications</Link>,
  },
  {
    key: '/settings',
    icon: <SettingOutlined />,
    label: <Link to="/settings">Settings</Link>,
  },
];

const labelMap: Record<string, string> = {
  grievances: 'Grievances',
  users: 'Users',
  notifications: 'Notifications',
  settings: 'Settings',
};

export const AppShell = () => {
  const { pathname } = useLocation();
  const navigate = useNavigate();
  const { session, signOut } = useAuth();

  const selectedKey = useMemo(() => {
    if (pathname.startsWith('/users')) {
      return '/users';
    }

    if (pathname.startsWith('/notifications')) {
      return '/notifications';
    }

    if (pathname.startsWith('/settings')) {
      return '/settings';
    }

    return '/grievances';
  }, [pathname]);

  const items = navigationItems.filter((item) => {
    if (item?.key === '/users' || item?.key === '/settings') {
      return session?.roles.includes('SystemAdministrator');
    }

    return true;
  });

  const breadcrumbItems = pathname
    .split('/')
    .filter(Boolean)
    .map((segment) => ({
      title: labelMap[segment] ?? segment,
    }));

  return (
    <Layout className="min-h-screen bg-[var(--page-background)]">
      <Sider
        width={248}
        breakpoint="lg"
        collapsedWidth={0}
        className="border-r border-[var(--panel-border)] !bg-[var(--nav-background)]"
      >
        <div className="flex h-16 items-center gap-3 border-b border-[var(--panel-border)] px-5">
          <div className="flex h-10 w-10 items-center justify-center rounded-md border border-[var(--accent-strong)] bg-[var(--accent-soft)] text-[var(--accent-strong)]">
            <CommentOutlined />
          </div>
          <div>
            <Typography.Text className="block !text-xs !font-semibold uppercase tracking-[0.16em] !text-[var(--accent-strong)]">
              Pan Apps
            </Typography.Text>
            <Typography.Title level={5} className="!mb-0 !text-[var(--text-strong)]">
              Pan GMS
            </Typography.Title>
          </div>
        </div>
        <div className="px-3 py-4">
          <Menu
            mode="inline"
            selectedKeys={[selectedKey]}
            items={items}
            className="border-none bg-transparent"
          />
        </div>
      </Sider>
      <Layout>
        <Header className="flex h-auto flex-col gap-4 border-b border-[var(--panel-border)] !bg-white px-6 py-4 md:flex-row md:items-center md:justify-between">
          <div className="space-y-2">
            <Breadcrumb items={breadcrumbItems.length > 0 ? breadcrumbItems : [{ title: 'Workspace' }]} />
            <Typography.Title level={4} className="!mb-0 !text-[var(--text-strong)]">
              {labelMap[pathname.split('/')[1] ?? 'grievances'] ?? 'Workspace'}
            </Typography.Title>
          </div>
          <Space size={12} wrap>
            <Tag color="green">{session?.roles.join(', ')}</Tag>
            <Space size={10}>
              <Avatar className="bg-[var(--accent-strong)]">
                {session?.username.slice(0, 1).toUpperCase()}
              </Avatar>
              <div className="min-w-0">
                <Typography.Text className="block !text-sm !font-semibold !text-[var(--text-strong)]">
                  {session?.username}
                </Typography.Text>
                <Typography.Text className="!text-xs !text-[var(--text-subtle)]">
                  {session?.email}
                </Typography.Text>
              </div>
            </Space>
            <Button
              icon={<LogoutOutlined />}
              onClick={() => {
                signOut();
                navigate('/login', { replace: true });
              }}
            >
              Sign out
            </Button>
          </Space>
        </Header>
        <Content className="p-3 md:p-6">
          <div className="rounded-lg border border-[var(--panel-border)] bg-[var(--workspace-background)]">
            <div className="px-5 py-6 md:px-8">
              <Outlet />
            </div>
          </div>
        </Content>
      </Layout>
    </Layout>
  );
};
