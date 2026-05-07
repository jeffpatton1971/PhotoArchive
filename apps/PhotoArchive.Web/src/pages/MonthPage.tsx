import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { getMonth, type MonthDetailResponse } from '../api/photoArchiveApi';
import DayCard from '../components/DayCard';

const monthNames = [
  'January',
  'February',
  'March',
  'April',
  'May',
  'June',
  'July',
  'August',
  'September',
  'October',
  'November',
  'December',
];

function MonthPage() {
  const { year, month } = useParams<{ year: string; month: string }>();

  const yearNumber = Number(year);
  const monthNumber = Number(month);

  const [monthDetail, setMonthDetail] = useState<MonthDetailResponse | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    if (
      !year ||
      !month ||
      Number.isNaN(yearNumber) ||
      Number.isNaN(monthNumber)
    ) {
      setIsLoading(false);
      setErrorMessage('The archive year and month must be numbers.');
      return;
    }

    async function loadMonth() {
      try {
        const data = await getMonth(yearNumber, monthNumber);

        setMonthDetail(data);
      } catch (error) {
        console.error(`Failed to load archive month ${yearNumber}-${monthNumber}:`, error);
        setErrorMessage(`Could not load archive month ${yearNumber}-${monthNumber}.`);
      } finally {
        setIsLoading(false);
      }
    }

    loadMonth();
  }, [year, month, yearNumber, monthNumber]);

  if (isLoading) {
    return (
      <section className="page-content">
        <p>Loading archive month...</p>
      </section>
    );
  }

  if (errorMessage) {
    return (
      <section className="page-content">
        <h2>Archive month unavailable</h2>
        <p>{errorMessage}</p>
      </section>
    );
  }

  if (!monthDetail) {
    return (
      <section className="page-content">
        <h2>Archive month unavailable</h2>
        <p>No archive data was returned.</p>
      </section>
    );
  }

  const monthName =
    monthNames[monthDetail.month - 1] ?? `Month ${monthDetail.month}`;

  return (
    <section className="page-content">
      <h2>
        {monthName} {monthDetail.year}
      </h2>
      <p>{monthDetail.photoCount} photos</p>

      <div className="day-grid">
        {monthDetail.days.map((daySummary) => (
          <DayCard
            key={daySummary.day}
            year={daySummary.year}
            month={daySummary.month}
            day={daySummary.day}
            photoCount={daySummary.photoCount}
          />
        ))}
      </div>
    </section>
  );
}

export default MonthPage;