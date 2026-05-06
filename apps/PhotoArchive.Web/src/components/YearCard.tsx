import { Link } from 'react-router-dom';

type YearCardProps = {
  year: number;
  photoCount: number;
};

function YearCard({ year, photoCount }: YearCardProps) {
  return (
    <article className="year-card">
      <h3>{year}</h3>
      <p>{photoCount} photos</p>
      <Link to={`/archive/${year}`}>View year</Link>
    </article>
  );
}

export default YearCard;