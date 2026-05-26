import { Card } from 'antd';
import type { PropsWithChildren, ReactNode } from 'react';
import clsx from 'clsx';

type PageCardProps = PropsWithChildren<{
  title?: ReactNode;
  extra?: ReactNode;
  className?: string;
}>;

export const PageCard = ({ title, extra, className, children }: PageCardProps) => (
  <Card
    title={title}
    extra={extra}
    className={clsx('rounded-md border border-[var(--panel-border)] shadow-none', className)}
    styles={{
      header: {
        borderBottom: '1px solid var(--panel-border)',
        color: 'var(--text-strong)',
        fontSize: 14,
        fontWeight: 600,
      },
      body: {
        padding: 20,
      },
    }}
  >
    {children}
  </Card>
);
