import { defineConfig, globalIgnores } from 'eslint/config';
import tsParser from '@typescript-eslint/parser';

export default defineConfig([
  globalIgnores(['dist']),
  {
    files: ['**/*.ts'],
    languageOptions: { parser: tsParser },
    rules: { curly: ['error', 'all'] },
  },
]);
