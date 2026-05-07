
export type YearsResponse = {
  years: YearSummary[];
};

export type YearSummary = {
  year: number;
  photoCount: number;
};

export type YearDetailResponse = {
  year: number;
  photoCount: number;
  months: MonthSummary[];
};

export type MonthSummary = {
  year: number;
  month: number;
  photoCount: number;
};

export type MonthDetailResponse = {
  year: number;
  month: number;
  photoCount: number;
  days: DaySummary[];
};

export type DaySummary = {
  year: number;
  month: number;
  day: number;
  photoCount: number;
};

export type PhotoSummary = {
  slug: string;
  title: string;
  takenAt: string;
  year: number;
  month: number;
  day: number;
  gallery: string | null;
  postUrl: string | null;
  postId: string | null;
  sortIndex: number;
  source: string;
  originalUrl: string;
  thumbUrl: string;
};

export type ApiLink = {
  href: string;
};

export type PagedResponse<T> = {
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  items: T[];
  links: Record<string, ApiLink>;
};

const apiBaseUrl = 'http://localhost:5296';

export async function getYears(): Promise<YearsResponse> {
  const response = await fetch(`${apiBaseUrl}/years`);

  if (!response.ok) {
    throw new Error(`Failed to load years. API returned ${response.status}.`);
  }

  return response.json();
}

export async function getYear(year: number): Promise<YearDetailResponse> {
  const response = await fetch(`${apiBaseUrl}/years/${year}`);

  if (!response.ok) {
    throw new Error(`Failed to load year ${year}. API returned ${response.status}.`);
  }

  return response.json();
}

export async function getMonth(
  year: number,
  month: number,
): Promise<MonthDetailResponse> {
  const response = await fetch(`${apiBaseUrl}/years/${year}/months/${month}`);

  if (!response.ok) {
    throw new Error(
      `Failed to load month ${year}-${month}. API returned ${response.status}.`,
    );
  }

  return response.json();
}

export async function getDayPhotos(
  year: number,
  month: number,
  day: number,
  page = 1,
  pageSize = 25,
): Promise<PagedResponse<PhotoSummary>> {
  const response = await fetch(
    `${apiBaseUrl}/years/${year}/months/${month}/days/${day}/photos?page=${page}&pageSize=${pageSize}`,
  );

  if (!response.ok) {
    throw new Error(
      `Failed to load photos for ${year}-${month}-${day}. API returned ${response.status}.`,
    );
  }

  return response.json();
}