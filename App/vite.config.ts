import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';

export default defineConfig({
  plugins: [react(), tailwindcss()],
  build: {
    rollupOptions: {
      output: {
        manualChunks(id) {
          if (!id.includes('node_modules')) {
            return;
          }

          if (id.includes('antd') || id.includes('@ant-design')) {
            return 'vendor-antd';
          }

          if (id.includes('@tanstack/react-query')) {
            return 'vendor-query';
          }

          if (id.includes('react-router')) {
            return 'vendor-router';
          }

          if (id.includes('axios')) {
            return 'vendor-network';
          }

          if (id.includes('react')) {
            return 'vendor-react';
          }
        },
      },
    },
  },
});
