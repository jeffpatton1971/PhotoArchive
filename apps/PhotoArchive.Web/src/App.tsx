import { getYears, type YearSummary } from './api/photoArchiveApi';
import { useEffect, useState } from 'react';
import './App.css';
import Hero from './components/Hero';
import SiteHeader from './components/SiteHeader';
import YearCard from './components/YearCard';

function App() {
  const [years, setYears] = useState<YearSummary[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    async function loadYears() {
      try {
        const data = await getYears();

        setYears(data.years);
      } catch (error) {
        console.error('Failed to load archive years:', error);
        setErrorMessage('Could not load archive years. Check that the API is running.');
      } finally {
        setIsLoading(false);
      }
    }

    loadYears();
  }, []);

  return (
    <main className="app-shell">
      <SiteHeader />

      <Hero
        title="Frontend shell is running."
        message="This is the first React screen for PhotoArchive. Next we will use components to build the archive browser."
      />

      <section className="archive-preview">
        <h2>Archive Preview</h2>

        {isLoading ? (
          <p>Loading archive years...</p>
        ) : errorMessage ? (
          <p>{errorMessage}</p>
        ) : (
          <div className="year-grid">
            {years.map((yearSummary) => (
              <YearCard
                key={yearSummary.year}
                year={yearSummary.year}
                photoCount={yearSummary.photoCount}
              />
            ))}
          </div>
        )}      </section>

      <footer className="site-footer">
        <p>PhotoArchive Web</p>
      </footer>
    </main>
  );
}

export default App;