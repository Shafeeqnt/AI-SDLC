import type { ReactNode } from 'react';

type SectionHeadingProps = {
  eyebrow?: string;
  title: string;
  description?: string;
  aside?: ReactNode;
};

export const SectionHeading = ({ eyebrow, title, description, aside }: SectionHeadingProps) => (
  <div className="flex flex-col gap-4 border-b border-[var(--panel-border)] pb-4 md:flex-row md:items-start md:justify-between">
    <div className="space-y-1">
      {eyebrow ? <p className="text-xs font-semibold uppercase tracking-[0.18em] text-[var(--accent-strong)]">{eyebrow}</p> : null}
      <h1 className="text-2xl font-semibold text-[var(--text-strong)]">{title}</h1>
      {description ? <p className="max-w-3xl text-sm text-[var(--text-subtle)]">{description}</p> : null}
    </div>
    {aside}
  </div>
);
