/**
 * ============================================================================
 * Ignition Monitoring Platform
 * ============================================================================
 *
 * Frontend Entry Point
 *
 * Author: Roger Larson
 * Created: 06/07/2026
 *
 * Responsibilities:
 * - Initialize React
 * - Load global styles
 * - Mount the application
 *
 * Notes:
 * All application routing begins in App.tsx.
 * Global theme and shared styles are loaded from index.css.
 * ============================================================================
 */

import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
