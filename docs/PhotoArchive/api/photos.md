# Photos API

## Endpoints

- `GET /photos`
- `GET /photos/{slug}`

## Pagination

All photo collection endpoints return `PagedResponse<PhotoDto>`.

## Notes

- Supports filtering by year, month, day
- Uses shared pagination implementation
