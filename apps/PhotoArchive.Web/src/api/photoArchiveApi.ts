export type YearSummary = {
  year: number;
  photoCount: number;
};

export type YearsResponse = {
  years: YearSummary[];
};

const apiBaseUrl = 'http://localhost:5296';

export async function getYears(): Promise<YearsResponse> {
  const response = await fetch(`${apiBaseUrl}/years`);

  if (!response.ok) {
    throw new Error(`Failed to load years. API returned ${response.status}.`);
  }

  return response.json();
}