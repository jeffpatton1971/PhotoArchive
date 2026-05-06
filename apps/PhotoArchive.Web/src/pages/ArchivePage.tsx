import { useEffect, useState } from 'react';
import { getYears, type YearSummary } from '../api/photoArchiveApi';
import YearCard from '../components/YearCard';

function ArchivePage() {
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
    <section className="archive-preview">
      <h2>Archive</h2>
      <p>Browse photos by year.</p>

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
      )}
    </section>
  );
}

export default ArchivePage;