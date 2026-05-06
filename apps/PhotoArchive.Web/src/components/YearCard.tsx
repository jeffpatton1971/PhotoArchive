type YearCardProps = {
  year: number;
  photoCount: number;
};

function YearCard({ year, photoCount }: YearCardProps) {
  return (
    <article className="year-card">
      <h3>{year}</h3>
      <p>{photoCount} photos</p>
      <a href={`/archive/${year}`}>View year</a>
    </article>
  );
}

export default YearCard;