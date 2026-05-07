import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import {
    getDayPhotos,
    type PagedResponse,
    type PhotoSummary,
} from '../api/photoArchiveApi';
import PhotoGrid from '../components/PhotoGrid';

function DayPage() {
    const { year, month, day } = useParams<{
        year: string;
        month: string;
        day: string;
    }>();

    const yearNumber = Number(year);
    const monthNumber = Number(month);
    const dayNumber = Number(day);

    const [photoPage, setPhotoPage] =
        useState<PagedResponse<PhotoSummary> | null>(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [isLoading, setIsLoading] = useState(true);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);

    useEffect(() => {
        if (
            !year ||
            !month ||
            !day ||
            Number.isNaN(yearNumber) ||
            Number.isNaN(monthNumber) ||
            Number.isNaN(dayNumber)
        ) {
            setIsLoading(false);
            setErrorMessage('The archive year, month, and day must be numbers.');
            return;
        }

        async function loadPhotos() {
            try {
                setIsLoading(true);
                setErrorMessage(null);

                const data = await getDayPhotos(
                    yearNumber,
                    monthNumber,
                    dayNumber,
                    currentPage,
                );

                setPhotoPage(data);
            } catch (error) {
                console.error(
                    `Failed to load photos for ${yearNumber}-${monthNumber}-${dayNumber}:`,
                    error,
                );
                setErrorMessage(
                    `Could not load photos for ${yearNumber}-${monthNumber}-${dayNumber}.`,
                );
            } finally {
                setIsLoading(false);
            }
        }

        loadPhotos();
    }, [year, month, day, yearNumber, monthNumber, dayNumber, currentPage]);

    function goToPreviousPage() {
        setCurrentPage((page) => Math.max(1, page - 1));
    }

    function goToNextPage() {
        setCurrentPage((page) => page + 1);
    }

    if (isLoading) {
        return (
            <section className="page-content">
                <p>Loading photos...</p>
            </section>
        );
    }

    if (errorMessage) {
        return (
            <section className="page-content">
                <h2>Photos unavailable</h2>
                <p>{errorMessage}</p>
            </section>
        );
    }

    if (!photoPage) {
        return (
            <section className="page-content">
                <h2>Photos unavailable</h2>
                <p>No photo data was returned.</p>
            </section>
        );
    }

    return (
        <section className="page-content">
            <h2>
                {monthNumber}/{dayNumber}/{yearNumber}
            </h2>
            <p>
                {photoPage.totalCount} photos · Page {photoPage.page} of{' '}
                {photoPage.totalPages}
            </p>

            <PhotoGrid photos={photoPage.items} />

            <div className="pagination-controls">
                <button
                    type="button"
                    onClick={goToPreviousPage}
                    disabled={photoPage.page <= 1}
                >
                    Previous
                </button>

                <span>
                    Page {photoPage.page} of {photoPage.totalPages}
                </span>

                <button
                    type="button"
                    onClick={goToNextPage}
                    disabled={photoPage.page >= photoPage.totalPages}
                >
                    Next
                </button>
            </div>
        </section>
    );
}

export default DayPage;