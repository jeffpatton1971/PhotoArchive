import { Link } from 'react-router-dom';
import type { PhotoSummary } from '../api/photoArchiveApi';

type PhotoCardProps = {
  photo: PhotoSummary;
};

function PhotoCard({ photo }: PhotoCardProps) {
  return (
    <article className="photo-card">
      <Link to={`/photos/${photo.slug}`}>
        <img src={photo.thumbUrl} alt={photo.title} />
      </Link>

      <h3>{photo.title}</h3>
      <p>
        {photo.month}/{photo.day}/{photo.year}
      </p>
    </article>
  );
}

export default PhotoCard;