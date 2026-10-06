import { defineConfig, globalIgnores } from 'eslint/config';
import tseslint from 'typescript-eslint';

export default defineConfig([
  globalIgnores(['dist']),
  {
    files: ['**/*.ts'],
    languageOptions: {
      parser: tseslint.parser,
      parserOptions: {
        projectService: true,
        tsconfigRootDir: import.meta.dirname,
      },
    },
    plugins: { '@typescript-eslint': tseslint.plugin },
    rules: {
      curly: ['error', 'all'],
      eqeqeq: 'error',
      '@typescript-eslint/no-floating-promises': [
        'error',
        {
          // The node:test runner awaits these itself
          allowForKnownSafeCalls: [
            { from: 'package', package: 'node:test', name: 'test' },
          ],
        },
      ],
    },
  },
]);
