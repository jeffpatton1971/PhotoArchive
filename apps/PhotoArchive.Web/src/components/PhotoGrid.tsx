import type { PhotoSummary } from '../api/photoArchiveApi';
import PhotoCard from './PhotoCard';

type PhotoGridProps = {
  photos: PhotoSummary[];
};

function PhotoGrid({ photos }: PhotoGridProps) {
  if (photos.length === 0) {
    return <p>No photos found.</p>;
  }

  return (
    <div className="photo-grid">
      {photos.map((photo) => (
        <PhotoCard key={photo.slug} photo={photo} />
      ))}
    </div>
  );
}

export default PhotoGrid;