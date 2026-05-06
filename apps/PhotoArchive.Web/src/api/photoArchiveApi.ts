export type YearSummary = {
  year: number;
  photoCount: number;
};

export type YearsResponse = {
  years: YearSummary[];
};

export type MonthSummary = {
  year: number;
  month: number;
  photoCount: number;
};

export type YearDetailResponse = {
  year: number;
  photoCount: number;
  months: MonthSummary[];
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