import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { getYear, type YearDetailResponse } from '../api/photoArchiveApi';
import MonthCard from '../components/MonthCard';
import Breadcrumbs from '../components/Breadcrumbs';

function YearPage() {
  const { year } = useParams<{ year: string }>();

  const yearNumber = Number(year);

  const [yearDetail, setYearDetail] = useState<YearDetailResponse | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    if (!year || Number.isNaN(yearNumber)) {
      setIsLoading(false);
      setErrorMessage('The archive year must be a number.');
      return;
    }

    async function loadYear() {
      try {
        const data = await getYear(yearNumber);

        setYearDetail(data);
      } catch (error) {
        console.error(`Failed to load archive year ${yearNumber}:`, error);
        setErrorMessage(`Could not load archive year ${yearNumber}.`);
      } finally {
        setIsLoading(false);
      }
    }

    loadYear();
  }, [year, yearNumber]);

  if (isLoading) {
    return (
      <section className="page-content">
        <p>Loading archive year...</p>
      </section>
    );
  }

  if (errorMessage) {
    return (
      <section className="page-content">
        <h2>Archive year unavailable</h2>
        <p>{errorMessage}</p>
      </section>
    );
  }

  if (!yearDetail) {
    return (
      <section className="page-content">
        <h2>Archive year unavailable</h2>
        <p>No archive data was returned.</p>
      </section>
    );
  }

  return (
    <section className="page-content">
      <Breadcrumbs
        items={[
          { label: 'Archive', to: '/archive' },
          { label: String(yearDetail.year) },
        ]}
      />
      <h2>{yearDetail.year}</h2>
      <p>{yearDetail.photoCount} photos</p>

      <div className="month-grid">
        {yearDetail.months.map((monthSummary) => (
          <MonthCard
            key={monthSummary.month}
            year={monthSummary.year}
            month={monthSummary.month}
            photoCount={monthSummary.photoCount}
          />
        ))}
      </div>
    </section>
  );
}

export default YearPage;