import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import PrivacyPolicy from './privacyPolicy.jsx'

const pathname = window.location.pathname.replace(/\/+$/, '') || '/'

createRoot(document.getElementById('root')).render(
    <StrictMode>
        {pathname === '/privacy' ? <PrivacyPolicy /> : <App />}
    </StrictMode>,
)