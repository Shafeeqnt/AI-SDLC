import { App as AntApp, ConfigProvider, Spin, theme } from 'antd';
import { QueryClientProvider } from '@tanstack/react-query';
import { lazy, Suspense, type ReactNode } from 'react';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import { queryClient } from '../shared/data/queryClient';
import { AuthProvider } from '../domains/authentication/application/AuthContext';
import { useAuth } from '../domains/authentication/application/useAuth';

const AppShell = lazy(() => import('../shared/ui/AppShell').then((module) => ({ default: module.AppShell })));
const LoginPage = lazy(() => import('../domains/authentication/presentation/LoginPage').then((module) => ({ default: module.LoginPage })));
const GrievancesPage = lazy(() => import('../domains/grievances/presentation/GrievancesPage').then((module) => ({ default: module.GrievancesPage })));
const NotificationsPage = lazy(() => import('../domains/notifications/presentation/NotificationsPage').then((module) => ({ default: module.NotificationsPage })));
const SettingsPage = lazy(() => import('../domains/settings/presentation/SettingsPage').then((module) => ({ default: module.SettingsPage })));
const UsersPage = lazy(() => import('../domains/users/presentation/UsersPage').then((module) => ({ default: module.UsersPage })));

const LoadingState = () => (
  <div className="flex min-h-screen items-center justify-center">
    <Spin size="large" />
  </div>
);

const ProtectedRoute = () => {
  const { session } = useAuth();

  if (!session) {
    return <Navigate to="/login" replace />;
  }

  return (
    <Suspense fallback={<LoadingState />}>
      <AppShell />
    </Suspense>
  );
};

const AdminRoute = ({ children }: { children: ReactNode }) => {
  const { session } = useAuth();
  if (!session?.roles.includes('SystemAdministrator')) {
    return <Navigate to="/grievances" replace />;
  }

  return <>{children}</>;
};

export const App = () => (
  <ConfigProvider
    theme={{
      algorithm: theme.defaultAlgorithm,
      token: {
        colorPrimary: '#028045',
        colorBgLayout: '#ebebeb',
        colorBorder: '#d1d1d1',
        borderRadius: 6,
        fontFamily: 'Inter, sans-serif',
      },
      components: {
        Layout: {
          siderBg: '#f5f5f5',
          headerBg: '#ffffff',
          bodyBg: '#ebebeb',
        },
        Menu: {
          itemSelectedBg: '#e9f6ee',
          itemSelectedColor: '#028045',
          itemHoverColor: '#242424',
        },
      },
    }}
  >
    <AntApp>
      <QueryClientProvider client={queryClient}>
        <AuthProvider>
          <BrowserRouter>
            <Suspense fallback={<LoadingState />}>
              <Routes>
                <Route path="/login" element={<LoginPage />} />
                <Route element={<ProtectedRoute />}>
                  <Route path="/" element={<Navigate to="/grievances" replace />} />
                  <Route path="/grievances" element={<GrievancesPage />} />
                  <Route path="/notifications" element={<NotificationsPage />} />
                  <Route
                    path="/users"
                    element={
                      <AdminRoute>
                        <UsersPage />
                      </AdminRoute>
                    }
                  />
                  <Route
                    path="/settings"
                    element={
                      <AdminRoute>
                        <SettingsPage />
                      </AdminRoute>
                    }
                  />
                </Route>
              </Routes>
            </Suspense>
          </BrowserRouter>
        </AuthProvider>
      </QueryClientProvider>
    </AntApp>
  </ConfigProvider>
);
