import { Link } from 'react-router-dom';

type DayCardProps = {
  year: number;
  month: number;
  day: number;
  photoCount: number;
};

function DayCard({ year, month, day, photoCount }: DayCardProps) {
  return (
    <article className="day-card">
      <h3>Day {day}</h3>
      <p>{photoCount} photos</p>
      <Link to={`/archive/${year}/${month}/${day}`}>View day</Link>
    </article>
  );
}

export default DayCard;