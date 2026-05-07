import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import {
  getPhoto,
  type PhotoDetailResponse,
} from '../api/photoArchiveApi';
import Breadcrumbs from '../components/Breadcrumbs';

function PhotoDetailPage() {
  const { slug } = useParams<{ slug: string }>();

  const [photoDetail, setPhotoDetail] = useState<PhotoDetailResponse | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

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
  useEffect(() => {
    if (!slug) {
      setIsLoading(false);
      setErrorMessage('A photo slug is required.');
      return;
    }

    const photoSlug = slug;

    async function loadPhoto() {
      try {
        setIsLoading(true);
        setErrorMessage(null);

        const data = await getPhoto(photoSlug);

        setPhotoDetail(data);
      } catch (error) {
        console.error(`Failed to load photo ${photoSlug}:`, error);
        setErrorMessage(`Could not load photo ${photoSlug}.`);
      } finally {
        setIsLoading(false);
      }
    }

    loadPhoto();
  }, [slug]);

  if (isLoading) {
    return (
      <section className="page-content">
        <p>Loading photo...</p>
      </section>
    );
  }

  if (errorMessage) {
    return (
      <section className="page-content">
        <h2>Photo unavailable</h2>
        <p>{errorMessage}</p>
      </section>
    );
  }

  if (!photoDetail) {
    return (
      <section className="page-content">
        <h2>Photo unavailable</h2>
        <p>No photo data was returned.</p>
      </section>
    );
  }

  const { photo } = photoDetail;

  return (
    <section className="page-content photo-detail-page">
      <Breadcrumbs
        items={[
          { label: 'Archive', to: '/archive' },
          { label: String(photo.year), to: `/archive/${photo.year}` },
          {
            label: monthNames[photo.month - 1] ?? `Month ${photo.month}`,
            to: `/archive/${photo.year}/${photo.month}`,
          },
          {
            label: String(photo.day),
            to: `/archive/${photo.year}/${photo.month}/${photo.day}`,
          },
          { label: photo.title },
        ]}
      />

      <h2>{photo.title}</h2>

      <img
        className="photo-detail-image"
        src={photo.originalUrl}
        alt={photo.title}
      />

      <dl className="photo-metadata">
        <div>
          <dt>Date</dt>
          <dd>
            {photo.month}/{photo.day}/{photo.year}
          </dd>
        </div>

        <div>
          <dt>Source</dt>
          <dd>{photo.source}</dd>
        </div>

        {photo.gallery ? (
          <div>
            <dt>Gallery</dt>
            <dd>{photo.gallery}</dd>
          </div>
        ) : null}

        {photo.postId ? (
          <div>
            <dt>Post</dt>
            <dd>{photo.postId}</dd>
          </div>
        ) : null}
      </dl>
    </section>
  );
}

export default PhotoDetailPage;