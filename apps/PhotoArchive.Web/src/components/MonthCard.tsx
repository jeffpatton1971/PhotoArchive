import { Link } from 'react-router-dom';

type MonthCardProps = {
  year: number;
  month: number;
  photoCount: number;
};

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

function MonthCard({ year, month, photoCount }: MonthCardProps) {
  const monthName = monthNames[month - 1] ?? `Month ${month}`;

  return (
    <article className="month-card">
      <h3>{monthName}</h3>
      <p>{photoCount} photos</p>
      <Link to={`/archive/${year}/${month}`}>View month</Link>
    </article>
  );
}

export default MonthCard;