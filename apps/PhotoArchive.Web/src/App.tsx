import { Route, Routes } from 'react-router-dom';
import './App.css';
import SiteHeader from './components/SiteHeader';
import ArchivePage from './pages/ArchivePage';
import HomePage from './pages/HomePage';
import OnThisDayPage from './pages/OnThisDayPage';

function App() {
  return (
    <main className="app-shell">
      <SiteHeader />

      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/archive" element={<ArchivePage />} />
        <Route path="/on-this-day" element={<OnThisDayPage />} />
      </Routes>

      <footer className="site-footer">
        <p>PhotoArchive Web</p>
      </footer>
    </main>
  );
}

export default App;